using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Background : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public static readonly float LEAVE_SIDES_OPEN_BY_PERCENT = (1 - (2 / (1 + Mathf.Sqrt(5)))); // percent of the side that should not be overlapped by square. amount is for both sides combined
    private SpriteRenderer _sr;
    public float GetWidth => transform.localScale.x;
    public Vector2 GetTopLeftCorner =>  new Vector2(-_sr.bounds.extents.x, _sr.bounds.extents.y);
    public Bounds GetBounds => _sr.bounds;

    private void Awake()
    {
        ResourceLocator.AddResource("Background", this);

        _sr = GetComponent<SpriteRenderer>();

        (float height, float width) = BGUtils.GetScreenSize();

        float scale;
        float defaultScale = 0.9f * width;
        if (height < width) // is landscape
        {
            if ((width - height) < (LEAVE_SIDES_OPEN_BY_PERCENT * width)) scale = (1 - LEAVE_SIDES_OPEN_BY_PERCENT) * width;
            else scale = height;
        }
        else // is portrait
        {
            if (height / width < 1.9f)//(height - width) < (LEAVE_SIDES_OPEN_BY_PERCENT * height))
            {
                scale = (1 - LEAVE_SIDES_OPEN_BY_PERCENT) * height;
                print("Leave sides open scale set");
            }
            else
            {
                scale = defaultScale;
            }
            //scale = 0.9f * width;
        }
        if (scale > defaultScale)
        {
            scale = defaultScale;
            print("Scale too large, overridden to default");
        }
        transform.localScale = Vector2.one * scale;
        //transform.localPosition = Vector3.zero;

        ThemeVisitor.Visit(this);
    }




}