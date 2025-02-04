﻿USE [master]
GO
/****** Object:  Database [OCP_Import]    Script Date: 10/1/2020 9:43:41 AM ******/
CREATE DATABASE [OCP_Import]
 CONTAINMENT = NONE
 ON  PRIMARY 
GO
ALTER DATABASE [OCP_Import] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OCP_Import].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OCP_Import] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OCP_Import] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OCP_Import] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OCP_Import] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OCP_Import] SET ARITHABORT OFF 
GO
ALTER DATABASE [OCP_Import] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OCP_Import] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OCP_Import] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OCP_Import] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OCP_Import] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OCP_Import] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OCP_Import] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OCP_Import] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OCP_Import] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OCP_Import] SET  DISABLE_BROKER 
GO
ALTER DATABASE [OCP_Import] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OCP_Import] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OCP_Import] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OCP_Import] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OCP_Import] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OCP_Import] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OCP_Import] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OCP_Import] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [OCP_Import] SET  MULTI_USER 
GO
ALTER DATABASE [OCP_Import] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OCP_Import] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OCP_Import] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OCP_Import] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [OCP_Import] SET DELAYED_DURABILITY = DISABLED 
GO
USE [OCP_Import]
GO
/****** Object:  Table [dbo].[tblSchedulerHistory]    Script Date: 10/1/2020 9:43:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSchedulerHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SellerId] [int] NOT NULL,
	[Run_at] [datetime] NOT NULL,
 CONSTRAINT [PK_tblSchedulerHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblSchedulerSettings]    Script Date: 10/1/2020 9:43:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSchedulerSettings](
	[SettingId] [int] IDENTITY(1,1) NOT NULL,
	[FtpHost] [varchar](10) NOT NULL,
	[FtpUserName] [nvarchar](100) NOT NULL,
	[FtpPassword] [nvarchar](1000) NOT NULL,
	[FtpPort] [nvarchar](10) NOT NULL,
	[FtpFilePath] [nvarchar](max) NOT NULL,
	[SyncTime] [nvarchar](20) NOT NULL,
	[Brand] [nvarchar](200) NOT NULL,
	[SellerId] [int] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
 CONSTRAINT [PK_tblSchedulerSettings] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSeller]    Script Date: 10/1/2020 9:43:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSeller](
	[SellerId] [int] IDENTITY(1,1) NOT NULL,
	[ShopifyAccessToken] [nvarchar](max) NOT NULL,
	[MyShopifyDomain] [nvarchar](max) NOT NULL,
	[ShopifyChargeId] [bigint] NOT NULL,
	[ShopName] [nvarchar](300) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[TimezoneOffset] [nvarchar](50) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[InstallStatus] [nvarchar](50) NOT NULL,
	[UnInstallDateTime] [datetime] NOT NULL,
	[ShopDomain] [nvarchar](200) NOT NULL,
	[Host] [varchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[SellerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[tblSchedulerHistory]  WITH CHECK ADD FOREIGN KEY([SellerId])
REFERENCES [dbo].[tblSeller] ([SellerId])
GO
ALTER TABLE [dbo].[tblSchedulerSettings]  WITH CHECK ADD  CONSTRAINT [FK__tblSchedu__Selle__164452B1] FOREIGN KEY([SellerId])
REFERENCES [dbo].[tblSeller] ([SellerId])
GO
ALTER TABLE [dbo].[tblSchedulerSettings] CHECK CONSTRAINT [FK__tblSchedu__Selle__164452B1]
GO
USE [master]
GO
ALTER DATABASE [OCP_Import] SET  READ_WRITE 
GO
