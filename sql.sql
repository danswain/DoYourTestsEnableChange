USE [PayMore]
GO

/****** Object:  Table [dbo].[PendingTransaction]    Script Date: 06/18/2014 17:58:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PendingTransaction](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SessionId] [uniqueidentifier] NOT NULL,
	[ProductName] [text] NULL,
	[Price] [varchar](50) NULL,
	[Quantity] [varchar](50) NULL,
	[ReturnUrl] [text] NULL,
	[CancelUrl] [text] NULL,
 CONSTRAINT [PK_PendingTransaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


