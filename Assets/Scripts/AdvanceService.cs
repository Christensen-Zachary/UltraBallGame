using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    public bool IsAdvancing => Advanceables.Any(x => x.IsMoving);

    public List<Advanceable> Advanceables { get; private set; } = new List<Advanceable>();
    private Player _player;
    private EndTurnDestroyService _endTurnDestroyService;

    private void Awake()
    {
        ResourceLocator.AddResource("AdvanceService", this);

        _player = ResourceLocator.GetResource<Player>("Player");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
    }


    public IEnumerator Advance()
    {
        Advanceables.ForEach(x => { if (x != null) x.MoveDown(); });

        yield return new WaitUntil(() => Advanceables.All(x => !x.IsMoving));

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
