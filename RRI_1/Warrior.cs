// Warrior.cs
// Melee borac s visokim HP i obranom.

using UnityEngine;

public class Warrior : Character
{
    private bool _isBlocking;

    public Warrior(string name) : base(name, maxHp: 120, baseAttack: 15, baseDefense: 8) { }

    public override int CalculateAttackDamage()
    {
        return BaseAttack;
    }

    // Shield Block: aktivira pojačanu obranu za jedan potez
    public override void UseSpecialAbility(Character target)
    {
        _isBlocking  = true;
        BaseDefense += 8;
        Debug.Log($"{Name} koristi SHIELD BLOCK! Obrana povećana na {BaseDefense}.");
    }

    // Blok traje samo jedan potez
    public override void OnTurnEnd()
    {
        if (_isBlocking)
        {
            BaseDefense -= 8;
            _isBlocking  = false;
            Debug.Log($"{Name} spušta štit. Obrana vraćena na {BaseDefense}.");
        }
    }
}
