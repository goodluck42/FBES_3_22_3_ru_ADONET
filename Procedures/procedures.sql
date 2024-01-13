-- CREATE PROCEDURE [sp_GetAllItems]
-- AS
-- BEGIN
--     SELECT * FROM [Items]
-- END


-- CREATE PROCEDURE [sp_GetItemCount]
-- @Result AS INT OUTPUT
-- AS
-- BEGIN
--     SELECT @Result = COUNT(*)
--     FROM [Items]
-- END

--GO
-- CREATE PROCEDURE [sp_GetAverageQuantity]
-- @Result AS FLOAT OUTPUT
-- AS
-- BEGIN
--     SELECT @Result = AVG(CAST(I.[Quantity] AS FLOAT))
--     FROM [Items] AS I
-- END

-- GO
-- CREATE PROCEDURE [sp_GetItemById]
-- @Id AS INT
-- AS
-- BEGIN
--     SELECT *
--     FROM [Items] AS I
--     WHERE @Id = I.Id
-- END

-----------------------

-- DECLARE @Result TABLE
--                 (
--                     [Id]       INT,
--                     [Name]     NVARCHAR(450),
--                     [Quantity] INT
--                 )
--
-- INSERT INTO @Result
-- EXECUTE [dbo].sp_GetAllItems
--
--
-- SELECT * FROM @Result


