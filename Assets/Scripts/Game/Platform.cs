using UnityEngine;
using UralHedgehog;

public class Platform : MonoBehaviour, IPlatform
{
    private Camera _mainCamera;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private Vector2 _screenBounds;
    private float _platformShift;
    private float _leftClamp;
    private float _rightClamp;
    
    public Vector3 Position
    {
        get => transform.position;
        private set => transform.position = value;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    
    public void Init(float platformWidth)
    {
        _screenBounds = _mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        
        _spriteRenderer.size = new Vector2(platformWidth, _spriteRenderer.size.y);
        _boxCollider2D.size = new Vector2(platformWidth, _boxCollider2D.size.y);
        
        _platformShift = _spriteRenderer.bounds.size.x / 2 * -1;
        _leftClamp = _screenBounds.x * -1 - _platformShift;
        _rightClamp = _screenBounds.x + _platformShift;
    }

    private void LateUpdate()
    {
        if (Game.Instance.GameState == GameState.PLAY) Movement();
    }

    private void Movement()
    {
        var mousePositionWorldX = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0)).x;
        var pos = Mathf.Clamp(mousePositionWorldX, _leftClamp, _rightClamp);
        Position = new Vector3(pos, Position.y);
    }
}
