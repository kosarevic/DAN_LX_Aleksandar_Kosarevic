--If database doesnt exist it is automatically created.
IF DB_ID('Zadatak_1') IS NULL
CREATE DATABASE Zadatak_1
GO
--Newly created database is set to be in use.
USE Zadatak_1
--All tables are reseted clean.
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblEmployee')
drop table tblEmployee
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblSector')
drop table tblSector
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblLocation')
drop table tblLocation

create table tblLocation
(
LocationID int primary key IDENTITY(1,1),
Adress varchar(100) COLLATE Serbian_Latin_100_CI_AI not null,
Town varchar(100) COLLATE Serbian_Latin_100_CI_AI not null,
Country varchar(100) COLLATE Serbian_Latin_100_CI_AI not null
)

create table tblSector
(
SectorID int primary key IDENTITY(1,1),
Title varchar(100) not null
)

create table tblEmployee
(
EmployeeID int primary key IDENTITY(1,1),
FirstName varchar(100) not null,
LastName varchar(100) not null,
JMBG char(13) unique not null,
DateOfBirth date not null,
Gender char(1) check(Gender in ('M', 'F', 'X')) not null,
RegistrationNumber char(9) unique not null,
PhoneNumber varchar(20),
LocationID int foreign key references tblLocation(LocationID) not null,
SectorID int foreign key references tblSector(SectorID) not null,
ManagerID int
)

insert into tblLocation (Adress, Town, Country) values ('Adress 1', 'Town 1', 'Country 1')
insert into tblLocation (Adress, Town, Country) values ('Adress 2', 'Town 2', 'Country 2')
insert into tblLocation (Adress, Town, Country) values ('Adress 3', 'Town 3', 'Country 3')

insert into tblSector(Title) values ('Sector 1');
insert into tblSector(Title) values ('Sector 2');
insert into tblSector(Title) values ('Sector 3');

insert into tblEmployee (FirstName, LastName, JMBG, DateOfBirth, Gender, RegistrationNumber, PhoneNumber ,LocationID, SectorID, ManagerID) values ('Name 1','Surname 1', '0101990111111', '1-1-1990', 'M', 111111111, '064/1111111', 1, 1, 3);
insert into tblEmployee (FirstName, LastName, JMBG, DateOfBirth, Gender, RegistrationNumber, PhoneNumber ,LocationID, SectorID, ManagerID) values ('Name 2','Surname 2', '0111990111112', '1-11-1990', 'F', 111111112, '064/2222222', 2, 2, 0);
insert into tblEmployee (FirstName, LastName, JMBG, DateOfBirth, Gender, RegistrationNumber, PhoneNumber ,LocationID, SectorID, ManagerID) values ('Name 3','Surname 3', '1101990111113', '11-1-1990', 'X', 111111113, '064/3333333', 3, 3, 0);
insert into tblEmployee (FirstName, LastName, JMBG, DateOfBirth, Gender, RegistrationNumber, PhoneNumber ,LocationID, SectorID, ManagerID) values ('Name 4','Surname 4', '0101990111114', '1-1-1990', 'M', 111111114, '064/1111111', 1, 1, 2);
insert into tblEmployee (FirstName, LastName, JMBG, DateOfBirth, Gender, RegistrationNumber, PhoneNumber ,LocationID, SectorID, ManagerID) values ('Name 5','Surname 5', '0111990111115', '1-11-1990', 'F', 111111115, '064/2222222', 2, 2, 0);
insert into tblEmployee (FirstName, LastName, JMBG, DateOfBirth, Gender, RegistrationNumber, PhoneNumber ,LocationID, SectorID, ManagerID) values ('Name 6','Surname 6', '1101990111116', '11-1-1990', 'X', 111111116, '064/3333333', 3, 3, 4);

select * from tblEmployee e
join tblLocation l on e.LocationID = l.LocationID
join tblSector s on e.SectorID = s.SectorID