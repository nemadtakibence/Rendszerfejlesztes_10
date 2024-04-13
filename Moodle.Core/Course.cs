using System.Collections.Generic;

namespace Moodle.Core;

public class Course{
    public string Name {get;set;}
    public string Code {get;set;}
    public string Dept {get;set;}
    public int Credit {get;set;}
    public List<string> ApprovedDegrees {get;set;}
    public List<string>? Users {get;set;}
}