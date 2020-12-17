using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour {
    public int duration;
    public String description;
    public double Strength { get; set; }

    public abstract void AtRoundEnd(Character c);
    public virtual void Activate(Character c, Skill s) { }
    public virtual void Deactivate(Character c) { }

    public virtual void AfterDamage(Character c, int damage) { }
}
