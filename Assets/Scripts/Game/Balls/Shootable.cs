using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public Rigidbody2D RB { get; set; }
    private Vector3 _offScreen = Vector3.one * 100;
    [field: SerializeField]
    public int Speed { get; private set; }
    
    [field: SerializeField]
    public float Damage { get; set; } = 1;
    public bool IsReturned { get; private set; }
    public bool IsBuffed { get; set; } = false;

    public Vector2 LagVelocity { get; set; } = Vector2.zero;


    [field: SerializeField]
    public ParticleSystem PSReturn { get; set; }
    public int ReturnEmitCount { get; set; } = 6;

    public LevelService _levelService;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();

        ThemeVisitor.Visit(this);
    }


    private void FixedUpdate()
    {
        if (RB.velocity != Vector2.zero) LagVelocity = RB.velocity;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Brick")) & ~(1 << LayerMask.NameToLayer("Ball"));

        //if (collision.contactCount > 1)
        //{
        //    for (int i = 0; i < collision.contactCount; i++)
        //    {
        //        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/MidPrediction"));
        //        obj.transform.position = collision.contacts[i].point;
        //        obj.transform.localScale = Vector3.one * 0.3f;
        //        obj.GetComponent<SpriteRenderer>().color = Color.red;
        //        print($"CName {collision.contacts[i].collider.name} point {collision.contacts[i].normal}");
        //    }
        //}

        RaycastHit2D[] hits = Physics2D.CircleCastAll(collision.contacts[0].point, 0.25f, LagVelocity.normalized, 0.05f, layerMask);
        if (hits.Length != 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Damageable"))
                {
                    hits[i].collider.GetComponent<Damageable>().Damage(Damage);

                    //if (hits[i].collider.TryGetComponent(out Damageable damageable))
                    //{
                    //    damageable.Damage(Damage); 
                    //}
                }
                else
                {
                    //print($"Did not find {hits[i].collider.name} tag {hits[i].collider.tag}");
                }
            }
        }

    }

    public void Fire(Vector2 direction)
    {
        IsReturned = false;
        transform.localPosition = Vector3.zero; // from zero because are children of parent shooting from
        RB.velocity = Vector2.zero;
        RB.AddForce(direction * Speed);
    }

    public void RandomizeDirection()
    {
        if (IsReturned) return;

        RB.velocity = Vector2.zero;
        float randomAngle = Random.Range(0, Mathf.PI);
        RB.AddForce(new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Speed);
    }

    public void HorizontalDirection()
    {
        if (IsReturned) return;

        RB.velocity = Vector2.zero;
        RB.AddForce(new Vector2(Random.Range(0f, 1f) < 0.5f ? -1 : 1, 0) * Speed);
    }

    public void VerticalDirection()
    {
        if (IsReturned) return;

        RB.velocity = Vector2.zero;
        RB.AddForce(new Vector2(0, Random.Range(0f, 1f) < 0.5f ? -1 : 1) * Speed);
    }

    public void Return()
    {
        IsReturned = true;
        PSReturn.Emit(ReturnEmitCount);

        RB.velocity = Vector3.zero;
        transform.localPosition = _offScreen;

        PowerupAttachment powerupAttachment = GetComponentInChildren<PowerupAttachment>();
        if (powerupAttachment != null)
        {
            Damage = 1;
            IsBuffed = false;
        }
    }
}
