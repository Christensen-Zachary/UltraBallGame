
using UnityEngine;

public class SelectedCursor : MonoBehaviour
{
    public bool InUse { get; set; } = false;


    public void ReturnSelectedCursor()
    {
        InUse = false;

        transform.SetParent(null, true);
        transform.position = new Vector3(1000, 1000, 0);
    }
}
