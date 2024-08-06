CREATE TABLE [dbo].[NLog_Delete] (
   [ID] [int] IDENTITY(1,1) NOT NULL,
   [OriginalID] [int] NOT NULL,
   [MachineName] [nvarchar](200) NULL,
   [Logged] [datetime] NOT NULL,
   [Level] [varchar](5) NOT NULL,
   [Message] [nvarchar](max) NOT NULL,
   [Logger] [nvarchar](300) NULL,
   [Properties] [nvarchar](max) NULL,
   [Exception] [nvarchar](max) NULL,
   [OperationTime] [datetime] NOT NULL DEFAULT GETDATE(),
   [TransactionId] [nvarchar](50) NOT NULL,
   [EntityId] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_dbo_NLog_Delete] PRIMARY KEY CLUSTERED ([ID] ASC) 
   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
