using System;
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
    [SerializeField] private bool _disableOnEnable = false;
    [SerializeField] private bool _enableOnEnable = false;


    private void OnEnable()
    {
        if (_disableOnEnable) AnimatedDisable();
        if (_enableOnEnable) AnimatedEnable();
    }
    public override void AnimatedDisable()
    {
        IsDisabling = true;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        if(!IsAnimating) StartCoroutine(DoAnimateCanvasGroup(_canvasGroup.alpha, 0, _duration, _disableDelay));
    }

    public override void AnimatedEnable()
    {
        IsEnabling = true;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        if (!IsAnimating) StartCoroutine(DoAnimateCanvasGroup(0, 1, _duration, _enableDelay));
    }

    public void AnimatedEnableWithoutDelay()
    {
        if (!IsAnimating) StartCoroutine(DoAnimateCanvasGroup(0, 1, _duration, 0f));
    }

    private IEnumerator DoAnimateCanvasGroup(float startValue, float endValue, float duration, float delay)
    {
        IsAnimating = true;
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(startValue, endValue, _curve.Evaluate(elapsedTime/duration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _canvasGroup.alpha = endValue;
        IsAnimating = false;

        if (IsEnabling) IsEnabling = false;
        if (IsDisabling) IsDisabling = false;

        yield return null;
    }
}
