



-- =============================================
-- Procedure: PR_Users_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Users_SelectAll]
AS
BEGIN
    SELECT * FROM Users
    ORDER BY UserID;
END
GO

-- =============================================
-- Procedure: PR_Users_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Users_SelectByPK]
    @UserID INT
AS
BEGIN
    SELECT * FROM Users
    WHERE UserID = @UserID
    ORDER BY UserID;
END
GO

-- =============================================
-- Procedure: PR_Users_Insert 'jay','123','jay@gmail.com','123456789',1,'2003-09-09','2003-09-09'
-- =============================================
CREATE PROCEDURE [dbo].[PR_Users_Insert]
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @MobileNo NVARCHAR(100),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME
AS
BEGIN
    INSERT INTO Users (UserName, Password, Email, MobileNo, IsActive, Created, Modified)
    VALUES (@UserName, @Password, @Email, @MobileNo, @IsActive, @Created, @Modified);
END
GO

-- =============================================
-- Procedure: PR_Users_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Users_UpdateByPK]
    @UserID INT,
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @MobileNo NVARCHAR(100),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME
AS
BEGIN
    UPDATE Users
    SET 
        UserName = @UserName,
        Password = @Password,
        Email = @Email,
        MobileNo = @MobileNo,
        IsActive = @IsActive,
        Created = @Created,
        Modified = @Modified
    WHERE UserID = @UserID;
END
GO

-- =============================================
-- Procedure: PR_Users_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Users_DeleteByPK]
    @UserID INT
AS
BEGIN
    DELETE FROM Users
    WHERE UserID = @UserID;
END
GO


----------------------------------------------------------------------------------------------------------------------------------------------


-- =============================================
-- Procedure: PR_Departments_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Departments_SelectAll]
AS
BEGIN
    SELECT * FROM Departments
    ORDER BY DepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Departments_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Departments_SelectByPK]
    @DepartmentID INT
AS
BEGIN
    SELECT * FROM Departments
    WHERE DepartmentID = @DepartmentID
    ORDER BY DepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Departments_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Departments_Insert]
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(250),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    INSERT INTO Departments (DepartmentName, Description, IsActive, Created, Modified, UserID)
    VALUES (@DepartmentName, @Description, @IsActive, @Created, @Modified, @UserID);
END
GO

-- =============================================
-- Procedure: PR_Departments_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Departments_UpdateByPK]
    @DepartmentID INT,
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(250),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    UPDATE Departments
    SET 
        DepartmentName = @DepartmentName,
        Description = @Description,
        IsActive = @IsActive,
        Created = @Created,
        Modified = @Modified,
        UserID = @UserID
    WHERE DepartmentID = @DepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Departments_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Departments_DeleteByPK]
    @DepartmentID INT
AS
BEGIN
    DELETE FROM Departments
    WHERE DepartmentID = @DepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Doctors_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Doctors_SelectAll]
AS
BEGIN
    SELECT * FROM Doctors
    ORDER BY DoctorID;
END
GO

-- =============================================
-- Procedure: PR_Doctors_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Doctors_SelectByPK]
    @DoctorID INT
AS
BEGIN
    SELECT * FROM Doctors
    WHERE DoctorID = @DoctorID
    ORDER BY DoctorID;
END
GO

-- =============================================
-- Procedure: PR_Doctors_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Doctors_Insert]
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Qualification NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    INSERT INTO Doctors (Name, Phone, Email, Qualification, Specialization, IsActive, Created, Modified, UserID)
    VALUES (@Name, @Phone, @Email, @Qualification, @Specialization, @IsActive, @Created, @Modified, @UserID);
END
GO

-- =============================================
-- Procedure: PR_Doctors_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Doctors_UpdateByPK]
    @DoctorID INT,
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Qualification NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    UPDATE Doctors
    SET 
        Name = @Name,
        Phone = @Phone,
        Email = @Email,
        Qualification = @Qualification,
        Specialization = @Specialization,
        IsActive = @IsActive,
        Created = @Created,
        Modified = @Modified,
        UserID = @UserID
    WHERE DoctorID = @DoctorID;
