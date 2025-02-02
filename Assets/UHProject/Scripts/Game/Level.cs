﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UralHedgehog;

public class Level : MonoBehaviour, ILevel
{
    private const float ELEMENT_WIDTH = 1.76f;
    private const float ELEMENT_HEIGHT = 0.52f;
    private const float OFFSET = 1.5f;
    private const float OFFSET_WALL = 0.1f;
    private const float SIZE_WALL = 0.5f;
    
    [SerializeField] private Transform _wrapper;
    [SerializeField] private Transform _wrapperWalls;
    [SerializeField] private Wall _wall;
    [SerializeField] private List<ComplexityData> _complexityDatas;

    private Camera _mainCamera;
    private Vector2 _screenBounds;
    private ComplexityData _data;
    private IPlayer _player;
    private Platform _platform;
    private Ball _ball;
    private int _countBall;
    
    private int _gridSize;
    private List<int> _bricks;
    
    public event Action Completed;
    public event Action Lose;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void Init(IPlayer player)
    {
        _data = _complexityDatas[Game.Instance.Complexity];
        _player = player;
        
        _screenBounds = _mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
    }
    
    public void Load()
    {
        _player.ResetScore();
        _player.SetBall();
        
        CreateWalls();
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
        if (_platform != null) Destroy(_platform.gameObject);
        ClearChild(_wrapper);
        ClearChild(_wrapperWalls);
        ClearBall();
    }

    private void CreateWalls()
    {
        var wUp = Instantiate(_wall, _wrapperWalls);
        wUp.Init(new Vector2( 0f, _screenBounds.y + OFFSET_WALL), new Vector2(_screenBounds.x * 2, SIZE_WALL));
        var wDown = Instantiate(_wall, _wrapperWalls);
        wDown.Init(new Vector2( 0f, _screenBounds.y * - 1 - OFFSET_WALL), new Vector2(_screenBounds.x * 2, SIZE_WALL), true);
        var wLeft = Instantiate(_wall, _wrapperWalls);
        wLeft.Init(new Vector2( _screenBounds.x * - 1 - OFFSET_WALL, 0f), new Vector2(SIZE_WALL, _screenBounds.y * 2));
        var wRight = Instantiate(_wall, _wrapperWalls);
        wRight.Init(new Vector2( _screenBounds.x + OFFSET_WALL, 0f), new Vector2(SIZE_WALL, _screenBounds.y * 2));
        
    }

    private void CreatePlatform()
    {
        _platform = Instantiate(_data.Platform, new Vector3(0f, -4f), Quaternion.identity, transform);
        _platform.Init(_screenBounds, _data.PlatformWidth);
    }

    private void CreateBall()
    {
        _ball = Instantiate(_data.Ball, transform);
        _ball.Init(_platform, _data.BallForce);
        _ball.Fail += Failing;
    }
    
    private void CreateBricks()
    {
        ClearChild(_wrapper);
        
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
                b.Init(RemoveBrick, transform);
                _bricks.Add(brickId);
                brickId++;
            }
            
            minX += ELEMENT_WIDTH / 2;
            gridSizeX--;
        }
    }
    
    private static void ClearChild(Transform wrapper)
    {
        while (wrapper.childCount > 0) DestroyImmediate(wrapper.GetChild(0).gameObject);
    }
    
    private void RemoveBrick()
    {
        _player.AddScore();
        _bricks.RemoveAt(_bricks.Count - 1);
        
        if (_bricks.Count > 0) return;

        ClearBall();
        Destroy(_platform.gameObject);
        Game.Instance.ChangeState(GameState.VICTORY);
    }

    private void ClearBall()
    {
        if (_ball == null) return;
        
        _ball.Fail -= Failing;
        Destroy(_ball.gameObject);
    }

    private void Failing()
    {
        _player.RemoveBall();
        
        if (_player.Balls != 0) return;
        
        ClearBall();
        Game.Instance.ChangeState(GameState.DEFEAT);
    }
}