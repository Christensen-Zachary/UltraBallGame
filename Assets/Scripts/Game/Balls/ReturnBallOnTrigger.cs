using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBallOnTrigger : MonoBehaviour
{
    private Vector2 _offScreen = Vector2.one * 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shootable shootable))
        {
            shootable.Return();
        }
        else if (collision.TryGetComponent(out EvilBrickAttackBall evilBrickAttackBall))
        {
            evilBrickAttackBall.transform.position = _offScreen;
            Destroy(evilBrickAttackBall.gameObject, 3f);
        }
    }
}
