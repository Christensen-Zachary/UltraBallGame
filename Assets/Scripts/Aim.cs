using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    public GameObject EndPredictionSprite { get; private set; }
    [field: SerializeField]
    public GameObject MidPredictionSprite { get; private set; }
    private List<GameObject> EndPointPredictionSprites { get; set; }
    private List<GameObject> MidPointPredictionSprites { get; set; }
    [field: SerializeField]
    private const float MAX_DOTS = 25f;

    public Vector2 Direction { get; private set; }
    private float ContactOffset { get; set; }
    private float MaxDistance { get; set; } = 10;

    

    private void Awake()
    {
        ResourceLocator.AddResource("Aim", this);

        Background background = ResourceLocator.GetResource<Background>("Background");
        MaxDistance = Vector2.Distance(background.GetBounds.extents, -background.GetBounds.extents);


        ThemeVisitor.Visit(this);

        Grid grid = ResourceLocator.GetResource<Grid>("Grid");
        EndPointPredictionSprites = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(EndPredictionSprite);
            obj.transform.SetParent(transform);
            obj.transform.localScale = grid.UnitScale * Vector2.one;
            EndPointPredictionSprites.Add(obj);
        }

        MidPointPredictionSprites = new List<GameObject>();
        HidePrediction();

        ContactOffset = Physics2D.defaultContactOffset * 200;
    }

    public GameObject GetMidPointPredictionSprite()
    {
        GameObject obj = MidPointPredictionSprites.Find(x => x.activeSelf == false);
        if (obj == null)
        {
            obj = Instantiate(MidPredictionSprite);
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one * 0.25f;
            MidPointPredictionSprites.Add(obj);
        }

        return obj;
    }

    public void ShowPrediction(Vector2 from, Vector2 to, float radius)
    {
        Direction = to;
        Direction.Normalize();

        HidePrediction();
        List<Vector2> predictions = GetPredictions(from, to, 3, radius);
        for (int i = 0; i < predictions.Count; i++)
        {
            GameObject obj = EndPointPredictionSprites.Find(x => x.activeSelf == false);
            if (obj != null)
            {
                obj.SetActive(true);
                obj.transform.position = predictions[i];
            }

            if (i != predictions.Count - 1)
            {
                float distance = Vector2.Distance(predictions[i], predictions[i + 1]);
                int numberOfDots = Mathf.CeilToInt(distance / MaxDistance * MAX_DOTS);
                Vector2 inc = (predictions[i + 1] - predictions[i]) / numberOfDots;
                for (int j = 1; j < numberOfDots; j++)
                {
                    GameObject dot = GetMidPointPredictionSprite();
                    dot.transform.position = predictions[i] + j * inc;
                    dot.SetActive(true);
                }
            }
        }
    }

    public void HidePrediction()
    {
        EndPointPredictionSprites.ForEach(x => x.SetActive(false));
        MidPointPredictionSprites.ForEach(x => x.SetActive(false));
    }

    private List<Vector2> GetPredictions(Vector2 from, Vector2 direction, int predictionCount, float radius)
    {
        direction = direction.normalized;
        List<Vector2> predictions = new List<Vector2>() { from };

        int layerMask = (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Reflect")) & ~(1 << LayerMask.NameToLayer("Ball"));
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
                //else if (hit.collider.name.Equals(lastName))
                //{
                //    RaycastHit2D[] hits = Physics2D.CircleCastAll(from, radius, direction, Mathf.Infinity, layerMask);
                //    for (int j = 0; j < hits.Length; j++)
                //    {
                //        if (true)//!hits[j].collider.name.Equals(lastName))
                //        {
                //            if (hits[j].collider.CompareTag(BGStrings.TAG_BOTTOMWALL))
                //            {
                //                return predictions;
                //            }

                //            predictions.Add(hits[j].centroid);
                //            break;
                //        }
                //    }
                //}
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
