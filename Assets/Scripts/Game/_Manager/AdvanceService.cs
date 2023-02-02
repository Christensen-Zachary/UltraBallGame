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

    private float _moveTime = 1.5f;

    private void Awake()
    {
        ResourceLocator.AddResource("AdvanceService", this);

        _player = ResourceLocator.GetResource<Player>("Player");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        _grid = ResourceLocator.GetResource<Grid>("Grid");
    }


    public IEnumerator Advance()
    {

        Vector2 startPosition = AdvanceableParent.transform.position;
        Vector2 endPosition = startPosition - new Vector2(0, _grid.UnitScale);

        // use the y value from cos(x) on interval [0,pi/2]
        float timer = 0;
        while (timer < _moveTime)
        {
            timer += Time.deltaTime;
            // timer / _moveTime goes from 0 to 1
            // multiply that by PI to move between 0 and PI
            // add PI to adjust to correct cosine phase for go up to 1 from 0
            // add 0.5 and multiply 0.5 to put above x axis for all positive values
            // 1/2 * COS(PI * x + PI) + 1/2
            AdvanceableParent.transform.position = Vector2.Lerp(startPosition, endPosition, 0.5f * Mathf.Cos(Mathf.PI * timer / _moveTime + Mathf.PI) + 0.5f);
            yield return null;
        }
        AdvanceableParent.transform.position = endPosition;

        Advanceables.ForEach(x =>
        {
            if (Mathf.Approximately(x.transform.position.y, _player.transform.position.y))
            {
                Damageable damageable = x.GetComponentInChildren<Damageable>();
                if (damageable != null)
                {
                    _player.Health -= 1f;
                }
                _endTurnDestroyService.AddGameObject(x.gameObject);
            }
        });
    }

}
