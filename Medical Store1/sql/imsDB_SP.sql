USE [imsDB]
GO
/****** Object:  StoredProcedure [dbo].[insertSales]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[insertSales]
@done int,
@date datetime,
@totamt float,
@totdis float,
@given float,
@return float,
@payType tinyint, --0 means cash 1 means cash, 2 means debit
@remainAmount float,
@custID bigint
as
insert into sales values (@done,@date,@totamt,@totdis,@given,@return,@payType,@remainAmount,@custID)
GO
/****** Object:  StoredProcedure [dbo].[st_checkProductPriceExist]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_checkProductPriceExist]
@proID bigint
as select * from productPrice where pp_proID = @proID
GO
/****** Object:  StoredProcedure [dbo].[st_deleteCategory]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteCategory]
@id int
as
delete from categories
where cat_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_deleteCustomer]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteCustomer]
@id bigint
as
delete from customer where cust_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_deleteExpenditure]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteExpenditure]
@id bigint
as
delete from daily_expenditure where expense_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_deleteProduct]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteProduct]
@id int
as
delete from products
where pro_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_deleteProductFromPID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteProductFromPID]
@mPID bigint
as
delete from purchase_invoice_details
where pid_id = @mPID
GO
/****** Object:  StoredProcedure [dbo].[st_deleteSalesDetails]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteSalesDetails]
@sale_id bigint,
@pro_id bigint
as
delete from SaleDetails where SaleDetails.sd_SalID =@sale_id and SaleDetails.sd_ProID =@pro_id
GO
/****** Object:  StoredProcedure [dbo].[st_deleteSupplier]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteSupplier]
@suppID int
as
delete from supplier where sup_id = @suppID
GO
/****** Object:  StoredProcedure [dbo].[st_deleteUser]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_deleteUser]
@id int
as
delete from users where usr_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_getAllStock]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure  [dbo].[st_getAllStock]
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_packing as 'Packing',
p.pro_barcode as 'Barcode',
format (p.pro_expiry, 'dd-MMM-yyyy') as 'Expiry Date',
pp.pp_buyingPrice as 'Buying Price',
pp.pp_sellingPrice as 'Selling Price',
c.cat_name as 'Category',
s.st_quan as 'Available Stock',
s.st_pack_quan as 'Per Packing Quantity',
pp.pp_buyingPrice * s.st_quan as 'Total Amount',
pp.pp_sellingPrice * s.st_quan as 'Total Amount Sell',
case when (s.st_quan <= 50) then 'Low' else 
case when (s.st_quan <  100  and s.st_quan > 50) then 'Average' else
case when (s.st_quan >= 100) then 'Good' end end end as 'Status'
from stock s
inner join products p
on p.pro_id = s.st_proID
inner join categories c
on c.cat_id = p.pro_catID
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_expiry > CURRENT_TIMESTAMP or p.pro_expiry is null
GO
/****** Object:  StoredProcedure [dbo].[st_getAllStockSearch]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getAllStockSearch]
@data varchar(50)
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_packing as 'Packing',
p.pro_barcode as 'Barcode',
format (p.pro_expiry, 'dd-MMM-yyyy') as 'Expiry Date',
pp.pp_buyingPrice as 'Buying Price',
pp.pp_sellingPrice as 'Selling Price',
c.cat_name as 'Category',
s.st_quan as 'Available Stock',
s.st_pack_quan as 'Per Packing Quantity',
pp.pp_buyingPrice * s.st_quan as 'Total Amount',
pp.pp_sellingPrice * s.st_quan as 'Total Amount Sell',
case when (s.st_quan <= 50) then 'Low' else case when (s.st_quan <  100  and s.st_quan > 50) then 'Average' else
case when (s.st_quan >= 100) then 'Good' end end end as 'Status'
from stock s
inner join products p
on p.pro_id = s.st_proID
inner join categories c
on c.cat_id = p.pro_catID
inner join productPrice pp
on p.pro_id = pp.pp_proID
where 
p.pro_expiry > CURRENT_TIMESTAMP or p.pro_expiry is null
and 
(
p.pro_name like '%'+@data+'%'
or
p.pro_barcode like '%'+@data+'%'
or
c.cat_name like '%'+@data+'%'
)

GO
/****** Object:  StoredProcedure [dbo].[st_getAllStockWRTAboutToProduct]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getAllStockWRTAboutToProduct]
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_packing as 'Packing',
p.pro_barcode as 'Barcode',
format (p.pro_expiry, 'dd-MMM-yyyy') as 'Expiry Date',
pp.pp_buyingPrice as 'Buying Price',
pp.pp_sellingPrice as 'Selling Price',
c.cat_name as 'Category',
s.st_quan as 'Available Stock',
s.st_pack_quan as 'Per Packing Quantity',
pp.pp_buyingPrice * s.st_quan as 'Total Amount',
pp.pp_sellingPrice * s.st_quan as 'Total Amount Sell',
case when (s.st_quan <= 50) then 'Low' else case when (s.st_quan <  100  and s.st_quan > 50) then 'Average' else
case when (s.st_quan >= 100) then 'Good' end end end as 'Status'
from stock s
inner join products p
on p.pro_id = s.st_proID
inner join categories c
on c.cat_id = p.pro_catID
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_expiry < DATEADD(month,3,CURRENT_TIMESTAMP) and p.pro_expiry > CURRENT_TIMESTAMP   
GO
/****** Object:  StoredProcedure [dbo].[st_getAllStockWRTExpiredProduct]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getAllStockWRTExpiredProduct]
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_packing as 'Packing',
p.pro_barcode as 'Barcode',
format (p.pro_expiry, 'dd-MMM-yyyy') as 'Expiry Date',
pp.pp_buyingPrice as 'Buying Price',
pp.pp_sellingPrice as 'Selling Price',
c.cat_name as 'Category',
s.st_quan as 'Available Stock',
s.st_pack_quan as 'Per Packing Quantity',
pp.pp_buyingPrice * s.st_quan as 'Total Amount',
pp.pp_sellingPrice * s.st_quan as 'Total Amount Sell',
case when (s.st_quan <= 50) then 'Low' else case when (s.st_quan <  100  and s.st_quan > 50) then 'Average' else
case when (s.st_quan >= 100) then 'Good' end end end as 'Status'
from stock s
inner join products p
on p.pro_id = s.st_proID
inner join categories c
on c.cat_id = p.pro_catID
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_expiry < CURRENT_TIMESTAMP
GO
/****** Object:  StoredProcedure [dbo].[st_getCategoriesData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getCategoriesData]
as
select
c.cat_id as 'ID',
c.cat_name as 'Category',
case when (c.cat_isActive = 1) then 'Yes' else 'No' end as 'Status'
from categories c
order by c.cat_name asc
GO
/****** Object:  StoredProcedure [dbo].[st_getCategoriesDataList]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getCategoriesDataList]
as
select
c.cat_id as 'ID',
c.cat_name as 'Category'
from categories c
order by c.cat_name asc
GO
/****** Object:  StoredProcedure [dbo].[st_getCategoryDataLike]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getCategoryDataLike]
@data varchar(50)
as
select 
c.cat_id as 'ID',
c.cat_name as 'Category',
case when (cat_isActive = 1) then 'Yes' else 'No' end as 'Status'
from categories c
where 
c.cat_name like '%'+@data+'%'
order by c.cat_name asc
GO
/****** Object:  StoredProcedure [dbo].[st_getCustomerData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getCustomerData]
as
select 
c.cust_id as 'ID',
c.cust_name as 'Customer Name',
c.cust_mob_no as 'Mobile No',
c.cust_address as 'Address'
from customer c order by c.cust_name
GO
/****** Object:  StoredProcedure [dbo].[st_getCustomerDataSearch]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getCustomerDataSearch]
@data varchar(30)
as
select 
c.cust_id as 'ID',
c.cust_name as 'Customer Name',
c.cust_mob_no as 'Mobile No',
c.cust_address as 'Address'
from customer c 
where
c.cust_name like '%'+@data+'%'
or
c.cust_address like '%'+@data+'%'
order by c.cust_name
GO
/****** Object:  StoredProcedure [dbo].[st_getCustomersMoneyRemaining]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getCustomersMoneyRemaining]
as
select distinct
s.sal_id as 'Sales ID',
c.cust_id as 'Customer ID',
s.sal_totAmt as 'Total Amount',
s.sal_totDis as 'Total Discount',
s.sal_amtGiven as 'Amount Given',
s.sal_amountRemaining as 'Amount Remaining',
c.cust_name as 'Customer Name',
c.cust_address as 'Customer Address',
format (s.sal_date, 'dd-MMM-yyyy') as 'Date',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join users u
on u.usr_id = s.sal_doneBy
inner join customer c
on c.cust_id = s.sal_custID
where s.sal_amountRemaining is not null or s.sal_amountRemaining = 0
GO
/****** Object:  StoredProcedure [dbo].[st_getCustomersMoneyRemainingData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getCustomersMoneyRemainingData]
@data varchar(50)
as
select distinct
s.sal_id as 'Sales ID',
c.cust_id as 'Customer ID',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amountRemaining as 'Amount Remaining',
c.cust_name as 'Customer Name',
c.cust_address as 'Customer Address',
format (s.sal_date, 'dd-MMM-yyyy') as 'Date',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join users u
on u.usr_id = s.sal_doneBy
inner join customer c
on c.cust_id = s.sal_custID
where s.sal_amountRemaining is not null
and 
(
s.sal_id like '%'+@data+'%'
or
c.cust_name like '%'+@data+'%'
or
c.cust_address like '%'+@data+'%'
or
u.usr_name like '%'+@data+'%'
)
GO
/****** Object:  StoredProcedure [dbo].[st_getCustommerList]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Procedure [dbo].[st_getCustommerList]
as
select c.cust_id as 'ID',
c.cust_name as 'Customer Name'
from customer c order by c.cust_name
GO
/****** Object:  StoredProcedure [dbo].[st_getDailyExp]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getDailyExp]
as
select 
de.expense_id as 'Expense ID',
de.expense_desc as 'Description',
de.expense_amount as 'Amount',
format (de.expense_date, 'dd-MMM-yyyy hh:mm:ss tt' ) as 'Date'
from daily_expenditure de order by de.expense_date asc
GO
/****** Object:  StoredProcedure [dbo].[st_getEmail]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getEmail]
@email varchar(50)
as
select usr_email as 'EMAIL' from users where usr_email = @email
GO
/****** Object:  StoredProcedure [dbo].[st_getExpID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getExpID]
as
select top 1 de.expense_id as 'Expense ID' from daily_expenditure de order by de.expense_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getIncome]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getIncome]
as
select 
di.income_id as 'Income ID',
di.income_desc as 'Income Desc',
di.income_amount as 'Income Amount',
di.income_date as 'Income Date'
from daily_income di order by di.income_date asc
GO
/****** Object:  StoredProcedure [dbo].[st_getIncomeAmountDate]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getIncomeAmountDate]
@date datetime
as
select
sum (di.income_amount) as 'Total Income Amount'
from daily_income di where convert (date, income_date) = @date
GO
/****** Object:  StoredProcedure [dbo].[st_getIncomeExpenseBalance]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getIncomeExpenseBalance]
as
SELECT CONVERT(date,Inc.income_date) as Date, TotalInc, TotalExp, TotalInc - TotalExp AS ProfitNLoss
FROM
(
  SELECT daily_income.income_date, SUM(daily_income.income_amount) as TotalInc
  FROM daily_income
  GROUP BY daily_income.income_date) as Inc
LEFT OUTER JOIN 
(

  SELECT daily_exp.exp_date, SUM(daily_exp.exp_amount) as TotalExp
  FROM daily_exp
  GROUP BY daily_exp.exp_date) as Exp

ON CONVERT(date,Inc.income_date) = CONVERT(date,Exp.exp_date)

UNION

SELECT CONVERT(date,Exp.exp_date) as Date, TotalInc, TotalExp, TotalInc - TotalExp AS ProfitNLoss
FROM
(
  SELECT daily_income.income_date, SUM(daily_income.income_amount) as TotalInc
  FROM daily_income
  GROUP BY daily_income.income_date) as Inc
RIGHT OUTER JOIN 
(

  SELECT daily_exp.exp_date, SUM(daily_exp.exp_amount) as TotalExp
  FROM daily_exp
  GROUP BY daily_exp.exp_date) as Exp

ON CONVERT(date,Inc.income_date) = CONVERT(date,Exp.exp_date)
GO
/****** Object:  StoredProcedure [dbo].[st_getLastPID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getLastPID] 
as
select 
top 1 pii.pi_id from purchase_invoice pii order by pii.pi_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getProductByBarcode]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getProductByBarcode]
@barcode nvarchar(100)
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_barcode as 'Barcode',
p.pro_packing as 'Packing'
--pp.pp_sellingPrice as 'Selling Price',
--pp.pp_sellingPrice * pp.pp_discount / 100 as 'Discount',
--pp.pp_sellingPrice - (pp.pp_sellingPrice * pp.pp_discount / 100) as 'Final Selling Price'
from products p
--inner join productPrice pp
--on p.pro_id = pp.pp_proID
where p.pro_barcode = @barcode
GO
/****** Object:  StoredProcedure [dbo].[st_getProductBYID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getProductBYID]
@proID bigint 
as
select p.pro_packing as 'Packing' from products p where p.pro_id  =@proID
GO
/****** Object:  StoredProcedure [dbo].[st_getProductCount]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getProductCount]
as
select 
sum(pp.pp_sellingPrice) as 'Selling Price',
sum(pp.pp_buyingPrice) as 'Buying Price',
count(pp.pp_proID) as 'Total Products'
from productPrice pp
GO
/****** Object:  StoredProcedure [dbo].[st_getProductPackQuantity]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getProductPackQuantity]
@proID bigint 
as
select s.st_pack_quan as 'Packing Quantity' from stock s where s.st_proID =@proID
GO
/****** Object:  StoredProcedure [dbo].[st_getProductPrByBarcode]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getProductPrByBarcode]
@barcode nvarchar(100)
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_barcode as 'Barcode',
pp.pp_sellingPrice as 'Selling Price',
pp.pp_sellingPrice * pp.pp_discount / 100 as 'Discount',
pp.pp_sellingPrice - (pp.pp_sellingPrice * pp.pp_discount / 100) as 'Final Selling Price',
p.pro_packing as 'Packing',
p.pro_expiry as 'Expiry',
pp.pp_itemSP as 'Per Item SP'
from products p
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_barcode = @barcode
GO
/****** Object:  StoredProcedure [dbo].[st_getProductPrByName]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getProductPrByName]
@name varchar(50)
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_barcode as 'Barcode',
pp.pp_sellingPrice as 'Selling Price',
pp.pp_sellingPrice * pp.pp_discount / 100 as 'Discount',
pp.pp_sellingPrice - (pp.pp_sellingPrice * pp.pp_discount / 100) as 'Final Selling Price',
p.pro_packing as 'Packing',
p.pro_expiry as 'Expiry',
pp.pp_itemSP as 'Per Item SP'
from products p
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_barcode = @name
GO
/****** Object:  StoredProcedure [dbo].[st_getProductPRodbyID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getProductPRodbyID]
@proID bigint
as
select 
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_barcode as 'Barcode',
pp.pp_sellingPrice as 'Selling Price',
pp.pp_sellingPrice * pp.pp_discount / 100 as 'Discount',
pp.pp_sellingPrice - (pp.pp_sellingPrice * pp.pp_discount / 100) as 'Final Selling Price',
p.pro_packing as 'Packing',
p.pro_expiry as 'Expiry',
pp.pp_itemSP as 'Per Item SP'
from products p
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_id = @proID
GO
/****** Object:  StoredProcedure [dbo].[st_getProductQuantity]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getProductQuantity]
@proID bigint 
as
select s.st_quan as 'Quantity' from stock s where s.st_proID =@proID
GO
/****** Object:  StoredProcedure [dbo].[st_getProductsData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getProductsData]
as
select
p.pro_id as 'Product ID',
p.pro_name as 'Product',
format(p.pro_expiry, 'dd-MMM-yyyy') as 'Expiry',
p.pro_barcode as 'Barcode',
c.cat_name as 'Category',
c.cat_id as 'Category ID',
p.pro_packing as 'Packing'
from products p
inner join categories c on c.cat_id = p.pro_catID
GO
/****** Object:  StoredProcedure [dbo].[st_getProductsDataLike]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getProductsDataLike]
@data varchar(50)
as
select
p.pro_id as 'Product ID',
p.pro_name as 'Product',
format(p.pro_expiry, 'dd-MMM-yyyy') as 'Expiry',
p.pro_barcode as 'Barcode',
c.cat_name as 'Category',
c.cat_id as 'Category ID',
p.pro_packing as 'Packing'
from products p
inner join categories c on c.cat_id = p.pro_catID
where 
p.pro_name like '%'+@data+'%'
or
p.pro_barcode like '%'+@data+'%'
or
p.pro_expiry like '%'+@data+'%'
or
c.cat_name like '%'+@data+'%'
order by p.pro_name, c.cat_name asc
GO
/****** Object:  StoredProcedure [dbo].[st_getProductsWRTCategory]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getProductsWRTCategory]
@catID int
as
select
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_packing as 'Packing',
pp.pp_buyingPrice as 'Buying Price',
case when (pp.pp_sellingPrice is null) then 0 else pp.pp_sellingPrice end as 'Selling Price',
case when (pp.pp_discount is null) then 0 else pp.pp_discount end as 'Discount',
case when (pp.pp_profitPercent is null) then 0 else pp.pp_profitPercent end as 'Profit Percentage',
case when (pp.pp_itemSP is null) then 0 else pp.pp_sellingPrice end as 'Per Item Selling Price'
from products p 
inner join productPrice pp
on p.pro_id = pp.pp_proID
inner join categories c on
c.cat_id = p.pro_catID
where c.cat_id = @catID
GO
/****** Object:  StoredProcedure [dbo].[st_getProductsWRTCategoryData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getProductsWRTCategoryData]
@catID int,
@data varchar(50)
as
select
p.pro_id as 'Product ID',
p.pro_name as 'Product',
p.pro_packing as 'Packing',
pp.pp_buyingPrice as 'Buying Price',
case when (pp.pp_sellingPrice is null) then 0 else pp.pp_sellingPrice end as 'Selling Price',
case when (pp.pp_discount is null) then 0 else pp.pp_discount end as 'Discount',
case when (pp.pp_profitPercent is null) then 0 else pp.pp_profitPercent end as 'Profit Percentage',
case when (pp.pp_itemSP is null) then 0 else pp.pp_sellingPrice end as 'Per Item Selling Price'
from products p 
inner join productPrice pp
on p.pro_id = pp.pp_proID
inner join categories c on
c.cat_id = p.pro_catID
where c.cat_id = @catID
and p.pro_name like '%'+@data+'%'
GO
/****** Object:  StoredProcedure [dbo].[st_getPurchaseInvoiceDetails]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getPurchaseInvoiceDetails]
@pid bigint
as
select
pid.pid_id as 'mPID',
pid.pid_proID as 'Product ID',
p.pro_name as 'Product',
pid.pid_proquan as 'Quantity',
pid.pid_total_amount as 'Total Price',
pp.pp_buyingPrice as 'Per Unit Price'
from purchase_invoice pii
inner join purchase_invoice_details pid 
inner join products p on p.pro_id = pid.pid_proID
on pii.pi_id = pid.pid_purchaseID
inner join productPrice pp on p.pro_id = pp.pp_proID
where pii.pi_id =@pid
GO
/****** Object:  StoredProcedure [dbo].[st_getPurchaseInvoiceList]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getPurchaseInvoiceList]
@month int,
@year int
as
select pii.pi_id as 'ID',
su.sup_company+' '+format(pii.pi_date, 'dd-MMM-yyyy') as 'Company'
from purchase_invoice pii 
inner join supplier su
on su.sup_id = pii.pi_suppID
where month(pii.pi_date) = @month and year(pii.pi_date) = @year
GO
/****** Object:  StoredProcedure [dbo].[st_getRecipt]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getRecipt]
as
select
s.sal_id as 'Sales ID',
p.pro_barcode as 'Barcode',
p.pro_name as 'Product',
sd.sd_quan as 'Quantity',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amtRetunr as 'Amount Returned',
format (s.sal_date, 'dd-MMM-yyyy hh:mm:ss tt') as 'Date',
sd.sd_sellingPrice as 'Product Price',
sd.sd_sellDisc as 'Per Product Discount',
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0 AND dbo.udf_GetNumeric(p.pro_packing) = sd.sd_itemQuan) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0) then sd.sd_sellingPrice*sd.sd_itemQuan else
case when (sd.sd_itemQuan = 0) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan > 1) then sd.sd_sellingPrice*sd.sd_quan end end end end as 'Per Product Total',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join products p
on p.pro_id = sd.sd_ProID
inner join productPrice pp
on pp.pp_proID = p.pro_id
inner join users u
on u.usr_id = s.sal_doneBy
where s.sal_id = (select top 1 ss.sal_id from sales ss order by ss.sal_id desc)
and u.usr_id = 7
order by s.sal_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getReciptWRTID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getReciptWRTID]
@salesID bigint
as
select
s.sal_id as 'Sales ID',
p.pro_barcode as 'Barcode',
p.pro_id as 'Product ID',
p.pro_name as 'Product',
sd.sd_quan as 'Quantity',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amtRetunr as 'Amount Returned',
format (s.sal_date, 'dd-MMM-yyyy hh:mm:ss tt') as 'Date',
sd.sd_sellingPrice as 'Product Price',
sd.sd_sellDisc as 'Per Product Discount',
--dbo.udf_GetNumeric(p.pro_packing) as Packing_Unit,
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0 AND dbo.udf_GetNumeric(p.pro_packing) = sd.sd_itemQuan) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0) then sd.sd_sellingPrice*sd.sd_itemQuan else
case when (sd.sd_itemQuan = 0) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan > 1) then sd.sd_sellingPrice*sd.sd_quan end end end end as 'Per Product Total',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type',
p.pro_packing as 'Packing',
sd.sd_itemQuan as 'Item Quantity',
s.sal_amountRemaining as 'Amount Remaining'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join products p
on p.pro_id = sd.sd_ProID
inner join productPrice pp
on pp.pp_proID = p.pro_id
inner join users u
on u.usr_id = s.sal_doneBy
where s.sal_id = @salesID
order by s.sal_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getReport]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getReport]
as
select 
count(p.pro_name) as 'Product',
sum(s.st_quan) as 'Stock',
sum(pp.pp_buyingPrice * s.st_quan) as 'BP',
sum(pp.pp_sellingPrice * s.st_quan) as 'SP'
from stock s
inner join products p
on p.pro_id = s.st_proID
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_expiry > CURRENT_TIMESTAMP or p.pro_expiry is null
GO
/****** Object:  StoredProcedure [dbo].[st_getReportAbouttoExpireProduct]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getReportAbouttoExpireProduct]
as
select 
count(p.pro_name) as 'Product',
sum(s.st_quan) as 'Stock',
sum(pp.pp_buyingPrice * s.st_quan) as 'BP',
sum(pp.pp_sellingPrice * s.st_quan) as 'SP'
from stock s
inner join products p
on p.pro_id = s.st_proID
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_expiry < DATEADD(month,3,CURRENT_TIMESTAMP) and p.pro_expiry > CURRENT_TIMESTAMP
GO
/****** Object:  StoredProcedure [dbo].[st_getReportExpireProduct]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getReportExpireProduct]
as
select 
count(p.pro_name) as 'Product',
sum(s.st_quan) as 'Stock',
sum(pp.pp_buyingPrice * s.st_quan) as 'BP',
sum(pp.pp_sellingPrice * s.st_quan) as 'SP'
from stock s
inner join products p
on p.pro_id = s.st_proID
inner join productPrice pp
on p.pro_id = pp.pp_proID
where p.pro_expiry < CURRENT_TIMESTAMP
GO
/****** Object:  StoredProcedure [dbo].[st_getSaleDateByID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getSaleDateByID]
@saleId bigint
as
select s.sal_date as 'sale date' from sales s where s.sal_id=@saleId
GO
/****** Object:  StoredProcedure [dbo].[st_getSaleEarning]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSaleEarning]
as
select distinct
sum(s.sal_totAmt) as 'Total Amount',
sum(s.sal_amountRemaining) as 'Total Remaining'
from sales s
GO
/****** Object:  StoredProcedure [dbo].[st_getSaleEarningToday]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSaleEarningToday]
@date date
as
select distinct
sum(s.sal_totAmt) as 'Total Amount',
sum(s.sal_amountRemaining) as 'Total Remaining'
from sales s
where convert (date,s.sal_date) = @date
--and s.sal_amountRemaining is null
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesDailyWise]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesDailyWise]
as
SELECT CAST(s.sal_date AS DATE) [Date], Count(1)  [Sales Count], sum(s.sal_totAmt) [Total Amount]
FROM sales s
GROUP BY CAST(s.sal_date AS DATE)
ORDER BY 1 
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getSalesData]
@saleID bigint
as
select
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtRetunr as 'Amount Returned'
from sales s
where s.sal_id = @saleID
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getSalesID]
as
select top 1 s.sal_id as 'Sales ID' from sales s order by s.sal_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesInvoices]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesInvoices]
@date date
as
select distinct
s.sal_id as 'Sales ID',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amtRetunr as 'Amount Returned',
format (s.sal_date, 'dd-MMM-yyyy hh:mm:ss tt') as 'Date',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type',
u.usr_id as 'User ID'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join users u
on u.usr_id = s.sal_doneBy
where convert (date,s.sal_date) = @date
and s.sal_amountRemaining is null or s.sal_amountRemaining =0
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesInvoicesData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesInvoicesData]
@date date,
@data varchar(50)
as
select distinct
s.sal_id as 'Sales ID',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amtRetunr as 'Amount Returned',
format (s.sal_date, 'dd-MMM-yyyy hh:mm:ss tt') as 'Date',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type',
u.usr_id as 'User ID'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join users u
on u.usr_id = s.sal_doneBy
where convert (date,s.sal_date) = @date
and
(
s.sal_id like '%'+@data+'%'
or
u.usr_name like '%'+@data+'%'
)
and s.sal_amountRemaining is null or s.sal_amountRemaining = 0
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesMonthWise]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesMonthWise]
as
select YEAR(s.sal_date) [Year],
MONTH(s.sal_date) [Month],
DATENAME(MONTH,s.sal_date) [Month Name],
count(1) [sales Count], sum(s.sal_totAmt) [Total Amount] 
from sales s
group by YEAR(s.sal_date), MONTH(s.sal_date), DATENAME(MONTH,s.sal_date) 
order by 1,2
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesRecipt]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesRecipt]
as
select
s.sal_id as 'Sales ID',
p.pro_barcode as 'Barcode',
p.pro_name as 'Product',
sd.sd_quan as 'Quantity',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amtRetunr as 'Amount Returned',
format (s.sal_date, 'dd-MMM-yyyy hh:mm:ss tt') as 'Date',
sd.sd_sellingPrice as 'Product Price',
sd.sd_sellDisc * sd.sd_quan as 'Per Product Discount',
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0 AND dbo.udf_GetNumeric(p.pro_packing) = sd.sd_itemQuan) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0) then sd.sd_sellingPrice*sd.sd_itemQuan else
case when (sd.sd_itemQuan = 0) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan > 1) then sd.sd_sellingPrice*sd.sd_quan end end end end as 'Per Product Total',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type',
sd.sd_itemQuan as 'Units',
s.sal_amountRemaining as 'Amount Remaining',
p.pro_packing as 'Packing'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join products p
on p.pro_id = sd.sd_ProID
inner join productPrice pp
on pp.pp_proID = p.pro_id
inner join users u
on u.usr_id = s.sal_doneBy
where s.sal_id = (select top 1 ss.sal_id from sales ss order by ss.sal_id desc)
and u.usr_id = 7
order by s.sal_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesReciptUser]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesReciptUser] 
@user int
as
select
s.sal_id as 'Sales ID',
p.pro_barcode as 'Barcode',
p.pro_name as 'Product',
sd.sd_quan as 'Quantity',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amtRetunr as 'Amount Returned',
format (s.sal_date, 'dd-MMM-yyyy hh:mm:ss tt') as 'Date',
sd.sd_sellingPrice as 'Product Price',
sd.sd_sellDisc as 'Per Product Discount',
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0 AND dbo.udf_GetNumeric(p.pro_packing) = sd.sd_itemQuan) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0) then sd.sd_sellingPrice*sd.sd_itemQuan else
case when (sd.sd_itemQuan = 0) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan > 1) then sd.sd_sellingPrice*sd.sd_quan end end end end as 'Per Product Total',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type',
sd.sd_itemQuan as 'Units',
s.sal_amountRemaining as 'Amount Remaining',
p.pro_packing as 'Packing'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join products p
on p.pro_id = sd.sd_ProID
inner join productPrice pp
on pp.pp_proID = p.pro_id
inner join users u
on u.usr_id = s.sal_doneBy
where s.sal_id = (select top 1 ss.sal_id from sales ss order by ss.sal_id desc)
and u.usr_id = @user
order by s.sal_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesReciptUserSaleID]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesReciptUserSaleID]
@saleiD bigint
as
select
s.sal_id as 'Sales ID',
p.pro_barcode as 'Barcode',
p.pro_name as 'Product',
sd.sd_quan as 'Quantity',
s.sal_totDis as 'Total Discount',
s.sal_totAmt as 'Total Amount',
s.sal_amtGiven as 'Amount Given',
s.sal_amtRetunr as 'Amount Returned',
format (s.sal_date, 'dd-MMM-yyyy hh:mm:ss tt') as 'Date',
sd.sd_sellingPrice as 'Product Price',
sd.sd_sellDisc as 'Per Product Discount',
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0 AND dbo.udf_GetNumeric(p.pro_packing) = sd.sd_itemQuan) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan = 1 AND sd.sd_itemQuan > 0) then sd.sd_sellingPrice*sd.sd_itemQuan else
case when (sd.sd_itemQuan = 0) then sd.sd_sellingPrice*sd.sd_quan else
case when (sd.sd_quan > 1) then sd.sd_sellingPrice*sd.sd_quan end end end end as 'Per Product Total',
u.usr_name as 'User',
case when (s.sal_payType = 0) then 'Cash' else
case when (s.sal_payType = 1) then 'Debit Card' else
case when (s.sal_payType = 2) then 'Credit Card' end end end as 'Payment Type',
sd.sd_itemQuan as 'Units',
s.sal_amountRemaining as 'Amount Remaining',
p.pro_packing as 'Packing'
from sales s
inner join SaleDetails sd
on s.sal_id = sd.sd_SalID
inner join products p
on p.pro_id = sd.sd_ProID
inner join productPrice pp
on pp.pp_proID = p.pro_id
inner join users u
on u.usr_id = s.sal_doneBy
where s.sal_id = @saleiD
order by s.sal_id desc
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesStatsWRTUser]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getSalesStatsWRTUser]
@year int,
@month int
as
select YEAR(s.sal_date) [Year],
MONTH(s.sal_date) [Month],
DATENAME(MONTH,s.sal_date) [Month Name],
count(1) [sales Count], sum(s.sal_totAmt) [Total Amount] 
from sales s
where year(s.sal_date) = @year
and month(s.sal_date) = @month
group by YEAR(s.sal_date), MONTH(s.sal_date), DATENAME(MONTH,s.sal_date) 
order by 1,2
GO
/****** Object:  StoredProcedure [dbo].[st_getSalesYearWise]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSalesYearWise]
as
SELECT YEAR(s.sal_date) [Year], Count(1) [Sales Count], sum(s.sal_totAmt) [Total Amount]  
FROM sales s
GROUP BY YEAR(s.sal_date)
ORDER BY 1
GO
/****** Object:  StoredProcedure [dbo].[st_getSupplierCDB]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSupplierCDB]
as
select distinct
pid.pid_id as 'mPID',
sup.sup_id as 'Supplier ID',
sum(pii.pi_total_price) as 'Total Price',
sup.sup_company as 'Supplier Company',
sup.sup_address as 'Supplier Address',
pii.pi_amount_given as 'Amount Given',
pii.pi_amount_remain as 'Amount Remaining',
format (pii.pi_date, 'dd-MMM-yyyy') as 'Date'
from purchase_invoice pii
inner join purchase_invoice_details pid 
inner join products p on p.pro_id = pid.pid_proID
on pii.pi_id = pid.pid_purchaseID
inner join productPrice pp on p.pro_id = pp.pp_proID
inner join supplier sup on pii.pi_suppID = sup.sup_id
group by pid.pid_id, sup.sup_id, sup.sup_company,
sup.sup_address, pii.pi_amount_given, pii.pi_amount_remain, pii.pi_date
GO
/****** Object:  StoredProcedure [dbo].[st_getSupplierData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE procedure [dbo].[st_getSupplierData]
 as
 select 
s.sup_id as 'ID',
s.sup_company as 'Company',
s.sup_contactPerson as 'Contact Person',
s.sup_mobile as 'Mobile',
s.sup_address as 'Address',
case when (s.sup_status = 1) then 'Active' else 'In-Active' end as 'Status'
 from supplier s 
 order by s.sup_company asc
GO
/****** Object:  StoredProcedure [dbo].[st_getSupplierDataLike]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE procedure [dbo].[st_getSupplierDataLike]
 @data varchar(50)
 as
 select 
s.sup_id as 'ID',
s.sup_company as 'Company',
s.sup_contactPerson as 'Contact Person',
s.sup_mobile as 'Mobile',
s.sup_address as 'Address',
case when (s.sup_status = 1) then 'Active' else 'In-Active' end as 'Status'
 from supplier s 
 where 
s.sup_company like '%'+@data+'%'
or
s.sup_contactPerson like '%'+@data+'%'
or
s.sup_mobile like '%'+@data+'%'
or
s.sup_address like '%'+@data+'%'
 order by s.sup_company asc
GO
/****** Object:  StoredProcedure [dbo].[st_getSupplierList]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getSupplierList]
as
select 
s.sup_id as 'ID',
s.sup_company as 'Company'
 from supplier s where s.sup_status = 1
 order by s.sup_company asc
GO
/****** Object:  StoredProcedure [dbo].[st_getSupplierPurchaseCDB]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSupplierPurchaseCDB]
as
select
pii.pi_id as 'Purchase Invoice ID',
sup.sup_id as 'Supplier ID',
sup.sup_company as 'Supplier Company',
sup.sup_address as 'Supplier Address',
pii.pi_total_price as 'Total Amount',
pii.pi_amount_given as 'Amount Given',
pii.pi_amount_remain as 'Amount Remaining',
format (pii.pi_date, 'dd-MMM-yyyy') as 'Date'
from purchase_invoice pii
inner join supplier sup
on pii.pi_suppID = sup.sup_id
GO
/****** Object:  StoredProcedure [dbo].[st_getSupplierPurchaseCDBSearch]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getSupplierPurchaseCDBSearch]
@data varchar(50)
as
select
pii.pi_id as 'Purchase Invoice ID',
sup.sup_id as 'Supplier ID',
sup.sup_company as 'Supplier Company',
sup.sup_address as 'Supplier Address',
pii.pi_total_price as 'Total Amount',
pii.pi_amount_given as 'Amount Given',
pii.pi_amount_remain as 'Amount Remaining',
format (pii.pi_date, 'dd-MMM-yyyy') as 'Date'
from purchase_invoice pii
inner join supplier sup
on pii.pi_suppID = sup.sup_id
where
sup.sup_company like '%'+@data+'%'
or
sup.sup_address like '%'+@data+'%'
or
pii.pi_id like '%'+@data+'%'
GO
/****** Object:  StoredProcedure [dbo].[st_getTotalStockQuantity]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getTotalStockQuantity]
as
select sum(s.st_quan) as 'Total Stock'
from stock s
GO
/****** Object:  StoredProcedure [dbo].[st_getUserData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getUserData]
as
select 
u.usr_id as 'ID',
u.usr_name as 'NAME',
u.usr_username as 'USERNAME',
u.usr_password as 'PASSWORD',
u.usr_phone as 'PHONE',
u.usr_email as 'Email',
case when (usr_status = 1) then 'Active' else 'In-Active' end as 'Status'
from users u order by u.usr_name asc
GO
/****** Object:  StoredProcedure [dbo].[st_getUserDataLike]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_getUserDataLike]
@data varchar(50)
as
select 
u.usr_id as 'ID',
u.usr_name as 'NAME',
u.usr_username as 'USERNAME',
u.usr_password as 'PASSWORD',
u.usr_phone as 'PHONE',
u.usr_email as 'Email',
case when (usr_status = 1) then 'Active' else 'In-Active' end as 'Status'
from users u 
where 
u.usr_name like '%'+@data+'%'
or
u.usr_username like '%'+@data+'%'
or
u.usr_phone like '%'+@data+'%'
or
u.usr_email like '%'+@data+'%'
order by u.usr_name asc
GO
/****** Object:  StoredProcedure [dbo].[st_getUserDetails]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_getUserDetails]
@user varchar(30),
@pass varchar(30)
as
select
u.usr_id as 'ID',
u.usr_name as 'Name',
u.usr_username as 'Username',
u.usr_password as 'Password',
u.usr_email as 'Email'
from users u
where u.usr_username = @user and u.usr_password = @pass
GO
/****** Object:  StoredProcedure [dbo].[st_gtExpAmount]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_gtExpAmount]
@date datetime
as
select de.exp_amount as 'Amount' from daily_exp de where convert (date,de.exp_date)= @date
GO
/****** Object:  StoredProcedure [dbo].[st_insertCategory]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_insertCategory]
@name varchar(50),
@isActive tinyint
as
insert into categories values
(@name,@isActive)
GO
/****** Object:  StoredProcedure [dbo].[st_insertCustomer]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_insertCustomer]
@name varchar(30),
@mob_no varchar(20),
@address varchar(50)
as
insert into customer values(@name,@mob_no,@address)
GO
/****** Object:  StoredProcedure [dbo].[st_insertCustomers]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_insertCustomers]
@name varchar(30),
@mobile varchar(20),
@address varchar(50)
as
insert into customer values (@name,@mobile,@address)
GO
/****** Object:  StoredProcedure [dbo].[st_insertDailyExp]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_insertDailyExp]
@desc varchar(200),
@amount money,
@date datetime
as
insert into daily_expenditure values (@desc,@amount,@date)
GO
/****** Object:  StoredProcedure [dbo].[st_insertDeletedItemPI]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_insertDeletedItemPI]
@pi bigint,
@usrID int,
@proID bigint,
@quan int,
@date date
as
insert into trackDeletedItemPi values (@pi,@usrID,@proID,@quan,@date)
GO
/****** Object:  StoredProcedure [dbo].[st_insertEXP]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_insertEXP]
@expID bigint,
@amount money,
@date datetime
as
insert into daily_exp values (@expID,@amount,@date)
GO
/****** Object:  StoredProcedure [dbo].[st_insertIncome]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_insertIncome]
@desc varchar(200),
@amount money,
@date datetime
as
insert into daily_income values (@desc,@amount,@date)
GO
/****** Object:  StoredProcedure [dbo].[st_insertIncomeFSales]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_insertIncomeFSales]
@amount money,
@date datetime
as
insert into daily_income (income_amount, income_date) values (@amount,@date)
GO
/****** Object:  StoredProcedure [dbo].[st_insertProductPrice]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_insertProductPrice]
@proID bigint,
@bp money
as
insert into productPrice(pp_proID,pp_buyingPrice) values (@proID,@bp)
GO
/****** Object:  StoredProcedure [dbo].[st_insertRefundReturn]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_insertRefundReturn]
@saleID bigint,
@date datetime,
@doneby int,
@proID bigint,
@quan tinyint,
@amount money
as
insert into refund_return values (@saleID,@date,@doneby,@proID,@quan,@amount)
GO
/****** Object:  StoredProcedure [dbo].[st_insertSalesDetails]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_insertSalesDetails]
@saleID bigint,
@proID bigint,
@quan int,
@sp money,
@disc float,
@itemQuan int
as 
insert into SaleDetails values(@saleID,@proID,@quan,@sp,@disc,@itemQuan) 
GO
/****** Object:  StoredProcedure [dbo].[st_insertStock]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_insertStock]
@proID bigint,
@quan int,
@pack_quan int
as
insert into stock values (@proID,@quan,@pack_quan)
GO
/****** Object:  StoredProcedure [dbo].[st_insertSupplier]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_insertSupplier]
@company varchar(100),
@conPerson varchar(50),
@mobile varchar(15),
@address nvarchar(100),
@status tinyint
as
insert into supplier values (@company,@conPerson,@mobile,@address,@status)
GO
/****** Object:  StoredProcedure [dbo].[st_insertUsers]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_insertUsers]
@name varchar(40),
@username varchar(30),
@pwd varchar(30),
@phone varchar(15),
@email varchar(50),
@status tinyint
as
insert into users values (@name,@username,@pwd,@phone,@email,@status)
GO
/****** Object:  StoredProcedure [dbo].[st_productInsert]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_productInsert]
@name varchar(50),
@barcode nvarchar(100),
@expiry date,
@catID int,
@pack nvarchar(20)
as insert into products values (@name,@barcode,@expiry,@catID,@pack)
GO
/****** Object:  StoredProcedure [dbo].[st_productUpdate]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_productUpdate]
@name varchar(50),
@barcode nvarchar(100),
@expiry date,
@catID int,
@pack nvarchar(20),
@id bigint
as
update products
set
pro_name = @name,
pro_barcode = @barcode,
pro_expiry = @expiry,
pro_catID = @catID,
pro_packing = @pack
where pro_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_purchaseInvoice_Insert]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_purchaseInvoice_Insert]
@date date,
@doneBy int,
@suppID int,
@total_amount money,
@amount_given money,
@amount_remain money
as
insert into purchase_invoice values (@date,@doneBy,@suppID,@total_amount,@amount_given,@amount_remain)
GO
/****** Object:  StoredProcedure [dbo].[st_PurchaseInvoiceDetails]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_PurchaseInvoiceDetails]
@purchaseID bigint,
@proID int,
@quan int,
@amount money
as
insert into purchase_invoice_details values (@purchaseID, @proID, @quan,@amount)
GO
/****** Object:  StoredProcedure [dbo].[st_updateCategory]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateCategory]
@name varchar(50),
@isActive tinyint,
@id int
as
update categories
set
cat_name = @name,
cat_isActive = @isActive
where cat_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_updateCustomer]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateCustomer]
@name varchar(30),
@mob_no varchar(20),
@address varchar(50),
@id bigint
as
update customer set 
cust_name =@name,
cust_mob_no =@mob_no,
cust_address =@address
where cust_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_updateCustomers]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateCustomers]
@name varchar(30),
@mobile varchar(20),
@address varchar(50),
@id bigint
as
update customer set
cust_name=@name, cust_mob_no=@mobile, cust_address=@address
where cust_id=@id
GO
/****** Object:  StoredProcedure [dbo].[st_updateDailyExp]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateDailyExp]
@desc varchar(200),
@amount money,
@date datetime,
@id bigint
as
update daily_expenditure set 
expense_desc =@desc,
expense_amount =@amount,
expense_date =@date
 where expense_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_updateEXP]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateEXP]
@amount money,
@date datetime
as
update daily_exp set 
exp_amount = @amount
where 
convert (date, exp_date) = @date
GO
/****** Object:  StoredProcedure [dbo].[st_updateIncome]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateIncome]
@desc varchar (200),
@amount money,
@date datetime,
@id bigint
as 
update daily_income set
income_desc = @desc,
income_amount =@amount,
income_date =@date
where income_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_updateIncomeFSales]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateIncomeFSales]
@amount money,
@date datetime
as
update daily_income set 
income_amount =  @amount
where convert (date, income_date) = @date
GO
/****** Object:  StoredProcedure [dbo].[st_updateProductPrice]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateProductPrice]
@proID bigint,
@bp money,
@sp money,
@dis float,
@profPer float,
@itemSp money
as
update productPrice
set
pp_buyingPrice = @bp,
pp_sellingPrice = @sp,
pp_discount = @dis,
pp_profitPercent = @profPer,
pp_itemSP = @itemSp
where pp_proID = @proID
GO
/****** Object:  StoredProcedure [dbo].[st_updateProductPrice1]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateProductPrice1]
@proID bigint,
@bp money
as
update productPrice
set
pp_buyingPrice = @bp
where pp_proID = @proID
GO
/****** Object:  StoredProcedure [dbo].[st_updatePurchaseSuppCDB]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updatePurchaseSuppCDB]
@amountRemain float,
@amountGiven float,
@id bigint
as
update purchase_invoice
set
pi_amount_given =@amountGiven,
pi_amount_remain = @amountRemain
where pi_id = @id
GO
/****** Object:  StoredProcedure [dbo].[st_updateQuantityinSalesDetails]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateQuantityinSalesDetails]
@quan tinyint,
@saleID bigint
as
update SaleDetails
set
sd_quan =@quan
where sd_SalID = @saleID
GO
/****** Object:  StoredProcedure [dbo].[st_updateQuantitySalesDetails]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateQuantitySalesDetails]
@quan tinyint,
@saleID bigint,
@proID bigint
as
update SaleDetails
set
sd_quan =@quan
where sd_SalID = @saleID and sd_ProID = @proID
GO
/****** Object:  StoredProcedure [dbo].[st_updateSaleAmount]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateSaleAmount]
@totalamount float,
@amountReturn float,
@saleID bigint
as
update sales
set
sal_totAmt = @totalamount,
sal_amtRetunr = @amountReturn
where sal_id = @saleID
GO
/****** Object:  StoredProcedure [dbo].[st_updateSaleEarning]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateSaleEarning]
@amountRemain float,
@amountGiven float,
@amountReturn float,
@id bigint,
@saleID bigint
as
update sales
set
sal_amountRemaining = @amountRemain,
sal_amtGiven = @amountGiven,
sal_amtRetunr = @amountReturn
where sal_id = @saleID and sal_custID =@id
GO
/****** Object:  StoredProcedure [dbo].[st_updateSalesData]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateSalesData]
@saleID bigint,
@totalAmount float,
@amountReturn float
as
update sales
set
sal_totAmt =@totalAmount,
sal_amtRetunr =@amountReturn
where sal_id =@saleID
GO
/****** Object:  StoredProcedure [dbo].[st_updateSalesDataRemain]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[st_updateSalesDataRemain]
@saleID bigint,
@totalAmount float,
@amountRemain float
as
update sales
set
sal_totAmt =@totalAmount,
sal_amountRemaining =@amountRemain
where sal_id =@saleID
GO
/****** Object:  StoredProcedure [dbo].[st_updateSalesReturn]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateSalesReturn]
@saleID bigint,
@proID bigint,
@quantity int,
@itemQty int
as
update SaleDetails
set
sd_quan = @quantity,
sd_itemQuan =@itemQty
where sd_SalID =@saleID and sd_ProID =@proID
GO
/****** Object:  StoredProcedure [dbo].[st_updateStock]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateStock]
@quan int,
@pack_quan int,
@proID int
as 
update stock
set 
st_quan = @quan,
st_pack_quan =@pack_quan
where st_proID =@proID
GO
/****** Object:  StoredProcedure [dbo].[st_updateSupplier]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateSupplier]
@company varchar(100),
@conPerson varchar(50),
@mobile varchar(15),
@address nvarchar(100),
@status tinyint,
@suppID int
as 
update supplier
set
sup_company = @company,
sup_contactPerson = @conPerson,
sup_mobile = @mobile,
sup_address = @address,
sup_status = @status
where sup_id = @suppID
GO
/****** Object:  StoredProcedure [dbo].[st_updateUsers]    Script Date: 8/29/2020 12:21:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[st_updateUsers]
@name varchar(40),
@username varchar(30),
@pwd varchar(30),
@phone varchar(15),
@email varchar(50),
@status tinyint,
@id int
as
update users 
set
usr_name = @name,
usr_username = @username,
usr_password = @pwd,
usr_phone = @phone,
usr_email = @email,
usr_status = @status
where usr_id = @id
GO
