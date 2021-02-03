using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls map UI displaying player's progress in a current path.
/// </summary>
public class MapUI : MonoBehaviour {
	public TMP_Text pathText;
	public List<Image> areas;

	private SceneController sceneController;
	private Team playerTeam;

	public delegate void OnClickDelegate();
	public event OnClickDelegate onClick;

	void Awake() {
		sceneController = GameMaster.instance.GetComponent<SceneController>();
		playerTeam = Team.playerTeamInstance;
	}

	void Update() {
		Show();
	}

	/// <summary>
	/// Creates map of path based on player's current position.
	/// This map differs from Map scene. Only current path name and area progress is displayed.
	/// </summary>
	private void Show() {
		EnvironmentPath lastPath = playerTeam.Paths.LastOrDefault();

		// Set text
		pathText.text = playerTeam.Paths.LastOrDefault().ToDescription();

		// Royal Hall is different, it does not have any area
		if (lastPath == EnvironmentPath.RoyalHall) {
			pathText.transform.parent.GetComponent<Image>().color = Color.yellow;
			areas[0].transform.parent.gameObject.SetActive(false);
			return;
		}

		// Set areas
		int lastArea = (playerTeam.CurrentArea - 1) % 3;
		for (int i = 0; i < lastArea; ++i)
			areas[i].color = Color.green;
		if (lastArea < areas.Count && lastArea >= 0)
			areas[lastArea].color = Color.yellow;
	}

	/// <summary>
	/// Changes scene to Map scene.
	/// Used as pointer on down listener.
	/// </summary>
	public void OnClick() {
		onClick?.Invoke();
		sceneController.ChangeFromGameScene("Map");
	}
}
