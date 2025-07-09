-- Stored Procedures for Table: الموظفون

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_الموظفون_ByID
(
    @معرّف_الموظف int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM الموظفون
        WHERE معرّف_الموظف = @معرّف_الموظف;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_الموظفون
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM الموظفون;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_الموظفون
(
    @معرّف_الشخص int,
    @النوع nvarchar(100),
    @تاريخ_التوظيف date,
    @تاريخ_الإنهاء nvarchar(15) = NULL,
    @نشط bit = NULL,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@معرّف_الشخص)) IS NULL OR LTRIM(RTRIM(@النوع)) IS NULL OR LTRIM(RTRIM(@تاريخ_التوظيف)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO الموظفون ([معرّف_الشخص],[النوع],[تاريخ_التوظيف],[تاريخ_الإنهاء],[نشط])
        VALUES (    LTRIM(RTRIM(@معرّف_الشخص)),
    LTRIM(RTRIM(@النوع)),
    LTRIM(RTRIM(@تاريخ_التوظيف)),
    LTRIM(RTRIM(@تاريخ_الإنهاء)),
    LTRIM(RTRIM(@نشط)));

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_الموظفون_ByID
(
    @معرّف_الموظف int,
    @معرّف_الشخص int,
    @النوع nvarchar(100),
    @تاريخ_التوظيف date,
    @تاريخ_الإنهاء nvarchar(15) = NULL,
    @نشط bit = NULL
)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@معرّف_الشخص)) IS NULL OR LTRIM(RTRIM(@معرّف_الشخص)) = '' OR LTRIM(RTRIM(@النوع)) IS NULL OR LTRIM(RTRIM(@النوع)) = '' OR LTRIM(RTRIM(@تاريخ_التوظيف)) IS NULL OR LTRIM(RTRIM(@تاريخ_التوظيف)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE الموظفون
        SET     [معرّف_الشخص] = LTRIM(RTRIM(@معرّف_الشخص)),
    [النوع] = LTRIM(RTRIM(@النوع)),
    [تاريخ_التوظيف] = LTRIM(RTRIM(@تاريخ_التوظيف)),
    [تاريخ_الإنهاء] = LTRIM(RTRIM(@تاريخ_الإنهاء)),
    [نشط] = LTRIM(RTRIM(@نشط))
        WHERE معرّف_الموظف = @معرّف_الموظف;
        
        -- Optionally, you can check if the update was successful and raise an error if no rows were updated
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('No rows were updated. Please check the PersonID or other parameters.', 16, 1);
            RETURN;
        END
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Handle errors
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Delete_الموظفون_ByID
(
    @معرّف_الموظف int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM الموظفون WHERE معرّف_الموظف = @معرّف_الموظف)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM الموظفون WHERE معرّف_الموظف = @معرّف_الموظف;

        -- Ensure at least one row was deleted
        IF @@ROWCOUNT = 0
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END
    END TRY
    BEGIN CATCH
        -- Handle all errors (including FK constraint violations)
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Search_الموظفون_ByColumn
(
    @ColumnName NVARCHAR(128),  -- Column name without spaces
    @SearchValue NVARCHAR(255), -- Value to search for
    @Mode NVARCHAR(20) = 'Anywhere' -- Search mode (default: Anywhere)
)
AS
BEGIN
    BEGIN TRY
        DECLARE @ActualColumn NVARCHAR(128);
        DECLARE @SQL NVARCHAR(MAX);
        DECLARE @SearchPattern NVARCHAR(255);

        -- Map input column name to actual database column name
        SET @ActualColumn = 
            CASE @ColumnName
                WHEN 'معرّف_الموظف' THEN 'معرّف_الموظف'
        WHEN 'معرّف_الشخص' THEN 'معرّف_الشخص'
        WHEN 'النوع' THEN 'النوع'
        WHEN 'تاريخ_التوظيف' THEN 'تاريخ_التوظيف'
        WHEN 'تاريخ_الإنهاء' THEN 'تاريخ_الإنهاء'
        WHEN 'نشط' THEN 'نشط'
                ELSE NULL
            END;

        -- Validate the column name
        IF @ActualColumn IS NULL
        BEGIN
            RAISERROR('Invalid Column Name provided.', 16, 1);
            RETURN;
        END

        -- Validate the search value (ensure it's not empty or NULL)
        IF ISNULL(LTRIM(RTRIM(@SearchValue)), '') = ''
        BEGIN
            RAISERROR('Search value cannot be empty.', 16, 1);
            RETURN;
        END

        -- Prepare the search pattern based on the mode
        SET @SearchPattern =
            CASE 
                WHEN @Mode = 'Anywhere' THEN '%' + LTRIM(RTRIM(@SearchValue)) + '%'
                WHEN @Mode = 'StartsWith' THEN LTRIM(RTRIM(@SearchValue)) + '%'
                WHEN @Mode = 'EndsWith' THEN '%' + LTRIM(RTRIM(@SearchValue))
                WHEN @Mode = 'ExactMatch' THEN LTRIM(RTRIM(@SearchValue))
                ELSE '%' + LTRIM(RTRIM(@SearchValue)) + '%'
            END;

        -- Build the dynamic SQL query safely
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('الموظفون') + 
                   N' WHERE ' + QUOTENAME(@ActualColumn) + N' LIKE @SearchPattern OPTION (RECOMPILE)';

        -- Execute the dynamic SQL with parameterized search pattern
        EXEC sp_executesql @SQL, N'@SearchPattern NVARCHAR(255)', @SearchPattern;
    END TRY
    BEGIN CATCH
        -- Handle errors
        EXEC SP_HandleError;
    END CATCH
END;
GO
