using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutCellSize : MonoBehaviour
{
    private GridLayoutGroup GridLayoutGroup { get; set; }
    public Canvas _canvas;

    private void Awake()
    {
        GridLayoutGroup = GetComponent<GridLayoutGroup>();
        GridLayoutGroup.cellSize = Vector2.one * _canvas.pixelRect.width / 8f;
    }
}
