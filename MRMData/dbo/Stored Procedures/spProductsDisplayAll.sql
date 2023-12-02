CREATE PROCEDURE [dbo].[spProductsDisplayAll]	
AS
begin
      set nocount on;

	  select Id, [Prod_title ] ,[Description], RetailPrice,QuantityInStock
	  from dbo.Item
	  order by [Prod_title ]

end
 