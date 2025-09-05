using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Praticse.Helpers;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
 
    public class UserController : Controller
    {
        private IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Users_ValidateLogin";
                    sqlCommand.Parameters.Add("@LoginIdentifier", SqlDbType.VarChar).Value = userLoginModel.@LoginIdentifier;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                            HttpContext.Session.SetString("EmailAddress", dr["Email"].ToString());
                        }

                        return RedirectToAction("Index", "dashboard");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User is not found";
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }


        public IActionResult UserRegistration(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Users_Insert";

                    // Add parameters
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = userRegistrationModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = userRegistrationModel.Password;
                    sqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = userRegistrationModel.Email;
                    sqlCommand.Parameters.Add("@MobileNo", SqlDbType.NVarChar, 100).Value = userRegistrationModel.MobileNo;
                    sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userRegistrationModel.IsActive;

                    // Execute the stored procedure
                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    if (rowsAffected > 0)
                    {
                        TempData["SuccessMessage"] = "User registered successfully!";
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Registration failed. Please try again.";
                        return View(userRegistrationModel);
                    }
                }
                else
                {
                    // Model validation failed
                    return View(userRegistrationModel);
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL specific exceptions
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Unique constraint violation
                {
                    TempData["ErrorMessage"] = "Username or Email already exists. Please choose different credentials.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Database error occurred. Please try again later.";
                }
                return View(userRegistrationModel);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred: " + e.Message;
                return View(userRegistrationModel);
            }
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View(new UserRegistrationModel());
        }

        [HttpPost]
        public IActionResult Registration(UserRegistrationModel userRegistrationModel)
        {
            return UserRegistration(userRegistrationModel);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }



        public IActionResult Index()
        {

            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Users_SelectAll";

            connection.Open();
            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return View(table);

        }

         
        
        int? userid = CommonVariable.UserID();


        [CheckAccess]

        public IActionResult Profile()
        {
            try
            {
                // Get current user ID from session or context (assuming UserID = 1 for demo)
                int? currentUserID = userid; // You would get this from your authentication system

                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using var connection = new SqlConnection(connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Users_SelectByPK";
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = currentUserID;

                connection.Open();
                using var reader = command.ExecuteReader();
                var table = new DataTable();
                table.Load(reader);

                if (table.Rows.Count > 0)
                {
                    var user = new UsersModel
                    {
                        UserID = Convert.ToInt32(table.Rows[0]["UserID"]),
                        UserName = table.Rows[0]["UserName"].ToString(),
                        Password = table.Rows[0]["Password"].ToString(),
                        Email = table.Rows[0]["Email"].ToString(),
                        MobileNo = table.Rows[0]["MobileNo"].ToString(),
                        IsActive = Convert.ToBoolean(table.Rows[0]["IsActive"]),
                        Created = Convert.ToDateTime(table.Rows[0]["Created"]),
                        Modified = Convert.ToDateTime(table.Rows[0]["Modified"])
                    };
                    return View(user);
                }
                else
                {
                    TempData["ErrorMessage"] = "User profile not found.";
                    return View(new UsersModel());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the profile.";
                Console.WriteLine(ex.ToString());
                return View(new UsersModel());
            }
        }

        [CheckAccess]
        [HttpPost]
        public IActionResult UpdateProfile(UsersModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "PR_Users_UpdateByPK";
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;
                            command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userModel.UserName;
                            command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = userModel.Password;
                            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = userModel.Email;
                            command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = userModel.MobileNo;
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;
                            command.ExecuteNonQuery();
                        }
                    }
                    TempData["SuccessMessage"] = "Profile updated successfully.";
                    return RedirectToAction("Profile");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the profile.";
                Console.WriteLine(ex.ToString());
            }
            return View("Profile", userModel);
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                // Get current user ID from session or context
                int? currentUserID = userid; // You would get this from your authentication system

                if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    TempData["ErrorMessage"] = "All password fields are required.";
                    return RedirectToAction("Profile");
                }

                if (newPassword != confirmPassword)
                {
                    TempData["ErrorMessage"] = "New password and confirm password do not match.";
                    return RedirectToAction("Profile");
                }

                // Verify current password
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // First verify current password
                    using (SqlCommand verifyCommand = connection.CreateCommand())
                    {
                        verifyCommand.CommandType = CommandType.StoredProcedure;
                        verifyCommand.CommandText = "PR_Users_SelectByPK";
                        verifyCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = currentUserID;

                        using var reader = verifyCommand.ExecuteReader();
                        var table = new DataTable();
                        table.Load(reader);

                        if (table.Rows.Count == 0 || table.Rows[0]["Password"].ToString() != currentPassword)
                        {
                            TempData["ErrorMessage"] = "Current password is incorrect.";
                            return RedirectToAction("Profile");
                        }
                    }

                    // Update password
                    using (SqlCommand updateCommand = connection.CreateCommand())
                    {
                        updateCommand.CommandType = CommandType.Text;
                        updateCommand.CommandText = "UPDATE Users SET Password = @NewPassword, Modified = GETDATE() WHERE UserID = @UserID";
                        updateCommand.Parameters.Add("@NewPassword", SqlDbType.NVarChar).Value = newPassword;
                        updateCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = currentUserID;
                        updateCommand.ExecuteNonQuery();
                    }
                }

                TempData["SuccessMessage"] = "Password changed successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while changing the password.";
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Profile");
        }


        [HttpPost]
        public IActionResult Save(UsersModel user)
        {
            try
            {
                if (user.profileImg == null || user.profileImg.Length == 0)
                {
                    TempData["ErrorMessage"] = "Please select an image file to upload.";
                    return RedirectToAction("Profile");
                }

                // Validate file size (5MB)
                if (user.profileImg.Length > 5 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "File size must be less than 5MB.";
                    return RedirectToAction("Profile");
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(user.profileImg.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["ErrorMessage"] = "Only JPG, PNG, and GIF files are allowed.";
                    return RedirectToAction("Profile");
                }

                // Save image with fixed name "profile" (will override existing)
                string filePath = ImageHelper.SaveImageWithFixedName(user.profileImg, "Profile", "profile");

                Console.WriteLine($"File path: {filePath}");
                Console.WriteLine($"User name: {user.UserName}");
                Console.WriteLine($"User ID: {user.UserID}");
                Console.WriteLine($"Image saved successfully and will override previous image");

                TempData["SuccessMessage"] = "Profile image uploaded successfully! Your profile picture has been updated.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error uploading image: {ex.Message}";
                Console.WriteLine($"Upload error: {ex.Message}");
            }

            return RedirectToAction("Profile");
        }

    }
}
