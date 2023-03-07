using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownEndlessly : MonoBehaviour
{
    private float _speed = 0f;
    private void Update() 
    {
        _speed += 9.8f * Mathf.Pow(Time.deltaTime, 2);
        transform.position -= new Vector3(0, _speed, 0);  
    }
}
