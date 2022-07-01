using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMThemePreview : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private GState _state = GState.SetupLevel;

    private FacBrick _facBrick;
    private Grid _grid;
    private LevelService _levelService;

    private void Awake()
    {
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");

        _levelService.Bricks.ForEach(x => _facBrick.Create(x));
    }


}
