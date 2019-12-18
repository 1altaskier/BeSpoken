SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[AddSalesperson]
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Address nvarchar(50),
	@City nvarchar(50),
	@State nvarchar(2),
	@Zip int,
	@Phone nvarchar(20),
	@AltPhone nvarchar(20) NULL,
	@StartDate date,
	@ManagerId int,
	@SalespersonId int output

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF NOT EXISTS(SELECT [SalesPersonId] FROM [dbo].[SalesPerson] WHERE FirstName = @FirstName AND LastName = @LastName AND StartDate = @StartDate)
	BEGIN
		INSERT INTO [dbo].[SalesPerson]
			([FirstName]
			,[LastName]
			,[Address]
			,[City]
			,[State]
			,[Zip]
			,[Phone]
			,[AltPhone]
			,[StartDate]
			,[ManagerId])
		VALUES
			(@FirstName,
			@LastName,
			@Address,
			@City,
			@State,
			@Zip,
			@Phone,
			@AltPhone,
			@StartDate,
			@ManagerId)

		SELECT @SalespersonId = @@IDENTITY
	END
ELSE
	BEGIN
		SELECT @SalespersonId = -1
	END
END
GO
