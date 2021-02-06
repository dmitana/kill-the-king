using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blindness : Skill {
    public override void ApplySkill(Character attacker, Character target) {
        BlindnessDebuff effect = (BlindnessDebuff) Instantiate(effects[0], target.gameObject.transform);
        target.AddEffect(effect);
        battleController.Log = $"{attacker} used {skillName} on {target} which now has only {100 * strength} % chance" +
                               $" that his next attack will hit its target";
    }
}
