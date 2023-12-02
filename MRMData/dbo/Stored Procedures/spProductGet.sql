CREATE PROCEDURE [dbo].[spProductGet]	
AS
begin
      set nocount on;

	  select Id, PrdctTitle, Description ,RetailPrice, QuantityInStock
	  from dbo.Product
	  order by PrdctTitle

end
 