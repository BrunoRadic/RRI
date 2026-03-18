// Mage.cs
// Čarobnjak s niskim HP ali visokim spell damageon

using UnityEngine;

public class Mage : Character
{
    private int _currentMana;
    private int _maxMana;

    public Mage(string name) : base(name, maxHp: 80, baseAttack: 20, baseDefense: 3)
    {
        _maxMana     = 100;
        _currentMana = 100;
    }

    public override int CalculateAttackDamage()
    {
        return BaseAttack;
    }

    // Fireball: pojačani napad koji troši manu
    // Ako nema dovoljno mane, koristi obični napad
    public override void UseSpecialAbility(Character target)
    {
        int manaCost = 40;

        if (_currentMana < manaCost)
        {
            Debug.Log($"{Name} nema dovoljno mane za Fireball! Koristi obični napad.");
            Attack(target);
            return;
        }

        _currentMana -= manaCost;
        int fireDamage = BaseAttack * 2;
        Debug.Log($"{Name} baca FIREBALL! ({fireDamage} štete, mana: {_currentMana}/{_maxMana})");
        target.TakeDamage(fireDamage);
    }

    // Regeneracija mane na kraju svakog poteza
    public override void OnTurnEnd()
    {
        _currentMana = Mathf.Min(_currentMana + 20, _maxMana);
        Debug.Log($"{Name} regenerira manu. Mana: {_currentMana}/{_maxMana}");
    }
}
