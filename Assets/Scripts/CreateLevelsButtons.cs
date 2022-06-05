using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CreateLevelsButtons : MonoBehaviour
{
    [field: SerializeField]
    public GameObject ButtonPrefab { get; set; } // set in editor

    public void CreateButtons(int from, int to)
    {
        print("CreateButtons");
        for (int i = from; i <= to; i++)
        {
            print($"i: {i}");
            GameObject obj = Instantiate(ButtonPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(transform);

            Action<int> setListener = (capturedi) =>
            {
                obj.GetComponent<Button>().onClick.AddListener(() => LoadLevel(capturedi));
                obj.GetComponent<BtnLoadLevel>().SetText(capturedi.ToString());
            };

            setListener(i);
        }
        print("End CreateButtons");
    }

    public void LoadLevel(int levelNumber)
    {
        ES3.Save(BGStrings.ES_LEVELNUM, levelNumber);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}
