using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTurnAfterHit : MonoBehaviour
{

    private bool _hasCollided = false;

    public EndTurnDestroyService _endTurnDestroyService;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasCollided && collision.collider.CompareTag("Ball"))
        {
            _hasCollided = true;
            _endTurnDestroyService.AddGameObject(gameObject);
        }
    }
}
