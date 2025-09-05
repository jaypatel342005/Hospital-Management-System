using System.Data;

namespace Hospital_Management_System.Models
{
    public class DashboardModel
    {
        public KPIModel KPIData { get; set; } = new KPIModel();

        public string AppointmentsTrendJson { get; set; } = "{}";
        public string MonthlyRevenueJson { get; set; } = "{}";
        public string AppointmentStatusJson { get; set; } = "{}";
        public string PatientsByGenderJson { get; set; } = "{}";
        public string DoctorsByDepartmentJson { get; set; } = "{}";
        public string PatientAgeDistributionJson { get; set; } = "{}";
        public string DepartmentRevenueJson { get; set; } = "{}";
        public string WeeklyAppointmentPatternJson { get; set; } = "{}";
        public string DoctorWorkloadJson { get; set; } = "{}";
        public string MonthlyRegistrationTrendJson { get; set; } = "{}";
        public string TopPerformingDoctorsJson { get; set; } = "{}";

        public DataTable UpcomingAppointments { get; set; } = new DataTable();
        public DataTable PendingBills { get; set; } = new DataTable();
        public DataTable RecentPatients { get; set; } = new DataTable();
        public DataTable ActiveDoctors { get; set; } = new DataTable();

        public DoctorOfMonthModel DoctorOfTheMonth { get; set; } = new DoctorOfMonthModel();
        public List<RecentRegistrationModel> RecentRegistrations { get; set; } = new List<RecentRegistrationModel>();
    }

    public class KPIModel
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TodaysAppointments { get; set; }
        public int ActiveDepartments { get; set; }
        public decimal PaidBills { get; set; }
        public decimal PartialBills { get; set; }
        public decimal UnpaidBills { get; set; }
        public decimal MonthlyRevenue { get; set; }
    }

    public class DoctorOfMonthModel
    {
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PatientCount { get; set; }
    }

    public class RecentRegistrationModel
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
    }
}
