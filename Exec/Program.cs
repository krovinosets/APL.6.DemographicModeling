using Demographic;
using FileOperations;

namespace Exec;

public static class Program
{
    public static void Main(string[] args)
    {
        string initialAgePath = @"C:\\Users\\andre\\OneDrive\\libs\\csv\\InitialAge.csv";
        string deathRulesPath = @"C:\\Users\\andre\\OneDrive\\libs\\csv\\DeathRules.csv";
        int startDate = 1970;
        int endDate = 2021;
        int population = 130000000;
        string outputFileCommon = @"C:\\Users\\andre\\OneDrive\\libs\\csv\\new1.csv";
        string outputFileAges = @"C:\\Users\\andre\\OneDrive\\libs\\csv\\new2.csv";
        
        Console.WriteLine(args[0]);
        
        try
        {
            IEngine engine = new Engine(initialAgePath, deathRulesPath, startDate, endDate, population);
            Data data = engine.Model();
            
            SaveData(outputFileCommon, outputFileAges, data);
        } 
        catch(Exception e)
        {
            Console.WriteLine("Ошибка при запуске программы");
            Console.WriteLine(e.Message);
        }
    }

    public static void SaveData(string outputFileCommon, string outputFileAges, Data data)
    {
        Csv csv = new Csv();
        csv.WriteFirstFile(outputFileCommon, new[] { "Year", "Population", "female", "male" }, data.DataFirst);
        csv.WriteSecondFile(outputFileAges, new[] { "Sex", "0-18", "19-45", "45-65", "65-100" }, data.DataSecond);
    }
}
