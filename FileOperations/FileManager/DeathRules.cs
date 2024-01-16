using System.Collections;

namespace FileOperations.FileManager;

public class DeathRules : IRules
{
    private List<int> _intervals;
    private List<List<double>> _deathRate;
    
    public DeathRules(string path)
    {
        
        if(!path.Contains(".csv"))
            throw new FileNotFoundException("Файл не имеет .csv расширения");
        
        if (!File.Exists(path))
            throw new FileNotFoundException("Файл не найден");
        
        if (new FileInfo(path).Length > Int32.MaxValue)
            throw new FileLoadException("Файл слишком большой");
        
        _intervals = new List<int>();
        _deathRate = new List<List<double>>();
        
        using var reader = new StreamReader(path);
        var headers = reader.ReadLine();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line?.Split(", ");

            if (values == null)
                throw new Exception($"{path} file has broken lines");
                
            if (values.Length != 4)
                throw new Exception("Файл не соотвествует стандарту");
            
            try
            {
                int interval = Convert.ToInt32(values[0]);
                double deathRateMale = Convert.ToDouble(values[2].Replace('.',','));
                double deathRateFemale = Convert.ToDouble(values[3].Replace('.',','));
                
                _intervals.Add(interval);
                _deathRate.Add(new List<double>(){deathRateMale, deathRateFemale});
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

    private List<double> GetInterval(int key)
    {
        int i = 0;
        for (; i < _intervals.Count; i++)
        {
            int num = _intervals[i];
            if (key < num)
                break;
        }
        
        return _deathRate[i-1];
    }
    
    public double Get(int key)
    {
        if (key is < 0 or > 100)
            throw new IndexOutOfRangeException("get exception");

        return GetInterval(key)[0];
    }
    
    public double Get(int key, bool male)
    {
        if (key is < 0 or > 100)
            throw new IndexOutOfRangeException("get exception");
        
        
        return GetInterval(key)[Convert.ToInt32(!male)];
    }
    
    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}