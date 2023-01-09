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

                EndTurnDestroyService endTurnDestroyService = EndTurnDestroyService;
                Ignite(shootable, endTurnDestroyService, PSGameObjectToClone);

                DestroyFirePowerup();
            }

        }
    }

    public static void Ignite(Shootable shootable, EndTurnDestroyService endTurnDestroyService, GameObject psToClone)
    {
        GameObject clone = Instantiate(psToClone);
        endTurnDestroyService.AddGameObject(clone);
        shootable.IsBuffed = true;
        shootable.Damage = 10;
        clone.transform.SetParent(shootable.transform);
        clone.transform.localPosition = Vector3.zero;
        clone.SetActive(true);
        clone.GetComponent<ParticleSystem>().Play();
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
