using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class StaticTest : MonoBehaviour
{
    public string levelstr = "";

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 1999);
        print($"Getting level {rand}");

        FieldInfo field = typeof(UnityTest).GetField($"LEVEL{rand}");
        levelstr = field.GetValue(null) as string;
        if (levelstr == null)
        {
            print($"levelstr was NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelstr = "";
        }

        //int rand = Random.Range(0, 1999);
        //print($"Getting level {rand}");

        //FieldInfo field = typeof(UnityTest).GetField($"LEVEL{rand}");
        //levelstr = field.GetValue(null) as string;
        //if (levelstr == null)
        //{
        //    print($"levelstr was NULL");
        //}
    }
}
