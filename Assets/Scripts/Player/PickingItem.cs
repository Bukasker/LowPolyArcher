using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingItem : MonoBehaviour
{
    public Item item;
    public GameObject Item;
    public bool canPickUp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPickUp == true)
        {
            if (PlayerController.isEpressed)
            {
                Inventory.instance.Add(item);
                Destroy(gameObject);
                canPickUp = false;
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Interact")
        {
            Item = col.gameObject;
            canPickUp = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Interact")
        {
            Item = null;
            canPickUp = false;
        }
    }
}
