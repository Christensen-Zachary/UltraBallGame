using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerupManager : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private Grid _grid;
    private LevelService _levelService;
    private Player _player;
    private FacBrick _facBrick;
    private FacBall _facBall;
    private EndTurnDestroyService _endTurnDestroyService;

    private int _closestColumn = 0;
    private List<(GameObject, BrickType)> FloorBricks = new List<(GameObject, BrickType)>();
    private bool _usedFloorBricks = false;
    private bool _usedFireBalls = false;

    private void Awake() 
    {
        ResourceLocator.AddResource("PowerupManager", this);

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _player = ResourceLocator.GetResource<Player>("Player");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
    }

    public void EndTurnPowerupManager()
    {
        _usedFloorBricks = false;
        _usedFireBalls = false;
    }

    public void UseFloorBricks()
    {
        if (_levelService.FloorBricksPowerUpCount > 0 && !_usedFloorBricks)
        {
            AddFloorBricks(); 
        }
    }

    public void AddFloorBricks()
    {
        _usedFloorBricks = true;
        _levelService.FloorBricksPowerUpCount--;

        _closestColumn = GetClosestColumnToPlayer();
        _player.MovePlayer(new Vector2(_grid.GetPosition(_closestColumn, 0).x, _player.transform.position.y));

        FloorBricks.Clear();
        for (int i = 0; i < _grid.NumberOfDivisions; i++)
        {
            BrickType brickType = BrickType.Square;
            if (i == _closestColumn - 1) brickType = BrickType.Triangle0;
            else if (i == _closestColumn + 1) brickType = BrickType.Triangle90;

            if (i != _closestColumn)
            {
                GameObject obj = _facBrick.Create(new Brick { BrickType = brickType, Col = i, Row = 0, Health = _levelService.Balls.Count * 2 }, new Type[] { typeof(Advanceable) });
                _endTurnDestroyService.AddGameObject(obj);
                obj.GetComponentInChildren<Damageable>()._doesCountTowardsWinning = false;
                FloorBricks.Add((obj, brickType));
            }
        }
    }

    public void AdjustFloorBricks()
    {
        if (_usedFloorBricks)
        {
            int closestColumn = GetClosestColumnToPlayer();

            if (closestColumn != _closestColumn)
            {
                //print($"ClosestColumn {closestColumn} _closestCloumn {_closestColumn}");
                _closestColumn = closestColumn;
                (GameObject, BrickType)[] tempBricks = new (GameObject, BrickType)[_levelService.NumberOfDivisions - 1];
                bool passedClosestColumn = false;
                for (int i = 0; i < _levelService.NumberOfDivisions; i++)
                {
                    if (i == closestColumn)
                    {
                        passedClosestColumn = true;
                        continue;
                    }

                    BrickType brickType;
                    if (i == closestColumn - 1) brickType = BrickType.Triangle0;
                    else if (i == closestColumn + 1) brickType = BrickType.Triangle90;
                    else brickType = BrickType.Square;

                    (GameObject, BrickType) brick = FloorBricks.Find(x => x.Item2 == brickType);

                    brick.Item1.transform.localPosition = _grid.GetPosition(i, 0);
                    tempBricks[passedClosestColumn ? i - 1 : i] = brick;

                    FloorBricks.Remove(brick);
                }
                FloorBricks = tempBricks.ToList();
            }
        }
    }

    private int GetClosestColumnToPlayer()
    {
        int closestColumn = 0;
        float closestColumnDistance = 100;
        Vector2 playerPosition = new Vector2(_player.transform.position.x, 0);
        for (int i = 0; i < _grid.NumberOfDivisions; i++)
        {
            Vector2 position = new Vector2(_grid.GetPosition(i, 0).x, 0);
            if (Vector2.Distance(playerPosition, position) < closestColumnDistance)
            {
                closestColumn = i;
                closestColumnDistance = Vector2.Distance(playerPosition, position);
            }

        }

        return closestColumn;
    }

    public void UseFirePowerup()
    {
        if (_levelService.FireBallsPowerUpCount > 0)
        {
            SetBallsOnFire(); 
        }
    }

    private void SetBallsOnFire()
    {
        if (_usedFireBalls) return;

        _usedFireBalls = true;
        _levelService.FireBallsPowerUpCount--;

        for (int i = 0; i < _levelService.Balls.Count; i++) 
        {
            GameObject obj = _facBrick.Create(new Brick(BrickType.FirePowerup, 100, 100));
            obj.transform.SetParent(_player.transform);
            obj.transform.localPosition =  Vector3.zero;
            obj.transform.localScale = Vector3.one;
            _endTurnDestroyService.AddGameObject(obj);
        }
    }

    public void UseExtraBalls()
    {
        if (_levelService.ExtraBallPowerUpCount > 0)
        {
            _levelService.ExtraBallPowerUpCount--;
            for (int i = 0; i < _levelService.Balls.Count; i++)
            {
                _endTurnDestroyService.AddGameObject(
                    _facBall.Create(_levelService.Balls[i])
                );
                _levelService.BallCounter++;
            }
        }
    }
}
