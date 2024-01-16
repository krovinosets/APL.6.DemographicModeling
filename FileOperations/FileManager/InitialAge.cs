using System.Collections;

namespace FileOperations.FileManager;

public class InitialAge : IRules
{
    private Dictionary<int, double> _ages;
    
    public InitialAge(string path)
    {

        if(!path.Contains(".csv"))
            throw new FileNotFoundException("Файл не имеет .csv расширения");
        
        if (!File.Exists(path))
            throw new FileNotFoundException("Файл не найден");
        
        if (new FileInfo(path).Length > Int32.MaxValue)
            throw new FileLoadException("Файл слишком большой");
        
        _ages = new Dictionary<int, double>();
        
        using var reader = new StreamReader(path);
        var headers = reader.ReadLine();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line?.Split(", ");

            if (values == null)
                throw new Exception($"{path} файл содержит поломанные заголовки");

            if (values.Length != 2)
                throw new Exception("Файл не соотвествует стандарту");
            
            try
            {
                int key = Convert.ToInt32(values[0]);
                double val = Convert.ToDouble(values[1].Replace('.',','));
                // setlocale
                _ages.Add(key, val);
            }
            catch (FormatException)
            {
                throw new FormatException("Input string is not a sequence of digits.");
            }
            catch (OverflowException)
            {
                throw new OverflowException("The number cannot fit in an Int32.");
            }
        }
    }
    
    public double Get(int key)
    {
        return _ages[key];
    }

    public IEnumerator GetEnumerator()
    {
        return _ages.GetEnumerator();
    }
}