using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speedOfProjectile = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeSpan = 10f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private float countlife = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeSpan);
    }

    private void Start()
    {
        if (direction != Vector2.zero)
        {
            rb.velocity = direction * speedOfProjectile;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        statHandler statHandler = other.GetComponent<statHandler>();
        if (statHandler != null && other.CompareTag("Player"))
        {
            statHandler.DealDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Update()
    {
        countlife += Time.deltaTime;
        if (countlife > lifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
