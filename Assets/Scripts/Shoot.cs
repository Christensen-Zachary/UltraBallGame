using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _offScreen = Vector2.one * 100;
    [field: SerializeField]
    public int Speed { get; private set; }
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
            transform.localPosition = Vector2.zero; // from zero because are children of parent shooting from
            _rb.AddForce(direction * Speed);
        }
    }

    public void Return()
    {
        _rb.velocity = Vector2.zero;
        transform.localPosition = _offScreen;
    }
}
