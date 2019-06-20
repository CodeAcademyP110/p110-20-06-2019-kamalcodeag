----create database Company

--use Company


--create table Departments(
--	Id int primary key identity,
--	Name nvarchar(100) not null unique
--)

--create table Positions(
--	Id int primary key identity,
--	Name nvarchar(100) not null unique,
--	DepartmentId int references Departments(Id),
--	ReportsTo int 
--)

--create table Employees(
--	Id int primary key identity,
--	Firstname nvarchar(50) not null,
--	Lastname nvarchar(50),
--	Email nvarchar(100) not null unique,
--	Password nvarchar(255) not null,
--	Salary decimal(18, 2),
--	PositionId int references Positions(Id),
--	StartDate date
--)

--insert into Departments values('Management'), ('HR'), ('IT'), ('Finance'), ('Marketing'), ('Administration')

--insert into Positions values('CEO', 1, NULL), ('CFO', 1, 1), ('CTO', 1, 1), ('HR Director', 2, 1),
--					  ('HR Senior Specialist', 2, 4), ('HR Specialist', 2, 5),
--					  ('IT Director', 3, 3), ('IT sysadmin', 3, 7), ('IT Senior Developer', 3, 7),
--					  ('IT Developer', 3, 9), ('IT Junior Developer', 3, 10)

--create view EmployeesReport as
--select 
--	emp.Id,
--	CONCAT(emp.Firstname, ' ', emp.Lastname) Fullname,
--	emp.Email, 
--	emp.Salary,
--	emp.StartDate,
--	pos.Name Position,
--	pos2.Name ReportsTo,
--	dep.Name Departament
--from Employees emp
--join Positions pos on emp.PositionId = pos.Id
--join Departments dep on pos.DepartmentId = dep.Id
--left join Positions pos2 on pos.ReportsTo = pos2.Id

--SQL Injection

--select * from Employees 

--update Employees
--set Firstname = 'z'
--where Id = 5000