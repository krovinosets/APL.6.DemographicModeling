using Demographic.Person;

namespace Demographic;

public class Data
{
    private readonly List<IPerson> _people;
    public Dictionary<int, List<int>> DataFirst;
    public Tuple<List<int>, List<int>> DataSecond = null!; // null! - точно не будет нуллом, обещаем программе (not nullable)

    public Data(int startDate, List<IPerson> people)
    {
        _people = people;

        Tuple<int, int> sexCounter = CountSex();
        DataFirst = new Dictionary<int, List<int>>()
        {
            {startDate, new List<int>(){_people.Count, sexCounter.Item1, sexCounter.Item2}}
        };
    }

    public void CountAges(int endDate)
    {
        List<int> femalesAges = new List<int>(){0, 0, 0, 0};
        List<int> malesAges = new List<int>(){0, 0, 0, 0};
        foreach (var person in _people)
        {
            int age = person.Age(endDate);
            int i = 0;
            if (age is >= 19 and <= 44)
                i = 1;
            else if (age is >= 45 and <= 65)
                i = 2;
            else if (age is >= 66 and <= 100)
                i = 3;
            if (person is Male)
                malesAges[i]++;
            else
                femalesAges[i]++;
        }

        DataSecond = new Tuple<List<int>, List<int>>(femalesAges, malesAges);
    }
    
    public Tuple<int, int> CountSex()
    {
        int females = 0;
        int males = 0;
        foreach (var person in _people)
        {
            if (person is Male)
                males++;
            else
                females++;
        }

        return new Tuple<int, int>(females, males);
    }
}