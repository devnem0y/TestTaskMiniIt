using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Hide_1 без события в конце
/// Hide_2 с событием
/// </summary>
public class TransitionEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay;
    [SerializeField] private GameObject _labelNext;
    
    private Action _callback;
    
    public void Init(Action callback, TransitionMode transitionMode)
    {
        switch (transitionMode)
        {
            case TransitionMode.NONE:
                _animator.Play("Normal");
                callback?.Invoke();
                break;
            case TransitionMode.SHOW:
                _callback?.Invoke();
                StartCoroutine(ShowDelay(false));
                break;
            case TransitionMode.HIDE:
                _animator.Play("Hide_1");
                callback?.Invoke();
                break;
            case TransitionMode.HIDE_SHOW:
                _callback = callback;
                _animator.Play("Hide_2"); 
                break;
            case TransitionMode.STATIC:
                _callback = callback;
                _animator.Play("Static");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(transitionMode), transitionMode, null);
        }
    }

    private void AnimationEvent()
    {
        _callback?.Invoke();
        StartCoroutine(ShowDelay(true));
    }
    
    private IEnumerator ShowDelay(bool isDelay)
    {
        yield return new WaitForSeconds(isDelay ? _delay : 0.25f);
        _animator.Play("Show");
        var currentAnimationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, currentAnimationTime + 0.15f);
    }

    public void OnShow()
    {
        if (_labelNext != null) _labelNext.SetActive(false);
        StartCoroutine(ShowDelay(false));
    }

    public void ShowLabelNext()
    {
        _labelNext.SetActive(true);
    }
}
