using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour {
    public int duration;
    public double BaseAttackStrength { get; set; }

    public abstract void applyEffect(Character c);
}
