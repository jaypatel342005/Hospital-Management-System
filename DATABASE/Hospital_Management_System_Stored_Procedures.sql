


-- =============================================
-- Procedure: PR_Users_SelectAll
-- Returns all users with their details
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Users_SelectAll]
AS
BEGIN
    SELECT
        UserID,
        UserName,
        Password,
        Email,
        MobileNo,
        IsActive,
        Created,
        Modified 
    FROM Users
    ORDER BY UserID;
END
GO

-- =============================================
-- Procedure: PR_Users_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Users_SelectByPK]
    @UserID INT
AS
BEGIN
    SELECT
        UserID,
        UserName,
        Password,
        Email,
        MobileNo,
        IsActive,
        Created,
        Modified
    FROM Users
    WHERE UserID = @UserID;
END
GO

-- =============================================
-- Procedure: PR_Users_Insert 'jay','123','jay@gmail.com','123456789',1
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Users_Insert]
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @MobileNo NVARCHAR(100),
    @IsActive BIT
AS
BEGIN
    INSERT INTO Users (UserName, Password, Email, MobileNo, IsActive, Created, Modified)
    VALUES (@UserName, @Password, @Email, @MobileNo, @IsActive, GETDATE(), GETDATE());
END
GO

-- =============================================
-- Procedure: PR_Users_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Users_UpdateByPK]
    @UserID INT,
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @MobileNo NVARCHAR(100),
    @IsActive BIT
AS
BEGIN
    UPDATE Users
    SET 
        UserName = @UserName,
        Password = @Password,
        Email = @Email,
        MobileNo = @MobileNo,
        IsActive = @IsActive,
        Modified = GETDATE()
    WHERE UserID = @UserID;
END
GO

-- =============================================
-- Procedure: PR_Users_DeleteByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Users_DeleteByPK]
    @UserID INT
AS
BEGIN
    DELETE FROM Users
    WHERE UserID = @UserID;
END
GO

-- =============================================
-- Procedure: PR_Departments_SelectAll
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Departments_SelectAll]
AS
BEGIN
    SELECT
        Departments.DepartmentID,
        Departments.DepartmentName,
        Departments.Description,
        Departments.IsActive,
        Departments.Created,
        Departments.Modified,
        Departments.UserID,
        Users.UserName 
    FROM Departments
        INNER JOIN Users ON Departments.UserID = Users.UserID
    ORDER BY Departments.DepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Departments_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Departments_SelectByPK]
    @DepartmentID INT
AS
BEGIN
    SELECT
        Departments.DepartmentID,
        Departments.DepartmentName,
        Departments.Description,
        Departments.IsActive,
        Departments.Created,
        Departments.Modified,
        Departments.UserID,
        Users.UserName
    FROM Departments
        INNER JOIN Users ON Departments.UserID = Users.UserID
    WHERE Departments.DepartmentID = @DepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Departments_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Departments_Insert]
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(250),
    @IsActive BIT,
    @UserID INT
AS
BEGIN
    INSERT INTO Departments (DepartmentName, Description, IsActive, UserID, Created, Modified)
    VALUES (@DepartmentName, @Description, @IsActive, @UserID, GETDATE(), GETDATE());
END
GO

-- =============================================
-- Procedure: PR_Departments_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Departments_UpdateByPK]
    @DepartmentID INT,
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(250),
    @IsActive BIT,
    @UserID INT
AS
BEGIN
    UPDATE Departments
    SET 
        DepartmentName = @DepartmentName,
        Description = @Description,
        IsActive = @IsActive,
        UserID = @UserID,
        Modified = GETDATE()
    WHERE DepartmentID = @DepartmentID;
END
GO

-- =============================================
-- Procedure: PR_Departments_DeleteByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Departments_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Doctors_SelectAll]
AS
BEGIN
    SELECT
        Doctors.DoctorID,
        Doctors.Name,
        Doctors.Phone,
        Doctors.Email,
        Doctors.Qualification,
        Doctors.Specialization,
        Doctors.IsActive,
        Doctors.Created,
        Doctors.Modified,
        Doctors.UserID,
        Users.UserName 
    FROM Doctors
        INNER JOIN Users ON Doctors.UserID = Users.UserID
    ORDER BY Doctors.DoctorID;
