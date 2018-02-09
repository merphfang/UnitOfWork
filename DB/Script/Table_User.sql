/****** Object:  Table [dbo].[User]    Script Date: 2/9/2018 2:13:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Roles] [nvarchar](200) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_User_IsActive]  DEFAULT ((1)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_User_CreatedDate]  DEFAULT (getdate()),
	[UpdatedDate] [datetime] NOT NULL CONSTRAINT [DF_User_UpdatedDate]  DEFAULT (getdate()),
 CONSTRAINT [UN_User_Email] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


