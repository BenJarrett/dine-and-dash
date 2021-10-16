USE [master]
GO
/****** Object:  Database [DineAndDash]    Script Date: 10/16/2021 10:45:53 AM ******/
CREATE DATABASE [DineAndDash]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DineAndDash', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DineAndDash.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DineAndDash_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DineAndDash_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DineAndDash] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DineAndDash].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DineAndDash] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DineAndDash] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DineAndDash] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DineAndDash] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DineAndDash] SET ARITHABORT OFF 
GO
ALTER DATABASE [DineAndDash] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DineAndDash] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DineAndDash] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DineAndDash] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DineAndDash] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DineAndDash] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DineAndDash] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DineAndDash] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DineAndDash] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DineAndDash] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DineAndDash] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DineAndDash] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DineAndDash] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DineAndDash] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DineAndDash] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DineAndDash] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DineAndDash] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DineAndDash] SET RECOVERY FULL 
GO
ALTER DATABASE [DineAndDash] SET  MULTI_USER 
GO
ALTER DATABASE [DineAndDash] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DineAndDash] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DineAndDash] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DineAndDash] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DineAndDash] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DineAndDash', N'ON'
GO
ALTER DATABASE [DineAndDash] SET QUERY_STORE = OFF
GO
USE [DineAndDash]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 10/16/2021 10:45:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [uniqueidentifier] NOT NULL,
	[userId] [uniqueidentifier] NOT NULL,
	[totalCost] [decimal](18, 2) NOT NULL,
	[paymentId] [uniqueidentifier] NOT NULL,
	[completed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 10/16/2021 10:45:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[id] [uniqueidentifier] NOT NULL,
	[type] [varchar](25) NULL,
	[userId] [uniqueidentifier] NULL,
	[accountNumber] [numeric](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductOrders]    Script Date: 10/16/2021 10:45:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductOrders](
	[id] [uniqueidentifier] NOT NULL,
	[orderId] [uniqueidentifier] NOT NULL,
	[productId] [uniqueidentifier] NOT NULL,
	[productQuantity] [numeric](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10/16/2021 10:45:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[id] [uniqueidentifier] NOT NULL,
	[type] [int] NOT NULL,
	[productName] [varchar](50) NOT NULL,
	[productDescription] [varchar](800) NOT NULL,
	[price] [decimal](18, 2) NOT NULL,
	[quantity] [numeric](18, 0) NOT NULL,
	[forSale] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/16/2021 10:45:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [uniqueidentifier] NOT NULL,
	[firstName] [varchar](20) NOT NULL,
	[lastName] [varchar](50) NOT NULL,
	[customerCreated] [datetime] NOT NULL,
	[contactEmail] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[ProductOrders] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Payments] FOREIGN KEY([paymentId])
REFERENCES [dbo].[Payments] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Payments]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Users]
GO
ALTER TABLE [dbo].[ProductOrders]  WITH CHECK ADD  CONSTRAINT [FK_ProductOrders_Orders] FOREIGN KEY([orderId])
REFERENCES [dbo].[Orders] ([id])
GO
ALTER TABLE [dbo].[ProductOrders] CHECK CONSTRAINT [FK_ProductOrders_Orders]
GO
ALTER TABLE [dbo].[ProductOrders]  WITH CHECK ADD  CONSTRAINT [FK_ProductOrders_Trucks] FOREIGN KEY([productId])
REFERENCES [dbo].[Products] ([id])
GO
ALTER TABLE [dbo].[ProductOrders] CHECK CONSTRAINT [FK_ProductOrders_Trucks]
GO
USE [master]
GO
ALTER DATABASE [DineAndDash] SET  READ_WRITE 
GO
