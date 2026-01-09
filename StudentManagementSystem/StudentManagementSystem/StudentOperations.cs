using System;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace StudentManagementSystem
{
    public static class StudentOperations
    {
        static string connectionString = @"Server=LAPTOP-0K5QK01J\SQLEXPRESS;Database=StudentsDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // -------------------- VIEW STUDENTS --------------------
        public static void ViewStudents()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Students";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\nID\tName\tAge\tEmail\tCourse");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["StudentID"]}\t{reader["FullName"]}\t{reader["Age"]}\t{reader["Email"]}\t{reader["Course"]}");
                }
            }
        }

        // -------------------- ADD STUDENT --------------------
        public static void AddStudent()
        {
            Console.Write("Full Name: ");
            string name = Console.ReadLine();

            Console.Write("Age: ");
            if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0)
            {
                Console.WriteLine("Invalid age!");
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine();
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Console.WriteLine("Invalid email!");
                return;
            }

            Console.Write("Course: ");
            string course = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Students (FullName, Age, Email, Course) VALUES (@name, @age, @email, @course)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@course", course);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Student added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // -------------------- UPDATE STUDENT --------------------
        public static void UpdateStudent()
        {
            Console.Write("Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("Full Name: ");
            string name = Console.ReadLine();

            Console.Write("Age: ");
            if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0)
            {
                Console.WriteLine("Invalid age!");
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine();
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Console.WriteLine("Invalid email!");
                return;
            }

            Console.Write("Course: ");
            string course = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Students SET FullName=@name, Age=@age, Email=@email, Course=@course WHERE StudentID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@course", course);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Student updated successfully!" : "Student not found.");
            }
        }

        // -------------------- DELETE STUDENT --------------------
        public static void DeleteStudent()
        {
            Console.Write("Enter Student ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Students WHERE StudentID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Student deleted successfully!" : "Student not found.");
            }
        }
    }
}
