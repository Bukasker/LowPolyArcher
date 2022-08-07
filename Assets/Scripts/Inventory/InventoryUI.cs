using UnityEngine;
using System.Linq;
public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    public Transform InventoryPanel;
    [SerializeField]
    InventorySlot[] slots;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = GameObject.FindGameObjectsWithTag("slot").Select(s => s.GetComponent<InventorySlot>()).ToArray();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                Debug.Log("ADDED" + inventory.items[i]);
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
