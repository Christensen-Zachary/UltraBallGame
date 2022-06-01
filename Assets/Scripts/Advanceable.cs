using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advanceable : MonoBehaviour
{

    private bool _isMoving = false;
    private float _moveTime = 1f;

    public bool IsMoving => _isMoving;
    public Grid _grid;

    public void MoveDown()
    {
        if (!_isMoving)
        {
            _isMoving = true;
            StartCoroutine(MoveDownRoutine()); 
        }
    }


    private IEnumerator MoveDownRoutine()
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = transform.position - new Vector3(0, _grid.UnitScale, 0);
        float timer = 0;
        while (timer < _moveTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, endPosition, timer / _moveTime);
            yield return null;
        }

        transform.position = endPosition;

        _isMoving = false;
    }
    
}
