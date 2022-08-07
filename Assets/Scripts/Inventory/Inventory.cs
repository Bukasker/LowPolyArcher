using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Inventory : MonoBehaviour
{
    public int space = 20;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public static Inventory instance;
    void Awake()
    {
        instance = this;
    }
    public List<Item> items = new List<Item>();
    public void Add(Item item)
    {
        items.Add(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}


