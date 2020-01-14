USE [master]
GO
/****** Object:  Database [huraceTestDB]    Script Date: 13/01/2020 18:49:21 ******/
CREATE DATABASE [huraceTestDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'huraceTestDB', FILENAME = N'/var/opt/mssql/data/huraceTestDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'huraceTestDB_log', FILENAME = N'/var/opt/mssql/data/huraceTestDB_log.ldf' , SIZE = 270336KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [huraceTestDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [huraceTestDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [huraceTestDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [huraceTestDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [huraceTestDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [huraceTestDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [huraceTestDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [huraceTestDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [huraceTestDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [huraceTestDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [huraceTestDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [huraceTestDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [huraceTestDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [huraceTestDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [huraceTestDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [huraceTestDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [huraceTestDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [huraceTestDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [huraceTestDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [huraceTestDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [huraceTestDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [huraceTestDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [huraceTestDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [huraceTestDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [huraceTestDB] SET RECOVERY FULL 
GO
ALTER DATABASE [huraceTestDB] SET  MULTI_USER 
GO
ALTER DATABASE [huraceTestDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [huraceTestDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [huraceTestDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [huraceTestDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [huraceTestDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'huraceTestDB', N'ON'
GO
ALTER DATABASE [huraceTestDB] SET QUERY_STORE = OFF
GO
USE [huraceTestDB]
GO
/****** Object:  Schema [hurace]    Script Date: 13/01/2020 18:49:21 ******/
CREATE SCHEMA [hurace]
GO
/****** Object:  Table [hurace].[Country]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[StartList]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Gender]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Skier]    Script Date: 13/01/2020 18:49:21 ******/
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
	[imageUrl] [varchar](max) NULL,
 CONSTRAINT [PK_SKIER] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [hurace].[StartState]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [hurace].[StartListQuery]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [hurace].[StartListQuery]
AS
SELECT        hurace.StartList.skierId, hurace.StartList.raceId, hurace.StartList.startNumber, hurace.StartList.startStateId, hurace.StartState.startStatedescription, hurace.Skier.countryId, hurace.Skier.firstName, hurace.Skier.lastName, 
                         hurace.Skier.dateOfBirth, hurace.Skier.genderId, hurace.Gender.genderDescription, hurace.Country.countryCode, hurace.Country.countryName, hurace.Skier.imageUrl
FROM            hurace.Country INNER JOIN
                         hurace.Skier ON hurace.Country.id = hurace.Skier.countryId INNER JOIN
                         hurace.StartList ON hurace.Skier.id = hurace.StartList.skierId INNER JOIN
                         hurace.StartState ON hurace.StartList.startStateId = hurace.StartState.id INNER JOIN
                         hurace.Gender ON hurace.Skier.genderId = hurace.Gender.id
GO
/****** Object:  Table [hurace].[TimeData]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[TimeData](
	[time] [int] NOT NULL,
	[skierId] [int] NOT NULL,
	[skierEventId] [int] NOT NULL,
	[raceId] [int] NOT NULL,
	[sensorId] [int] NOT NULL,
 CONSTRAINT [PK_TimeData_1] PRIMARY KEY CLUSTERED 
(
	[skierId] ASC,
	[raceId] ASC,
	[sensorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Sensor]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[Sensor](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sensorDescription] [varchar](255) NULL,
	[raceId] [int] NOT NULL,
	[sensorNumber] [int] NULL,
 CONSTRAINT [PK_Sensor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [hurace].[AverageSensorTime]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [hurace].[AverageSensorTime]
AS
SELECT        AVG(hurace.TimeData.time) AS avgTime, hurace.Sensor.sensorNumber
FROM            hurace.Sensor INNER JOIN
                         hurace.TimeData ON hurace.Sensor.id = hurace.TimeData.sensorId
GROUP BY hurace.Sensor.sensorNumber
GO
/****** Object:  View [hurace].[TimeDataRanking]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [hurace].[TimeDataRanking]
AS
SELECT        TOP (100) PERCENT hurace.Country.countryCode, hurace.Country.countryName, hurace.StartList.startNumber, hurace.StartList.startStateId, hurace.Skier.firstName, hurace.Skier.countryId, hurace.Skier.lastName, 
                         MAX(hurace.TimeData.time) AS raceTime, hurace.TimeData.raceId, hurace.TimeData.skierId, hurace.Skier.dateOfBirth, hurace.Skier.genderId, hurace.Gender.genderDescription, hurace.Skier.imageUrl
FROM            hurace.Skier INNER JOIN
                         hurace.TimeData INNER JOIN
                         hurace.StartList ON hurace.TimeData.skierId = hurace.StartList.skierId ON hurace.Skier.id = hurace.StartList.skierId INNER JOIN
                         hurace.Country ON hurace.Skier.countryId = hurace.Country.id INNER JOIN
                         hurace.Gender ON hurace.Skier.genderId = hurace.Gender.id
GROUP BY hurace.Country.countryCode, hurace.Country.countryName, hurace.StartList.startNumber, hurace.StartList.startStateId, hurace.Skier.firstName, hurace.Skier.countryId, hurace.Skier.lastName, hurace.TimeData.raceId, 
                         hurace.TimeData.skierId, hurace.Skier.dateOfBirth, hurace.Skier.genderId, hurace.Gender.genderDescription, hurace.Skier.imageUrl
GO
/****** Object:  Table [hurace].[Race]    Script Date: 13/01/2020 18:49:21 ******/
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
	[raceDescription] [varchar](max) NULL,
	[raceStateId] [int] NOT NULL,
 CONSTRAINT [PK_RACE] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [hurace].[SensorRanking]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*SELECT TOP (100) PERCENT 
hurace.Country.countryCode, hurace.Country.countryName, hurace.StartList.startNumber, hurace.StartList.startStateId, hurace.Skier.firstName, hurace.Skier.countryId, hurace.Skier.lastName, hurace.TimeData.time, hurace.TimeData.raceId, hurace.TimeData.skierId, hurace.Skier.dateOfBirth, hurace.Skier.genderId, hurace.Gender.genderDescription
FROM   hurace.Skier 
		INNER JOIN
         hurace.TimeData 
		 INNER JOIN
         hurace.StartList ON hurace.TimeData.skierId = hurace.StartList.skierId ON hurace.Skier.id = hurace.StartList.skierId INNER JOIN
         hurace.Country ON hurace.Skier.countryId = hurace.Country.id 
		 INNER JOIN
         hurace.Gender ON hurace.Skier.genderId = hurace.Gender.id
		 join hurace.Sensor on hurace.Sensor.id = hurace.TimeData.sensorId
		 where hurace.TimeData.raceId = 228 and hurace.Sensor.sensorNumber = 1
		 order by hurace.TimeData.time asc*/
CREATE VIEW [hurace].[SensorRanking]
AS
SELECT        TOP (100) PERCENT s.countryId, s.dateOfBirth, s.firstName, s.lastName, s.genderId, g.genderDescription, c.countryCode, c.countryName, td.sensorId, td.skierEventId, td.time, sen.sensorDescription, sen.sensorNumber, 
                         td.raceId, td.skierId, hurace.StartList.startStateId, hurace.StartList.startNumber, s.imageUrl
FROM            hurace.TimeData AS td INNER JOIN
                         hurace.Race AS r ON r.id = td.raceId INNER JOIN
                         hurace.Skier AS s ON s.id = td.skierId INNER JOIN
                         hurace.Sensor AS sen ON sen.id = td.sensorId INNER JOIN
                         hurace.Country AS c ON c.id = s.countryId INNER JOIN
                         hurace.Gender AS g ON g.id = s.genderId INNER JOIN
                         hurace.StartList ON td.raceId = hurace.StartList.raceId AND td.skierId = hurace.StartList.skierId AND s.id = hurace.StartList.skierId
GO
/****** Object:  Table [hurace].[Discipline]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[EventType]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[EventType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[eventDescription] [varchar](255) NOT NULL,
 CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Location]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[PossibleDiscipline]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[RaceData]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[RaceEvent]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[RaceEvent](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[raceDataId] [int] NOT NULL,
 CONSTRAINT [PK_RaceEvent] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[RaceState]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[Season]    Script Date: 13/01/2020 18:49:21 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[SkierDiscipline]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[SkierDiscipline](
	[skierId] [int] NOT NULL,
	[disciplineId] [int] NOT NULL,
 CONSTRAINT [PK_SkierDiscipline] PRIMARY KEY CLUSTERED 
(
	[skierId] ASC,
	[disciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hurace].[SkierEvent]    Script Date: 13/01/2020 18:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hurace].[SkierEvent](
	[skierId] [int] NOT NULL,
	[raceDataId] [int] NOT NULL,
	[raceId] [int] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SKIEREVENT] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [hurace].[EventType] ON 

INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (1, N'RaceStarted')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (2, N'RaceFinished')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (3, N'RaceCanceled')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (4, N'Skier Started')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (5, N'Skier Finished')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (6, N'Skier Disqualified')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (7, N'Skier Failure')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (8, N'Skier SplitTime')
INSERT [hurace].[EventType] ([id], [eventDescription]) VALUES (9, N'Skier Canceled')
SET IDENTITY_INSERT [hurace].[EventType] OFF
SET IDENTITY_INSERT [hurace].[Gender] ON 

INSERT [hurace].[Gender] ([id], [genderDescription]) VALUES (1, N'male')
INSERT [hurace].[Gender] ([id], [genderDescription]) VALUES (2, N'female')
SET IDENTITY_INSERT [hurace].[Gender] OFF
SET IDENTITY_INSERT [hurace].[RaceState] ON 

INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (1, N'Upcoming')
INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (2, N'Running')
INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (3, N'Finished')
INSERT [hurace].[RaceState] ([id], [raceStateDescription]) VALUES (4, N'Canceled')
SET IDENTITY_INSERT [hurace].[RaceState] OFF
SET IDENTITY_INSERT [hurace].[StartState] ON 

INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (1, N'Waiting')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (2, N'Running')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (3, N'Finished')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (4, N'Disqualified')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (5, N'Canceled')
INSERT [hurace].[StartState] ([id], [startStatedescription]) VALUES (6, N'Draw Ready')
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
ALTER TABLE [hurace].[RaceEvent]  WITH CHECK ADD  CONSTRAINT [FK_RaceEvent_RaceData] FOREIGN KEY([raceDataId])
REFERENCES [hurace].[RaceData] ([id])
GO
ALTER TABLE [hurace].[RaceEvent] CHECK CONSTRAINT [FK_RaceEvent_RaceData]
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
ALTER TABLE [hurace].[SkierDiscipline]  WITH CHECK ADD  CONSTRAINT [FK_SkierDiscipline_Discipline] FOREIGN KEY([disciplineId])
REFERENCES [hurace].[Discipline] ([id])
GO
ALTER TABLE [hurace].[SkierDiscipline] CHECK CONSTRAINT [FK_SkierDiscipline_Discipline]
GO
ALTER TABLE [hurace].[SkierDiscipline]  WITH CHECK ADD  CONSTRAINT [FK_SkierDiscipline_Skier] FOREIGN KEY([skierId])
REFERENCES [hurace].[Skier] ([id])
GO
ALTER TABLE [hurace].[SkierDiscipline] CHECK CONSTRAINT [FK_SkierDiscipline_Skier]
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
ALTER TABLE [hurace].[StartList]  WITH CHECK ADD  CONSTRAINT [FK_StartList_Skier] FOREIGN KEY([skierId])
REFERENCES [hurace].[Skier] ([id])
GO
ALTER TABLE [hurace].[StartList] CHECK CONSTRAINT [FK_StartList_Skier]
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
ALTER TABLE [hurace].[TimeData]  WITH CHECK ADD  CONSTRAINT [FK_TimeData_SkierEvent] FOREIGN KEY([skierEventId])
REFERENCES [hurace].[SkierEvent] ([id])
GO
ALTER TABLE [hurace].[TimeData] CHECK CONSTRAINT [FK_TimeData_SkierEvent]
GO
ALTER TABLE [hurace].[TimeData]  WITH CHECK ADD  CONSTRAINT [FK_TimeData_StartList] FOREIGN KEY([raceId], [skierId])
REFERENCES [hurace].[StartList] ([raceId], [skierId])
GO
ALTER TABLE [hurace].[TimeData] CHECK CONSTRAINT [FK_TimeData_StartList]
GO
ALTER TABLE [hurace].[TimeData]  WITH CHECK ADD  CONSTRAINT [TimeDataRaceFK] FOREIGN KEY([raceId])
REFERENCES [hurace].[Race] ([id])
GO
ALTER TABLE [hurace].[TimeData] CHECK CONSTRAINT [TimeDataRaceFK]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Sensor (hurace)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 284
               Right = 351
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TimeData (hurace)"
            Begin Extent = 
               Top = 63
               Left = 866
               Bottom = 350
               Right = 1071
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 3210
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 3525
         Or = 2475
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'AverageSensorTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'AverageSensorTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[51] 4[10] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "td"
            Begin Extent = 
               Top = 83
               Left = 256
               Bottom = 213
               Right = 426
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "r"
            Begin Extent = 
               Top = 0
               Left = 590
               Bottom = 130
               Right = 761
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s"
            Begin Extent = 
               Top = 204
               Left = 552
               Bottom = 334
               Right = 722
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "sen"
            Begin Extent = 
               Top = 98
               Left = 25
               Bottom = 228
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 383
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "g"
            Begin Extent = 
               Top = 261
               Left = 824
               Bottom = 370
               Right = 1283
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "StartList (hurace)"
            Begin Extent = 
               Top = 104
               Left = 1000
               Bottom = 234
               Right = 1170
            End
            DisplayFlags = 280
            Top' , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'SensorRanking'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Column = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2685
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'SensorRanking'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'SensorRanking'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Country (hurace)"
            Begin Extent = 
               Top = 311
               Left = 1553
               Bottom = 424
               Right = 1723
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Skier (hurace)"
            Begin Extent = 
               Top = 143
               Left = 667
               Bottom = 273
               Right = 837
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "StartList (hurace)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "StartState (hurace)"
            Begin Extent = 
               Top = 289
               Left = 311
               Bottom = 391
               Right = 508
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Gender (hurace)"
            Begin Extent = 
               Top = 338
               Left = 993
               Bottom = 434
               Right = 1179
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
  ' , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'StartListQuery'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'       Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'StartListQuery'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'StartListQuery'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[46] 4[20] 2[26] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Skier (hurace)"
            Begin Extent = 
               Top = 130
               Left = 395
               Bottom = 442
               Right = 696
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TimeData (hurace)"
            Begin Extent = 
               Top = 77
               Left = 1314
               Bottom = 403
               Right = 1674
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "StartList (hurace)"
            Begin Extent = 
               Top = 77
               Left = 946
               Bottom = 409
               Right = 1173
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Country (hurace)"
            Begin Extent = 
               Top = 128
               Left = 140
               Bottom = 241
               Right = 317
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Gender (hurace)"
            Begin Extent = 
               Top = 450
               Left = 981
               Bottom = 629
               Right = 1310
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 5280
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
    ' , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'TimeDataRanking'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     Or = 555
         Or = 555
         Or = 9495
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'TimeDataRanking'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'hurace', @level1type=N'VIEW',@level1name=N'TimeDataRanking'
GO
USE [master]
GO
ALTER DATABASE [huraceTestDB] SET  READ_WRITE 
GO
