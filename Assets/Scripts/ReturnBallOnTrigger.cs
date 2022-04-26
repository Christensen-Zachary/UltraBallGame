using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBallOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shootable shootable))
        {
            shootable.Return();
        }
    }
}