END
GO

-- =============================================
-- Procedure: PR_Doctors_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Doctors_SelectByPK]
    @DoctorID INT
AS
BEGIN
    SELECT
        Doctors.DoctorID,
        Doctors.Name,
        Doctors.Phone,
        Doctors.Email,
        Doctors.Qualification,
        Doctors.Specialization,
        Doctors.IsActive,
        Doctors.Created,
        Doctors.Modified,
        Doctors.UserID,
        Users.UserName
    FROM Doctors
        INNER JOIN Users ON Doctors.UserID = Users.UserID
    WHERE Doctors.DoctorID = @DoctorID;
END
GO

-- =============================================
-- Procedure: PR_Doctors_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Doctors_Insert]
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Qualification NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @IsActive BIT,
    @UserID INT
AS
BEGIN
    INSERT INTO Doctors (Name, Phone, Email, Qualification, Specialization, IsActive, UserID, Created, Modified)
    VALUES (@Name, @Phone, @Email, @Qualification, @Specialization, @IsActive, @UserID, GETDATE(), GETDATE());
END
GO

-- =============================================
-- Procedure: PR_Doctors_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Doctors_UpdateByPK]
    @DoctorID INT,
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Qualification NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @IsActive BIT,
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
        UserID = @UserID,
        Modified = GETDATE()
    WHERE DoctorID = @DoctorID;
END
GO

-- =============================================
-- Procedure: PR_Doctors_UpdateStatusByPK
-- Description: Toggle doctor's active status (Active/Inactive)
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Doctors_UpdateStatusByPK]
    @DoctorID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Check if doctor exists
        IF NOT EXISTS (SELECT 1 FROM Doctors WHERE DoctorID = @DoctorID)
        BEGIN
            RAISERROR('Doctor with ID %d does not exist.', 16, 1, @DoctorID);
            RETURN;
        END
        
        -- Toggle the IsActive status
        UPDATE Doctors
        SET 
            IsActive = CASE 
                WHEN IsActive = 1 THEN 0 
                ELSE 1 
            END,
            Modified = GETDATE()
        WHERE DoctorID = @DoctorID;
        
        -- Return success message with current status
        SELECT 
            DoctorID,
            Name,
            IsActive,
            CASE 
                WHEN IsActive = 1 THEN 'Doctor activated successfully'
                ELSE 'Doctor deactivated successfully'
            END AS StatusMessage
        FROM Doctors 
        WHERE DoctorID = @DoctorID;
        
    END TRY
    BEGIN CATCH
        -- Handle any errors
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO


-- =============================================
-- Procedure: PR_Doctors_DeleteByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Doctors_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_DoctorDepartments_SelectAll]
AS
BEGIN
    SELECT
        DoctorDepartments.DoctorDepartmentID,
        DoctorDepartments.DoctorID,
        DoctorDepartments.DepartmentID,
        DoctorDepartments.Created,
        DoctorDepartments.Modified,
        DoctorDepartments.UserID, 
        Doctors.Name AS DoctorName,
        Departments.DepartmentName,
        Users.UserName 
    FROM DoctorDepartments
        INNER JOIN Doctors ON DoctorDepartments.DoctorID = Doctors.DoctorID
        INNER JOIN Departments ON DoctorDepartments.DepartmentID = Departments.DepartmentID
        INNER JOIN Users ON DoctorDepartments.UserID = Users.UserID
    ORDER BY DoctorDepartments.DoctorDepartmentID;
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_DoctorDepartments_SelectByPK]
    @DoctorDepartmentID INT
AS
BEGIN
    SELECT
        DoctorDepartments.DoctorDepartmentID,
        DoctorDepartments.DoctorID,
        DoctorDepartments.DepartmentID,
        DoctorDepartments.Created,
        DoctorDepartments.Modified,
        DoctorDepartments.UserID,
        Doctors.Name AS DoctorName,
        Departments.DepartmentName,
        Users.UserName
    FROM DoctorDepartments
        INNER JOIN Doctors ON DoctorDepartments.DoctorID = Doctors.DoctorID
        INNER JOIN Departments ON DoctorDepartments.DepartmentID = Departments.DepartmentID
        INNER JOIN Users ON DoctorDepartments.UserID = Users.UserID
    WHERE DoctorDepartments.DoctorDepartmentID = @DoctorDepartmentID;
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_DoctorDepartments_Insert]
    @DoctorID INT,
    @DepartmentID INT,
    @UserID INT
