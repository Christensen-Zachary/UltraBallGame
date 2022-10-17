using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFireDirection : MonoBehaviour, IGetFireDirection
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("RandomFireDirection", this);
    }

    public Vector2 GetFireDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Mathf.Abs(Random.Range(0.1f, 1));
        return new Vector2(x, y);
    }
}
