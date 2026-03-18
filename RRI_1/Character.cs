// Character.cs

using UnityEngine;

public abstract class Character
{
    public string Name        { get; private set; }
    public int    MaxHp       { get; private set; }
    public int    CurrentHp   { get; private set; }
    public bool   IsAlive     => CurrentHp > 0;

    protected int BaseAttack  { get; private set; }
    public int BaseDefense { get; protected set; }

    public Inventory Inventory { get; private set; }

    protected Character(string name, int maxHp, int baseAttack, int baseDefense, int inventorySlots = 4)
    {
        Name        = name;
        MaxHp       = maxHp;
        CurrentHp   = maxHp;
        BaseAttack  = baseAttack;
        BaseDefense = baseDefense;
        Inventory   = new Inventory(inventorySlots);
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        CurrentHp = Mathf.Min(CurrentHp + amount, MaxHp);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        int reduced = Mathf.Max(damage - BaseDefense, 1);
        CurrentHp = Mathf.Max(CurrentHp - reduced, 0);
        Debug.Log($"{Name} prima {reduced} štete (ATK: {damage}, DEF: {BaseDefense}). HP: {CurrentHp}/{MaxHp}");

        if (CurrentHp == 0)
            Debug.Log($"{Name} je eliminiran!");
    }

    // DamageBuff.Apply()
    public void ApplyPermanentBonus(int attackBonus, int defenseBonus)
    {
        BaseAttack  += attackBonus;
        BaseDefense += defenseBonus;
    }

    public void Attack(Character target)
    {
        int damage = CalculateAttackDamage();
        Debug.Log($"{Name} napada {target.Name}!");
        target.TakeDamage(damage);
    }

    public abstract int CalculateAttackDamage();

    public abstract void UseSpecialAbility(Character target);

    public abstract void OnTurnEnd();

    public void PrintStatus()
    {
        Debug.Log($"[{Name}] HP: {CurrentHp}/{MaxHp} | ATK: {BaseAttack} | DEF: {BaseDefense}");
    }
}
