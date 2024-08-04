using System.Collections;
using UnityEngine;

public class playerControllerMelee : MonoBehaviour
{
    float spawnPointX = 0; // where to spawn on dead
    float spawnPointY = 0;

    // PlayerProfile data
    [SerializeField] private characterProfile profile;
    private float speed;
    private float jumpForce;
    private float range;
    audioManager audioManager;

    // Basic Control
    private float horizontal;
    private bool isFacingRight = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] Transform attackpoint;
    [SerializeField] private LayerMask enemyLayers;

    // Dash
    [SerializeField] private TrailRenderer tr;
    private bool canDash = true;
    private bool isDashing = false;
    private int dashDamage = 10;
    private float dashingPower;
    private float dashingTime = 0.4f;
    private float dashingCoolDown = 0.5f;

    private statHandler statHandler;

    // Animation
    [SerializeField] private Animator anim;
    bool isAttacking;

    // Damage
    float damageDealtForNormalAttack = 10f;
    float cooldownForNormalAttack = 0.3f;

    private SpriteRenderer sr;

    // ParticleSystem
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem fallParticle;
    float movementParticleAfterVelocity = 9f;
    float dustFormationPeriod = 0.15f;
    float counter;

    float transitionDuration = 0.7f;
    float acceleration = 7f;
    float velPower = 0.9f;
    float deceleration = 7f;
    float gravityScale = 2f;
    float fallGravityMultiplier = 1.2f;

    float lastGroundedTime;
    float blah;
    [SerializeField] float frictionAmount = 10f;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<audioManager>();
    }

    private void Start()
    {
        speed = profile.charProfileHolder.speed;
        jumpForce = profile.charProfileHolder.jumpForce;
        range = profile.charProfileHolder.range;
        dashingPower = speed * 3f;
        statHandler = GetComponent<statHandler>();
        sr = GetComponent<SpriteRenderer>();
    }

    //respawn Logic
    IEnumerator respawn()
    {
        audioManager.PlaySFX(audioManager.respawnSound);
        yield return new WaitForSeconds(0.3f);
        gameObject.tag = "Player";
        statHandler.enabled = true;
        transform.position = new Vector3(spawnPointX, spawnPointY, transform.position.z);
        statHandler.GainHealth(statHandler.MaxHealth);
    }

    void Update()
    {
        if (anim.GetBool("dead"))
        {
            anim.SetBool("dead", false);
            StartCoroutine(respawn());
        }
        if (!anim.GetBool("dead"))
        {
            counter += Time.deltaTime;
            if (movementParticle != null && isGrounded())
            {
                if (Mathf.Abs(rb.velocity.x) > movementParticleAfterVelocity)
                {
                    if (counter > dustFormationPeriod)
                    {
                        movementParticle.Play();
                        counter = 0;
                    }
                }
            }                     

            if (cooldownForNormalAttack > 0)
            {
                cooldownForNormalAttack -= Time.deltaTime;
            }
            else
            {
                cooldownForNormalAttack = 0f;
            }

            if (isDashing)
            {
                return;
            }

            // TODO: FIX THE ISSUE; FIX WHAT?
            if (isGrounded() && lastGroundedTime > 2f)
            {
                fallParticle.Play();
                audioManager.PlaySFX(audioManager.landingSound);
                lastGroundedTime = 0f;
            }

            horizontal = Input.GetAxisRaw("Horizontal");

            Flip();

            // Walk and Animation
            if (horizontal != 0f && isGrounded())
            {
                anim.SetBool("run", true);
            }
            else
            {
                anim.SetBool("run", false);
            }

            // Jumping and Animation
            if (isGrounded() && Input.GetButtonDown("Jump") && lastGroundedTime < 0.2f)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                audioManager.PlaySFX(audioManager.jumpSound);
            }
            if (Input.GetButtonUp("Jump"))
            {
                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector2.down * rb.velocity.y * 0.5f, ForceMode2D.Impulse);
                }
            }
            if (isGrounded())
            {
                anim.SetBool("onGround", true);
                lastGroundedTime = 0.0f;
            }
            else
            {
                anim.SetBool("onGround", false);
                lastGroundedTime += Time.deltaTime;
            }

            // Dash Animation
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }

            //attack
            if (Input.GetKeyDown(KeyCode.J) && (cooldownForNormalAttack == 0))
            {
                Attack();
                audioManager.PlaySFX(audioManager.attack);
            }

            // invisToEnemy and smoke
            if (smoke)
            {
                timeForSmokeInvis -= Time.deltaTime;
                if (timeForSmokeInvis <= 0)
                {
                    anim.SetTrigger("smoke");
                    gameObject.tag = "Player";
                    timeForSmokeInvis = 10f;
                    smoke = false;
                    StartCoroutine(Opacity(1f));
                }
            }
            if (smokeCooldown == 0f)
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    gameObject.tag = "Untagged";
                    anim.SetTrigger("smoke");
                    smokeCooldown = 15f;
                    smoke = true;
                    StartCoroutine(Opacity(0.5f));                    
                }
            }
            if (smokeCooldown > 0)
            {
                smokeCooldown -= Time.deltaTime;
                if (smokeCooldown < 0)
                {
                    smokeCooldown = 0f;
                }

            }
        }        
    }

    IEnumerator Opacity(float targetAlpha)
    {
        float startAlpha = sr.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / transitionDuration);
            UnityEngine.Color color = sr.color;
            color.a = alpha;
            sr.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha value is set
        UnityEngine.Color finalColor = sr.color;
        finalColor.a = targetAlpha;
        sr.color = finalColor;
    }

    bool smoke;
    float timeForSmokeInvis = 10f;
    float smokeCooldown = 5f;

    void Attack()
    {
        anim.SetTrigger("attack");
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, range, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            GameObject enemyGameObject = enemy.gameObject;

            statHandler enemyStatHandler = enemyGameObject.GetComponent<statHandler>();

            if (enemyStatHandler != null)
            {
                enemyStatHandler.DealDamage(damageDealtForNormalAttack);                
            }
        }
        cooldownForNormalAttack = 0.35f;
    }

    public bool isGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return grounded;
    }

    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        var gravityDefault = rb.gravityScale;
        rb.gravityScale = 0f;
        tr.emitting = true;
        rb.velocity = new Vector2(isFacingRight ? dashingPower : -dashingPower, 0f);
        anim.SetBool("dash", true);
        audioManager.PlaySFX(audioManager.dashSound);
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = gravityDefault;
        isDashing = false;
        anim.SetBool("dash", false);
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (isDashing)
            {
                statHandler enemyStatHandler = collider.GetComponent<statHandler>();
                if (enemyStatHandler != null)
                {
                    enemyStatHandler.DealDamage(dashDamage);
                }
            }
        }
        if (collider.CompareTag("SpawnPoint"))
        {
            spawnPointX = collider.transform.position.x;
            spawnPointY = collider.transform.position.y + 2f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("spikeTrap"))
        {
            statHandler.DealDamage(100f);
        }
    }

    private void FixedUpdate()
    {
        if (!anim.GetBool("dead"))
        {
            #region run
            float targetSpeed = horizontal * speed;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right);
            #endregion

            #region TimerToRegisterJumps(NotImplementedYet)
            lastGroundedTime += Time.deltaTime;
            blah = lastGroundedTime - Time.deltaTime;
            #endregion

            #region Friction
            if (lastGroundedTime > 0)
            {
                float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
                amount *= Mathf.Sign(rb.velocity.x);
                rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }
            #endregion

            #region Gravity
            if (rb.velocity.y < 0)
                rb.gravityScale = gravityScale * fallGravityMultiplier;
            else
                rb.gravityScale = gravityScale;
            #endregion
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackpoint.position, range);
    }
}