AS
BEGIN
    INSERT INTO DoctorDepartments (DoctorID, DepartmentID, UserID, Created, Modified)
    VALUES (@DoctorID, @DepartmentID, @UserID, GETDATE(), GETDATE());
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_DoctorDepartments_UpdateByPK]
    @DoctorDepartmentID INT,
    @DoctorID INT,
    @DepartmentID INT,
    @UserID INT
AS
BEGIN
    UPDATE DoctorDepartments
    SET 
        DoctorID = @DoctorID,
        DepartmentID = @DepartmentID,
        UserID = @UserID,
        Modified = GETDATE()
    WHERE DoctorDepartmentID = @DoctorDepartmentID;
END
GO

-- =============================================
-- Procedure: PR_DoctorDepartments_DeleteByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_DoctorDepartments_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Patients_SelectAll]
AS
BEGIN
    SELECT
        Patients.PatientID,
        Patients.Name,
        Patients.DateOfBirth,
        Patients.Gender,
        Patients.Email,
        Patients.Phone,
        Patients.Address,
        Patients.City,
        Patients.State,
        Patients.IsActive,
        Patients.Created,
        Patients.Modified,
        Patients.UserID,
        Users.UserName
    FROM Patients
        INNER JOIN Users ON Patients.UserID = Users.UserID
    ORDER BY Patients.PatientID;
END
GO

-- =============================================
-- Procedure: PR_Patients_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Patients_SelectByPK]
    @PatientID INT
AS
BEGIN
    SELECT
        Patients.PatientID,
        Patients.Name,
        Patients.DateOfBirth,
        Patients.Gender,
        Patients.Email,
        Patients.Phone,
        Patients.Address,
        Patients.City,
        Patients.State,
        Patients.IsActive,
        Patients.Created,
        Patients.Modified,
        Patients.UserID,
        Users.UserName
    FROM Patients
        INNER JOIN Users ON Patients.UserID = Users.UserID
    WHERE Patients.PatientID = @PatientID;
END
GO

-- =============================================
-- Procedure: PR_Patients_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Patients_Insert]
    @Name NVARCHAR(100),
    @DateOfBirth DATETIME,
    @Gender NVARCHAR(10),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(100),
    @Address NVARCHAR(250),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @IsActive BIT,
    @UserID INT
AS
BEGIN
    INSERT INTO Patients (Name, DateOfBirth, Gender, Email, Phone, Address, City, State, IsActive, UserID, Created, Modified)
    VALUES (@Name, @DateOfBirth, @Gender, @Email, @Phone, @Address, @City, @State, @IsActive, @UserID, GETDATE(), GETDATE());
END
GO

-- =============================================
-- Procedure: PR_Patients_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Patients_UpdateByPK]
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
        UserID = @UserID,
        Modified = GETDATE()
    WHERE PatientID = @PatientID;
END
GO

-- =============================================
-- Procedure:	
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Patients_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Appointments_SelectAll]
AS
BEGIN
    SELECT
        Appointments.AppointmentID,
        Appointments.DoctorID,
        Appointments.PatientID,
        Appointments.UserID,
        Appointments.AppointmentDate,
        Appointments.AppointmentStatus,
        Appointments.Description,
        Appointments.SpecialRemarks,
        Appointments.Created,
        Appointments.Modified,
        Appointments.TotalConsultedAmount, 
        Doctors.Name AS DoctorName,
        Patients.Name AS PatientName,
        Users.UserName
    FROM Appointments
        INNER JOIN Doctors ON Appointments.DoctorID = Doctors.DoctorID
        INNER JOIN Patients ON Appointments.PatientID = Patients.PatientID
        INNER JOIN Users ON Appointments.UserID = Users.UserID
    ORDER BY Appointments.AppointmentID;
