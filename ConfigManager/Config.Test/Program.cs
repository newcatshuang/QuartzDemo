using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Config.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration Configuration = builder.Build();
            var sss = Configuration.GetConnectionString("des");
            Console.WriteLine($"option1 = {Configuration["Option1"]}");
            Console.WriteLine($"option2 = {Configuration["option2"]}");
            Console.WriteLine($"suboption1 = {Configuration["subsection:suboption1"]}");
            Console.WriteLine();
            Console.WriteLine("Wizards:");
            Console.Write($"{Configuration["wizards:0:Name"]}, ");
            Console.WriteLine($"age {Configuration["wizards:0:Age"]}");
            Console.Write($"{Configuration["wizards:1:Name"]}, ");
            Console.WriteLine($"age {Configuration["wizards:1:Age"]}");
            Console.WriteLine();
            Console.WriteLine("Press a key...");
            Console.ReadKey();
        }
    }
}
