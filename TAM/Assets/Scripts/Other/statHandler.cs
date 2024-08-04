using System.Collections;
using UnityEngine;

public class statHandler : MonoBehaviour
{
    [SerializeField] private characterProfile profile;

    [SerializeField] private float currentHealth;
    private SpriteRenderer sr;
    Animator anim;

    Color original;
    audioManager audioManager;
    public float maxHealth;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        original = sr.color;
        anim = GetComponent<Animator>();
        //audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<audioManager>();
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    private void Start()
    {
        currentHealth = profile.charProfileHolder.maxHealth;
        maxHealth = currentHealth;
        anim.SetBool("dead", false);
    }

    public void DealDamage(float damageAmount)
    {
        if (!anim.GetBool("dead"))
        {
            currentHealth -= damageAmount;
            sr.color = new Color(1f, 0f, 0f, 0.9f);
            anim.SetTrigger("hurt");
            StartCoroutine(damageDelayColorChange());
            if (currentHealth <= 0)
            {
                Die();
                currentHealth = 0f;
            }
        }
    }

    public void GainHealth(float healthAmount)
    {
        if (!anim.GetBool("dead"))
        {
            if (currentHealth <= maxHealth)
                currentHealth += healthAmount;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        anim.SetBool("dead", true);
        //audioManager.PlaySFX(audioManager.dead);
        this.enabled = false;
        gameObject.tag = "Untagged";
    }

    private IEnumerator damageDelayColorChange()
    {
        yield return new WaitForSeconds(0.35f);
        sr.color = original;
    }

}