END
GO

-- =============================================
-- Procedure: PR_Appointments_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Appointments_SelectByPK]
    @AppointmentID INT
AS
BEGIN
    SELECT
        Appointments.AppointmentID,
        Appointments.DoctorID,
        Appointments.PatientID,
        Appointments.UserID,
        Appointments.AppointmentDate,
        Appointments.AppointmentStatus,
        Appointments.Description,
        Appointments.SpecialRemarks,
        Appointments.Created,
        Appointments.Modified,
        Appointments.TotalConsultedAmount,
        Doctors.Name AS DoctorName,
        Patients.Name AS PatientName,
        Users.UserName
    FROM Appointments
        INNER JOIN Doctors ON Appointments.DoctorID = Doctors.DoctorID
        INNER JOIN Patients ON Appointments.PatientID = Patients.PatientID
        INNER JOIN Users ON Appointments.UserID = Users.UserID
    WHERE Appointments.AppointmentID = @AppointmentID;
END
GO

-- =============================================
-- Procedure: PR_Appointments_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Appointments_Insert]
    @DoctorID INT,
    @PatientID INT,
    @UserID INT,
    @AppointmentDate DATETIME,
    @AppointmentStatus NVARCHAR(50),
    @Description NVARCHAR(250),
    @SpecialRemarks NVARCHAR(100),
    @TotalConsultedAmount DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Appointments (DoctorID, PatientID, UserID, AppointmentDate, AppointmentStatus, Description, SpecialRemarks, TotalConsultedAmount, Created, Modified)
    VALUES (@DoctorID, @PatientID, @UserID, @AppointmentDate, @AppointmentStatus, @Description, @SpecialRemarks, @TotalConsultedAmount, GETDATE(), GETDATE());
END
GO

-- =============================================
-- Procedure: PR_Appointments_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Appointments_UpdateByPK]
    @AppointmentID INT,
    @DoctorID INT,
    @PatientID INT,
    @UserID INT,
    @AppointmentDate DATETIME,
    @AppointmentStatus NVARCHAR(50),
    @Description NVARCHAR(250),
    @SpecialRemarks NVARCHAR(100),
    @TotalConsultedAmount DECIMAL(5,2)
AS
BEGIN
    UPDATE Appointments
    SET 
        DoctorID = @DoctorID,
        PatientID = @PatientID,
        UserID = @UserID,
        AppointmentDate = @AppointmentDate,
        AppointmentStatus = @AppointmentStatus,
        Description = @Description,
        SpecialRemarks = @SpecialRemarks,
        TotalConsultedAmount = @TotalConsultedAmount,
        Modified = GETDATE()
    WHERE AppointmentID = @AppointmentID;
END
GO

-- =============================================
-- Procedure: PR_Appointments_DeleteByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Appointments_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_MedicalRecords_SelectAll]
AS
BEGIN
    SELECT
        MedicalRecords.RecordID,
        MedicalRecords.PatientID,
        MedicalRecords.DoctorID,
        MedicalRecords.VisitDate,
        MedicalRecords.Diagnosis,
        MedicalRecords.Treatment,
        MedicalRecords.Created, 
        Patients.Name AS PatientName,
        Doctors.Name AS DoctorName
    FROM MedicalRecords
        INNER JOIN Patients ON MedicalRecords.PatientID = Patients.PatientID
        INNER JOIN Doctors ON MedicalRecords.DoctorID = Doctors.DoctorID
    ORDER BY MedicalRecords.RecordID;
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_MedicalRecords_SelectByPK]
    @RecordID INT
AS
BEGIN
    SELECT
        MedicalRecords.RecordID,
        MedicalRecords.PatientID,
        MedicalRecords.DoctorID,
        MedicalRecords.VisitDate,
        MedicalRecords.Diagnosis,
        MedicalRecords.Treatment,
        MedicalRecords.Created,
        Patients.Name AS PatientName,
        Doctors.Name AS DoctorName
    FROM MedicalRecords
        INNER JOIN Patients ON MedicalRecords.PatientID = Patients.PatientID
        INNER JOIN Doctors ON MedicalRecords.DoctorID = Doctors.DoctorID
    WHERE MedicalRecords.RecordID = @RecordID;
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_MedicalRecords_Insert]
    @PatientID INT,
    @DoctorID INT,
    @VisitDate DATE,
    @Diagnosis TEXT,
    @Treatment TEXT
