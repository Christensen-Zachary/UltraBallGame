using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }
    private Camera _mainCamera;

    private Grid _grid;
    private Aim _aim;
    private float _radius = 1;

    public PlayerHealth _playerHealth;
    public float _health = 100; // only public for the editor
    public float Health { get { return _health; } set { _health = value; if (_playerHealth != null) { _playerHealth.SetNumber((int)value); } } }

    public List<Shootable> Shootables { get; private set; } = new List<Shootable>();
    public bool IsFireRunning { get; private set; } = true;

    public Slider _movePlayerSlider; // set in editor, checked for null so is not necessary

    private Vector2 _leftMostPosition;
    private Vector2 _rightMostPosition;
    private float _distanceBetweenBounds;

    private float _maxYForMovePlayer;

    void Awake()
    {
        ResourceLocator.AddResource("Player", this);

        Health = 100f;

        _mainCamera = Camera.main;
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _aim = ResourceLocator.GetResource<Aim>("Aim");

        transform.localPosition = _grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, _grid.NumberOfDivisions - 1);
        transform.localScale = _grid.UnitScale * Vector2.one;

        _leftMostPosition = _grid.GetPosition(0, _grid.NumberOfDivisions - 1);
        _rightMostPosition = _grid.GetPosition(_grid.NumberOfDivisions - 1, _grid.NumberOfDivisions - 1);
        _distanceBetweenBounds = Vector2.Distance(_leftMostPosition, _rightMostPosition);
        _maxYForMovePlayer = _grid.GetPosition(0, 0).y;

        if (_movePlayerSlider != null)
        {
            _movePlayerSlider.value = 0.5f;
            _movePlayerSlider.onValueChanged.AddListener((float value) => { MovePlayerBySlider(value); });
        }

        ThemeVisitor.Visit(this);
    }

    public void SetRadius()
    {
        Shootable shootable = Shootables.First();
        _radius = _grid.UnitScale * shootable.GetComponent<CircleCollider2D>().radius * shootable.transform.localScale.x;
    }

    
    public void ShowAim(Vector2 direction)
    {
        direction.Normalize();
        _aim.ShowPrediction(transform.position, direction, _radius);
    }

    public void HideAim()
    {
        _aim.HidePrediction();
    }

    public void RunFire(Vector2 direction)
    {
        direction.Normalize();
        StartCoroutine(Fire(direction));
    }

    private IEnumerator Fire(Vector2 direction)
    {
        IsFireRunning = true;
        foreach (var ball in Shootables)
        {
            ball.Fire(direction);
            yield return new WaitForSeconds(0.2f);
        }
        IsFireRunning = false;
    }

    public void EndFire()
    {
        StopAllCoroutines();
        IsFireRunning = false;
        foreach (var ball in Shootables)
        {
            ball.Return();
        }
    }


    public bool IsFireComplete()
    {
        return !Shootables.Any(x => x.IsReturned == false) && !IsFireRunning;
    }



    public void MovePlayer(Vector2 newPosition)
    {
        if (newPosition.y >= transform.position.y && newPosition.x > _leftMostPosition.x && newPosition.x < _rightMostPosition.x && newPosition.y < _maxYForMovePlayer)
        {
            newPosition = new Vector2(newPosition.x, transform.position.y);
            transform.position = newPosition;
            
            if (_movePlayerSlider != null)
            {
                _movePlayerSlider.SetValueWithoutNotify((newPosition.x - _leftMostPosition.x) / _distanceBetweenBounds);
            }
        }
    }


    public void MovePlayerBySlider(float value)
    {
        transform.position = new Vector2(Vector2.Lerp(_leftMostPosition, _rightMostPosition, value).x, transform.position.y);
    }
}
