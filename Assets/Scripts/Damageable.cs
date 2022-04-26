using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [field: SerializeField]
    public float Health { get; set; } = 10;
    [field: SerializeField]
    public AudioSource HitSound { get; set; }
    [field: SerializeField]
    private ShrinkGrow ShrinkGrow { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Shootable shootable))
        {
            Health -= shootable.Damage;
            HitSound.Play();
            ShrinkGrow.React();
            if (Health <= 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                ShrinkGrow.HideSprite();
                Destroy(gameObject, 1);
            }
        }
    }
}
