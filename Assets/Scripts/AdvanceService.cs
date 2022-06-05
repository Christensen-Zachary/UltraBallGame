using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    public GameObject AdvanceableParent { get; set; }

    public List<Advanceable> Advanceables { get; private set; } = new List<Advanceable>();
    private Player _player;
    private EndTurnDestroyService _endTurnDestroyService;
    private Grid _grid;

    private float _moveTime = 1f;

    private void Awake()
    {
        ResourceLocator.AddResource("AdvanceService", this);

        _player = ResourceLocator.GetResource<Player>("Player");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        _grid = ResourceLocator.GetResource<Grid>("Grid");
    }


    public IEnumerator Advance()
    {
        //Advanceables.ForEach(x => { if (x != null) x.StartMoveDown(); });

        Vector2 startPosition = AdvanceableParent.transform.position;
        Vector2 endPosition = startPosition - new Vector2(0, _grid.UnitScale);

        float timer = 0;
        while (timer < _moveTime)
        {
            timer += Time.deltaTime;
            //Advanceables.ForEach(x => { if (x != null) x.MoveDown(); });
            AdvanceableParent.transform.position = Vector2.Lerp(startPosition, endPosition, timer / _moveTime);
            yield return null;
        }

        //Advanceables.ForEach(x => { if (x != null) x.EndMoveDown(); });

        Advanceables.ForEach(x =>
        {
            if (Mathf.Approximately(x.transform.position.y, _player.transform.position.y))
            {
                Damageable damageable = x.GetComponentInChildren<Damageable>();
                if (damageable != null)
                {
                    _player.Health -= 5f;
                }
                _endTurnDestroyService.AddGameObject(x.gameObject);
            }
        });
    }

}
