using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace data_nm
{
    public class approved_degrees
    {
        public int id { get; }
        public int course_id { get; }
        public int degree_id { get; }
        public approved_degrees(int id, int course_id, int degree_id)
        {
            this.id = id;
            this.course_id = course_id;
            this.degree_id = degree_id;
        }
        public approved_degrees() { }
        public List<approved_degrees> approved_degrees_in()
        {
            List<approved_degrees> ad = new List<approved_degrees>();
            int id = 0, course_id = 0, degree_id = 0;
            string connectionString = "Data Source=moodleDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM approved_degrees";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            course_id = Convert.ToInt32(reader["course_id"]);
                            degree_id = Convert.ToInt32(reader["degree_id"]);
                            ad.Add(new approved_degrees(id, course_id, degree_id));
                        }
                    }
                }
                connection.Close();
            }
            return ad;
        }
        public void approved_degrees_update()
        {
            string connectionString = "Data Source=moodleDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE approved_degrees SET course_id = @course_id WHERE id = @id";
                string updateQuery2 = "UPDATE approved_degrees SET degree_id = @degree_id WHERE id = @id";

                int idToUpdate = int.Parse(Console.ReadLine());
                int new_course_id = int.Parse(Console.ReadLine());
                int new_degree_id = int.Parse(Console.ReadLine());

                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@course_id", new_course_id);

                    updateCommand.Parameters.AddWithValue("@id", idToUpdate);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Az adatbázisban {rowsAffected} rekord frissítve lett.");
                    }
                    else
                    {
                        Console.WriteLine("Nincs megfelelő rekord az adatbázisban a frissítéshez.");
                    }
                }
                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery2, connection))
                {
                    updateCommand.Parameters.AddWithValue("@degree_id", new_degree_id);
                    updateCommand.Parameters.AddWithValue("@id", idToUpdate);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Az adatbázisban {rowsAffected} rekord frissítve lett.");
                    }
                    else
                    {
                        Console.WriteLine("Nincs megfelelő rekord az adatbázisban a frissítéshez.");
                    }
                }

                connection.Close();
            }
        }
    }
    public class courses
    {
        public int id { get; }
        public string name { get; }
        public string code { get; }
        public string department { get; }
        public int credit { get; }
        public courses(int id, string name, string code, string department, int credit)
        {
            this.id = id;
            this.name = name;
            this.code = code;
            this.department = department;
            this.credit = credit;
        }

        public courses() { }
        public List<courses> courses_in()
        {
            List<courses> ad = new List<courses>();
            int id = 0, credit = 0;
            string name, code, department;
            string connectionString = "Data Source=moodleDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM courses";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = reader["name"].ToString();
                            code = reader["code"].ToString();
                            department = reader["department"].ToString();
                            credit = Convert.ToInt32(reader["credit"]);
                            ad.Add(new courses(id, name, code, department, credit));
                        }
                    }
                }
                connection.Close();
            }
            return ad;
        }
    }
    public class degrees
    {
        public int id { get; }
        public string name { get; }
        public degrees(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public degrees() { }
        public List<degrees> degrees_in()
        {
            List<degrees> ad = new List<degrees>();
            int id = 0;
            string name;
            string connectionString = "Data Source=moodleDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM degrees";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = reader["name"].ToString();
                            ad.Add(new degrees(id, name));
                        }
                    }
                }
                connection.Close();
            }
            return ad;
        }
    }
    public class events
    {
        public int id { get; }
        public int course_id { get; }
        public string name { get; }
        public string description { get; }
        public string date { get; }
        public events(int id, int course_id, string name, string description, string date)
        {
            this.id = id;
            this.course_id = course_id;
            this.name = name;
            this.description = description;
            this.date = date;
        }

        public events() { }
        public List<events> events_in()
        {
            List<events> ad = new List<events>();
            int id = 0, course_id = 0;
            string name, description, date;
            string connectionString = "Data Source=moodleDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM events";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            course_id = Convert.ToInt32(reader["course_id"]);
                            name = reader["name"].ToString();
                            description = reader["description"].ToString();
                            date = reader["date"].ToString();
                            //Console.WriteLine($"ID: {id}, Course ID: {course_id}, Name: {name}");
                            ad.Add(new events(id, course_id, name, description, date));
                        }
                    }
                }
                connection.Close();
            }
            return ad;
        }
    }
    public class mycourses
    {
        public int id { get; }
        public int course_id { get; }
        public int user_id { get; }
        public mycourses(int id, int course_id, int user_id)
        {
            this.id = id;
            this.course_id = course_id;
            this.user_id = user_id;
        }
        public mycourses() { }
        public List<mycourses> mycourses_in()
        {
            List<mycourses> ad = new List<mycourses>();
            int id = 0, course_id = 0, user_id = 0;
            string connectionString = "Data Source=moodleDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM mycourses";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            course_id = Convert.ToInt32(reader["course_id"]);
                            user_id = Convert.ToInt32(reader["user_id"]);
                            ad.Add(new mycourses(id, course_id, user_id));
                        }
                    }
                }
                connection.Close();
            }
            return ad;
        }
    }
    public class users
    {
        public int id { get; }
        public string name { get; }
        public string username { get; }
        public string password { get; }
        public int degree_id { get; }
        public users(int id, string name, string username, string password, int degree_id)
        {
            this.id = id;
            this.name = name;
            this.username = username;
            this.password = password;
            this.degree_id = degree_id;
        }

        public users() { }
        public List<users> users_in()
        {
            List<users> ad = new List<users>();
            int id = 0, degree_id = 0;
            string name, username, password;
            string connectionString = "Data Source=moodleDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = reader["name"].ToString();
                            username = reader["username"].ToString();
                            password = reader["password"].ToString();
                            degree_id = Convert.ToInt32(reader["degreee_id"]);
                            ad.Add(new users(id, name, username, password, degree_id));
                        }
                    }
                }
                connection.Close();
            }
            return ad;
        }
    }
}
