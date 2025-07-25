-- Stored Procedures for Table: نتائج_الامتحانات_للطلاب

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_نتائج_الامتحانات_للطلاب_ByID
(
    @معرّف_النتيجة int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM نتائج_الامتحانات_للطلاب
        WHERE معرّف_النتيجة = @معرّف_النتيجة;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_نتائج_الامتحانات_للطلاب
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM نتائج_الامتحانات_للطلاب;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_نتائج_الامتحانات_للطلاب
(
    @معرّف_الطالب int,
    @معرّف_الاختبار int,
    @الدرجة decimal(5,2) = NULL,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@معرّف_الطالب)) IS NULL OR LTRIM(RTRIM(@معرّف_الاختبار)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO نتائج_الامتحانات_للطلاب ([معرّف_الطالب],[معرّف_الاختبار],[الدرجة])
        VALUES (    LTRIM(RTRIM(@معرّف_الطالب)),
    LTRIM(RTRIM(@معرّف_الاختبار)),
    LTRIM(RTRIM(@الدرجة))
);

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_نتائج_الامتحانات_للطلاب_ByID
(
    @معرّف_النتيجة int,
    @معرّف_الطالب int,
    @معرّف_الاختبار int,
    @الدرجة decimal(5,2) = NULL

)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@معرّف_الطالب)) IS NULL OR LTRIM(RTRIM(@معرّف_الطالب)) = '' OR LTRIM(RTRIM(@معرّف_الاختبار)) IS NULL OR LTRIM(RTRIM(@معرّف_الاختبار)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE نتائج_الامتحانات_للطلاب
        SET     [معرّف_الطالب] = LTRIM(RTRIM(@معرّف_الطالب)),
    [معرّف_الاختبار] = LTRIM(RTRIM(@معرّف_الاختبار)),
    [الدرجة] = LTRIM(RTRIM(@الدرجة))

        WHERE معرّف_النتيجة = @معرّف_النتيجة;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_نتائج_الامتحانات_للطلاب_ByID
(
    @معرّف_النتيجة int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM نتائج_الامتحانات_للطلاب WHERE معرّف_النتيجة = @معرّف_النتيجة)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM نتائج_الامتحانات_للطلاب WHERE معرّف_النتيجة = @معرّف_النتيجة;

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


CREATE OR ALTER PROCEDURE SP_Search_نتائج_الامتحانات_للطلاب_ByColumn
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
                WHEN 'معرّف_النتيجة' THEN 'معرّف_النتيجة'
        WHEN 'معرّف_الطالب' THEN 'معرّف_الطالب'
        WHEN 'معرّف_الاختبار' THEN 'معرّف_الاختبار'
        WHEN 'الدرجة' THEN 'الدرجة'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('نتائج_الامتحانات_للطلاب') + 
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
