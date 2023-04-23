using UnityEngine;

public class ProjectileDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Reward"))
        {
            Destroy(col.gameObject);
        }
    }
}
