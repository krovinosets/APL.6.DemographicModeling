using Demographic.Person;
using FileOperations;
using FileOperations.FileManager;

namespace Demographic;

public class Engine : IEngine
{
    private const int PeopleGroupAmount = 1000;
    private readonly int _population;
    private readonly int _startDate;
    private readonly int _endDate;
    private readonly InitialAge _initialAge;
    private readonly DeathRules _deathRules;
    private List<IPerson> _children;

    private readonly YearTickEvent _event;
    
    public Engine(string initialAgePath, 
                  string deathRulesPath, int startDate = 1970, 
                  int endDate = 2021, 
                  int population = 130000000)
    {
        _event = new YearTickEvent();
        _children = new List<IPerson>();
        _startDate = startDate;
        _endDate = endDate;
        _population = population;
        _initialAge = new InitialAge(initialAgePath);
        _deathRules = new DeathRules(deathRulesPath);
    }
    
    
    /// <summary>
    /// Нормализатор групп соотношений
    /// </summary>
    /// <returns>Словарь из [ID] = количество</returns>
    private Dictionary<int, int> GetGroups()
    {
        Dictionary<int, int> groups = new Dictionary<int, int>();
        foreach (KeyValuePair<int, double> line in _initialAge)
        {
            int amountGroup = (int)(Math.Round(line.Value / 2.0) * 2);
            groups.Add(line.Key, amountGroup);
        }
        
        return groups;
    }
    
    /// <summary>
    /// Создание начальных групп людей относительно PeopleGroupAmount
    /// </summary>
    /// <returns></returns>
    private List<IPerson> PeopleGenerator()
    {
        List<IPerson> people = new List<IPerson>();
        int half = (int) Math.Floor((double)_population / 2) / PeopleGroupAmount;
        for (var i = 0; i < half; i += PeopleGroupAmount)
        {
            var groups = GetGroups();
            foreach (KeyValuePair<int, int> line in groups)
            {
                for (int n = 0; n < line.Value; n++)
                {
                    int birthDate = _startDate - line.Key;
                    Female female = new Female(birthDate, _event);
                    Male male = new Male(birthDate, _event);
                    people.Add(male);
                    people.Add(female);
                    female.ChildBirth += RegisterEvent;
                }
            }
        }

        return people;
    }
    
    /// <summary>
    /// Подписка на ивент
    /// </summary>
    /// <param name="child"></param>
    private void RegisterEvent(IPerson child)
    {
        if (child is Female fm)
        {
            fm.ChildBirth += RegisterEvent;
        }
        _children.Add(child);
    }
    
    private void DeregisterEvent(IPerson person)
    {
        if (person is Female fm && person.IsDead())
        {
            fm.ChildBirth -= RegisterEvent;
        }
    }
    
    public Data Model()
    {
        List<IPerson> people = PeopleGenerator();
        Data data = new Data(_startDate, people);
        
        for (int date = _startDate; date <= _endDate; date++)
        {
            _event.OnEvent(date, _deathRules);
            
            Array.ForEach(people.ToArray(), person => DeregisterEvent(person));
            int died = people.RemoveAll(person => person.IsDead());
            people.AddRange(_children);

            Tuple<int, int> sexCounter = data.CountSex();
            data.DataFirst.Add(date + 1, new List<int>()
            {
                people.Count, sexCounter.Item1, sexCounter.Item2
            });
            
            Console.WriteLine($"Год - {date}, Население - {people.Count * PeopleGroupAmount}, Родилось - {_children.Count}, Умерло - {died}");
            _children.Clear();
        }
        data.CountAges(_endDate);
        double percent = Math.Round(((100.0 * (people.Count * PeopleGroupAmount)) / _population) - 100.0, 3);
        Console.WriteLine($"Итоговое население - {people.Count * PeopleGroupAmount}");
        Console.WriteLine($"Было в начале - {_population}");
        Console.WriteLine($"Разница: {Math.Abs(people.Count * PeopleGroupAmount - _population)} ({percent}%)");
        return data;
    }
}