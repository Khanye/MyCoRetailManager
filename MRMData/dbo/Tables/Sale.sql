CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CashierId] NVARCHAR(128) NOT NULL, 
    [SaleDate] DATETIME2 NOT NULL, 
    [Subtotal] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL, 
    [Total] MONEY NOT NULL, 
    CONSTRAINT [FK_Sale_ToUser] FOREIGN KEY (CashierID) REFERENCES [User](Id)
)
