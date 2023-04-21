using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scuftify {
    internal class Login {


        string username;
        //users
        Dictionary<string, string> accounts = new() {
                {"joran", "enja"},
                {"thijmen", "claudia"},
                {"ili", "mirza"},
                {"kien", "kein"}
        };
        public string login() {
            //login
            username = "";
            //doe dit zolang de username en password niet kloppen
            while (true) {
                bool breaky = false;
                // vraag om username en password
                Console.Write("username: ");
                string user = Console.ReadLine();
                Console.Write("password: ");
                string pass = string.Empty;
                ConsoleKey key;
                do {
                    // zorg dat de password niet zichtbaar is
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && pass.Length > 0) {
                        Console.Write("\b \b");
                        pass = pass[0..^1];
                    } else if (!char.IsControl(keyInfo.KeyChar)) {
                        Console.Write("*");
                        pass += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);
                Console.WriteLine();
                // kijk of de username en password kloppen
                foreach (var i in accounts) {
                    if (user == i.Key && pass == i.Value) {
                        username = user;
                        breaky = true;
                        break;
                    }
                }
                // als de username en password kloppen, ga verder
                if (breaky) {
                    Console.Clear();
                    break;
                }

            }
            return username;
        }
    }
}
