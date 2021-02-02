using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHighlightUI : MonoBehaviour {
    public Character character;
    public Image image;

    void Update()
    {
        if (character.InBattle) {
            if (character.IsSelected) {
                image.gameObject.SetActive(true);
                image.color = Color.green;
            }
            else if (character.IsValidTarget) {
                image.gameObject.SetActive(true);
                image.color = character.IsTargeted ? Color.red : Color.blue;
            }
            else {
                image.gameObject.SetActive(false);
            }
        }
        else
            image.gameObject.SetActive(false);
    }
}
