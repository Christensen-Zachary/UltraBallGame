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
    private LevelService _levelService;
    private BallCounter _ballCounter;
    private float _radius = 1;

    public PlayerHealth _playerHealth;
    public float _health = 5; // only public for the editor
    public float Health { get { return _health; } set { _health = value; if (_playerHealth != null) { _playerHealth.SetNumber((int)value); } } }

    public List<Shootable> Shootables { get; private set; } = new List<Shootable>();
    private List<Shootable> LoadedShots { get; set; } = new List<Shootable>();
    private Vector2 _direction = Vector2.up;
    private float _timeBetweenShots = 0.1f;
    private float _timer = 0.1f;
    public bool IsFireRunning { get; private set; } = false;

    public Slider _movePlayerSlider; // set in editor, checked for null so is not necessary

    private Vector2 _leftMostPosition;
    private Vector2 _rightMostPosition;
    public float LeftMostCol { get; private set; }
    public float RightMostCol { get; private set; }
    private float _distanceBetweenBounds;

    private float _maxYForMovePlayer;

    private int _glowID;

    void Awake()
    {
        ResourceLocator.AddResource("Player", this);
        _glowID = Shader.PropertyToID("_Glow");

        Health = 100f;

        _mainCamera = Camera.main;
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _aim = ResourceLocator.GetResource<Aim>("Aim");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _ballCounter = ResourceLocator.GetResource<BallCounter>("BallCounter");

        transform.localPosition = _grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, 0);
        transform.localScale = _grid.UnitScale * Vector2.one;

        LeftMostCol = 1;
        RightMostCol = _grid.NumberOfDivisions - 1 - 1;
        _leftMostPosition = _grid.GetPosition(LeftMostCol, _grid.NumberOfDivisions - 1);
        _rightMostPosition = _grid.GetPosition(RightMostCol, _grid.NumberOfDivisions - 1);
        _distanceBetweenBounds = Vector2.Distance(_leftMostPosition, _rightMostPosition);
        _maxYForMovePlayer = _grid.GetPosition(0, _grid.GameBoardHeight).y;

        if (_movePlayerSlider != null)
        {
            _movePlayerSlider.value = 0.5f;
            _movePlayerSlider.onValueChanged.AddListener((float value) => { MovePlayerBySlider(value); });
        }

        ThemeVisitor.Visit(this);
    }


    private void Update() 
    {
        if (IsFireRunning)
        {
            _timer += Time.deltaTime;
            if (_timer > _timeBetweenShots)
            {
                _timer -= _timeBetweenShots;
                if (LoadedShots.Count > 0)
                {
                    Shootable shootable = LoadedShots[0];
                    LoadedShots.Remove(shootable);
                    if (shootable != null)
                    {
                        shootable.Fire(_direction);
                        _ballCounter.Subtract(1);
                    }
                }
                else
                {
                    IsFireRunning = false;
                    _timer = _timeBetweenShots;
                    LoadedShots.Clear();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EvilBrickAttackBall evilBrickAttackBall))
        {
            Health -= 25f;
        }
    }

    public void EnableHDR(bool enable)
    {
        _aim.EnableHDR(enable);
        if (enable) GetComponent<SpriteRenderer>().material.SetFloat(_glowID, ThemeData.PlayerBrightness);
        else GetComponent<SpriteRenderer>().material.SetFloat(_glowID, 0);
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
        _direction = direction;
        Shootables.ForEach(x => LoadedShots.Add(x));
        IsFireRunning = true;
        // StartCoroutine(Fire(direction));
    }

    private IEnumerator Fire(Vector2 direction)
    {
        IsFireRunning = true;
        foreach (var ball in Shootables)
        {
            ball.Fire(direction);
            _ballCounter.Count--;
            yield return new WaitForSeconds(0.05f);
        }
        IsFireRunning = false;
    }

    public void EndFire()
    {
        StopAllCoroutines();
        LoadedShots.Clear();
        _timer = _timeBetweenShots;
        IsFireRunning = false;
        foreach (var ball in Shootables)
        {
            ball.Return();
        }
    }


    public bool IsFireComplete()
    {
        return !IsFireRunning && !Shootables.Any(x => x.IsReturned == false);
    }



    public void MovePlayer(Vector2 newPosition)
    {
        if (newPosition.y >= transform.localPosition.y && newPosition.x > _leftMostPosition.x && newPosition.x < _rightMostPosition.x && newPosition.y < _maxYForMovePlayer)
        {
            if (_movePlayerSlider != null)
            {
                _movePlayerSlider.SetValueWithoutNotify((newPosition.x - _leftMostPosition.x) / _distanceBetweenBounds);
            }

            newPosition = new Vector2(newPosition.x, transform.localPosition.y);
            transform.localPosition = newPosition;
        }
    }


    public void MovePlayerBySlider(float value)
    {
        transform.position = new Vector2(Vector2.Lerp(_leftMostPosition, _rightMostPosition, value).x, transform.position.y);
    }
}
