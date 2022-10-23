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
        for (int i = from; i <= to; i++)
        {
            GameObject obj = Instantiate(ButtonPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;

            Action<int> setListener = (capturedi) =>
            {
                obj.GetComponent<Button>().onClick.AddListener(() => LoadLevel(capturedi));
                obj.GetComponent<BtnLoadLevel>().SetText(capturedi.ToString());
            };

            setListener(i);
        }
    }

    public void LoadLevel(int levelNumber)
    {
        ES3.Save(BGStrings.ES_LEVELNUM, levelNumber);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}
