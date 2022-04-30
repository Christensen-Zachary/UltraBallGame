using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector3 _offScreen = Vector3.one * 100;
    [field: SerializeField]
    public int Speed { get; private set; }
    
    [field: SerializeField]
    public float Damage { get; set; } = 1;
    public bool IsReturned { get; private set; }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction)
    {
        if (_rb.velocity != Vector2.zero)
        {
            string errMsg = "Ball veclocity was not zero when firing";
            Debug.LogError(errMsg);
            throw new System.Exception(errMsg);
        }
        else
        {
            IsReturned = false;
            transform.localPosition = Vector3.zero; // from zero because are children of parent shooting from
            _rb.AddForce(direction * Speed);
        }
    }

    public void Return()
    {
        IsReturned = true;
        _rb.velocity = Vector3.zero;
        transform.localPosition = _offScreen;
    }
}