END
GO

-- =============================================
-- Procedure: PR_Doctors_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Doctors_DeleteByPK]
    @DoctorID INT
AS
BEGIN
    DELETE FROM Doctors
    WHERE DoctorID = @DoctorID;
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_DoctorDepartments_SelectAll]
AS
BEGIN
    SELECT * FROM DoctorDepartments
    ORDER BY DoctorDepartmentID;
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_DoctorDepartments_SelectByPK]
    @DoctorDepartmentID INT
AS
BEGIN
    SELECT * FROM DoctorDepartments
    WHERE DoctorDepartmentID = @DoctorDepartmentID
    ORDER BY DoctorDepartmentID;
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_DoctorDepartments_Insert]
    @DoctorID INT,
    @DepartmentID INT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    INSERT INTO DoctorDepartments (DoctorID, DepartmentID, Created, Modified, UserID)
    VALUES (@DoctorID, @DepartmentID, @Created, @Modified, @UserID);
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_DoctorDepartments_UpdateByPK]
    @DoctorDepartmentID INT,
    @DoctorID INT,
    @DepartmentID INT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    UPDATE DoctorDepartments
    SET 
        DoctorID = @DoctorID,
        DepartmentID = @DepartmentID,
        Created = @Created,
        Modified = @Modified,
        UserID = @UserID
    WHERE DoctorDepartmentID = @DoctorDepartmentID;
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_DoctorDepartments_DeleteByPK]
    @DoctorDepartmentID INT
AS
BEGIN
    DELETE FROM DoctorDepartments
    WHERE DoctorDepartmentID = @DoctorDepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Patients_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Patients_SelectAll]
AS
BEGIN
    SELECT * FROM Patients
    ORDER BY PatientID;
END
GO

-- =============================================
-- Procedure: PR_Patients_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Patients_SelectByPK]
    @PatientID INT
AS
BEGIN
    SELECT * FROM Patients
    WHERE PatientID = @PatientID
    ORDER BY PatientID;
END
GO

-- =============================================
-- Procedure: PR_Patients_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Patients_Insert]
    @Name NVARCHAR(100),
    @DateOfBirth DATETIME,
    @Gender NVARCHAR(10),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(100),
    @Address NVARCHAR(250),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    INSERT INTO Patients (Name, DateOfBirth, Gender, Email, Phone, Address, City, State, IsActive, Created, Modified, UserID)
    VALUES (@Name, @DateOfBirth, @Gender, @Email, @Phone, @Address, @City, @State, @IsActive, @Created, @Modified, @UserID);
END
GO

-- =============================================
-- Procedure: PR_Patients_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Patients_UpdateByPK]
    @PatientID INT,
    @Name NVARCHAR(100),
    @DateOfBirth DATETIME,
    @Gender NVARCHAR(10),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(100),
    @Address NVARCHAR(250),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @IsActive BIT,
    @Created DATETIME,
    @Modified DATETIME,
    @UserID INT
AS
BEGIN
    UPDATE Patients
    SET 
        Name = @Name,
        DateOfBirth = @DateOfBirth,
        Gender = @Gender,
        Email = @Email,
        Phone = @Phone,
        Address = @Address,
        City = @City,
        State = @State,
        IsActive = @IsActive,
        Created = @Created,
        Modified = @Modified,
        UserID = @UserID
    WHERE PatientID = @PatientID;
END
GO

-- =============================================
-- Procedure: PR_Patients_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Patients_DeleteByPK]
    @PatientID INT
AS
BEGIN
    DELETE FROM Patients
    WHERE PatientID = @PatientID;
END
GO

