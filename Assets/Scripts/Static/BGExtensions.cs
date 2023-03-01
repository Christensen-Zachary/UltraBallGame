using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left) =>
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);

    public static void SetRight(this RectTransform rt, float right) =>
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);

    public static void SetTop(this RectTransform rt, float top) =>
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);

    public static void SetBottom(this RectTransform rt, float bottom) =>
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
}

public static class ListExtensions
{
    // from https://stackoverflow.com/questions/273313/randomize-a-listt
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
