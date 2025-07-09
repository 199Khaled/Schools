-- Stored Procedures for Table: الاختبارات

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_الاختبارات_ByID
(
    @معرّف_الاختبار int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM الاختبارات
        WHERE معرّف_الاختبار = @معرّف_الاختبار;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_الاختبارات
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM الاختبارات;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_الاختبارات
(
    @معرّف_المادة int,
    @معرّف_الصف int,
    @تاريخ_الاختبار date,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@معرّف_المادة)) IS NULL OR LTRIM(RTRIM(@معرّف_الصف)) IS NULL OR LTRIM(RTRIM(@تاريخ_الاختبار)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO الاختبارات ([معرّف_المادة],[معرّف_الصف],[تاريخ_الاختبار])
        VALUES (    LTRIM(RTRIM(@معرّف_المادة)),
    LTRIM(RTRIM(@معرّف_الصف)),
    LTRIM(RTRIM(@تاريخ_الاختبار)));

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_الاختبارات_ByID
(
    @معرّف_الاختبار int,
    @معرّف_المادة int,
    @معرّف_الصف int,
    @تاريخ_الاختبار date
)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@معرّف_المادة)) IS NULL OR LTRIM(RTRIM(@معرّف_المادة)) = '' OR LTRIM(RTRIM(@معرّف_الصف)) IS NULL OR LTRIM(RTRIM(@معرّف_الصف)) = '' OR LTRIM(RTRIM(@تاريخ_الاختبار)) IS NULL OR LTRIM(RTRIM(@تاريخ_الاختبار)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE الاختبارات
        SET     [معرّف_المادة] = LTRIM(RTRIM(@معرّف_المادة)),
    [معرّف_الصف] = LTRIM(RTRIM(@معرّف_الصف)),
    [تاريخ_الاختبار] = LTRIM(RTRIM(@تاريخ_الاختبار))
        WHERE معرّف_الاختبار = @معرّف_الاختبار;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_الاختبارات_ByID
(
    @معرّف_الاختبار int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM الاختبارات WHERE معرّف_الاختبار = @معرّف_الاختبار)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM الاختبارات WHERE معرّف_الاختبار = @معرّف_الاختبار;

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


CREATE OR ALTER PROCEDURE SP_Search_الاختبارات_ByColumn
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
                WHEN 'معرّف_الاختبار' THEN 'معرّف_الاختبار'
        WHEN 'معرّف_المادة' THEN 'معرّف_المادة'
        WHEN 'معرّف_الصف' THEN 'معرّف_الصف'
        WHEN 'تاريخ_الاختبار' THEN 'تاريخ_الاختبار'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('الاختبارات') + 
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
