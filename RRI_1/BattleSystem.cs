// BattleSystem.cs
// Upravlja logikom borbe između dva Character objekta

using UnityEngine;

public class BattleSystem
{
    private const int MaxTurns = 15;

    public Character SimulateBattle(Character a, Character b)
    {
        Debug.Log($"\n>>> BORBA: {a.Name} vs {b.Name} <<<");
        a.PrintStatus();
        b.PrintStatus();
        Debug.Log("");

        int turn = 1;

        while (a.IsAlive && b.IsAlive && turn <= MaxTurns)
        {
            Debug.Log($"--- Potez {turn} ---");

            // Svaki treći potez koristi posebnu sposobnost
            if (turn % 3 == 0)
            {
                Debug.Log($"[SPECIAL] {a.Name}:");
                a.UseSpecialAbility(b);
            }
            else
            {
                a.Attack(b);
            }

            if (!b.IsAlive) break;

            if (turn % 3 == 0)
            {
                Debug.Log($"[SPECIAL] {b.Name}:");
                b.UseSpecialAbility(a);
            }
            else
            {
                b.Attack(a);
            }

            // Koristi health potion ako HP padne ispod 40%
            if (a.IsAlive && (float)a.CurrentHp / a.MaxHp < 0.4f)
                a.Inventory.UseItem("Health potion", a);

            if (b.IsAlive && (float)b.CurrentHp / b.MaxHp < 0.4f)
                b.Inventory.UseItem("Health potion", b);

            // Kraj poteza za oba lika
            a.OnTurnEnd();
            b.OnTurnEnd();

            a.PrintStatus();
            b.PrintStatus();
            Debug.Log("");

            turn++;
        }

        return DetermineWinner(a, b, turn);
    }

    private Character DetermineWinner(Character a, Character b, int turns)
    {
        if (!a.IsAlive && !b.IsAlive)
        {
            Debug.Log("REZULTAT: Oboje eliminirani. Nema pobjednika.");
            return null;
        }
        if (!a.IsAlive)
        {
            Debug.Log($"REZULTAT: {b.Name} pobjeđuje!");
            return b;
        }
        if (!b.IsAlive)
        {
            Debug.Log($"REZULTAT: {a.Name} pobjeđuje!");
            return a;
        }

        Debug.Log($"REZULTAT: Dostignut limit od {MaxTurns} poteza. Borba neriješena.");
        return null;
    }
}
