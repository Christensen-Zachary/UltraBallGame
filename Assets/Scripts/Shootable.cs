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

    private Vector2 _lagPosition = Vector2.zero;
    public Vector2 LagVelocity { get; set; } = Vector2.zero;
    public Vector2 LagPosition { get; set; } = Vector2.zero;

    private Vector2 MaxVelocity = Vector2.zero;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        _radius = GetComponent<CircleCollider2D>().radius * 0.6f;
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
        int layerMask = (1 << LayerMask.NameToLayer("Brick") | 1 << LayerMask.NameToLayer("Wall")) & ~(1 << LayerMask.NameToLayer("Ball"));
        RaycastHit2D[] hits = Physics2D.CircleCastAll(collision.contacts[0].point, _radius, LagVelocity.normalized, 0.1f, layerMask);
        if (hits.Length == 0)
        {
            print("No hits");
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/MidPrediction"));
            obj.transform.position = collision.contacts[0].point;
            obj.transform.localScale = Vector3.one * 0.3f;
            obj.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            print($"{hits.Length} hits {hits[0].collider.name}");
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.TryGetComponent(out Damageable damageable))
                {
                    damageable.Damage(Damage);
                }
            }
        }

        if (collision.collider.CompareTag("Brick"))
        {
            //if (collision.GetContact(0).point)
        }

        if (RB.velocity.magnitude > MaxVelocity.magnitude)
        {
            MaxVelocity = GetComponent<Rigidbody2D>().velocity;
            //print($"New max velocity mag = {MaxVelocity.magnitude}");
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
            IsReturned = false;
            transform.localPosition = Vector3.zero; // from zero because are children of parent shooting from
            RB.AddForce(direction * Speed);
        }
    }

    public void Return()
    {
        IsReturned = true;
        RB.velocity = Vector3.zero;
        transform.localPosition = _offScreen;
    }
}
