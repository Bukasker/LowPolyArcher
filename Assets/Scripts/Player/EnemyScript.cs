using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class EnemyScript : MonoBehaviour
{
    public Slider slider;
    public GameObject Canvas;
    private Animator animator;
    public GameObject[] rb ;
    public event EventHandler OnDeathEnter;
    public bool isDead = false;
    void Start()
    {
        
        OnDeathEnter += OnDeath;
        animator = GetComponent<Animator>();
        ArrowDealDamage.EnemyHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        int i = rb.Length;
        if(ArrowDealDamage.EnemyHealth > 0)
        {
            for(int j = 0; j < i; j++)
            {
                rb[j].SetActive(false);
            }
        }
        slider.value = ArrowDealDamage.EnemyHealth;
        if (ArrowDealDamage.EnemyHealth <= 0)
        {
            OnDeathEnter?.Invoke(this, EventArgs.Empty);
            isDead = true;
            Destroy(Canvas);
            for (int j = 0; j < i; j++)
            {
                rb[j].SetActive(true);
            }
        }
    }
    private void OnDeath(object sender, EventArgs e)
    {
        if (!isDead)
        {
            animator.enabled = !animator.enabled;
        }
    }
}
