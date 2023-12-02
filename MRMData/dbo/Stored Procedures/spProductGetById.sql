CREATE PROCEDURE [dbo].[spProductGetById]
	@Id int
AS
begin
      set nocount on;

	  select Id, PrdctTitle, Description ,RetailPrice, QuantityInStock,IsTaxable
	  from dbo.Product
	  where Id = @Id;
end
