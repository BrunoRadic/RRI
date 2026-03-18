// HealthPotion.cs
// Jednokratni predmet koji obnavlja HP ciljanog lika

using UnityEngine;

public class HealthPotion : IItem
{
    public string Name        { get; private set; }
    public string Description { get; private set; }
    public float  Weight      { get; private set; }

    private int _healAmount;

    public HealthPotion(string name, int healAmount, float weight = 0.5f)
    {
        Name        = name;
        Description = $"Obnavlja {healAmount} HP.";
        Weight      = weight;
        _healAmount = healAmount;
    }

    public void Apply(Character target)
    {
        target.Heal(_healAmount);
        Debug.Log($"{target.Name} koristi '{Name}' i obnavlja {_healAmount} HP.");
    }

    public bool IsConsumed() => true;
}
