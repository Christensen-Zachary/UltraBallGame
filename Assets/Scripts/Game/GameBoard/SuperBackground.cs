using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBackground : MonoBehaviour
{
    public bool _allowThemeColorChange = true;

    private void Awake()
    {
        Camera mainCamera = Camera.main;
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);

        // fit exactly to screen
        (float height, float width) = BGUtils.GetScreenSize();
        transform.localScale = new Vector2(width, height);

        if (_allowThemeColorChange) ThemeVisitor.Visit(this);
    }
}
