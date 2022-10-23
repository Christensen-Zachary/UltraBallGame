using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public Rigidbody2D RB { get; set; }
    private Vector3 _offScreen = Vector3.one * 100;
    private float _radius;
    [field: SerializeField]
    public int Speed { get; private set; }
    
    [field: SerializeField]
    public float Damage { get; set; } = 1;
    public bool IsReturned { get; private set; }
    public bool IsBuffed { get; set; } = false;

    private Vector2 _lagPosition = Vector2.zero;
    public Vector2 LagVelocity { get; set; } = Vector2.zero;
    public Vector2 LagPosition { get; set; } = Vector2.zero;

    private Vector2 MaxVelocity = Vector2.zero;

    [field: SerializeField]
    public ParticleSystem PSReturn { get; set; }
    public int ReturnEmitCount { get; set; } = 6;

    public LevelService _levelService;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        _radius = GetComponent<CircleCollider2D>().radius * 0.6f;

        ThemeVisitor.Visit(this);
    }


    private void FixedUpdate()
    {
        LagPosition = _lagPosition;
        _lagPosition = transform.position;
        if (RB.velocity != Vector2.zero) LagVelocity = RB.velocity;
    }

    private void Update()
    {
        //LagPosition = transform.position;
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
                    print($"Did not find {hits[i].collider.name} tag {hits[i].collider.tag}");
                }
            }
        }

    }

    public void Fire(Vector2 direction)
    {
        if (RB.velocity != Vector2.zero)
        {
            string errMsg = "Ball veclocity was not zero when firing";
            Debug.LogError(errMsg);
            throw new System.Exception(errMsg);
        }
        else
        {
            _levelService.BallCounter--;
            IsReturned = false;
            transform.localPosition = Vector3.zero; // from zero because are children of parent shooting from
            RB.AddForce(direction * Speed);
        }
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
            //Destroy(powerupAttachment.gameObject);
            Damage = 1;
            IsBuffed = false;
        }
    }
}
