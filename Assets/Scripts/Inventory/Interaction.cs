using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public GameObject Item2;
    public bool canPickUp;
    public GameObject pickUpText;
    private Animator anim;
    public bool canInteract;
    public bool Open = false ;
    void Start()
    {

    }
    void Update()
    {
        if (canPickUp == true || canInteract == true)
        {
            pickUpText.SetActive(true);
        }
        else
        {
            pickUpText.SetActive(false);
        }
        if (canPickUp == true)
        {
            if (PlayerController.isEpressed)
            {
                canPickUp = false;
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Door")
        {
            Item2 = col.gameObject;
            canInteract = true;
        }
        if (col.tag == "arrow")
        {
            Item2 = col.gameObject;
            canPickUp = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "arrow")
        {
            Item2 = null;
            canPickUp = false;
        }
        if (col.tag == "Door")
        {
            Item2 = null;
            canInteract = false;
        }
    }

}
