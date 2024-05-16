using System;
using UnityEngine;
using UralHedgehog;

public class Brick : MonoBehaviour
{
    [SerializeField] private AudioComponent _audio;
    [SerializeField] private GameObject _vfx;
    
    private Action _callback;
    private Transform _wrapper;
    
    public void Init(Action callback, Transform wrapper)
    {
        _callback = callback;
        _wrapper = wrapper;
        
        _audio.Init();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Ball")) return;
        
        _audio.Play(Sound.FX_0);
        _callback?.Invoke();
        var vfx = Instantiate(_vfx, transform.position, Quaternion.identity, _wrapper);
        Destroy(vfx, 2.5f);
        Destroy(gameObject, 0.15f);
    }
}
