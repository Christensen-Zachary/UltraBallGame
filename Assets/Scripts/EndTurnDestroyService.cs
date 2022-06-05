using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnDestroyService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private List<GameObject> GameObjects { get; set; } = new List<GameObject>();

    private void Awake()
    {
        ResourceLocator.AddResource("EndTurnDestroyService", this);
    }

    public void AddGameObject(GameObject gameObject)
    {
        GameObjects.Add(gameObject);    
    }

    public void DestroyGameObjects()
    {
        GameObjects.ForEach(x => {
            if (x.TryGetComponent(out Advanceable advanceable))
            {
                advanceable.RemoveFromList();
            }
            Destroy(x); 
        });
        GameObjects = new List<GameObject>();
    }


}
