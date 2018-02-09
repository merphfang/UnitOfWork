/****** Object:  Table [dbo].[Customer]    Script Date: 2/9/2018 2:12:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Address1] [nvarchar](200) NOT NULL,
	[Address2] [nvarchar](200) NULL,
	[City] [nvarchar](50) NULL,
	[Province] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL CONSTRAINT [DF_Customer_Country]  DEFAULT (N'CANADA'),
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


