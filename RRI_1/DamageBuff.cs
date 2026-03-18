// DamageBuff.cs

using UnityEngine;

public class DamageBuff : IItem
{
    public string Name        { get; private set; }
    public string Description { get; private set; }
    public float  Weight      { get; private set; }

    private int  _attackBonus;
    private int  _defenseBonus;
    private bool _applied;

    public DamageBuff(string name, int attackBonus, int defenseBonus, float weight = 2f)
    {
        Name          = name;
        _attackBonus  = attackBonus;
        _defenseBonus = defenseBonus;
        Weight        = weight;
        Description   = $"Trajno: +{attackBonus} ATK, +{defenseBonus} DEF.";
        _applied      = false;
    }

    public void Apply(Character target)
    {
        if (_applied)
        {
            Debug.Log($"'{Name}' je već primijenjen.");
            return;
        }

        target.ApplyPermanentBonus(_attackBonus, _defenseBonus);
        _applied = true;
        Debug.Log($"{target.Name} opremio '{Name}': +{_attackBonus} ATK, +{_defenseBonus} DEF.");
    }

    public bool IsConsumed() => false;
}
