using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacGame : MonoBehaviour
{
    public GameObject _prefabGame;
    public bool _createdLastUpdate = false;
    public List<GameObject> _objs;
    public int _numberOfGameBoardsToCreate = 5;
    public float _screenWidth;

    private void Start()
    {
        _screenWidth = BGUtils.GetScreenSize().width;
    }

    void Update()
    {
        if (_createdLastUpdate)
        {
            _createdLastUpdate = false;
            for (int i = 0; i < _numberOfGameBoardsToCreate; i++)
            {
                _objs[i].transform.localScale = Vector3.one * 1.5f / _numberOfGameBoardsToCreate;
                _objs[i].transform.position = new Vector3(transform.localScale.x - _screenWidth / 2f + i * _screenWidth / _numberOfGameBoardsToCreate, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _createdLastUpdate = true;
            _objs = new List<GameObject>();
            for (int i = 0; i < _numberOfGameBoardsToCreate; i++)
            {
                GameObject obj = Instantiate(_prefabGame);
                _objs.Add(obj);
            }
        }
    }
}
