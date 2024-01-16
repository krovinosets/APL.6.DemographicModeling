using FileOperations.FileManager;

namespace Demographic.Person;

public class Person : IPerson
{
    protected readonly int BirthDate;
    protected int? DeathDate;
    protected bool Dead;
    protected readonly YearTickEvent EventSource;

    protected Person(int birthDate, YearTickEvent yearTickEvent)
    {
        BirthDate = birthDate;
        EventSource = yearTickEvent;
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        EventSource.YearTick += NextCycle;
    }

    private void DeregisterEvent()
    {
        EventSource.YearTick -= NextCycle;
    }
    
    public virtual void NextCycle(int currentDate, DeathRules rule)
    {
        double rate = rule.Get(Age(currentDate), this is Male);
        if (ProbabilityCalculator.IsEventHappened(rate))
        {
            SetDeath(currentDate);
            DeregisterEvent();
        }
    }

    public bool IsDead() => Dead;

    public bool SetDeath(int deathDate)
    {
        if (deathDate <= 0)
            return false;

        DeathDate = deathDate;
        Dead = true;
        return true;
    }

    public int Age(int date) => (date - BirthDate);
}