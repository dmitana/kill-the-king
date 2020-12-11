using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour {
    public int duration;
    public String description;
    public double Strength { get; set; }

    public abstract void ApplyEffect(Character c);
    public abstract void Deactivate(Character c);
}
