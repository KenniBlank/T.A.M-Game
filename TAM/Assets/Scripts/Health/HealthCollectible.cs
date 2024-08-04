using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthCollectible : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private float healthValue = 10f;

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Player"){
            statHandler playerStatHandler = collision.GetComponent<statHandler>();
        if (playerStatHandler != null)
            {
                // Call the DealDamage method with the health value as damage amount
                playerStatHandler.DealDamage(-healthValue);
            }

            Destroy(gameObject);
        }
    }
}
