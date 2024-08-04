using System.Collections;
using UnityEngine;

public class ShamanControl : MonoBehaviour
{
    [SerializeField] Transform[] teleportPoints;
    [SerializeField] private characterProfile profile;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject orcEnemyPrefab;
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private ParticleSystem attackNormalParticle;

    private float range;
    private float attackTimer;
    private float distanceToPlayer;
    private float timeRanged;
    private float timerForSpawningOrcs;

    private statHandler playerStatHandler;
    private bool teleport = false;
    float spawnOrcY;
    float spawnOrcX;
    audioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<audioManager>();
        spawnOrcY = transform.position.y - 1f;
        spawnOrcX = transform.position.x;
        if (profile != null && profile.charProfileHolder != null)
        {
            range = profile.charProfileHolder.range;
        }
    }

    private void Update()
    {
        if (!GetComponent<statHandler>().enabled)
        {
            Destroy(gameObject, 5f);
        }
        else
        {
            if (!anim.GetBool("dead"))
            {
                attackTimer += Time.deltaTime;
                timerForSpawningOrcs += Time.deltaTime;

                statHandler shamanStatHandler = GetComponent<statHandler>();
                if (shamanStatHandler != null && timerForSpawningOrcs > 30f)
                {
                    anim.SetTrigger("jump");
                    StartCoroutine(SpawnOrcs());
                    timerForSpawningOrcs = 0;
                }

                // Teleporting enemy away
                if (teleport)
                {
                    TeleportAway();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("dead"))
            return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
            return;

        playerStatHandler = playerObject.GetComponent<statHandler>();
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Rotate to face the player
        if (player.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0f, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        // Handle ranged attack
        if (distanceToPlayer <= 15f)
        {
            if (timeRanged > 2.5f)
            {
                anim.SetTrigger("ranged");
                audioManager.PlaySFX(audioManager.magic);
                timeRanged = 0f;
            }
        }
        timeRanged += Time.deltaTime;

        //teleport temp
        timerTemp -= Time.deltaTime;
        if (timerTemp < 0)
        {
            teleport = true;
            timerTemp = 10f;
        }

        // Handle melee attack
        /*if (attackTimer > 2f && playerStatHandler.enabled && distanceToPlayer < range)
        {
            attackTimer = 0f;
            StartCoroutine(DelayAttackForDodge());
        }*/
    }
    float timerTemp = 10f;

    private IEnumerator SpawnOrcs()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < spawnCount; i++)
        {
            float randomX = Random.Range(spawnOrcX - 15, spawnOrcX + 15);
            Vector2 spawnPosition = new Vector2(randomX, spawnOrcY);
            Instantiate(orcEnemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("stone"))
        {
            transform.Rotate(0f, 180f, 0f);
        }
    }

    /*private IEnumerator DelayAttackForDodge()
    {
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(1f);
        attackNormalParticle.Play();
        distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);
        if (distanceToPlayer <= range && player.GetComponent<playerControllerMelee>().isGrounded())
        {
            if (playerStatHandler != null)
            {
                playerStatHandler.DealDamage(10f);
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(2f * (player.GetComponent<Transform>().rotation == Quaternion.Euler(0, 0, 0) ? 1f : -1f), 0f), ForceMode2D.Impulse);
            }
        }
        teleport = true;
    }*/

    private void TeleportAway()
    {
        if (teleportPoints.Length > 0)
        {
            /*// Getting the farthest point from Player
            Transform tmp = teleportPoints[0];
            float awayFromPlayer = Vector2.Distance(teleportPoints[0].position, player.position);

            foreach (Transform telPoint in teleportPoints)
            {
                float x = Vector2.Distance(telPoint.position, player.position);
                if (x > awayFromPlayer)
                {
                    tmp = telPoint;
                    awayFromPlayer = x;
                }
            }

            // Teleporting the shaman to the point
            transform.position = new Vector2(tmp.position.x, tmp.position.y + 1.569f);

            teleport = false;*/

            Transform randomPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];

            transform.position = new Vector2(randomPoint.position.x, randomPoint.position.y + 1.569f);

            teleport = false;
        }
    }
}
