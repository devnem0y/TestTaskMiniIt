using System;
using UnityEngine;
using UralHedgehog;

public class Ball : MonoBehaviour
{
    private const int SPEED = 220;
    
    private IPlatform _platform;
    private Rigidbody2D _rigidbody;
    private float _force;
    private bool _isMove;

    private Transform Transform => transform;

    public event Action Fail;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(IPlatform platform, float ballForce)
    {
        _platform = platform;
        _force = ballForce;
        
        Reboot();
    }

    private void Update()
    {
        if (Game.Instance.GameState != GameState.PLAY) return;
        
        if (_isMove) return;
        transform.position = new Vector2(_platform.Position.x, _platform.Position.y + 0.43f);

        if (Input.GetMouseButtonDown(0))
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(new Vector2(0f, _force));
            _isMove = true;
            //AudioManager.instance.PlaySound("fx_1");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            Fail?.Invoke();
            Reboot();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            var hitPoint = other.contacts[0].point;
            var platformCenter = new Vector2(_platform.Position.x, _platform.Position.y);
            _rigidbody.velocity = Vector2.zero;

            var difference = platformCenter.x - hitPoint.x;
            _rigidbody.AddForce(hitPoint.x < platformCenter.x
                ? new Vector2(-Mathf.Abs(difference * _force - SPEED), _force)
                : new Vector2(Mathf.Abs(difference * _force - SPEED), _force));

            //AudioManager.instance.PlaySound("fx_2");
        }
    }

    private void Reboot()
    {
        _isMove = false;
        _rigidbody.isKinematic = true;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Transform.position = new Vector2(_platform.Position.x, _platform.Position.y + 0.43f);
        Transform.rotation = Quaternion.identity;
    }
}
