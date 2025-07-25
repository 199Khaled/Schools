-- Stored Procedures for Table: الدرجات

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_الدرجات_ByID
(
    @معرّف_الدرجة int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM الدرجات
        WHERE معرّف_الدرجة = @معرّف_الدرجة;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_الدرجات
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM الدرجات;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_الدرجات
(
    @معرّف_الطالب int,
    @معرّف_مادة_الصف int,
    @الدرجة decimal(4,2),
    @تاريخ_الدرجة date,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@معرّف_الطالب)) IS NULL OR LTRIM(RTRIM(@معرّف_مادة_الصف)) IS NULL OR LTRIM(RTRIM(@الدرجة)) IS NULL OR LTRIM(RTRIM(@تاريخ_الدرجة)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO الدرجات ([معرّف_الطالب],[معرّف_مادة_الصف],[الدرجة],[تاريخ_الدرجة])
        VALUES (    LTRIM(RTRIM(@معرّف_الطالب)),
    LTRIM(RTRIM(@معرّف_مادة_الصف)),
    LTRIM(RTRIM(@الدرجة)),
    LTRIM(RTRIM(@تاريخ_الدرجة)));

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_الدرجات_ByID
(
    @معرّف_الدرجة int,
    @معرّف_الطالب int,
    @معرّف_مادة_الصف int,
    @الدرجة decimal(4,2),
    @تاريخ_الدرجة date
)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@معرّف_الطالب)) IS NULL OR LTRIM(RTRIM(@معرّف_الطالب)) = '' OR LTRIM(RTRIM(@معرّف_مادة_الصف)) IS NULL OR LTRIM(RTRIM(@معرّف_مادة_الصف)) = '' OR LTRIM(RTRIM(@الدرجة)) IS NULL OR LTRIM(RTRIM(@الدرجة)) = '' OR LTRIM(RTRIM(@تاريخ_الدرجة)) IS NULL OR LTRIM(RTRIM(@تاريخ_الدرجة)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE الدرجات
        SET     [معرّف_الطالب] = LTRIM(RTRIM(@معرّف_الطالب)),
    [معرّف_مادة_الصف] = LTRIM(RTRIM(@معرّف_مادة_الصف)),
    [الدرجة] = LTRIM(RTRIM(@الدرجة)),
    [تاريخ_الدرجة] = LTRIM(RTRIM(@تاريخ_الدرجة))
        WHERE معرّف_الدرجة = @معرّف_الدرجة;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_الدرجات_ByID
(
    @معرّف_الدرجة int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM الدرجات WHERE معرّف_الدرجة = @معرّف_الدرجة)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM الدرجات WHERE معرّف_الدرجة = @معرّف_الدرجة;

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


CREATE OR ALTER PROCEDURE SP_Search_الدرجات_ByColumn
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
                WHEN 'معرّف_الدرجة' THEN 'معرّف_الدرجة'
        WHEN 'معرّف_الطالب' THEN 'معرّف_الطالب'
        WHEN 'معرّف_مادة_الصف' THEN 'معرّف_مادة_الصف'
        WHEN 'الدرجة' THEN 'الدرجة'
        WHEN 'تاريخ_الدرجة' THEN 'تاريخ_الدرجة'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('الدرجات') + 
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
