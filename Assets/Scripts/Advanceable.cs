using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advanceable : MonoBehaviour
{

    private float _moveTime = 1f;

    public Grid _grid; // set in FacBrick
    public AdvanceService _advanceService; // set in FacBrick


    Vector2 startPosition;
    Vector2 endPosition;
    float timer;
    
    public void StartMoveDown()
    {
        startPosition = transform.position;
        endPosition = transform.position - new Vector3(0, _grid.UnitScale, 0);
        timer = 0;
    }

    public void MoveDown()
    {
        timer += Time.deltaTime;
        transform.position = Vector2.Lerp(startPosition, endPosition, timer / _moveTime);
    }

    public void EndMoveDown()
    {
        transform.position = endPosition;
    }


    public void RemoveFromList()
    {
        _advanceService.Advanceables.Remove(this);
    }
    
}
