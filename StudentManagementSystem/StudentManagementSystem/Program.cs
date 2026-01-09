using System;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem
{
    class Program
    {
        static string connectionString = @"Server=LAPTOP-0K5QK01J\SQLEXPRESS;Database=StudentsDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.WriteLine("======= Student Management System =====");

            // OPTIONAL: Add sample admin if it doesn't exist
            AddSampleAdmin();

            if (Login(out int roleId))
            {
                ShowMenu(roleId);
            }
            else
            {
                Console.WriteLine("Login failed. Exiting...");
            }
        }

        // -------------------- LOGIN --------------------
        static bool Login(out int roleId)
        {
            roleId = 0;
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT PasswordHash, PasswordSalt, RoleID FROM Users WHERE Username=@username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Check for null values
                    if (reader["PasswordHash"] == DBNull.Value || reader["PasswordSalt"] == DBNull.Value)
                    {
                        Console.WriteLine("This user does not have a password set.");
                        return false;
                    }

                    byte[] storedHash = (byte[])reader["PasswordHash"];
                    byte[] storedSalt = (byte[])reader["PasswordSalt"];
                    roleId = (int)reader["RoleID"];

                    if (PasswordHelper.VerifyPassword(password, storedHash, storedSalt))
                        return true;
                }
            }

            Console.WriteLine("Invalid username or password.");
            return false;
        }

        // -------------------- MENU --------------------
        static void ShowMenu(int roleId)
        {
            while (true)
            {
                Console.WriteLine("\n1. View Students");
                if (roleId == 1 || roleId == 2) // Admin or Teacher
                {
                    Console.WriteLine("2. Add Student");
                    Console.WriteLine("3. Update Student");
                }
                if (roleId == 1) // Admin only
                {
                    Console.WriteLine("4. Delete Student");
                }
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StudentOperations.ViewStudents();
                        break;
                    case "2":
                        if (roleId <= 2) StudentOperations.AddStudent();
                        break;
                    case "3":
                        if (roleId <= 2) StudentOperations.UpdateStudent();
                        break;
                    case "4":
                        if (roleId == 1) StudentOperations.DeleteStudent();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        // -------------------- SAMPLE ADMIN --------------------
        static void AddSampleAdmin()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username='admin'";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);

                con.Open();
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    string password = "admin123";
                    PasswordHelper.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

                    string insertQuery = "INSERT INTO Users (Username, PasswordHash, PasswordSalt, RoleID) VALUES (@username, @hash, @salt, @role)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@username", "admin");
                    insertCmd.Parameters.AddWithValue("@hash", hash);
                    insertCmd.Parameters.AddWithValue("@salt", salt);
                    insertCmd.Parameters.AddWithValue("@role", 1);

                    insertCmd.ExecuteNonQuery();
                    Console.WriteLine("Sample admin created: Username='admin', Password='admin123'");
                }
            }
        }
    }
}


