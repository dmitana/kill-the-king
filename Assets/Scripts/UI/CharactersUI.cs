using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls characters UI displaying player's characters along with exp bar.
/// </summary>
public class CharactersUI : MonoBehaviour {
	public List<GameObject> charactersUI;
	[Space]
	public Slider expBar;
	public TMP_Text levelExpText;

	private RectTransform characterUIRectTransform;
	private RectTransform expBarRectTransform;
	private RectTransform levelExpTextRectTransform;

	private Team playerTeam;
	private SceneController sceneController;

	public delegate void OnClickDelegate();
	public event OnClickDelegate onClick;

	void Awake() {
		characterUIRectTransform = charactersUI[0].GetComponent<RectTransform>();
		expBarRectTransform = expBar.transform.parent.GetComponent<RectTransform>();

		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

	void Update() {
		Show();
	}

	/// <summary>
	/// Creates characters UI along with exp bar.
	/// Exp bar width is set to width of the characters UI.
	/// </summary>
	private void Show() {
		int n = 0;

		// Characters
		for (int i = 0; i < charactersUI.Count; ++i) {
			if (i >= playerTeam.Characters.Count) {
				charactersUI[i].gameObject.SetActive(false);
				continue;
			}
			++n;
			var characterImg = charactersUI[i].GetComponentsInChildren<Image>()[1];
			if (playerTeam.Characters[i].characterImg != null) {
				characterImg.color = Color.white;
				characterImg.sprite = playerTeam.Characters[i].characterImg;
			}

			var characterTexts = charactersUI[i].GetComponentsInChildren<TMP_Text>();
			characterTexts[0].text = playerTeam.Characters[i].characterName;
			characterTexts[1].text = $"HP: {playerTeam.Characters[i].maxHealth}";
			characterTexts[2].text = $"STR: {playerTeam.Characters[i].baseStrength}";
		}

		// Exp bar
		Vector2 size = new Vector2(n * characterUIRectTransform.rect.width, expBarRectTransform.rect.height);
		expBarRectTransform.sizeDelta = size;
		expBar.value = (float) playerTeam.Exp / playerTeam.expPerLevel;
		levelExpText.text = $"Level {playerTeam.Level}: {playerTeam.Exp}/{playerTeam.expPerLevel}";
	}

	/// <summary>
	/// Changes scene to Characters detail scene.
	/// Used as pointer on down listener.
	/// </summary>
	public void OnClick() {
		onClick?.Invoke();
		sceneController.ChangeFromGameScene("CharactersDetail");
	}
}
