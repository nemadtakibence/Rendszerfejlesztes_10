using Microsoft.Data.Sqlite;
using System;
using System.Data.SQLite;
using data_nm;
using System.Text;

namespace MainProgram
{
    public class main
    {
        public static void Main(string[] args)
        {

            approved_degrees ad = new approved_degrees();
            List<approved_degrees> app_deg = ad.approved_degrees_in();
            //ad.approved_degrees_update();

            courses cr = new courses();
            List<courses> courses = cr.courses_in();

            degrees dr = new degrees();
            List<degrees> degrees = dr.degrees_in();

            events ev = new events();
            List<events> events = ev.events_in();

            mycourses mc = new mycourses();
            List<mycourses> mycourses = mc.mycourses_in();

            users u = new users();
            List<users> users = u.users_in();



            //foreach (var degrees in app_deg)
            //{
            //    Console.WriteLine(degrees.id + " " + degrees.course_id + " " + degrees.degree_id);
            //
        }
    }
}
