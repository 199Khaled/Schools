-- Stored Procedures for Table: الجداول_الزمنية

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_الجداول_الزمنية_ByID
(
    @معرّف_الجدول int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM الجداول_الزمنية
        WHERE معرّف_الجدول = @معرّف_الجدول;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_الجداول_الزمنية
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM الجداول_الزمنية;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_الجداول_الزمنية
(
    @معرّف_المعلم int,
    @معرّف_الفصل int,
    @معرّف_المادة int,
    @اليوم date,
    @وقت_البداية time,
    @وقت_النهاية time,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@معرّف_المعلم)) IS NULL OR LTRIM(RTRIM(@معرّف_الفصل)) IS NULL OR LTRIM(RTRIM(@معرّف_المادة)) IS NULL OR LTRIM(RTRIM(@اليوم)) IS NULL OR LTRIM(RTRIM(@وقت_البداية)) IS NULL OR LTRIM(RTRIM(@وقت_النهاية)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO الجداول_الزمنية ([معرّف_المعلم],[معرّف_الفصل],[معرّف_المادة],[اليوم],[وقت_البداية],[وقت_النهاية])
        VALUES (    LTRIM(RTRIM(@معرّف_المعلم)),
    LTRIM(RTRIM(@معرّف_الفصل)),
    LTRIM(RTRIM(@معرّف_المادة)),
    LTRIM(RTRIM(@اليوم)),
    LTRIM(RTRIM(@وقت_البداية)),
    LTRIM(RTRIM(@وقت_النهاية)));

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_الجداول_الزمنية_ByID
(
    @معرّف_الجدول int,
    @معرّف_المعلم int,
    @معرّف_الفصل int,
    @معرّف_المادة int,
    @اليوم date,
    @وقت_البداية time,
    @وقت_النهاية time
)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@معرّف_المعلم)) IS NULL OR LTRIM(RTRIM(@معرّف_المعلم)) = '' OR LTRIM(RTRIM(@معرّف_الفصل)) IS NULL OR LTRIM(RTRIM(@معرّف_الفصل)) = '' OR LTRIM(RTRIM(@معرّف_المادة)) IS NULL OR LTRIM(RTRIM(@معرّف_المادة)) = '' OR LTRIM(RTRIM(@اليوم)) IS NULL OR LTRIM(RTRIM(@اليوم)) = '' OR LTRIM(RTRIM(@وقت_البداية)) IS NULL OR LTRIM(RTRIM(@وقت_البداية)) = '' OR LTRIM(RTRIM(@وقت_النهاية)) IS NULL OR LTRIM(RTRIM(@وقت_النهاية)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE الجداول_الزمنية
        SET     [معرّف_المعلم] = LTRIM(RTRIM(@معرّف_المعلم)),
    [معرّف_الفصل] = LTRIM(RTRIM(@معرّف_الفصل)),
    [معرّف_المادة] = LTRIM(RTRIM(@معرّف_المادة)),
    [اليوم] = LTRIM(RTRIM(@اليوم)),
    [وقت_البداية] = LTRIM(RTRIM(@وقت_البداية)),
    [وقت_النهاية] = LTRIM(RTRIM(@وقت_النهاية))
        WHERE معرّف_الجدول = @معرّف_الجدول;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_الجداول_الزمنية_ByID
(
    @معرّف_الجدول int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM الجداول_الزمنية WHERE معرّف_الجدول = @معرّف_الجدول)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM الجداول_الزمنية WHERE معرّف_الجدول = @معرّف_الجدول;

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


CREATE OR ALTER PROCEDURE SP_Search_الجداول_الزمنية_ByColumn
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
                WHEN 'معرّف_الجدول' THEN 'معرّف_الجدول'
        WHEN 'معرّف_المعلم' THEN 'معرّف_المعلم'
        WHEN 'معرّف_الفصل' THEN 'معرّف_الفصل'
        WHEN 'معرّف_المادة' THEN 'معرّف_المادة'
        WHEN 'اليوم' THEN 'اليوم'
        WHEN 'وقت_البداية' THEN 'وقت_البداية'
        WHEN 'وقت_النهاية' THEN 'وقت_النهاية'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('الجداول_الزمنية') + 
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
