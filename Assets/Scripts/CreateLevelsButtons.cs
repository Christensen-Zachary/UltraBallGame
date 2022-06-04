using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CreateLevelsButtons : MonoBehaviour
{
    [field: SerializeField]
    public MainMenuUI MainMenuUI { get; set; } // set in editor
    [field: SerializeField]
    public GameObject ButtonPrefab { get; set; } // set in editor


    private void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetTop(100);
        rectTransform.SetBottom(100);
        rectTransform.SetLeft(100);
        rectTransform.SetRight(100);
    }

    public void CreateButtons(int from, int to)
    {
        for (int i = from; i <= to; i++)
        {
            GameObject obj = Instantiate(ButtonPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(transform);

            Action<int> setListener = (capturedi) =>
            {
                obj.GetComponent<Button>().onClick.AddListener(() => MainMenuUI.LoadLevel(capturedi));
                obj.GetComponent<BtnLoadLevel>().SetText(capturedi.ToString());
            };

            setListener(i);
        }
    }

}
