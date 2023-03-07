using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EndTurnDestroyService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private List<GameObject> GameObjects { get; set; } = new List<GameObject>();

    private FacBall _facBall;

    private void Awake()
    {
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");

        ResourceLocator.AddResource("EndTurnDestroyService", this);
    }

    public void AddGameObject(GameObject gameObject)
    {
        GameObjects.Add(gameObject);    
    }

    public void DestroyGameObjects()
    {
        GameObjects.ForEach(x => {
            if (x != null)
            {
                float destroyTime = 0;
                bool doDestroy = true;
                if (x.TryGetComponent(out Advanceable advanceable))
                {
                    advanceable.RemoveFromList();
                }
                Damageable damageable = x.GetComponentInChildren<Damageable>();
                if (damageable != null)
                {
                    damageable.AddToDestroyed();
                    damageable.FadeAndDestroy(1);
                    doDestroy = false;
                }
                Shootable shootable = x.GetComponent<Shootable>();
                if (shootable != null)
                {
                    _facBall.DestroyBall(shootable);
                    shootable.transform.localPosition = Vector3.one * 1000;
                    destroyTime = 3f;
                    doDestroy = false;
                }
                EvilBrick evilBrick = x.GetComponent<EvilBrick>();
                if (evilBrick != null)
                {
                    evilBrick.RemoveFromList();
                }
                if (doDestroy) Destroy(x, destroyTime);
            }
        });
        GameObjects = new List<GameObject>();
    }


}
