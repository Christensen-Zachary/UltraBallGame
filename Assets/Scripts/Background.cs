using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Background : MonoBehaviour
{
    private float _leaveSidesOpenByPercent = 1 / 3f; // percent of the side that should not be overlapped by square. amount is for both sides combined
    private SpriteRenderer _sr;
    public float GetWidth => transform.localScale.x;
    public Vector2 GetTopLeftCorner =>  new Vector2(-_sr.bounds.extents.x, _sr.bounds.extents.y);
    public Bounds GetBounds => _sr.bounds;
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }


    private void Awake()
    {
        ResourceLocator.AddResource("Background", this);

        _sr = GetComponent<SpriteRenderer>();

        (float height, float width) = BGUtils.GetScreenSize();

        float scale;
        if (height < width)
        {
            if ((width - height) < (_leaveSidesOpenByPercent * width)) scale = (1 - _leaveSidesOpenByPercent) * height;
            else scale = height;
        }
        else
        {
            if ((height - width) < (_leaveSidesOpenByPercent * height)) scale = (1 - _leaveSidesOpenByPercent) * width;
            else scale = width;
        }
        transform.localScale = Vector2.one * scale;
        //transform.localPosition = Vector3.zero;
    }



}
