using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] statHandler statHandlerComponent;
    [SerializeField] private float currentHealth;
    [SerializeField] private float totalHealth;
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image totalHealthBar;


    private void Awake(){
        
        totalHealth = statHandlerComponent.CurrentHealth;
        totalHealthBar.fillAmount = 1f;
        currentHealth = statHandlerComponent.CurrentHealth;
        Debug.Log("Starting with Health: " + currentHealth);
    }

    private void Update(){
        currentHealth = statHandlerComponent.CurrentHealth;
        currentHealthBar.fillAmount = (currentHealth/totalHealth);
    }
}
