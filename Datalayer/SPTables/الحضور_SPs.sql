-- Stored Procedures for Table: الحضور

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_الحضور_ByID
(
    @معرّف_الحضور int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM الحضور
        WHERE معرّف_الحضور = @معرّف_الحضور;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_الحضور
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM الحضور;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_الحضور
(
    @معرّف_الطالب int,
    @معرّف_الفصل int,
    @تاريخ_الحضور date = NULL,
    @الحالة nvarchar(10) = NULL,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@معرّف_الطالب)) IS NULL OR LTRIM(RTRIM(@معرّف_الفصل)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO الحضور ([معرّف_الطالب],[معرّف_الفصل],[تاريخ_الحضور],[الحالة])
        VALUES (    LTRIM(RTRIM(@معرّف_الطالب)),
    LTRIM(RTRIM(@معرّف_الفصل)),
    LTRIM(RTRIM(@تاريخ_الحضور)),
    LTRIM(RTRIM(@الحالة)));

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_الحضور_ByID
(
    @معرّف_الحضور int,
    @معرّف_الطالب int,
    @معرّف_الفصل int,
    @تاريخ_الحضور date = NULL,
    @الحالة nvarchar(10) = NULL
)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@معرّف_الطالب)) IS NULL OR LTRIM(RTRIM(@معرّف_الطالب)) = '' OR LTRIM(RTRIM(@معرّف_الفصل)) IS NULL OR LTRIM(RTRIM(@معرّف_الفصل)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE الحضور
        SET     [معرّف_الطالب] = LTRIM(RTRIM(@معرّف_الطالب)),
    [معرّف_الفصل] = LTRIM(RTRIM(@معرّف_الفصل)),
    [تاريخ_الحضور] = LTRIM(RTRIM(@تاريخ_الحضور)),
    [الحالة] = LTRIM(RTRIM(@الحالة))
        WHERE معرّف_الحضور = @معرّف_الحضور;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_الحضور_ByID
(
    @معرّف_الحضور int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM الحضور WHERE معرّف_الحضور = @معرّف_الحضور)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM الحضور WHERE معرّف_الحضور = @معرّف_الحضور;

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


CREATE OR ALTER PROCEDURE SP_Search_الحضور_ByColumn
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
                WHEN 'معرّف_الحضور' THEN 'معرّف_الحضور'
        WHEN 'معرّف_الطالب' THEN 'معرّف_الطالب'
        WHEN 'معرّف_الفصل' THEN 'معرّف_الفصل'
        WHEN 'تاريخ_الحضور' THEN 'تاريخ_الحضور'
        WHEN 'الحالة' THEN 'الحالة'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('الحضور') + 
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
