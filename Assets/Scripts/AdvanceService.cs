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

    private void Awake()
    {
        ResourceLocator.AddResource("AdvanceService", this);
    }


    public IEnumerator Advance()
    {
        Advanceables.ForEach(x => { if (x != null) x.MoveDown(); });

        yield return new WaitUntil(() => Advanceables.All(x => !x.IsMoving));
    }

}
