using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	public List<GameObject> charactersUI;
	[Space]
	public Slider expBar;
	public TMP_Text levelExpText;

	private RectTransform characterUIRectTransform;
	private RectTransform expBarRectTransform;
	private RectTransform levelExpTextRectTransform;

	private Team playerTeam;
	private SceneController sceneController;

	void Awake() {
		characterUIRectTransform = charactersUI[0].GetComponent<RectTransform>();
		expBarRectTransform = expBar.GetComponent<RectTransform>();
		levelExpTextRectTransform = levelExpText.GetComponent<RectTransform>();

		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

	void OnEnable() {
		Show();
	}

	private void Show() {
		int n = 0;

		// Characters
		for (int i = 0; i < charactersUI.Count; ++i) {
			if (i >= playerTeam.Characters.Count) {
				charactersUI[i].gameObject.SetActive(false);
				continue;
			}
			++n;
			var characterTexts = charactersUI[i].GetComponentsInChildren<TMP_Text>();
			characterTexts[0].text = playerTeam.Characters[i].characterName;
			characterTexts[1].text = $"Health: {playerTeam.Characters[i].maxHealth}";
			characterTexts[2].text = $"Strength: {playerTeam.Characters[i].baseStrength}";
		}

		// Exp bar
		Vector2 size = new Vector2(n * characterUIRectTransform.rect.width, expBarRectTransform.rect.height);
		expBarRectTransform.sizeDelta = size;
		levelExpTextRectTransform.sizeDelta = size;
		expBar.value = (float) playerTeam.Exp / playerTeam.expPerLevel;
		levelExpText.text = $"Level {playerTeam.Level}: {playerTeam.Exp}/{playerTeam.expPerLevel}";
	}

	public void OnClick() {
		sceneController.ChangeFromGameScene("CharactersDetail");
	}
}
