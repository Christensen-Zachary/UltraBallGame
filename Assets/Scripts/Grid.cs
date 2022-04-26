using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    public int NumberOfDivisions { get; private set; } = 12;
    [field: SerializeField]
    public float UnitScale { get; private set; } = 1;
    
    public Vector2 Origin { get; private set; } // origin is the location of GetPosition(0, 0). It is set to the top left corner

    private Background _background;
    private LevelService _levelService;

    private void Awake()
    {
        ResourceLocator.AddResource("Grid", this);

        _background = ResourceLocator.GetResource<Background>("Background");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");

        NumberOfDivisions = _levelService.NumberOfDivisions;

        UnitScale = _background.GetWidth / NumberOfDivisions;
        Origin = _background.GetTopLeftCorner + new Vector2(1, -1) * UnitScale / 2;
    }

    public Vector2 GetPosition(float col, float row)
    {
        return Origin + new Vector2(col, -row) * UnitScale;
    }

    public bool Contains(Vector2 point)
    {
        return _background.GetBounds.Contains(point);
    }
}
