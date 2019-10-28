USE [master]
GO
/****** Object:  Database [huraceDB]    Script Date: 28/10/2019 17:10:55 ******/
CREATE DATABASE [huraceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'huraceDB', FILENAME = N'/var/opt/mssql/data/huraceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'huraceDB_log', FILENAME = N'/var/opt/mssql/data/huraceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [huraceDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [huraceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [huraceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [huraceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [huraceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [huraceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [huraceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [huraceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [huraceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [huraceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [huraceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [huraceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [huraceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [huraceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [huraceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [huraceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [huraceDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [huraceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [huraceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [huraceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [huraceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [huraceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [huraceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [huraceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [huraceDB] SET RECOVERY FULL 
GO
ALTER DATABASE [huraceDB] SET  MULTI_USER 
GO
ALTER DATABASE [huraceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [huraceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [huraceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [huraceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [huraceDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'huraceDB', N'ON'
GO
ALTER DATABASE [huraceDB] SET QUERY_STORE = OFF
GO
USE [huraceDB]
GO
/****** Object:  Schema [hurace]    Script Date: 28/10/2019 17:10:55 ******/
CREATE SCHEMA [hurace]
GO
/****** Object:  Table [hurace].[Country]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Country](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[countryCode] [varchar](10) NOT NULL,
	[countryName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_COUNTRY] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Discipline]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Discipline](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[disciplineName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_DISCIPLINE] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[EventType]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[EventType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Gender]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Gender](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[genderDescription] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Location]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Location](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[locationName] [varchar](255) NOT NULL,
	[countryId] [int] NOT NULL,
 CONSTRAINT [PK_LOCATION] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[PossibleDiscipline]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[PossibleDiscipline](
	[locationId] [int] NOT NULL,
	[disciplineId] [int] NOT NULL,
 CONSTRAINT [PK_POSSIBLEDISCIPLINE] PRIMARY KEY CLUSTERED 
(
	[locationId] ASC,
	[disciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Race]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Race](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[seasonId] [int] NOT NULL,
	[disciplineId] [int] NOT NULL,
	[locationId] [int] NOT NULL,
	[raceDate] [date] NOT NULL,
	[genderId] [int] NOT NULL,
	[raceDescription] [varchar](max) NOT NULL,
	[raceStateId] [int] NOT NULL,
 CONSTRAINT [PK_RACE] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [hurace].[RaceData]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[RaceData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[raceId] [int] NOT NULL,
	[eventTypeId] [int] NOT NULL,
	[eventDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_RACEDATA] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[RaceState]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[RaceState](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[raceStateDescription] [varchar](100) NOT NULL,
 CONSTRAINT [PK_RaceState] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Season]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Season](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[startDate] [date] NOT NULL,
	[endDate] [date] NOT NULL,
 CONSTRAINT [PK_SEASON] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Sensor]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Sensor](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sensorDescription] [varchar](255) NULL,
	[raceId] [int] NOT NULL,
 CONSTRAINT [PK_Sensor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Skier]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Skier](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[countryId] [int] NOT NULL,
	[firstName] [varchar](150) NOT NULL,
	[lastName] [varchar](150) NOT NULL,
	[dateOfBirth] [date] NOT NULL,
	[genderId] [int] NOT NULL,
 CONSTRAINT [PK_SKIER] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[SkierEvent]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[SkierEvent](
	[skierId] [int] NOT NULL,
	[raceDataId] [int] NOT NULL,
	[raceId] [int] NOT NULL,
 CONSTRAINT [PK_SKIEREVENT] PRIMARY KEY CLUSTERED 
(
	[skierId] ASC,
	[raceDataId] ASC,
	[raceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[StartList]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[StartList](
	[raceId] [int] NOT NULL,
	[skierId] [int] NOT NULL,
	[startNumber] [int] NOT NULL,
	[startStateId] [int] NOT NULL,
 CONSTRAINT [PK_STARTLIST] PRIMARY KEY CLUSTERED 
(
	[raceId] ASC,
	[skierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[StartState]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[StartState](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[startStatedescription] [varchar](255) NOT NULL,
 CONSTRAINT [PK_StartState] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[TimeData]    Script Date: 28/10/2019 17:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[TimeData](
	[time] [datetime] NOT NULL,
	[skierId] [int] NOT NULL,
	[raceDataId] [int] NOT NULL,
	[raceId] [int] NOT NULL,
	[sensorId] [int] NOT NULL,
 CONSTRAINT [PK_TimeData_1] PRIMARY KEY CLUSTERED 
(
	[skierId] ASC,
	[raceId] ASC,
	[sensorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [hurace].[Country] ON 

INSERT [hurace].[Country] ([id], [countryCode], [countryName]) VALUES (1, N'AT', N'Austria')
INSERT [hurace].[Country] ([id], [countryCode], [countryName]) VALUES (2, N'CAN', N'Canada')
INSERT [hurace].[Country] ([id], [countryCode], [countryName]) VALUES (3, N'IT', N'Italy')
INSERT [hurace].[Country] ([id], [countryCode], [countryName]) VALUES (4, N'CH', N'Swiss')
INSERT [hurace].[Country] ([id], [countryCode], [countryName]) VALUES (5, N'N', N'Norway')
INSERT [hurace].[Country] ([id], [countryCode], [countryName]) VALUES (6, N'US', N'United States')
SET IDENTITY_INSERT [hurace].[Country] OFF
SET IDENTITY_INSERT [hurace].[Discipline] ON 

INSERT [hurace].[Discipline] ([id], [disciplineName]) VALUES (1, N'Super-G')
INSERT [hurace].[Discipline] ([id], [disciplineName]) VALUES (2, N'Downhill')
SET IDENTITY_INSERT [hurace].[Discipline] OFF
SET IDENTITY_INSERT [hurace].[EventType] ON 

INSERT [hurace].[EventType] ([id], [description]) VALUES (1, N'Race Started')
INSERT [hurace].[EventType] ([id], [description]) VALUES (2, N'Race Finished')
INSERT [hurace].[EventType] ([id], [description]) VALUES (3, N'Race Canceled')
INSERT [hurace].[EventType] ([id], [description]) VALUES (4, N'Skier Started')
INSERT [hurace].[EventType] ([id], [description]) VALUES (5, N'Skier Finished')
INSERT [hurace].[EventType] ([id], [description]) VALUES (6, N'Skier Disqualified')
INSERT [hurace].[EventType] ([id], [description]) VALUES (7, N'Skier Failiure')
INSERT [hurace].[EventType] ([id], [description]) VALUES (8, N'Skier Running')
SET IDENTITY_INSERT [hurace].[EventType] OFF
SET IDENTITY_INSERT [hurace].[Gender] ON 

INSERT [hurace].[Gender] ([id], [genderDescription]) VALUES (1, N'male')
INSERT [hurace].[Gender] ([id], [genderDescription]) VALUES (2, N'female')
SET IDENTITY_INSERT [hurace].[Gender] OFF
SET IDENTITY_INSERT [hurace].[Location] ON 

INSERT [hurace].[Location] ([id], [locationName], [countryId]) VALUES (2, N'Sölden', 1)
INSERT [hurace].[Location] ([id], [locationName], [countryId]) VALUES (4, N'Lake Lousie', 2)
INSERT [hurace].[Location] ([id], [locationName], [countryId]) VALUES (5, N'Beaver Creek', 6)
INSERT [hurace].[Location] ([id], [locationName], [countryId]) VALUES (6, N'Gröden', 3)
INSERT [hurace].[Location] ([id], [locationName], [countryId]) VALUES (7, N'Bormio', 3)
INSERT [hurace].[Location] ([id], [locationName], [countryId]) VALUES (8, N'Wengen', 4)
SET IDENTITY_INSERT [hurace].[Location] OFF
SET IDENTITY_INSERT [hurace].[Race] ON 

INSERT [hurace].[Race] ([id], [seasonId], [disciplineId], [locationId], [raceDate], [genderId], [raceDescription], [raceStateId]) VALUES (1, 2, 1, 2, CAST(N'2018-11-24' AS Date), 1, N'Super-G Lake Louise', 3)
INSERT [hurace].[Race] ([id], [seasonId], [disciplineId], [locationId], [raceDate], [genderId], [raceDescription], [raceStateId]) VALUES (2, 2, 2, 2, CAST(N'2018-11-25' AS Date), 1, N'Downhill Lake Louise', 3)
INSERT [hurace].[Race] ([id], [seasonId], [disciplineId], [locationId], [raceDate], [genderId], [raceDescription], [raceStateId]) VALUES (3, 2, 1, 6, CAST(N'2018-12-14' AS Date), 1, N'Super-G Gröden', 3)
INSERT [hurace].[Race] ([id], [seasonId], [disciplineId], [locationId], [raceDate], [genderId], [raceDescription], [raceStateId]) VALUES (6, 2, 2, 6, CAST(N'2018-12-15' AS Date), 1, N'Downhill Gröden', 3)
INSERT [hurace].[Race] ([id], [seasonId], [disciplineId], [locationId], [raceDate], [genderId], [raceDescription], [raceStateId]) VALUES (7, 2, 1, 7, CAST(N'2018-12-28' AS Date), 1, N'Super-G Bormio', 3)
INSERT [hurace].[Race] ([id], [seasonId], [disciplineId], [locationId], [raceDate], [genderId], [raceDescription], [raceStateId]) VALUES (8, 2, 2, 7, CAST(N'2018-12-29' AS Date), 1, N'Downhill Bormio', 3)
SET IDENTITY_INSERT [hurace].[Race] OFF
SET IDENTITY_INSERT [hurace].[RaceData] ON 

INSERT [hurace].[RaceData] ([id], [raceId], [eventTypeId], [eventDateTime]) VALUES (1, 1, 8, CAST(N'2019-10-27T14:52:00.000' AS DateTime))
INSERT [hurace].[RaceData] ([id], [raceId], [eventTypeId], [eventDateTime]) VALUES (4, 1, 8, CAST(N'2019-10-27T14:52:00.000' AS DateTime))
SET IDENTITY_INSERT [hurace].[RaceData] OFF
SET IDENTITY_INSERT [hurace].[RaceState] ON 

INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (1, N'Upcoming')
INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (2, N'Running ')
INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (3, N'Finished')
INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (4, N'Cannceled')
SET IDENTITY_INSERT [hurace].[RaceState] OFF
SET IDENTITY_INSERT [hurace].[Season] ON 

INSERT [hurace].[Season] ([id], [startDate], [endDate]) VALUES (1, CAST(N'2019-10-27' AS Date), CAST(N'2020-03-22' AS Date))
INSERT [hurace].[Season] ([id], [startDate], [endDate]) VALUES (2, CAST(N'2018-10-28' AS Date), CAST(N'2019-03-17' AS Date))
SET IDENTITY_INSERT [hurace].[Season] OFF
SET IDENTITY_INSERT [hurace].[Skier] ON 

INSERT [hurace].[Skier] ([id], [countryId], [firstName], [lastName], [dateOfBirth], [genderId]) VALUES (1, 1, N'Felix', N'Stumvoll', CAST(N'1998-03-18' AS Date), 1)
INSERT [hurace].[Skier] ([id], [countryId], [firstName], [lastName], [dateOfBirth], [genderId]) VALUES (2, 6, N'Sterling', N'Archer', CAST(N'1980-01-01' AS Date), 1)
SET IDENTITY_INSERT [hurace].[Skier] OFF
INSERT [hurace].[StartList] ([raceId], [skierId], [startNumber], [startStateId]) VALUES (1, 1, 1, 3)
INSERT [hurace].[StartList] ([raceId], [skierId], [startNumber], [startStateId]) VALUES (1, 2, 2, 3)
SET IDENTITY_INSERT [hurace].[StartState] ON 

INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (1, N'Waiting')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (2, N'Running')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (3, N'Finished')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (4, N'Disqualified')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (5, N'Canceled')
SET IDENTITY_INSERT [hurace].[StartState] OFF
ALTER TABLE [hurace].[Location]  WITH CHECK ADD  CONSTRAINT [LocationCountryFK] FOREIGN KEY([countryId])
REFERENCES [hurace].[Country] ([id])
GO
ALTER TABLE [hurace].[Location] CHECK CONSTRAINT [LocationCountryFK]
GO
ALTER TABLE [hurace].[PossibleDiscipline]  WITH CHECK ADD  CONSTRAINT [PossibleDisciplineDisciplineFK] FOREIGN KEY([disciplineId])
REFERENCES [hurace].[Discipline] ([id])
GO
ALTER TABLE [hurace].[PossibleDiscipline] CHECK CONSTRAINT [PossibleDisciplineDisciplineFK]
GO
ALTER TABLE [hurace].[PossibleDiscipline]  WITH CHECK ADD  CONSTRAINT [PossibleDisciplineLocation] FOREIGN KEY([locationId])
REFERENCES [hurace].[Location] ([id])
GO
ALTER TABLE [hurace].[PossibleDiscipline] CHECK CONSTRAINT [PossibleDisciplineLocation]
GO
ALTER TABLE [hurace].[Race]  WITH CHECK ADD  CONSTRAINT [FK_Race_Gender] FOREIGN KEY([genderId])
REFERENCES [hurace].[Gender] ([id])
GO
ALTER TABLE [hurace].[Race] CHECK CONSTRAINT [FK_Race_Gender]
GO
ALTER TABLE [hurace].[Race]  WITH CHECK ADD  CONSTRAINT [FK_Race_RaceState] FOREIGN KEY([raceStateId])
REFERENCES [hurace].[RaceState] ([id])
GO
ALTER TABLE [hurace].[Race] CHECK CONSTRAINT [FK_Race_RaceState]
GO
ALTER TABLE [hurace].[Race]  WITH CHECK ADD  CONSTRAINT [RaceLocationFK] FOREIGN KEY([locationId])
REFERENCES [hurace].[Location] ([id])
GO
ALTER TABLE [hurace].[Race] CHECK CONSTRAINT [RaceLocationFK]
GO
ALTER TABLE [hurace].[Race]  WITH CHECK ADD  CONSTRAINT [RaceSeasonFK] FOREIGN KEY([seasonId])
REFERENCES [hurace].[Season] ([id])
GO
ALTER TABLE [hurace].[Race] CHECK CONSTRAINT [RaceSeasonFK]
GO
ALTER TABLE [hurace].[RaceData]  WITH CHECK ADD  CONSTRAINT [FK_RaceData_EventType] FOREIGN KEY([eventTypeId])
REFERENCES [hurace].[EventType] ([id])
GO
ALTER TABLE [hurace].[RaceData] CHECK CONSTRAINT [FK_RaceData_EventType]
GO
ALTER TABLE [hurace].[RaceData]  WITH CHECK ADD  CONSTRAINT [RaceDataRaceFK] FOREIGN KEY([raceId])
REFERENCES [hurace].[Race] ([id])
GO
ALTER TABLE [hurace].[RaceData] CHECK CONSTRAINT [RaceDataRaceFK]
GO
ALTER TABLE [hurace].[Sensor]  WITH CHECK ADD  CONSTRAINT [FK_Sensor_Race] FOREIGN KEY([raceId])
REFERENCES [hurace].[Race] ([id])
GO
ALTER TABLE [hurace].[Sensor] CHECK CONSTRAINT [FK_Sensor_Race]
GO
ALTER TABLE [hurace].[Skier]  WITH CHECK ADD  CONSTRAINT [FK_Skier_Gender] FOREIGN KEY([genderId])
REFERENCES [hurace].[Gender] ([id])
GO
ALTER TABLE [hurace].[Skier] CHECK CONSTRAINT [FK_Skier_Gender]
GO
ALTER TABLE [hurace].[Skier]  WITH CHECK ADD  CONSTRAINT [SkierCountryFK] FOREIGN KEY([countryId])
REFERENCES [hurace].[Country] ([id])
GO
ALTER TABLE [hurace].[Skier] CHECK CONSTRAINT [SkierCountryFK]
GO
ALTER TABLE [hurace].[SkierEvent]  WITH CHECK ADD  CONSTRAINT [FK_SkierEvent_StartList] FOREIGN KEY([raceId], [skierId])
REFERENCES [hurace].[StartList] ([raceId], [skierId])
GO
ALTER TABLE [hurace].[SkierEvent] CHECK CONSTRAINT [FK_SkierEvent_StartList]
GO
ALTER TABLE [hurace].[SkierEvent]  WITH CHECK ADD  CONSTRAINT [SkierEvent_Race_id_fk] FOREIGN KEY([raceId])
REFERENCES [hurace].[Race] ([id])
GO
ALTER TABLE [hurace].[SkierEvent] CHECK CONSTRAINT [SkierEvent_Race_id_fk]
GO
ALTER TABLE [hurace].[SkierEvent]  WITH CHECK ADD  CONSTRAINT [SkierEvent_RaceData_id_fk] FOREIGN KEY([raceDataId])
REFERENCES [hurace].[RaceData] ([id])
GO
ALTER TABLE [hurace].[SkierEvent] CHECK CONSTRAINT [SkierEvent_RaceData_id_fk]
GO
ALTER TABLE [hurace].[StartList]  WITH CHECK ADD  CONSTRAINT [FK_StartList_StartState] FOREIGN KEY([startStateId])
REFERENCES [hurace].[StartState] ([id])
GO
ALTER TABLE [hurace].[StartList] CHECK CONSTRAINT [FK_StartList_StartState]
GO
ALTER TABLE [hurace].[TimeData]  WITH CHECK ADD  CONSTRAINT [FK_TimeData_Sensor] FOREIGN KEY([sensorId])
REFERENCES [hurace].[Sensor] ([id])
GO
ALTER TABLE [hurace].[TimeData] CHECK CONSTRAINT [FK_TimeData_Sensor]
GO
ALTER TABLE [hurace].[TimeData]  WITH CHECK ADD  CONSTRAINT [FK_TimeData_StartList] FOREIGN KEY([raceId], [skierId])
REFERENCES [hurace].[StartList] ([raceId], [skierId])
GO
ALTER TABLE [hurace].[TimeData] CHECK CONSTRAINT [FK_TimeData_StartList]
GO
ALTER TABLE [hurace].[TimeData]  WITH CHECK ADD  CONSTRAINT [TimeDataRaceDataFK] FOREIGN KEY([raceDataId])
REFERENCES [hurace].[RaceData] ([id])
GO
ALTER TABLE [hurace].[TimeData] CHECK CONSTRAINT [TimeDataRaceDataFK]
GO
ALTER TABLE [hurace].[TimeData]  WITH CHECK ADD  CONSTRAINT [TimeDataRaceFK] FOREIGN KEY([raceId])
REFERENCES [hurace].[Race] ([id])
GO
ALTER TABLE [hurace].[TimeData] CHECK CONSTRAINT [TimeDataRaceFK]
GO
USE [master]
GO
ALTER DATABASE [huraceDB] SET  READ_WRITE 
GO