-- =============================================
-- Procedure: PR_Appointments_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Appointments_SelectAll]
AS
BEGIN
    SELECT * FROM Appointments
    ORDER BY AppointmentID;
END
GO

-- =============================================
-- Procedure: PR_Appointments_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Appointments_SelectByPK]
    @AppointmentID INT
AS
BEGIN
    SELECT * FROM Appointments
    WHERE AppointmentID = @AppointmentID
    ORDER BY AppointmentID;
END
GO

-- =============================================
-- Procedure: PR_Appointments_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Appointments_Insert]
    @DoctorID INT,
    @PatientID INT,
    @AppointmentDate DATETIME,
    @AppointmentStatus NVARCHAR(50),
    @Created DATETIME,
    @Modified DATETIME
AS
BEGIN
    INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, AppointmentStatus, Created, Modified)
    VALUES (@DoctorID, @PatientID, @AppointmentDate, @AppointmentStatus, @Created, @Modified);
END
GO

-- =============================================
-- Procedure: PR_Appointments_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Appointments_UpdateByPK]
    @AppointmentID INT,
    @DoctorID INT,
    @PatientID INT,
    @AppointmentDate DATETIME,
    @AppointmentStatus NVARCHAR(50),
    @Created DATETIME,
    @Modified DATETIME
AS
BEGIN
    UPDATE Appointments
    SET 
        DoctorID = @DoctorID,
        PatientID = @PatientID,
        AppointmentDate = @AppointmentDate,
        AppointmentStatus = @AppointmentStatus,
        Created = @Created,
        Modified = @Modified
    WHERE AppointmentID = @AppointmentID;
END
GO

-- =============================================
-- Procedure: PR_Appointments_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Appointments_DeleteByPK]
    @AppointmentID INT
AS
BEGIN
    DELETE FROM Appointments
    WHERE AppointmentID = @AppointmentID;
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_MedicalRecords_SelectAll]
AS
BEGIN
    SELECT * FROM MedicalRecords
    ORDER BY RecordID;
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_MedicalRecords_SelectByPK]
    @RecordID INT
AS
BEGIN
    SELECT * FROM MedicalRecords
    WHERE RecordID = @RecordID
    ORDER BY RecordID;
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_MedicalRecords_Insert]
    @PatientID INT,
    @DoctorID INT,
    @VisitDate DATE,
    @Diagnosis TEXT,
    @Treatment TEXT,
    @Created DATETIME
AS
BEGIN
    INSERT INTO MedicalRecords (PatientID, DoctorID, VisitDate, Diagnosis, Treatment, Created)
    VALUES (@PatientID, @DoctorID, @VisitDate, @Diagnosis, @Treatment, @Created);
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_MedicalRecords_UpdateByPK]
    @RecordID INT,
    @PatientID INT,
    @DoctorID INT,
    @VisitDate DATE,
    @Diagnosis TEXT,
    @Treatment TEXT,
    @Created DATETIME
AS
BEGIN
    UPDATE MedicalRecords
    SET 
        PatientID = @PatientID,
        DoctorID = @DoctorID,
        VisitDate = @VisitDate,
        Diagnosis = @Diagnosis,
        Treatment = @Treatment,
        Created = @Created
    WHERE RecordID = @RecordID;
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_MedicalRecords_DeleteByPK]
    @RecordID INT
AS
BEGIN
    DELETE FROM MedicalRecords
    WHERE RecordID = @RecordID;
END
GO

-- =============================================
-- Procedure: PR_Medications_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Medications_SelectAll]
AS
BEGIN
    SELECT * FROM Medications
    ORDER BY MedicationID;
END
GO

-- =============================================
-- Procedure: PR_Medications_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Medications_SelectByPK]
    @MedicationID INT
AS
BEGIN
    SELECT * FROM Medications
    WHERE MedicationID = @MedicationID
    ORDER BY MedicationID;
END
GO

