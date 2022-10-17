using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePowerup : MonoBehaviour
{

    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; set; }

    private bool _hasCollided = false;
    public EndTurnDestroyService EndTurnDestroyService { get; set; }
    public AdvanceService AdvanceService { get; set; }
    
    [field: SerializeField]
    public GameObject PSGameObject { get; set; }
    [field: SerializeField]
    public GameObject PSGameObjectToClone { get; set; }


    private void Start()
    {
        PSGameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shootable shootable))
        {
            if (!_hasCollided && !shootable.IsBuffed)
            {
                _hasCollided = true;

                GameObject clone = Instantiate(PSGameObjectToClone);
                clone.transform.position = shootable.transform.position;
                EndTurnDestroyService.AddGameObject(clone);
                shootable.IsBuffed = true;
                shootable.Damage = 10;
                clone.transform.SetParent(shootable.transform);
                clone.SetActive(true);

                DestroyFirePowerup();
            }

        }
    }

    private void DestroyFirePowerup()
    {
        if (TryGetComponent(out Advanceable advanceable))
        {
            advanceable.RemoveFromList();
        }
        Destroy(gameObject);
    }
}
