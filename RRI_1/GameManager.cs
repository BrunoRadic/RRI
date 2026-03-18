// GameManager.cs

using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("========== RPG SIMULACIJA ==========\n");

        BattleSystem battleSystem = new BattleSystem();

        // --- Borba 1: Warrior vs Mage ---
        Debug.Log("=== Priprema borbe 1: Warrior vs Mage ===\n");

        Warrior warrior = new Warrior("Thorin");
        Mage    mage    = new Mage("Gandalf");

        warrior.Inventory.AddItem(new DamageBuff("Željezni mač", attackBonus: 5, defenseBonus: 0));
        warrior.Inventory.AddItem(new HealthPotion("Health potion", healAmount: 40));
        warrior.Inventory.UseItem("Željezni mač", warrior);

        // Mage nosi health potion
        mage.Inventory.AddItem(new HealthPotion("Health potion", healAmount: 30));

        Debug.Log("[Inventari]");
        warrior.Inventory.PrintInventory();
        mage.Inventory.PrintInventory();

        Character winner1 = battleSystem.SimulateBattle(warrior, mage);
        Debug.Log(winner1 != null
            ? $"Pobjednik borbe 1: {winner1.Name}\n"
            : "Borba 1: Nema pobjednika.\n");

        // --- Borba 2: novi Warrior vs novi Mage ---
        Debug.Log("=== Priprema borbe 2: Mage vs Warrior ===\n");

        Mage    mage2    = new Mage("Merlin");
        Warrior warrior2 = new Warrior("Bjorn");

        mage2.Inventory.AddItem(new DamageBuff("Grimoire", attackBonus: 4, defenseBonus: 0));
        mage2.Inventory.AddItem(new HealthPotion("Health potion", healAmount: 30));
        mage2.Inventory.UseItem("Grimoire", mage2);

        warrior2.Inventory.AddItem(new HealthPotion("Health potion", healAmount: 40));

        Debug.Log("[Inventari]");
        mage2.Inventory.PrintInventory();
        warrior2.Inventory.PrintInventory();

        Character winner2 = battleSystem.SimulateBattle(mage2, warrior2);
        Debug.Log(winner2 != null
            ? $"Pobjednik borbe 2: {winner2.Name}\n"
            : "Borba 2: Nema pobjednika.\n");

        // --- Sažetak ---
        Debug.Log("========== KRAJ SIMULACIJE ==========");
        Debug.Log($"Borba 1: {(winner1 != null ? winner1.Name + " pobjeđuje" : "Neriješeno")}");
        Debug.Log($"Borba 2: {(winner2 != null ? winner2.Name + " pobjeđuje" : "Neriješeno")}");
    }

    void Update() { }
}