AS
BEGIN
    INSERT INTO MedicalRecords (PatientID, DoctorID, VisitDate, Diagnosis, Treatment, Created)
    VALUES (@PatientID, @DoctorID, @VisitDate, @Diagnosis, @Treatment, GETDATE());
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_MedicalRecords_UpdateByPK]
    @RecordID INT,
    @PatientID INT,
    @DoctorID INT,
    @VisitDate DATE,
    @Diagnosis TEXT,
    @Treatment TEXT
AS
BEGIN
    UPDATE MedicalRecords
    SET 
        PatientID = @PatientID,
        DoctorID = @DoctorID,
        VisitDate = @VisitDate,
        Diagnosis = @Diagnosis,
        Treatment = @Treatment
    WHERE RecordID = @RecordID;
END
GO

-- =============================================
-- Procedure: PR_MedicalRecords_DeleteByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_MedicalRecords_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Medications_SelectAll]
AS
BEGIN
    SELECT
        MedicationID,
        Name,
        Description,
        Stock,
        UnitPrice,
        Created
    FROM Medications
    ORDER BY MedicationID;
END
GO

-- =============================================
-- Procedure: PR_Medications_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Medications_SelectByPK]
    @MedicationID INT
AS
BEGIN
    SELECT
        MedicationID,
        Name,
        Description,
        Stock,
        UnitPrice,
        Created
    FROM Medications
    WHERE MedicationID = @MedicationID;
END
GO

-- =============================================
-- Procedure: PR_Medications_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Medications_Insert]
    @Name NVARCHAR(100),
    @Description TEXT,
    @Stock INT,
    @UnitPrice DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Medications (Name, Description, Stock, UnitPrice, Created)
    VALUES (@Name, @Description, @Stock, @UnitPrice, GETDATE());
END
GO

-- =============================================
-- Procedure: PR_Medications_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Medications_UpdateByPK]
    @MedicationID INT,
    @Name NVARCHAR(100),
    @Description TEXT,
    @Stock INT,
    @UnitPrice DECIMAL(10,2)
AS
BEGIN
    UPDATE Medications
    SET 
        Name = @Name,
        Description = @Description,
        Stock = @Stock,
        UnitPrice = @UnitPrice
    WHERE MedicationID = @MedicationID;
END
GO

-- =============================================
-- Procedure: PR_Medications_DeleteByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Medications_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Prescriptions_SelectAll]
AS
BEGIN
    SELECT
        Prescriptions.PrescriptionID,
        Prescriptions.RecordID,
        Prescriptions.MedicationID,
        Prescriptions.Dosage,
        Prescriptions.Duration, 
        MedicalRecords.PatientID,
        MedicalRecords.DoctorID,
        Medications.Name AS MedicationName
    FROM Prescriptions
        INNER JOIN MedicalRecords ON Prescriptions.RecordID = MedicalRecords.RecordID
        INNER JOIN Medications ON Prescriptions.MedicationID = Medications.MedicationID
    ORDER BY Prescriptions.PrescriptionID;
END
GO

-- =============================================
-- Procedure: PR_Prescriptions_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Prescriptions_SelectByPK]
    @PrescriptionID INT
AS
BEGIN
    SELECT
        Prescriptions.PrescriptionID,
        Prescriptions.RecordID,
        Prescriptions.MedicationID,
        Prescriptions.Dosage,
        Prescriptions.Duration,
        MedicalRecords.PatientID,
        MedicalRecords.DoctorID,
        Medications.Name AS MedicationName
    FROM Prescriptions
        INNER JOIN MedicalRecords ON Prescriptions.RecordID = MedicalRecords.RecordID
        INNER JOIN Medications ON Prescriptions.MedicationID = Medications.MedicationID
    WHERE Prescriptions.PrescriptionID = @PrescriptionID;
END
GO

