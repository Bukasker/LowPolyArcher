using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject Item;
    private Animator anim;
    public bool canInteract;
    public bool Open;
    public GameObject pickUpText;
    void Start()
    {
        GameObject door = GameObject.FindWithTag("Door");
        anim = door.GetComponent<Animator>();
        Open = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerController.isEpressed && canInteract == true && Open == true)
        {
            anim.SetBool("isInteract", true);
            Open = false;

        }
        else if (PlayerController.isEpressed && canInteract == true && Open == false)
        {
            anim.SetBool("isInteract", false);
            Open = true;
        }
        if (canInteract == true)
        {
            pickUpText.SetActive(true);
        }
        else
        {
            pickUpText.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Door")
        {
            Item = col.gameObject;
            canInteract = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Door")
        {
            Item = null;
            canInteract = false;
        }
    }
}
