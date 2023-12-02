CREATE PROCEDURE [dbo].[spProductGet]	
AS
begin
      set nocount on;

	  select Id, PrdctTitle, Description ,RetailPrice, QuantityInStock,IsTaxable
	  from dbo.Product
	  order by PrdctTitle

end
 