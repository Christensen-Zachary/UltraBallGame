
using System.Collections.Generic;
using UnityEngine;

public class SelectedCursorManager : MonoBehaviour 
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    public GameObject SelectedCursorPrefab { get; set; } // reference set in editor in prefab
    private List<SelectedCursor> _selectedCursors = new List<SelectedCursor>();

    private void Awake() 
    {
        ResourceLocator.AddResource("SelectedCursorManager", this);    
    }


    public SelectedCursor GetSelectedCursor()
    {
        SelectedCursor cursor = _selectedCursors.Find(x => !x.InUse);
        if (cursor == null)
        {
            cursor = CreateSelectedCursor();
            _selectedCursors.Add(cursor);
        }
        cursor.InUse = true;
        return cursor;
    }

    public void ReturnSelectedCursors()
    {
        _selectedCursors.ForEach(x => x.ReturnSelectedCursor());
    }

    private SelectedCursor CreateSelectedCursor()
    {
        return Instantiate(SelectedCursorPrefab).GetComponent<SelectedCursor>();
    }
    
}