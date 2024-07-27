using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatedEnableDisable : MonoBehaviour
{
    public abstract void AnimatedEnable();
    public abstract void AnimatedDisable();

    public bool IsAnimating { get; set; }
    public bool IsEnabling { get; set; }
    public bool IsDisabling { get; set; }
}
