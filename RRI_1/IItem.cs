// IItem.cs
// Sučelje za sve predmete u igri

public interface IItem
{
    string Name { get; }
    string Description { get; }
    float Weight { get; }

    void Apply(Character target);
    bool IsConsumed();
}
