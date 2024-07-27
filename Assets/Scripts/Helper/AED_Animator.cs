using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AED_Animator : AnimatedEnableDisable
{
    [SerializeField] Animator _animator;
    [SerializeField] string _showTrigger;
    [SerializeField] string _hideTrigger;

    public override void AnimatedDisable()
    {
        IsDisabling = true;
        if (!IsAnimating) StartCoroutine(DoAnimateAnimator(_hideTrigger));
    }

    public override void AnimatedEnable()
    {
        IsEnabling = true;
        if (!IsAnimating) StartCoroutine(DoAnimateAnimator(_showTrigger));
    }

    private IEnumerator DoAnimateAnimator(string trigger)
    {
        IsAnimating = true;

        _animator.SetTrigger(trigger);
        float duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);

        IsAnimating = false;

        if (IsEnabling) IsEnabling = false;
        if (IsDisabling) IsDisabling = false;

        yield return null;
    }
}
