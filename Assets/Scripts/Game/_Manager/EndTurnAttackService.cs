using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndTurnAttackService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    public List<IAttack> Attacks { get; private set; } = new List<IAttack>();


    private Grid _grid;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");

        ResourceLocator.AddResource("EndTurnAttackService", this);
    }


    public IEnumerator Attack()
    {
        if (Attacks.Count > 0)
        {
            Attacks.ForEach(x => { if (x != null) { x.Attack(); } });

            // The ratio of time to speed is 2 seconds for a full travel. This calculates the ratio of that time needed by the ratio of the distance of the highest attack from the bottom
            float maxDistance = Mathf.Abs(_grid.GetPosition(0, 0).y - _grid.GetPosition(0, _grid.NumberOfDivisions - 1).y);
            float maxDistanceToBottom = 0;
            Vector2 bottomPosition = new Vector2(0, _grid.GetPosition(0, _grid.NumberOfDivisions - 1).y);
            foreach (IAttack attack in Attacks)
            {
                Vector2 attackPosition = new Vector2(0, attack.GetGameObject().transform.position.y);
                if (Vector2.Distance(attackPosition, bottomPosition) > maxDistanceToBottom)
                {
                    maxDistanceToBottom = Vector2.Distance(attackPosition, bottomPosition);
                }
            }

            yield return new WaitForSeconds(Background.BACKGROUND_RATIO * 4 * maxDistanceToBottom / maxDistance);
        }
    }

    public void ResetAttackService()
    {
        Attacks.Clear();
    }
}
