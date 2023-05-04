using System;
using UnityEngine;

public class ProjectileDestroyer : MonoBehaviour
{
    public event Action ProjectileDestroyed;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Reward"))
        {
            Destroy(col.gameObject);
            ProjectileDestroyed?.Invoke();
        }
    }
}
