using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    public int NumberOfDivisions { get; private set; }
    public bool _overrideNumberOfDivisions = false;
    public float GameBoardHeight => NumberOfDivisions * Background.BACKGROUND_RATIO;
    [field: SerializeField]
    public float UnitScale { get; private set; }
    
    public Vector2 Origin { get; private set; } // origin is the location of GetPosition(0, 0). It is set to the top left corner

    private Background _background;
    private LevelService _levelService;

    private void Awake()
    {
        ResourceLocator.AddResource("Grid", this);

        _background = ResourceLocator.GetResource<Background>("Background");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");

        if (_levelService != null)
        {
            if (_levelService.LevelLoaded && !_overrideNumberOfDivisions) NumberOfDivisions = _levelService.NumberOfDivisions;
        }

        UnitScale = _background.GetWidth / NumberOfDivisions;
        Origin = _background.GetBottomLeftCorner + Vector2.one * UnitScale / 2;
    }

    public Vector2 GetPosition(float col, float row)
    {
        return Origin + new Vector2(col, row) * UnitScale;
    }

    public bool Contains(Vector2 point)
    {
        return _background.GetBounds.Contains(point);
    }
}
