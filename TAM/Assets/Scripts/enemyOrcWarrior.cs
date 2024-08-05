using System.Collections;
using UnityEngine;

public class enemyOrcWarrior : MonoBehaviour
{
    [SerializeField] private characterProfile profile;
    private float speed;
    private float range;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;

    private bool walk;
    private float attackTimer;
    private float distanceToPlayer;

    private statHandler playerStatHandler;
    audioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = profile.charProfileHolder.speed;
        range = profile.charProfileHolder.range;
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<audioManager>();
    }

    private void Update()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerStatHandler = playerObject.GetComponent<statHandler>();
        }
        if (GetComponent<statHandler>().enabled == false)
        {
            rb.velocity = Vector2.zero;
            Destroy(gameObject, 5f);
            return;
        }

        if (walk && !anim.GetBool("dead"))
        {
            anim.SetBool("walk", true);
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (!anim.GetBool("dead"))
            attackTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("stone"))
        {
            transform.Rotate(0f, 180f, 0f);
            speed *= -1;
        }
    }

    private void FixedUpdate()
    {
        if (player == null || playerStatHandler == null)
        {
            return;
        }

        distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

        if (attackTimer > 1.5f && playerStatHandler.enabled)
        {
            if (distanceToPlayer <= range && ((transform.position.y - player.position.y) <= range))
            {
                anim.SetTrigger("attack");
                anim.SetBool("walk", false);
                walk = false;
                StartCoroutine(delayAttackForDodge());
            }
            attackTimer = 0;
        }

        if (distanceToPlayer <= (range * 2f) && distanceToPlayer >= 0.5f && !anim.GetBool("dead"))
        {
            if (transform.position.x - player.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
                speed = -Mathf.Abs(profile.charProfileHolder.speed);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0f, 0);
                speed = Mathf.Abs(profile.charProfileHolder.speed);
            }
        }

        if (distanceToPlayer > range)
        {
            walk = true;
        }
    }

    private float damageOnAttack = 50f;

    private IEnumerator delayAttackForDodge()
    {
        audioManager.PlaySFX(audioManager.attack);
        yield return new WaitForSeconds(0.4f);
        if (playerStatHandler != null && Mathf.Abs(transform.position.x - player.position.x) <= range)
        {
            playerStatHandler.DealDamage(damageOnAttack);
        }
    }
}
