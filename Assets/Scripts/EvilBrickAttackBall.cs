using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilBrickAttackBall : MonoBehaviour
{
    private Rigidbody2D _rb;

    public GameObject _psGameObject;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();    
    }

    private void Start()
    {
        _psGameObject.SetActive(true);   
    }

    public void Shoot(Vector2 direction)
    {
        _rb.AddForce(direction.normalized * 200f);
    }
}
