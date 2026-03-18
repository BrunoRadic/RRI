// Inventory.cs

using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<IItem> _items;
    private int   _maxSlots;
    private float _maxWeight;

    public int   Count         => _items.Count;
    public float CurrentWeight { get; private set; }

    public Inventory(int maxSlots, float maxWeight = 20f)
    {
        _maxSlots   = maxSlots;
        _maxWeight  = maxWeight;
        _items      = new List<IItem>();
        CurrentWeight = 0f;
    }

    public bool AddItem(IItem item)
    {
        if (_items.Count >= _maxSlots)
        {
            Debug.Log($"Inventory pun! Ne može dodati '{item.Name}'.");
            return false;
        }
        if (CurrentWeight + item.Weight > _maxWeight)
        {
            Debug.Log($"Premašena težina! Ne može dodati '{item.Name}'.");
            return false;
        }

        _items.Add(item);
        CurrentWeight += item.Weight;
        Debug.Log($"Dodan '{item.Name}' ({_items.Count}/{_maxSlots} slotova).");
        return true;
    }

    public bool UseItem(string itemName, Character target)
    {
        IItem item = _items.Find(i => i.Name == itemName);
        if (item == null)
        {
            Debug.Log($"'{itemName}' nije pronađen u inventaru.");
            return false;
        }

        item.Apply(target);

        if (item.IsConsumed())
        {
            _items.Remove(item);
            CurrentWeight -= item.Weight;
            Debug.Log($"'{itemName}' potrošen i uklonjen iz inventara.");
        }

        return true;
    }

    public void PrintInventory()
    {
        Debug.Log($"--- Inventar ({_items.Count}/{_maxSlots} slotova, {CurrentWeight}kg) ---");
        if (_items.Count == 0)
        {
            Debug.Log("  (prazan)");
            return;
        }
        foreach (IItem item in _items)
            Debug.Log($"  * {item.Name} - {item.Description}");
    }
}
