-- Check if db exists, create if not
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SensitiveWordsDb')
BEGIN
    EXEC('CREATE DATABASE SensitiveWordsDb');
END
GO

USE SensitiveWordsDb;
GO

-- Create Table
CREATE TABLE SensitiveWords (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SensitiveWords NVARCHAR(100) NOT NULL
);

--Insert List of Words
INSERT INTO dbo.SensitiveWords (SensitiveWords)
VALUES 
('ACTION'),
('ADD'),
('ALL'),
('ALLOCATE'),
('ALTER'),
('ANY'),
('APPLICATION'),
('ARE'),
('AREA'),
('ASC'),
('ASSERTION'),
('ATOMIC'),
('AUTHORIZATION'),
('AVG'),
('BEGIN'),
('BY'),
('CALL'),
('CASCADE'),
('CASCADED'),
('CATALOG'),
('CHECK'),
('CLOSE'),
('COLUMN'),
('COMMIT'),
('COMPRESS'),
('CONNECT'),
('CONNECTION'),
('CONSTRAINT'),
('CONSTRAINTS'),
('CONTINUE'),
('CONVERT'),
('CORRESPONDING'),
('CREATE'),
('CROSS'),
('CURRENT'),
('CURRENT_PATH'),
('CURRENT_SCHEMA'),
('CURRENT_SCHEMAID'),
('CURRENT_USER'),
('CURRENT_USERID'),
('CURSOR'),
('DATA'),
('DEALLOCATE'),
('DECLARE'),
('DEFAULT'),
('DEFERRABLE'),
('DEFERRED'),
('DELETE'),
('DESC'),
('DESCRIBE'),
('DESCRIPTOR'),
('DETERMINISTIC'),
('DIAGNOSTICS'),
('DIRECTORY'),
('DISCONNECT'),
('DISTINCT'),
('DO'),
('DOMAIN'),
('DOUBLEATTRIBUTE'),
('DROP'),
('EACH'),
('EXCEPT'),
('EXCEPTION'),
('EXEC'),
('EXECUTE'),
('EXTERNAL'),
('FETCH'),
('FLOAT'),
('FOREIGN'),
('FOUND'),
('FULL'),
('FUNCTION'),
('GET'),
('GLOBAL'),
('GO'),
('GOTO'),
('GRANT'),
('GROUP'),
('HANDLER'),
('HAVING'),
('IDENTITY'),
('IMMEDIATE'),
('INDEX'),
('INDEXED'),
('INDICATOR'),
('INITIALLY'),
('INNER'),
('INOUT'),
('INPUT'),
('INSENSITIVE'),
('INSERT'),
('INTERSECT'),
('INTO'),
('ISOLATION'),
('JOIN'),
('KEY'),
('LANGUAGE'),
('LAST'),
('LEAVE'),
('LEVEL'),
('LOCAL'),
('LONGATTRIBUTE'),
('LOOP'),
('MODIFIES'),
('MODULE'),
('NAMES'),
('NATIONAL'),
('NATURAL'),
('NEXT'),
('NULLIF'),
('ON'),
('ONLY'),
('OPEN'),
('OPTION'),
('ORDER'),
('OUT'),
('OUTER'),
('OUTPUT'),
('OVERLAPS'),
('OWNER'),
('PARTIAL'),
('PATH'),
('PRECISION'),
('PREPARE'),
('PRESERVE'),
('PRIMARY'),
('PRIOR'),
('PRIVILEGES'),
('PROCEDURE'),
('PUBLIC'),
('READ'),
('READS'),
('REFERENCES'),
('RELATIVE'),
('REPEAT'),
('RESIGNAL'),
('RESTRICT'),
('RETURN'),
('RETURNS'),
('REVOKE'),
('ROLLBACK'),
('ROUTINE'),
('ROW'),
('ROWS'),
('SCHEMA'),
('SCROLL'),
('SECTION'),
('SELECT'),
('SEQ'),
('SEQUENCE'),
('SESSION'),
('SESSION_USER'),
('SESSION_USERID'),
('SET'),
('SIGNAL'),
('SOME'),
('SPACE'),
('SPECIFIC'),
('SQL'),
('SQLCODE'),
('SQLERROR'),
('SQLEXCEPTION'),
('SQLSTATE'),
('SQLWARNING'),
('STATEMENT'),
('STRINGATTRIBUTE'),
('SUM'),
('SYSACC'),
('SYSHGH'),
('SYSLNK'),
('SYSNIX'),
('SYSTBLDEF'),
('SYSTBLDSC'),
('SYSTBT'),
('SYSTBTATT'),
('SYSTBTDEF'),
('SYSUSR'),
('SYSTEM_USER'),
('SYSVIW'),
('SYSVIWCOL'),
('TABLE'),
('TABLETYPE'),
('TEMPORARY'),
('TRANSACTION'),
('TRANSLATE'),
('TRANSLATION'),
('TRIGGER'),
('UNDO'),
('UNION'),
('UNIQUE'),
('UNTIL'),
('UPDATE'),
('USAGE'),
('USER'),
('USING'),
('VALUE'),
('VALUES'),
('VIEW'),
('WHERE'),
('WHILE'),
('WITH'),
('WORK'),
('WRITE'),
('ALLSCHEMAS'),
('ALLTABLES'),
('ALLVIEWS'),
('ALLVIEWTEXTS'),
('ALLCOLUMNS'),
('ALLINDEXES'),
('ALLINDEXCOLS'),
('ALLUSERS'),
('ALLTBTS'),
('TABLEPRIVILEGES'),
('TBTPRIVILEGES'),
('MYSCHEMAS'),
('MYTABLES'),
('MYTBTS'),
('MYVIEWS'),
('SCHEMAVIEWS'),
('DUAL'),
('SCHEMAPRIVILEGES'),
('SCHEMATABLES'),
('STATISTICS'),
('USRTBL'),
('STRINGTABLE'),
('LONGTABLE'),
('DOUBLETABLE'),
('SELECT * FROM');
GO

