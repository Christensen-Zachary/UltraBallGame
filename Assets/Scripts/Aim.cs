using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameObject PredictionSprite { get; set; }
    [field: SerializeField]
    private List<GameObject> PredictionSprites { get; set; }
    
    public Vector2 Direction { get; private set; }
    private float ContactOffset { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("Aim", this);

        PredictionSprites = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(PredictionSprite);
            obj.transform.SetParent(transform);
            PredictionSprites.Add(obj);
        }
        HidePrediction();

        ContactOffset = Physics2D.defaultContactOffset * 200;
    }

    public void ShowPrediction(Vector2 from, Vector2 to, float radius)
    {
        Direction = to;
        Direction.Normalize();

        HidePrediction();
        GetPredictions(from, to, 3, radius).ForEach(x =>
        {
            GameObject obj = PredictionSprites.Find(x => x.activeSelf == false);
            if (obj != null)
            {
                obj.SetActive(true);
                obj.transform.position = x;
            }
        });
    }

    public void HidePrediction()
    {
        PredictionSprites.ForEach(x => x.SetActive(false));
    }

    private List<Vector2> GetPredictions(Vector2 from, Vector2 direction, int predictionCount, float radius)
    {
        direction = direction.normalized;
        List<Vector2> predictions = new List<Vector2>() { from };

        int layerMask = (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Brick")) & ~(1 << LayerMask.NameToLayer("Ball"));
        string lastName = "";
        for (int i = 0; i < predictionCount; i++)
        {
            from += ContactOffset * direction;
            // predictions.Add(from); // used to see contactoffset
            RaycastHit2D hit = Physics2D.CircleCast(from, radius, direction, Mathf.Infinity, layerMask);
            if (hit)
            {
                if (hit.collider.CompareTag(BGStrings.TAG_BOTTOMWALL))
                {
                    return predictions;
                }
                else if (hit.collider.name.Equals(lastName))
                {
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(from, radius, direction, Mathf.Infinity, layerMask);
                    for (int j = 0; j < hits.Length; j++)
                    {
                        if (!hits[j].collider.name.Equals(lastName))
                        {
                            if (hits[j].collider.CompareTag(BGStrings.TAG_BOTTOMWALL))
                            {
                                return predictions;
                            }

                            predictions.Add(hits[j].centroid);
                            break;
                        }
                    }
                }
                else
                {
                    lastName = hit.collider.name;
                    predictions.Add(hit.centroid);
                    direction = Vector2.Reflect(direction, hit.normal);
                    from = hit.centroid;
                }
            }
        }

        return predictions;
    }
}
