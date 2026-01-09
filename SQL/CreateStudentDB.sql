-- =================================================
-- Script: CreateStudentsDB.sql
-- Purpose: Create Users and Students tables
-- =================================================

-- ===============================
-- Drop tables if they exist
-- ===============================
IF OBJECT_ID('Students', 'U') IS NOT NULL DROP TABLE Students;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;

-- ===============================
-- Create Users table
-- ===============================
CREATE TABLE Users (
    UserID INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,  -- SHA256 hash
    PasswordSalt VARBINARY(128) NOT NULL, -- Salt for password
    RoleID INT NOT NULL                   -- 1=Admin, 2=Teacher, 3=Student
);

-- ===============================
-- Create Students table
-- ===============================
CREATE TABLE Students (
    StudentID INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Course NVARCHAR(100) NOT NULL
);

-- ===============================
-- Note:
-- No users are inserted here.
-- The C# program (Program.cs) will automatically
-- create a default admin on first run:
-- Username: admin
-- Password: admin123
-- ===============================
