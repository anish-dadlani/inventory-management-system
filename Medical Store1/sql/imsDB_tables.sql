USE [imsDB]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[categories](
	[cat_id] [int] IDENTITY(1,1) NOT NULL,
	[cat_name] [varchar](50) NOT NULL,
	[cat_isActive] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[customer]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[customer](
	[cust_id] [bigint] IDENTITY(1,1) NOT NULL,
	[cust_name] [varchar](30) NULL,
	[cust_mob_no] [varchar](20) NULL,
	[cust_address] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[cust_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[daily_exp]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[daily_exp](
	[FK_exp_id] [bigint] NULL,
	[exp_amount] [money] NULL,
	[exp_date] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[daily_expenditure]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[daily_expenditure](
	[expense_id] [bigint] IDENTITY(1,1) NOT NULL,
	[expense_desc] [varchar](200) NOT NULL,
	[expense_amount] [money] NOT NULL,
	[expense_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[expense_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[daily_income]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[daily_income](
	[income_id] [bigint] IDENTITY(1,1) NOT NULL,
	[income_desc] [varchar](200) NULL,
	[income_amount] [money] NOT NULL,
	[income_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[income_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[productPrice]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[productPrice](
	[pp_proID] [bigint] NOT NULL,
	[pp_buyingPrice] [money] NOT NULL,
	[pp_sellingPrice] [money] NULL,
	[pp_discount] [float] NULL,
	[pp_profitPercent] [float] NULL,
	[pp_itemSP] [money] NULL,
 CONSTRAINT [UQ__productP__91A6C905A212BE60] UNIQUE NONCLUSTERED 
(
	[pp_proID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[products]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[products](
	[pro_id] [bigint] IDENTITY(1,1) NOT NULL,
	[pro_name] [varchar](50) NOT NULL,
	[pro_barcode] [nvarchar](100) NOT NULL,
	[pro_expiry] [date] NULL,
	[pro_catID] [int] NOT NULL,
	[pro_packing] [nvarchar](20) NULL,
 CONSTRAINT [PK__products__335E4CA6DDC403E8] PRIMARY KEY CLUSTERED 
(
	[pro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [barcodeUnique] UNIQUE NONCLUSTERED 
(
	[pro_barcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [productUnique] UNIQUE NONCLUSTERED 
(
	[pro_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[purchase_invoice]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[purchase_invoice](
	[pi_id] [bigint] IDENTITY(1,1) NOT NULL,
	[pi_date] [date] NOT NULL,
	[pi_doneBy] [int] NOT NULL,
	[pi_suppID] [int] NOT NULL,
	[pi_total_price] [money] NULL,
	[pi_amount_given] [money] NULL,
	[pi_amount_remain] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[pi_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[purchase_invoice_details]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[purchase_invoice_details](
	[pid_id] [bigint] IDENTITY(1,1) NOT NULL,
	[pid_purchaseID] [bigint] NOT NULL,
	[pid_proID] [int] NOT NULL,
	[pid_proquan] [int] NOT NULL,
	[pid_total_amount] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[pid_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[refund_return]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[refund_return](
	[ref_salesID] [bigint] NOT NULL,
	[ref_date] [datetime] NOT NULL,
	[ref_doneBy] [int] NOT NULL,
	[ref_proID] [bigint] NOT NULL,
	[ref_quantity] [tinyint] NOT NULL,
	[ref_amount] [money] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaleDetails]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleDetails](
	[sd_SalID] [bigint] NOT NULL,
	[sd_ProID] [bigint] NOT NULL,
	[sd_quan] [int] NULL,
	[sd_sellingPrice] [money] NULL,
	[sd_sellDisc] [float] NULL,
	[sd_itemQuan] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sales]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sales](
	[sal_id] [bigint] IDENTITY(1,1) NOT NULL,
	[sal_doneBy] [int] NOT NULL,
	[sal_date] [datetime] NOT NULL,
	[sal_totAmt] [float] NOT NULL,
	[sal_totDis] [float] NOT NULL,
	[sal_amtGiven] [float] NOT NULL,
	[sal_amtRetunr] [float] NULL,
	[sal_payType] [tinyint] NOT NULL,
	[sal_amountRemaining] [float] NULL,
	[sal_custID] [bigint] NULL,
 CONSTRAINT [PK__sales__FEF11768C7A331E4] PRIMARY KEY CLUSTERED 
(
	[sal_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[stock]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stock](
	[st_proID] [bigint] NOT NULL,
	[st_quan] [int] NOT NULL,
	[st_pack_quan] [int] NULL,
 CONSTRAINT [UQ__stock__89BC7A310C633DD5] UNIQUE NONCLUSTERED 
(
	[st_proID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[supplier]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[supplier](
	[sup_id] [int] IDENTITY(1,1) NOT NULL,
	[sup_company] [varchar](100) NOT NULL,
	[sup_contactPerson] [varchar](50) NOT NULL,
	[sup_mobile] [varchar](15) NOT NULL,
	[sup_address] [nvarchar](100) NOT NULL,
	[sup_status] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[sup_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[sup_company] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[trackDeletedItemPi]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trackDeletedItemPi](
	[pi_id] [bigint] NOT NULL,
	[usr_id] [int] NOT NULL,
	[pro_id] [bigint] NOT NULL,
	[pro_quan] [int] NOT NULL,
	[del_date] [date] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users]    Script Date: 8/29/2020 12:20:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[users](
	[usr_id] [int] IDENTITY(1,1) NOT NULL,
	[usr_name] [varchar](40) NOT NULL,
	[usr_username] [varchar](30) NOT NULL,
	[usr_password] [varchar](30) NOT NULL,
	[usr_phone] [varchar](15) NOT NULL,
	[usr_email] [varchar](50) NOT NULL,
	[usr_status] [tinyint] NOT NULL DEFAULT ((1)),
PRIMARY KEY CLUSTERED 
(
	[usr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [emailUnique] UNIQUE NONCLUSTERED 
(
	[usr_email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_users] UNIQUE NONCLUSTERED 
(
	[usr_username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [phoneUnique] UNIQUE NONCLUSTERED 
(
	[usr_phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[productPrice]  WITH CHECK ADD  CONSTRAINT [FK__productPr__pp_pr__0880433F] FOREIGN KEY([pp_proID])
REFERENCES [dbo].[products] ([pro_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[productPrice] CHECK CONSTRAINT [FK__productPr__pp_pr__0880433F]
GO
ALTER TABLE [dbo].[products]  WITH CHECK ADD  CONSTRAINT [FK__products__pro_ca__40C49C62] FOREIGN KEY([pro_catID])
REFERENCES [dbo].[categories] ([cat_id])
GO
ALTER TABLE [dbo].[products] CHECK CONSTRAINT [FK__products__pro_ca__40C49C62]
GO
ALTER TABLE [dbo].[purchase_invoice_details]  WITH CHECK ADD FOREIGN KEY([pid_purchaseID])
REFERENCES [dbo].[purchase_invoice] ([pi_id])
GO
ALTER TABLE [dbo].[refund_return]  WITH CHECK ADD  CONSTRAINT [FK__refund_re__ref_s__3335971A] FOREIGN KEY([ref_salesID])
REFERENCES [dbo].[sales] ([sal_id])
GO
ALTER TABLE [dbo].[refund_return] CHECK CONSTRAINT [FK__refund_re__ref_s__3335971A]
GO
ALTER TABLE [dbo].[SaleDetails]  WITH CHECK ADD  CONSTRAINT [FK__SaleDetai__sd_Sa__7BE56230] FOREIGN KEY([sd_SalID])
REFERENCES [dbo].[sales] ([sal_id])
GO
ALTER TABLE [dbo].[SaleDetails] CHECK CONSTRAINT [FK__SaleDetai__sd_Sa__7BE56230]
GO
ALTER TABLE [dbo].[trackDeletedItemPi]  WITH CHECK ADD  CONSTRAINT [FK__trackDele__pro_i__4E53A1AA] FOREIGN KEY([pro_id])
REFERENCES [dbo].[products] ([pro_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[trackDeletedItemPi] CHECK CONSTRAINT [FK__trackDele__pro_i__4E53A1AA]
GO