-- Create Stored Proc for getting all sensitive words
CREATE OR ALTER PROCEDURE dbo.pr_GetAllSensitiveWords
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SensitiveWords FROM dbo.SensitiveWords;
END
GO

-- Create Stored Proc for getting sensitive word by Id
CREATE OR ALTER PROCEDURE dbo.pr_GetSensitiveWordById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT SensitiveWords 
    FROM dbo.SensitiveWords swdb
    WHERE swdb.Id = @Id;
END
GO

-- Create Stored Proc for upserting sensitive word
CREATE OR ALTER PROCEDURE dbo.pr_UpsertSensitiveWord
    @SensitiveWord NVARCHAR(255),
    @Id INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if the word exists (case-insensitive match)
        SELECT @Id = Id
        FROM dbo.SensitiveWords
        WHERE LOWER(SensitiveWords) = LOWER(@SensitiveWord);

        IF @Id IS NULL
        BEGIN
            INSERT INTO dbo.SensitiveWords (SensitiveWords)
            VALUES (@SensitiveWord);

            SET @Id = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE dbo.SensitiveWords
            SET SensitiveWords = @SensitiveWord
            WHERE Id = @Id;
        END

        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        SET @Id = -1; -- Indicate error via output too (optional)

        -- Optional: capture and raise error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);

        RETURN -1; -- Failure
    END CATCH
END
GO

-- Create Stored Proc for upserting sensitive word
CREATE OR ALTER PROCEDURE dbo.pr_DeleteSensitiveWord
    @SensitiveWord NVARCHAR(255),
    @RowsAffected INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM dbo.SensitiveWords
        WHERE LOWER(SensitiveWords) = LOWER(@SensitiveWord);

        SET @RowsAffected = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        SET @RowsAffected = -1;

        -- Optional: Log the error or raise it again
        -- DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        -- PRINT @ErrorMessage;
    END CATCH
END
GO
