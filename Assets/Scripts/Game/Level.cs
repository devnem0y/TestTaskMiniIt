using System;
using System.Collections.Generic;
using UnityEngine;
using UralHedgehog;

public class Level : MonoBehaviour, ILevel
{
    private const float ELEMENT_WIDTH = 1.76f;
    private const float ELEMENT_HEIGHT = 0.52f;
    private const float OFFSET = 1.5f;
    
    [SerializeField] private Transform _wrapper;

    private ComplexityData _data;
    private IPlayer _player;
    private Platform _platform;
    private Ball _ball;
    private int _countBall;
    
    private int _gridSize;
    private List<int> _bricks;
    
    public event Action Completed;
    public event Action Lose;
    
    public void Init(ComplexityData data, IPlayer player)
    {
        _data = data;
        _player = player;
    }
    
    public void Load()
    {
        _player.ResetScore();
        _countBall = 3;
        
        CreatePlatform();
        CreateBall();
        CreateBricks();
    }

    public void Reload()
    {
        Clear();
        Load();
    }

    public void Clear()
    {
        Destroy(_platform.gameObject);
        ClearBricks(_wrapper);
    }

    private void CreatePlatform()
    {
        _platform = Instantiate(_data.Platform, new Vector3(0f, -4f), Quaternion.identity, transform);
        _platform.Init(_data.PlatformWidth);
    }

    private void CreateBall()
    {
        _ball = Instantiate(_data.Ball, transform);
        _ball.Init(_platform, _data.BallForce);
    }
    
    private void CreateBricks()
    {
        ClearBricks(_wrapper);
        
        _gridSize = _data.BricksSize;
        var brickId = 0;
        _bricks = new List<int>();
        
        var gridWidth = _gridSize * ELEMENT_WIDTH;
        var gridHeight = _gridSize * ELEMENT_HEIGHT;
        var minX = -gridWidth / 2 + ELEMENT_WIDTH / 2;
        var maxY = gridHeight / 2 - ELEMENT_HEIGHT / 2 + OFFSET;

        var gridSizeX = _gridSize;

        for (var y = 0; y < _gridSize; y++)
        {
            for (var x = 0; x < gridSizeX; x++)
            {
                var position = new Vector2(minX + x * ELEMENT_WIDTH, maxY - y * ELEMENT_HEIGHT);
                var b = Instantiate(_data.Brick, position, Quaternion.identity, _wrapper);
                b.Init(RemoveBrick);
                _bricks.Add(brickId);
                brickId++;
            }
            
            minX += ELEMENT_WIDTH / 2;
            gridSizeX--;
        }
    }
    
    private static void ClearBricks(Transform wrapper)
    {
        while (wrapper.childCount > 0) 
        {
            DestroyImmediate(wrapper.GetChild(0).gameObject);
        }
    }
    
    private void RemoveBrick()
    {
        _player.AddScore();
        _bricks.RemoveAt(_bricks.Count - 1);
        
        if (_bricks.Count > 0) return;

        Game.Instance.ChangeState(GameState.VICTORY);
    }

    private void Failing()
    {
        _countBall--;
        if (_countBall == 0) Game.Instance.ChangeState(GameState.DEFEAT);
    }
}