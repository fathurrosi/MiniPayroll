

USE [MANDE_03]
GO

/****** Object:  Table [dbo].[tbl_LeaveRequests]    Script Date: 25/06/2026 15:06:34 ******/
drop table [dbo].[tbl_LeaveRequests]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_LeaveRequests](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BranchCode] [varchar](20) NOT NULL,
	[DepartmentCode] [varchar](50) NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[LeaveTypeCode] [varchar](20) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[TotalDays] [int] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[ApprovalStatus] bit NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime2](7) NULL,

	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_LeaveRequests] ADD  DEFAULT ((0)) FOR [TotalDays]
GO

ALTER TABLE [dbo].[tbl_LeaveRequests] ADD  DEFAULT ('Pending') FOR [ApprovalStatus]
GO

ALTER TABLE [dbo].[tbl_LeaveRequests] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO


USE [MANDE_03]
GO

/****** Object:  View [dbo].[vw_LeaveRequest]    Script Date: 26/06/2026 16:13:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].vw_LeaveRequest
as
select 
a.Id, a.BranchCode, b.BranchName, a.DepartmentCode, c.DepartmentName, a.EmployeeId, d.FullName as EmployeeName,
a.LeaveTypeCode, e.LeaveName, a.StartDate, a.EndDate, a.TotalDays,
a.Reason, cast(a.ApprovalStatus as bit) as ApprovalStatus, a.ApprovedBy, f.FullName as ApprovedByName, a.ApprovedDate
from tbl_LeaveRequests a
left join tbl_Branches b
on a.Branchcode = b.BranchCode
left join tbl_Departments c 
on a.DepartmentCode = c.DepartmentCode
left join tbl_employees d
on a.EmployeeId = d.EmployeeId
left join tbl_LeaveType e 
on a.LeaveTypeCode = e.LeaveCode
left join tbl_employees f
on a.ApprovedBy = f.EmployeeId



--=========================================================dari sini ke atas, update 28/06/2026=====================================


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


USE [MANDE_03]
GO

/****** Object:  Table [dbo].[tbl_Departments]    Script Date: 22/06/2026 21:19:17 ******/
drop table [tbl_Departments]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Departments](
	[DepartmentCode] [varchar](50) NOT NULL,
	[DepartmentName] [varchar](100) NOT NULL,

	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK__Departme__B2079BED1D3A0B65] PRIMARY KEY CLUSTERED 
(
	[DepartmentCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [MANDE_03]
GO

/****** Object:  Table [dbo].[tbl_Positions]    Script Date: 22/06/2026 21:25:47 ******/
drop table [dbo].[tbl_Positions]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Positions](
	[PositionCode] [varchar](50) NOT NULL,
	[PositionName] [varchar](100) NULL,
	[GradeLevel] [varchar](20) NULL,

	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK__Position__60BB9A799D578AAB] PRIMARY KEY CLUSTERED 
(
	[PositionCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [MANDE_03]
GO

/****** Object:  View [dbo].[vw_EmployeeSalary]    Script Date: 24/06/2026 16:13:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[vw_BranchDetails]
as

select 
a.id, a.BranchCode, a.BranchName, a.Address, a.BranchHeadEmployeeId, a.CompanyProfileId, a.IsActive,
a.CreatedDate, a.UpdatedDate, a.CreatedBy, a.UpdatedBy,
b.EmployeeCode, b.FullName,
c.CompanyName
from tbl_Branches a
left join tbl_employees b on a.BranchHeadEmployeeId = b.EmployeeId
left join tbl_CompanyProfiles c on a.CompanyProfileId = c.CompanyProfileId
GO

