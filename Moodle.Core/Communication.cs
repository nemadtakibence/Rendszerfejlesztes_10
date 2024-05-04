//"kommunikációs objektumok", ezek az API végpontokban kerülnek felhasználásra
namespace Moodle.Core.Communication{
    public class LoginData{
        public string Username{get;set;}
        public string Password{get;set;}
    }
    public class NewCourse{
        public string Username{get;set;}
        public string Name{get;set;}
        public string Code{get;set;}
        public string Department{get;set;}
        public int Credit{get;set;}
    }
    public class NewEvent{
        public string Username{get;set;}
        public string CourseName{get;set;}
        public string Name{get;set;}
        public string Desc{get;set;}
        public DateTime Date{get;set;}
    }
}