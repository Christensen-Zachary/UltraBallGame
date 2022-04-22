using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameObject PredictionSprite { get; set; }
    
    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        ResourceLocator.AddResource("Aim", this);

        PredictionSprite = Instantiate(PredictionSprite);
        PredictionSprite.transform.SetParent(transform);
    }

    public void ShowPrediction(Vector2 from, Vector2 to)
    {
        PredictionSprite.SetActive(true);
        PredictionSprite.transform.localPosition = to;
        Direction = to;
        Direction.Normalize();
    }

    public void HidePrediction()
    {
        PredictionSprite.SetActive(false);
    }
}
