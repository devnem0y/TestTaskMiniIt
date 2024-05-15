using UnityEngine;

public class Wall : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Init(Vector2 position, Vector2 size, bool isGround = false)
    {
        transform.position = position;
        _spriteRenderer.size = size;
        _boxCollider2D.size = size;

        if (!isGround) return;
        
        tag = "Ground";
        _boxCollider2D.isTrigger = true;
    }
}