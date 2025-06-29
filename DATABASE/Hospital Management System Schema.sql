-- Create the main hospital database
CREATE DATABASE hms_database

-- Select the newly created database for use
USE hms_database

-- =====================================
-- USERS TABLE: Stores login & identity details
-- =====================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    MobileNo NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL
);

-- =====================================
-- DEPARTMENTS TABLE: Stores hospital departments
-- =====================================
CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250),
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- =====================================
-- DOCTORS TABLE: Stores doctors' personal and professional details
-- =====================================
CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Qualification NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- =====================================
-- DOCTOR-DEPARTMENT LINK TABLE: Associates doctors with departments
-- =====================================
CREATE TABLE DoctorDepartments (
    DoctorDepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT NOT NULL,
    DepartmentID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- =====================================
-- PATIENTS TABLE: Stores patient details
-- =====================================
CREATE TABLE Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    DateOfBirth DATETIME NOT NULL,
    Gender NVARCHAR(10) NOT NULL CHECK (Gender IN ('Male', 'Female', 'Other')),
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(100) NOT NULL,
    Address NVARCHAR(250) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- =====================================
-- APPOINTMENTS TABLE: Booking details for consultations
-- =====================================
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT NOT NULL,
    PatientID INT NOT NULL,
    UserID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL DEFAULT GETDATE(),
    AppointmentStatus NVARCHAR(50) NOT NULL DEFAULT 'Scheduled',
    Description NVARCHAR(250) NOT NULL,
    SpecialRemarks NVARCHAR(100) NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    TotalConsultedAmount DECIMAL(5,2),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- =====================================
-- MEDICAL RECORDS TABLE: History of diagnosis and treatments
-- =====================================
CREATE TABLE MedicalRecords (
    RecordID INT IDENTITY(1,1) PRIMARY KEY,
    PatientID INT NOT NULL,
    DoctorID INT NOT NULL,
    VisitDate DATE NOT NULL,
    Diagnosis TEXT NOT NULL,
    Treatment TEXT,
    Created DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
);

-- =====================================
-- MEDICATIONS TABLE: Medicine master data
-- =====================================
CREATE TABLE Medications (
    MedicationID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description TEXT,
    Stock INT DEFAULT 0,
    UnitPrice DECIMAL(10, 2),
    Created DATETIME DEFAULT GETDATE()
);

-- =====================================
-- PRESCRIPTIONS TABLE: Links medication to medical record
-- =====================================
CREATE TABLE Prescriptions (
    PrescriptionID INT IDENTITY(1,1) PRIMARY KEY,
    RecordID INT NOT NULL,
    MedicationID INT NOT NULL,
    Dosage NVARCHAR(100) NOT NULL,
    Duration NVARCHAR(50),
    FOREIGN KEY (RecordID) REFERENCES MedicalRecords(RecordID),
    FOREIGN KEY (MedicationID) REFERENCES Medications(MedicationID)
);

-- =====================================
-- WARDS TABLE: Hospital ward types and capacity
-- =====================================
CREATE TABLE Wards (
    WardID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Type NVARCHAR(20) NOT NULL CHECK (Type IN ('ICU', 'General', 'Private', 'Emergency')),
    Capacity INT NOT NULL
);

-- =====================================
-- ADMISSIONS TABLE: Tracks patient admissions to wards
-- =====================================
CREATE TABLE Admissions (
    AdmissionID INT IDENTITY(1,1) PRIMARY KEY,
    PatientID INT NOT NULL,
    WardID INT NOT NULL,
    AdmissionDate DATETIME NOT NULL,
    DischargeDate DATETIME,
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (WardID) REFERENCES Wards(WardID)
);

-- =====================================
-- BILLING TABLE: Billing details for patients
-- =====================================
CREATE TABLE Billing (
    BillID INT IDENTITY(1,1) PRIMARY KEY,
    PatientID INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    Details TEXT,
    BillingDate DATE DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Unpaid' CHECK (Status IN ('Paid', 'Unpaid', 'Partially Paid')),
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID)
);
