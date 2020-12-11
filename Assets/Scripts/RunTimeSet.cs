using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RuntimeSet<TKey, TValue> : ScriptableObject
{
    public List<TValue> itemList = new List<TValue>();
    public Dictionary<TKey, TValue> itemDictionary = new Dictionary<TKey, TValue>();

    public void AddList(TValue value)
    {
        if (!itemList.Contains(value))
            itemList.Add(value);
    }
    public void RemoveList(TValue value)
    {
        if (itemList.Contains(value))
            itemList.Remove(value);
    }

    public void AddDictionary(TKey key, TValue value)
    {
        var _isExits = itemDictionary.ContainsKey(key);

        if (!_isExits)
        {
            itemDictionary.Add(key, value);
        }

    }
    public void RemoveDictionary(TKey key)
    {
        itemDictionary.Remove(key);
    }
}

