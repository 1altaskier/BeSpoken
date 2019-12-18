SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE AddSale
	-- Add the parameters for the stored procedure here
	@ProductId int,
	@SalesPersonId int,
	@CustomerId int,
	@Qty int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Amount money;
	DECLARE @Cost int;
	DECLARE @Discount int;

	SELECT @Cost = PurchasePrice FROM [dbo].[Product] WHERE ProductId = @ProductId;

	SELECT @Discount = d.DiscountPercentage 
	FROM [dbo].[Discount] d 
	LEFT JOIN [dbo].[Product] p
		ON p.DiscountId = d.DiscountId
	WHERE p.ProductId = @ProductId;

	SELECT @Amount = (@Cost - ((@Cost * @Discount) / 100))

    -- Add Sale
	INSERT INTO [dbo].[Sales]
			   ([ProductId]
			   ,[SalesPersonId]
			   ,[CustomerId]
			   ,[SalesDate]
			   ,[Qty]
			   ,Amount)
		 VALUES
			   (@ProductId
			   ,@SalesPersonId
			   ,@CustomerId
			   ,GETDATE()
			   ,@Qty
			   ,@Amount)



	-- Decrement # Available
	UPDATE [dbo].[Product]
	SET [QtyOnHand] = (QtyOnHand - @Qty)
	WHERE ProductId = @ProductId



	-- Add Commission
	DECLARE @Qtr int;
	DECLARE @Yr int;
	DECLARE @CommissionPercentage float;
	DECLARE @Commission float;

	SELECT @Qtr = DATEPART(quarter, GETDATE());
	SELECT @Yr = DATEPART(yyyy, GETDATE());
	SELECT @CommissionPercentage = CommissionPercentage FROM [dbo].[Product] WHERE ProductId = @ProductId;
	SELECT @Commission = ((PurchasePrice * @CommissionPercentage) * @Qty) FROM Product WHERE ProductId = @ProductId;

	IF EXISTS (SELECT SalesPersonId FROM [dbo].[Commission] WHERE SalesPersonId = @SalesPersonId AND Qtr = @Qtr AND Year = @Yr)
		BEGIN
			DECLARE @NewCommission int;
			SELECT @NewCommission = Amount + CAST(@Commission AS money) FROM Commission WHERE SalesPersonId = @SalesPersonId AND Qtr = @Qtr AND Year = @Yr

			UPDATE [dbo].[Commission]
			SET [Amount] = @NewCommission
			WHERE SalesPersonId = @SalesPersonId
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Commission]
				([QtrId]
				,[Amount]
				,[SalesPersonId]
				,[Qtr]
				,[Year])
			VALUES
				(1
				,@Commission
				,@SalesPersonId
				,@Qtr
				,@Yr)
		END
END
GO
