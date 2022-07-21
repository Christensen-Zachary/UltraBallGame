using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilBrick : MonoBehaviour, IAttack
{
    public float Radius { get; set; }
    public EndTurnAttackService _endTurnAttackService;
    public Grid _grid;
    [field: SerializeField]
    public GameObject EvilBrickAttackBall { get; set; }



    public void RemoveFromList()
    {
        _endTurnAttackService.Attacks.Remove(this);
    }


    public void Attack()
    {
        //int layerMask = 1 << LayerMask.NameToLayer("Player");
        //RaycastHit2D hit = Physics2D.CircleCast(transform.position, Radius, new Vector2(0, -1), Mathf.Infinity, layerMask);

        //if (hit)
        //{
        //    hit.collider.GetComponent<Player>().Health -= 25;
        //}
        GameObject attackBall = Instantiate(EvilBrickAttackBall);
        attackBall.transform.SetParent(transform);
        attackBall.transform.localPosition = Vector2.zero;
        attackBall.GetComponent<EvilBrickAttackBall>().Shoot(new Vector2(0, -1).normalized * 24.5f * _grid.UnitScale * _grid.NumberOfDivisions);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
