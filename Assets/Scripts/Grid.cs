using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private Vector2 _origin; // origin is the location of GetPosition(0, 0). It is set to the top left corner
    [field: SerializeField]
    public int NumberOfDivisions { get; private set; } = 12;
    [field: SerializeField]
    public float UnitScale { get; set; } = 1;

    private Background _background;

    private void Awake()
    {
        ResourceLocator.AddResource("Grid", this);

        _background = ResourceLocator.GetResource<Background>("Background");

        UnitScale = _background.GetWidth / NumberOfDivisions;
        _origin = _background.GetTopLeftCorner + new Vector2(1, -1) * UnitScale / 2;
    }

    public Vector2 GetPosition(float col, float row)
    {
        return _origin + new Vector2(col, -row) * UnitScale;
    }

    public bool Contains(Vector2 point)
    {
        return _background.GetBounds.Contains(point);
    }
}
