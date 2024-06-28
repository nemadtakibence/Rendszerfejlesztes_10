using System;
using System.Collections.Generic;
using Moodle.Data;
using Moodle.Core.Roles;

public static class ACL
{
    private static Dictionary<string, string> accessControlList = new Dictionary<string, string>();
    private static bool initialized=false;

    /*public ACL()
    {
        accessControlList = new Dictionary<string, List<string>>();
    }*/
    public static void AddUser(string userName)
    {
        if (!accessControlList.ContainsKey(userName))
        {
            accessControlList.Add(userName, "");
        }
    }

    public static void AddPermission(string userName, string permission)
    {
        if (accessControlList.ContainsKey(userName))
        {
            accessControlList[userName]=permission;
        }
        else
        {
            throw new Exception("User not found in ACL.");
        }
    }

    public static bool HasPermission(string userName, string permission)
    {
        if (accessControlList.ContainsKey(userName))
        {
            return accessControlList[userName]==permission;
        }
        else
        {
            throw new Exception("User not found in ACL.");
        }
    }
    public static string GetPermission(string userName){
        if(accessControlList[userName]!=null){
            return accessControlList[userName];
        }
        return "xd";
    }
    public static void InitializeList(MoodleDbContext context){
        if(!initialized){
            int lectId = context.Degrees.SingleOrDefault(x => x.Name == "Oktató").Id;  
            foreach(var user in context.Users){
                if(user.Degree_Id==lectId){                    
                    ACL.AddUser(user.Username);
                    ACL.AddPermission(user.Username, Roles.Teacher);
                }
                else{
                    ACL.AddUser(user.Username);
                    ACL.AddPermission(user.Username, Roles.Student);
                }
                Console.WriteLine(user.Username);
                Console.WriteLine(ACL.accessControlList.ToString);
            }
            initialized=true;
        }
    }
}
