using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AED_CanvasGroup : AnimatedEnableDisable
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private float _enableDelay;
    [SerializeField] private float _disableDelay;

    private bool _isAnimating = false;

    public override void AnimatedDisable()
    {
        _canvasGroup.interactable = false;
        if(!_isAnimating) StartCoroutine(DoAnimateCanvasGroup(1, 0, _duration, _disableDelay));
    }

    public override void AnimatedEnable()
    {
        _canvasGroup.interactable = true;
        if (!_isAnimating) StartCoroutine(DoAnimateCanvasGroup(0, 1, _duration, _enableDelay));
    }

    public void AnimatedEnableWithoutDelay()
    {
        if (!_isAnimating) StartCoroutine(DoAnimateCanvasGroup(0, 1, _duration, 0f));
    }

    private IEnumerator DoAnimateCanvasGroup(float startValue, float endValue, float duration, float delay)
    {
        _isAnimating = true;
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(startValue, endValue, _curve.Evaluate(elapsedTime/duration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _canvasGroup.alpha = endValue;
        _isAnimating = false;
        yield return null;
    }
}
