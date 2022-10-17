using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLocator : MonoBehaviour
{
    private Dictionary<string, MonoBehaviour> Resources { get; set; } = new Dictionary<string, MonoBehaviour>();

    public void AddResource<T>(string key, T resource) where T : MonoBehaviour
    {
        if (Resources.ContainsKey(key))
        {
            string errMsg = $"Resource {key} was added to resource dictionary twice";
            Debug.LogError(errMsg);
            throw new System.Exception(errMsg);
        }
        else
        {
            Resources.Add(key, resource);
        }
    }

    public T GetResource<T>(string key) where T : MonoBehaviour
    {
        if (Resources.ContainsKey(key))
        {
            return Resources[key] as T;
        }
        
        string errMsg = $"Resource {key} was not found";
        Debug.LogError(errMsg);
        //throw new System.Exception(errMsg);
        return null;
    }
}
