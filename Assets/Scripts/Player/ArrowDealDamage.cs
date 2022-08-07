using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowDealDamage : MonoBehaviour
{
    public static float EnemyHealth;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "arrow")
        {
            EnemyHealth -= 30;
        }
        if (col.tag == "Sword")
        {
            EnemyHealth -= 40;
        }
    }

}