-- =============================================
-- Procedure: PR_Medications_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Medications_Insert]
    @Name NVARCHAR(100),
    @Description TEXT,
    @Stock INT,
    @UnitPrice DECIMAL(10, 2),
    @Created DATETIME
AS
BEGIN
    INSERT INTO Medications (Name, Description, Stock, UnitPrice, Created)
    VALUES (@Name, @Description, @Stock, @UnitPrice, @Created);
END
GO

-- =============================================
-- Procedure: PR_Medications_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Medications_UpdateByPK]
    @MedicationID INT,
    @Name NVARCHAR(100),
    @Description TEXT,
    @Stock INT,
    @UnitPrice DECIMAL(10, 2),
    @Created DATETIME
AS
BEGIN
    UPDATE Medications
    SET 
        Name = @Name,
        Description = @Description,
        Stock = @Stock,
        UnitPrice = @UnitPrice,
        Created = @Created
    WHERE MedicationID = @MedicationID;
END
GO

-- =============================================
-- Procedure: PR_Medications_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Medications_DeleteByPK]
    @MedicationID INT
AS
BEGIN
    DELETE FROM Medications
    WHERE MedicationID = @MedicationID;
END
GO

-- =============================================
-- Procedure: PR_Prescriptions_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Prescriptions_SelectAll]
AS
BEGIN
    SELECT * FROM Prescriptions
    ORDER BY PrescriptionID;
END
GO

-- =============================================
-- Procedure: PR_Prescriptions_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Prescriptions_SelectByPK]
    @PrescriptionID INT
AS
BEGIN
    SELECT * FROM Prescriptions
    WHERE PrescriptionID = @PrescriptionID
    ORDER BY PrescriptionID;
END
GO

-- =============================================
-- Procedure: PR_Prescriptions_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Prescriptions_Insert]
    @RecordID INT,
    @MedicationID INT,
    @Dosage NVARCHAR(100),
    @Duration NVARCHAR(50)
AS
BEGIN
    INSERT INTO Prescriptions (RecordID, MedicationID, Dosage, Duration)
    VALUES (@RecordID, @MedicationID, @Dosage, @Duration);
END
GO

-- =============================================
-- Procedure: PR_Prescriptions_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Prescriptions_UpdateByPK]
    @PrescriptionID INT,
    @RecordID INT,
    @MedicationID INT,
    @Dosage NVARCHAR(100),
    @Duration NVARCHAR(50)
AS
BEGIN
    UPDATE Prescriptions
    SET 
        RecordID = @RecordID,
        MedicationID = @MedicationID,
        Dosage = @Dosage,
        Duration = @Duration
    WHERE PrescriptionID = @PrescriptionID;
END
GO

-- =============================================
-- Procedure: PR_Prescriptions_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Prescriptions_DeleteByPK]
    @PrescriptionID INT
AS
BEGIN
    DELETE FROM Prescriptions
    WHERE PrescriptionID = @PrescriptionID;
END
GO

-- =============================================
-- Procedure: PR_Wards_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Wards_SelectAll]
AS
BEGIN
    SELECT * FROM Wards
    ORDER BY WardID;
END
GO

-- =============================================
-- Procedure: PR_Wards_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Wards_SelectByPK]
    @WardID INT
AS
BEGIN
    SELECT * FROM Wards
    WHERE WardID = @WardID
    ORDER BY WardID;
END
GO

-- =============================================
-- Procedure: PR_Wards_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Wards_Insert]
    @Name NVARCHAR(100),
    @Type NVARCHAR(20),
    @Capacity INT
AS
BEGIN
    INSERT INTO Wards (Name, Type, Capacity)
    VALUES (@Name, @Type, @Capacity);
END
GO

-- =============================================
-- Procedure: PR_Wards_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Wards_UpdateByPK]
    @WardID INT,
    @Name NVARCHAR(100),
    @Type NVARCHAR(20),
    @Capacity INT
