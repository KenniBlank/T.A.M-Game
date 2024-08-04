using System.Collections;
using UnityEngine;

public class platformMoverLeft : MonoBehaviour
{
    private Vector3 initialPosition;
    [SerializeField] float speed = 2.0f;
    [SerializeField] float delay = 1.0f;
    [SerializeField] float distance = 5f;

    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            // Move left
            yield return MoveToPosition(initialPosition + Vector3.left * distance, speed);
            yield return new WaitForSeconds(delay);

            // Move right
            yield return MoveToPosition(initialPosition + Vector3.right * distance, speed);
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
