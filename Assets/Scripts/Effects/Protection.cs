﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protection : Effect
{
    public override void ApplyEffect(Character c) {
        duration -= 1;
    }

    public override void Deactivate(Character c) {
        c.Defence -= Strength;
    }
}