using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int health;
    public int baseStrength;
    public List<Skill> skills;
    
    private List<Effect> _activeEffects;
    // Start is called before the first frame update
    void Start()
    {
        skills = new List<Skill>();
        _activeEffects = new List<Effect>();
    }

    public void AddEffect(Effect effect)
    {
        _activeEffects.Add(effect);
    }

    public void DecreaseHealth(int damage) {
        health -= damage;
    }
}
