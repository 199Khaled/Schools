-- Stored Procedures for Table: Schedules

Use [SchoolsDb];
Go


CREATE OR ALTER PROCEDURE SP_Get_Schedules_ByID
(
    @ScheduleID int
)
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve data
        SELECT *
        FROM Schedules
        WHERE ScheduleID = @ScheduleID;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_Get_All_Schedules
AS
BEGIN
    BEGIN TRY
        -- Attempt to retrieve all data from the table
        SELECT *
        FROM Schedules;
    END TRY
    BEGIN CATCH
        -- Call the centralized error handling procedure
        EXEC SP_HandleError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Add_Schedules
(
    @TeacherID int = NULL,
    @ClassroomID int = NULL,
    @SubjectID int = NULL,
    @Day varchar(10) = NULL,
    @StartTime time = NULL,
    @EndTime time = NULL,
    @NewID INT OUTPUT

)
AS
BEGIN
    BEGIN TRY
        -- Check if any required parameters are NULL
        IF 
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Insert the data into the table
        INSERT INTO Schedules ([TeacherID],[ClassroomID],[SubjectID],[Day],[StartTime],[EndTime])
        VALUES (    LTRIM(RTRIM(@TeacherID)),
    LTRIM(RTRIM(@ClassroomID)),
    LTRIM(RTRIM(@SubjectID)),
    LTRIM(RTRIM(@Day)),
    LTRIM(RTRIM(@StartTime)),
    LTRIM(RTRIM(@EndTime)));

        -- Set the new ID
        SET @NewID = SCOPE_IDENTITY();  -- Get the last inserted ID
    END TRY
    BEGIN CATCH
        EXEC SP_HandleError; -- Error handling
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_Update_Schedules_ByID
(
    @ScheduleID int,
    @TeacherID int = NULL,
    @ClassroomID int = NULL,
    @SubjectID int = NULL,
    @Day varchar(10) = NULL,
    @StartTime time = NULL,
    @EndTime time = NULL
)
AS
BEGIN
    BEGIN TRY
        -- Check if required parameters are NULL or contain only whitespace after trimming
        IF 
        BEGIN
            RAISERROR('One or more required parameters are NULL or have only whitespace.', 16, 1);
            RETURN;
        END

        -- Update the record in the table
        UPDATE Schedules
        SET     [TeacherID] = LTRIM(RTRIM(@TeacherID)),
    [ClassroomID] = LTRIM(RTRIM(@ClassroomID)),
    [SubjectID] = LTRIM(RTRIM(@SubjectID)),
    [Day] = LTRIM(RTRIM(@Day)),
    [StartTime] = LTRIM(RTRIM(@StartTime)),
    [EndTime] = LTRIM(RTRIM(@EndTime))
        WHERE ScheduleID = @ScheduleID;
        
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


CREATE OR ALTER PROCEDURE SP_Delete_Schedules_ByID
(
    @ScheduleID int
)
AS
BEGIN

    BEGIN TRY
        -- Check if the record exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM Schedules WHERE ScheduleID = @ScheduleID)
        BEGIN
            EXEC SP_HandleError;
            RETURN;
        END

        -- Attempt to delete the record
        DELETE FROM Schedules WHERE ScheduleID = @ScheduleID;

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


CREATE OR ALTER PROCEDURE SP_Search_Schedules_ByColumn
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
                WHEN 'ScheduleID' THEN 'ScheduleID'
        WHEN 'TeacherID' THEN 'TeacherID'
        WHEN 'ClassroomID' THEN 'ClassroomID'
        WHEN 'SubjectID' THEN 'SubjectID'
        WHEN 'Day' THEN 'Day'
        WHEN 'StartTime' THEN 'StartTime'
        WHEN 'EndTime' THEN 'EndTime'
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
        SET @SQL = N'SELECT * FROM ' + QUOTENAME('Schedules') + 
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
