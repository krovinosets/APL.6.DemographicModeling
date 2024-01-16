using FileOperations.FileManager;

namespace Demographic.Person;

public class Female : Person
{
    public delegate void Notify(IPerson child);
    public event Notify? ChildBirth;
    
    public Female(int birthDate, YearTickEvent yearTickEvent) : base(birthDate, yearTickEvent) { }

    public override void NextCycle(int currentDate, DeathRules rule)
    {
        GiveBirth(currentDate);
        base.NextCycle(currentDate, rule);
    }
    
    private void GiveBirth(int currentDate)
    {
        if (Dead || (currentDate - BirthDate) is < 18 or > 45)
            return;

        if (!ProbabilityCalculator.IsEventHappened(0.151))
            return;

        IPerson child;
        if (ProbabilityCalculator.IsEventHappened(0.55))
            child = new Female(currentDate, EventSource);
        else
            child = new Male(currentDate, EventSource);
        ChildBirth?.Invoke(child);
    }
}