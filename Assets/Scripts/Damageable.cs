using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [field: SerializeField]
    public float Health { get; set; } = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Shoot shoot))
        {
            Health -= shoot.Damage;
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
