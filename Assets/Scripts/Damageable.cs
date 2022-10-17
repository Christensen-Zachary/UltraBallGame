using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [field: SerializeField]
    public BrickFixCollision BrickFixCollision { get; set; }
    [field: SerializeField]
    private BrickCollision BrickCollision { get; set; }
    private float _health = 10;
    public float Health { get { return _health; } set { _health = value; BrickNumber.SetNumber((int)value); }  }
    [field: SerializeField]
    public BrickNumber BrickNumber { get; set; } // reference set on prefab in editor
    [field: SerializeField]
    public AudioSource HitSound { get; set; }
    [field: SerializeField]
    private ShrinkGrow ShrinkGrow { get; set; }
    public WinService WinService { get; set; }

    [field: SerializeField]
    public FacBrick FacBrick { get; set; } // set from FacBrick

    public bool _doesCountTowardsWinning = true;



    public Color MaxColor { get; set; } = new Color(
        //Convert.ToInt32("4B", 16) / 256f, 
        //Convert.ToInt32("4B", 16) / 256f, 
        //Convert.ToInt32("4B", 16) / 256f);
        Convert.ToInt32("0B", 16) / 256f,
        Convert.ToInt32("72", 16) / 256f,
        Convert.ToInt32("6D", 16) / 256f);
    public Color MinColor { get; set; } = new Color(
    //Convert.ToInt32("FF", 16) / 256f,
    //Convert.ToInt32("91", 16) / 256f,
    //Convert.ToInt32("A8", 16) / 256f);
        Convert.ToInt32("D5", 16) / 256f,
        Convert.ToInt32("FC", 16) / 256f,
        Convert.ToInt32("B4", 16) / 256f);
    public float MaxColorValue { get; set; } = 10;

    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; } // reference set on prefab in editor

    public void Damage(float damage)
    {
        Health -= damage;
        SetColor(Health);
        //HitSound.Play();
        ShrinkGrow.React();
        if (Health <= 0)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
            ShrinkGrow.HideSprite();
            BrickNumber.Hide();

            gameObject.transform.parent.position = Vector2.one * 100;
            AddToDestroyed();
            Destroy(transform.parent.gameObject, 1); // destroy after 1 second to show effects

            if (transform.parent.TryGetComponent(out Advanceable advanceable))
            {
                advanceable.RemoveFromList();
            }
        }
    }

    public void AddToDestroyed()
    {
        if (_doesCountTowardsWinning) WinService.NumberOfBricksDestroyed++;
    }

    public void SetColor(float value)
    {
        SpriteRenderer.color = Color.Lerp(MinColor, MaxColor, (value / MaxColorValue));
    }


}
