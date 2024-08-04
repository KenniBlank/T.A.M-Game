using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Animator animator;
    string magicAnimationName = "magic";

    private bool isMagicAnimationActive = false;

    private void Update()
    {
        bool isCurrentlyActive = IsMagicAnimationPlaying();

        if (isCurrentlyActive && !isMagicAnimationActive)
        {
            isMagicAnimationActive = true;
            SpawnProjectile();
        }
        else if (!isCurrentlyActive)
        {
            isMagicAnimationActive = false;
        }
    }

    private bool IsMagicAnimationPlaying()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(magicAnimationName) && stateInfo.normalizedTime < 1;
    }

    private void SpawnProjectile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Vector2 direction = (player.transform.position - transform.position).normalized;

            MagicProjectile magicProjectile = projectile.GetComponent<MagicProjectile>();
            if (magicProjectile != null)
            {
                magicProjectile.SetDirection(direction);
            }
        }
    }
}
