using System.Text;

namespace FileOperations;

public class Csv
{
    public void WriteFirstFile(string filePath, string[] headers, Dictionary<int, List<int>> ls)
    {
        // после выхода из using автоматически вызывается sw.Close()
        // Внутри вызывается Dispose для освобождения неуправляемыми ресурсами
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(string.Join(",", headers));
            foreach (KeyValuePair<int, List<int>> data in ls)
            {
                sw.WriteLine($"{Convert.ToString(data.Key)},{string.Join(",", data.Value)}");
            }
        }
    }
    
    public void WriteSecondFile(string filePath, string[] headers, Tuple<List<int>, List<int>> ls)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(string.Join(",", headers));
            sw.WriteLine($"Female,{string.Join(",", ls.Item1)}");
            sw.WriteLine($"Male,{string.Join(",", ls.Item2)}");
        }
    }
}