using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Moodle.Data.Password{
    public static class Password{
        public static bool VerifyPassword(string userName, string realHash, string passToVerify){
            byte[] bytesToVerify = Encoding.UTF8.GetBytes(userName+passToVerify);
            byte[] hashOutput;
            using(SHA256 sha256 = SHA256.Create()){
                hashOutput = sha256.ComputeHash(bytesToVerify);
            }
            string hash = Convert.ToBase64String(hashOutput);
            if(realHash == hash){
                return true;
            }
            return false;
        }
    }
}