-- =============================================
-- Procedure: PR_Prescriptions_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Prescriptions_Insert]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Prescriptions_UpdateByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Prescriptions_DeleteByPK]
    @PrescriptionID INT
AS
BEGIN
    DELETE FROM Prescriptions
    WHERE PrescriptionID = @PrescriptionID;
END
GO

-- =============================================
-- Procedure: PR_Wards_SelectAll
-- Returns all wards
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Wards_SelectAll]
AS
BEGIN
    SELECT
        WardID,
        Name,
        Type,
        Capacity
    FROM Wards
    ORDER BY WardID;
END
GO

-- =============================================
-- Procedure: PR_Wards_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Wards_SelectByPK]
    @WardID INT
AS
BEGIN
    SELECT
        WardID,
        Name,
        Type,
        Capacity
    FROM Wards
    WHERE WardID = @WardID;
END
GO

-- =============================================
-- Procedure: PR_Wards_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Wards_Insert]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Wards_UpdateByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Wards_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Admissions_SelectAll]
AS
BEGIN
    SELECT
        Admissions.AdmissionID,
        Admissions.PatientID,
        Admissions.WardID,
        Admissions.AdmissionDate,
        Admissions.DischargeDate, 
        Patients.Name AS PatientName,
        Wards.Name AS WardName
    FROM Admissions
        INNER JOIN Patients ON Admissions.PatientID = Patients.PatientID
        INNER JOIN Wards ON Admissions.WardID = Wards.WardID
    ORDER BY Admissions.AdmissionID;
END
GO

-- =============================================
-- Procedure: PR_Admissions_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Admissions_SelectByPK]
    @AdmissionID INT
AS
BEGIN
    SELECT
        Admissions.AdmissionID,
        Admissions.PatientID,
        Admissions.WardID,
        Admissions.AdmissionDate,
        Admissions.DischargeDate,
        Patients.Name AS PatientName,
        Wards.Name AS WardName
    FROM Admissions
        INNER JOIN Patients ON Admissions.PatientID = Patients.PatientID
        INNER JOIN Wards ON Admissions.WardID = Wards.WardID
    WHERE Admissions.AdmissionID = @AdmissionID;
END
GO

-- =============================================
-- Procedure: PR_Admissions_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Admissions_Insert]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Admissions_UpdateByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Admissions_DeleteByPK]
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Billing_SelectAll]
AS
BEGIN
    SELECT
        Billing.BillID,
        Billing.PatientID,
        Billing.Amount,
        Billing.Details,
        Billing.BillingDate,
        Billing.Status,
        Patients.Name AS PatientName 
    FROM Billing
        INNER JOIN Patients ON Billing.PatientID = Patients.PatientID
    ORDER BY Billing.BillID;
END
GO

-- =============================================
-- Procedure: PR_Billing_SelectByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Billing_SelectByPK]
    @BillID INT
AS
BEGIN
    SELECT
        Billing.BillID,
        Billing.PatientID,
        Billing.Amount,
        Billing.Details,
        Billing.BillingDate,
        Billing.Status,
        Patients.Name AS PatientName
    FROM Billing
        INNER JOIN Patients ON Billing.PatientID = Patients.PatientID
    WHERE Billing.BillID = @BillID;
END
GO

-- =============================================
-- Procedure: PR_Billing_Insert
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Billing_Insert]
    @PatientID INT,
    @Amount DECIMAL(10,2),
    @Details NVARCHAR(MAX),
    @BillingDate DATETIME,
    @Status NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Billing (PatientID, Amount, Details, BillingDate, Status)
    VALUES (@PatientID, @Amount, @Details, @BillingDate, @Status);
END
GO

-- =============================================
-- Procedure: PR_Billing_UpdateByPK
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PR_Billing_UpdateByPK]
    @BillID INT,
    @PatientID INT,
    @Amount DECIMAL(10,2),
    @Details NVARCHAR(MAX),
    @BillingDate DATETIME,
    @Status NVARCHAR(MAX)
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
CREATE OR ALTER PROCEDURE [dbo].[PR_Billing_DeleteByPK]
    @BillID INT
AS
BEGIN
    DELETE FROM Billing
    WHERE BillID = @BillID;
END
GO
