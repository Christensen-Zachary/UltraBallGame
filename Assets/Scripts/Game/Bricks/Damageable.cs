using System;
using System.Collections;
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
    [field: SerializeField]
    private BlinkGlow BlinkGlow { get; set; }
    
    public WinService WinService { get; set; }

    [field: SerializeField]
    public DamageCounter DamageCounter { get; set; } // set from FacBrick
    
    public PolygonCollider2D _reflectionCollider; // reference set in prefab

    public bool _doesCountTowardsWinning = true;
    public BrickData _brickData; // set in facBrick

    public static readonly float EFFECT_LENGTH = 1f;


    public Color MaxColor { get; set; } = new Color(
        Convert.ToInt32("0B", 16) / 256f,
        Convert.ToInt32("72", 16) / 256f,
        Convert.ToInt32("6D", 16) / 256f);
    public Color MinColor { get; set; } = new Color(
        Convert.ToInt32("D5", 16) / 256f,
        Convert.ToInt32("FC", 16) / 256f,
        Convert.ToInt32("B4", 16) / 256f);
    public float MaxColorValue { get; set; } = 10;

    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; } // reference set on prefab in editor


    public void Damage(float damage)
    {
        CountDamage(damage);

        if (Health <= 0)
        {
            AddToDestroyed();
            FadeAndDestroy(damage);
            RemoveFromAdvanceables();
        }
        else
        {
            ReactToDamage(damage);
        }
    }

    public void RemoveFromAdvanceables()
    {
        if (transform.parent.TryGetComponent(out Advanceable advanceable))
        {
            advanceable.RemoveFromList();
        }
    }

    public void FadeAndDestroy(float damage)
    {
        transform.parent.SetParent(null); // remove parent so facBrick does not return this destroyed object
        DisableComponents();
        StartCoroutine(FadeOut(damage));
        Destroy(transform.parent.gameObject, EFFECT_LENGTH); // destroy after _effectLength seconds to show effects
    }

    private IEnumerator FadeOut(float damage)
    {
        if (damage > 10) BlinkGlow.SetFadeOutColor(ThemeData.ExtraFireDmgBlink);
        else if (damage > 1) BlinkGlow.SetFadeOutColor(ThemeData.FireDmgBlink);
        else BlinkGlow.SetFadeOutColor(ThemeData.NormalDmgBlink);

        float timer = 0;

        while (timer < EFFECT_LENGTH)
        {
            timer += Time.deltaTime;

            SpriteRenderer.material.SetFloat("_FadeAmount", Mathf.Lerp(0, 1, timer / EFFECT_LENGTH));

            yield return null;
        }

        SpriteRenderer.material.SetFloat("_FadeAmount", 1);
    }

    private void CountDamage(float damage)
    {
        if (_doesCountTowardsWinning) DamageCounter.DamageCount += (int)((Health - damage < 0) ? Health : damage);
        Health -= damage;
        if (_brickData != null) _brickData.Brick.Health -= (int)damage;
    }

    private void ReactToDamage(float damage)
    {
        SetColor(Health);
        //HitSound.Play();
        ShrinkGrow.React();
        if (damage > 10) BlinkGlow.SetColor(ThemeData.ExtraFireDmgBlink, ThemeData.ExtraFireBlinkStrength);
        else if (damage > 1) BlinkGlow.SetColor(ThemeData.FireDmgBlink, ThemeData.FireBlinkStrength);
        else BlinkGlow.SetColor(ThemeData.NormalDmgBlink, ThemeData.NormalBlinkStrength);
        BlinkGlow.React();
    }

    private void DisableComponents()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        // ShrinkGrow.HideSprite();
        BrickNumber.Hide();
        _reflectionCollider.enabled = false;
    }

    public void AddToDestroyed()
    {
        if (_doesCountTowardsWinning) 
        {
            WinService.NumberOfBricksDestroyed++;
            DamageCounter.DestroyedCount++;
        }
    }

    public void SetColor(float value)
    {
        SpriteRenderer.color = Color.Lerp(MinColor, MaxColor, (value / MaxColorValue));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out Shootable shootable))
        {
            Damage(shootable.Damage);
        }
    }
}
