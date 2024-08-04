using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    private statHandler statHandlerComponent;
    [SerializeField] private float currentHealth;
    [SerializeField] private float totalHealth;
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image totalHealthBar;

    private void Awake(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null){
            statHandlerComponent = player.GetComponent<statHandler>();
            totalHealth = statHandlerComponent.CurrentHealth;
            currentHealth = statHandlerComponent.CurrentHealth;
            Debug.Log("Starting with Health: " + currentHealth);
        } else {
            Debug.LogError("Player GameObject not found!");
        }
    }

    private void Update(){
        currentHealth = statHandlerComponent.CurrentHealth;
        currentHealthBar.fillAmount = currentHealth/totalHealth;
    }
}
