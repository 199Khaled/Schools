-- Stored Procedures for Table: Persons

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_Persons_ByID
(
    @PersonID int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM Persons
        WHERE PersonID = @PersonID;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_Persons
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM Persons;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_Persons
(
    @Firstname nvarchar(50),
    @Lastname nvarchar(50),
    @DateOfBirth date,
    @Gender char(1),
    @City nvarchar(50) = NULL,
    @Phone nvarchar(20) = NULL,
    @Email nvarchar(100) = NULL,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF LTRIM(RTRIM(@Firstname)) IS NULL OR LTRIM(RTRIM(@Lastname)) IS NULL OR LTRIM(RTRIM(@DateOfBirth)) IS NULL OR LTRIM(RTRIM(@Gender)) IS NULL
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO Persons ([Firstname],[Lastname],[DateOfBirth],[Gender],[City],[Phone],[Email])
        VALUES (    LTRIM(RTRIM(@Firstname)),
    LTRIM(RTRIM(@Lastname)),
    LTRIM(RTRIM(@DateOfBirth)),
    LTRIM(RTRIM(@Gender)),
    LTRIM(RTRIM(@City)),
    LTRIM(RTRIM(@Phone)),
    LTRIM(RTRIM(@Email))
);

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_Persons_ByID
(
    @PersonID int,
    @Firstname nvarchar(50),
    @Lastname nvarchar(50),
    @DateOfBirth date,
    @Gender char(1),
    @City nvarchar(50) = NULL,
    @Phone nvarchar(20) = NULL,
    @Email nvarchar(100) = NULL

)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF LTRIM(RTRIM(@Firstname)) IS NULL OR LTRIM(RTRIM(@Firstname)) = '' OR LTRIM(RTRIM(@Lastname)) IS NULL OR LTRIM(RTRIM(@Lastname)) = '' OR LTRIM(RTRIM(@DateOfBirth)) IS NULL OR LTRIM(RTRIM(@DateOfBirth)) = '' OR LTRIM(RTRIM(@Gender)) IS NULL OR LTRIM(RTRIM(@Gender)) = ''
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE Persons
        SET     [Firstname] = LTRIM(RTRIM(@Firstname)),
    [Lastname] = LTRIM(RTRIM(@Lastname)),
    [DateOfBirth] = LTRIM(RTRIM(@DateOfBirth)),
    [Gender] = LTRIM(RTRIM(@Gender)),
    [City] = LTRIM(RTRIM(@City)),
    [Phone] = LTRIM(RTRIM(@Phone)),
    [Email] = LTRIM(RTRIM(@Email))

        WHERE PersonID = @PersonID;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_Persons_ByID
(
    @PersonID int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM Persons WHERE PersonID = @PersonID)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM Persons WHERE PersonID = @PersonID;

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


CREATE OR ALTER PROCEDURE SP_Search_Persons_ByColumn
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
                WHEN 'PersonID' THEN 'PersonID'
        WHEN 'Firstname' THEN 'Firstname'
        WHEN 'Lastname' THEN 'Lastname'
        WHEN 'DateOfBirth' THEN 'DateOfBirth'
        WHEN 'Gender' THEN 'Gender'
        WHEN 'City' THEN 'City'
        WHEN 'Phone' THEN 'Phone'
        WHEN 'Email' THEN 'Email'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('Persons') + 
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
