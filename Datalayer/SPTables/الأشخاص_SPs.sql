-- Stored Procedures for Table: الأشخاص

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_الأشخاص_ByID
(
    @معرّف_الشخص int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM الأشخاص
        WHERE معرّف_الشخص = @معرّف_الشخص;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_الأشخاص
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM الأشخاص;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_الأشخاص
(
    @الاسم_الأول nvarchar(50),
    @اسم_الأب nvarchar(50) = NULL,
    @اسم_الأم nvarchar(50) = NULL,
    @اسم_العائلة nvarchar(50),
    @تاريخ_الميلاد date = NULL,
    @الجنس char(5) = NULL,
    @المدينة nvarchar(50) = NULL,
    @الهاتف nvarchar(20) = NULL,
    @البريد_الإلكتروني nvarchar(100) = NULL,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@الاسم_الأول)) IS NULL OR LTRIM(RTRIM(@اسم_العائلة)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO الأشخاص ([الاسم_الأول],[اسم_الأب],[اسم_الأم],[اسم_العائلة],[تاريخ_الميلاد],[الجنس],[المدينة],[الهاتف],[البريد_الإلكتروني])
        VALUES (    LTRIM(RTRIM(@الاسم_الأول)),
    LTRIM(RTRIM(@اسم_الأب)),
    LTRIM(RTRIM(@اسم_الأم)),
    LTRIM(RTRIM(@اسم_العائلة)),
    LTRIM(RTRIM(@تاريخ_الميلاد)),
    LTRIM(RTRIM(@الجنس)),
    LTRIM(RTRIM(@المدينة)),
    LTRIM(RTRIM(@الهاتف)),
    LTRIM(RTRIM(@البريد_الإلكتروني)));

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_الأشخاص_ByID
(
    @معرّف_الشخص int,
    @الاسم_الأول nvarchar(50),
    @اسم_الأب nvarchar(50) = NULL,
    @اسم_الأم nvarchar(50) = NULL,
    @اسم_العائلة nvarchar(50),
    @تاريخ_الميلاد date = NULL,
    @الجنس char(5) = NULL,
    @المدينة nvarchar(50) = NULL,
    @الهاتف nvarchar(20) = NULL,
    @البريد_الإلكتروني nvarchar(100) = NULL
)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@الاسم_الأول)) IS NULL OR LTRIM(RTRIM(@الاسم_الأول)) = '' OR LTRIM(RTRIM(@اسم_العائلة)) IS NULL OR LTRIM(RTRIM(@اسم_العائلة)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE الأشخاص
        SET     [الاسم_الأول] = LTRIM(RTRIM(@الاسم_الأول)),
    [اسم_الأب] = LTRIM(RTRIM(@اسم_الأب)),
    [اسم_الأم] = LTRIM(RTRIM(@اسم_الأم)),
    [اسم_العائلة] = LTRIM(RTRIM(@اسم_العائلة)),
    [تاريخ_الميلاد] = LTRIM(RTRIM(@تاريخ_الميلاد)),
    [الجنس] = LTRIM(RTRIM(@الجنس)),
    [المدينة] = LTRIM(RTRIM(@المدينة)),
    [الهاتف] = LTRIM(RTRIM(@الهاتف)),
    [البريد_الإلكتروني] = LTRIM(RTRIM(@البريد_الإلكتروني))
        WHERE معرّف_الشخص = @معرّف_الشخص;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_الأشخاص_ByID
(
    @معرّف_الشخص int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM الأشخاص WHERE معرّف_الشخص = @معرّف_الشخص)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM الأشخاص WHERE معرّف_الشخص = @معرّف_الشخص;

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


CREATE OR ALTER PROCEDURE SP_Search_الأشخاص_ByColumn
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
                WHEN 'معرّف_الشخص' THEN 'معرّف_الشخص'
        WHEN 'الاسم_الأول' THEN 'الاسم_الأول'
        WHEN 'اسم_الأب' THEN 'اسم_الأب'
        WHEN 'اسم_الأم' THEN 'اسم_الأم'
        WHEN 'اسم_العائلة' THEN 'اسم_العائلة'
        WHEN 'تاريخ_الميلاد' THEN 'تاريخ_الميلاد'
        WHEN 'الجنس' THEN 'الجنس'
        WHEN 'المدينة' THEN 'المدينة'
        WHEN 'الهاتف' THEN 'الهاتف'
        WHEN 'البريد_الإلكتروني' THEN 'البريد_الإلكتروني'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('الأشخاص') + 
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
