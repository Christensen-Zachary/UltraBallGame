using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private float _health = 10;
    public float Health { get { return _health; } set { _health = value; BrickNumber.SetNumber((int)value); }  }
    [field: SerializeField]
    public BrickNumber BrickNumber { get; set; } // reference set on prefab in editor
    [field: SerializeField]
    public AudioSource HitSound { get; set; }
    [field: SerializeField]
    private ShrinkGrow ShrinkGrow { get; set; }

    public Color MaxColor { get; set; } = new Color(
        Convert.ToInt32("00", 16) / 256f, 
        Convert.ToInt32("92", 16) / 256f, 
        Convert.ToInt32("45", 16) / 256f);
    public Color MinColor { get; set; } = new Color(
        Convert.ToInt32("FC", 16) / 256f,
        Convert.ToInt32("EE", 16) / 256f,
        Convert.ToInt32("21", 16) / 256f);
    public float MaxColorValue { get; set; } = 10;

    [field: SerializeField]
    private SpriteRenderer SpriteRenderer { get; set; } // reference set on prefab in editor


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Shootable shootable))
        {
            Health -= shootable.Damage;
            SetColor(Health);
            HitSound.Play();
            ShrinkGrow.React();
            if (Health <= 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                ShrinkGrow.HideSprite();
                BrickNumber.Hide();

                Destroy(gameObject, 1);
            }
        }
    }

    public void SetColor(float value)
    {
        SpriteRenderer.color = Color.Lerp(MinColor, MaxColor, (value / MaxColorValue));
    }


}
