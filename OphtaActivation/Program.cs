using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OphtaActivation
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Salam, entre les credentiels:");
            var username = Console.ReadLine();
            var password = Console.ReadLine();
            var superUser = Security.ToMd5(password + username + DateTime.Today.Month + DateTime.Today.Year);
            var s = "admin";
            s = Security.ToMd5(s);
            File.WriteAllText("result.txt", "admin:" + s + "\r\n");
            s = Security.GetMacAddress();
            s = Security.ToMd5(s);
            File.AppendAllText("result.txt", "key:" + s+"\r\n");
            File.AppendAllText("result.txt", "secret:" + superUser);
            
            Console.WriteLine("Voilà les résultat de l'activation:");
            Console.WriteLine(File.ReadAllText("result.txt"));
            Console.ReadKey();
        }
    }
}
