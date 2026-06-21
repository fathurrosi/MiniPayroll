USE [MANDE_03]
GO
/****** Object:  Table [dbo].[tbl_LeaveType]    Script Date: 19/06/2026 21:27:03 ******/
drop table [dbo].[tbl_LeaveType]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_LeaveType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LeaveCode] [varchar](20) NULL,
	[LeaveName] [varchar](100) NULL,
	[DefaultAnnualQuota] [int] NULL,
	[IsPaid] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_LeaveType] ADD  DEFAULT ((1)) FOR [IsPaid]
GO

ALTER TABLE [dbo].[tbl_LeaveType] ADD  DEFAULT ((1)) FOR [IsActive]
GO

--//update pancingan

/****** Object:  Table [dbo].[tbl_Branches]    Script Date: 21/06/2026 20:41:03 ******/
drop table [dbo].[tbl_Branches]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Branches](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BranchCode] [varchar](20) NULL,
	[BranchName] [varchar](100) NULL,
	[Address] [varchar](191) NULL,
	[BranchHeadEmployeeId] [int] null,
	[CompanyProfileId] [int] NOT NULL,

	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