AS
BEGIN
    UPDATE Wards
    SET 
        Name = @Name,
        Type = @Type,
        Capacity = @Capacity
    WHERE WardID = @WardID;
END
GO

-- =============================================
-- Procedure: PR_Wards_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Wards_DeleteByPK]
    @WardID INT
AS
BEGIN
    DELETE FROM Wards
    WHERE WardID = @WardID;
END
GO

-- =============================================
-- Procedure: PR_Admissions_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Admissions_SelectAll]
AS
BEGIN
    SELECT * FROM Admissions
    ORDER BY AdmissionID;
END
GO

-- =============================================
-- Procedure: PR_Admissions_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Admissions_SelectByPK]
    @AdmissionID INT
AS
BEGIN
    SELECT * FROM Admissions
    WHERE AdmissionID = @AdmissionID
    ORDER BY AdmissionID;
END
GO

-- =============================================
-- Procedure: PR_Admissions_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Admissions_Insert]
    @PatientID INT,
    @WardID INT,
    @AdmissionDate DATETIME,
    @DischargeDate DATETIME
AS
BEGIN
    INSERT INTO Admissions (PatientID, WardID, AdmissionDate, DischargeDate)
    VALUES (@PatientID, @WardID, @AdmissionDate, @DischargeDate);
END
GO

-- =============================================
-- Procedure: PR_Admissions_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Admissions_UpdateByPK]
    @AdmissionID INT,
    @PatientID INT,
    @WardID INT,
    @AdmissionDate DATETIME,
    @DischargeDate DATETIME
AS
BEGIN
    UPDATE Admissions
    SET 
        PatientID = @PatientID,
        WardID = @WardID,
        AdmissionDate = @AdmissionDate,
        DischargeDate = @DischargeDate
    WHERE AdmissionID = @AdmissionID;
END
GO

-- =============================================
-- Procedure: PR_Admissions_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Admissions_DeleteByPK]
    @AdmissionID INT
AS
BEGIN
    DELETE FROM Admissions
    WHERE AdmissionID = @AdmissionID;
END
GO

-- =============================================
-- Procedure: PR_Billing_SelectAll
-- =============================================
CREATE PROCEDURE [dbo].[PR_Billing_SelectAll]
AS
BEGIN
    SELECT * FROM Billing
    ORDER BY BillID;
END
GO

-- =============================================
-- Procedure: PR_Billing_SelectByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Billing_SelectByPK]
    @BillID INT
AS
BEGIN
    SELECT * FROM Billing
    WHERE BillID = @BillID
    ORDER BY BillID;
END
GO

-- =============================================
-- Procedure: PR_Billing_Insert
-- =============================================
CREATE PROCEDURE [dbo].[PR_Billing_Insert]
    @PatientID INT,
    @Amount DECIMAL(10,2),
    @Details TEXT,
    @BillingDate DATE,
    @Status NVARCHAR(20)
AS
BEGIN
    INSERT INTO Billing (PatientID, Amount, Details, BillingDate, Status)
    VALUES (@PatientID, @Amount, @Details, @BillingDate, @Status);
END
GO

-- =============================================
-- Procedure: PR_Billing_UpdateByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Billing_UpdateByPK]
    @BillID INT,
    @PatientID INT,
    @Amount DECIMAL(10,2),
    @Details TEXT,
    @BillingDate DATE,
    @Status NVARCHAR(20)
AS
BEGIN
    UPDATE Billing
    SET 
        PatientID = @PatientID,
        Amount = @Amount,
        Details = @Details,
        BillingDate = @BillingDate,
        Status = @Status
    WHERE BillID = @BillID;
END
GO

-- =============================================
-- Procedure: PR_Billing_DeleteByPK
-- =============================================
CREATE PROCEDURE [dbo].[PR_Billing_DeleteByPK]
    @BillID INT
AS
BEGIN
    DELETE FROM Billing
    WHERE BillID = @BillID;
END
GO
