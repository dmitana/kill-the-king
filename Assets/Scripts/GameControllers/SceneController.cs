using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    private Event areaEvent;
    private BattleController _battleController;

    void Awake() {
        GameObject obj = GameObject.FindGameObjectWithTag("Event");
        areaEvent = (Event) obj.GetComponent(typeof(Event));

        GameObject gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        _battleController = (BattleController) gameMaster.GetComponent(typeof(BattleController));
    }

    public void ChangeScene(String newSceneName) {
        StartCoroutine(ChangeSceneCoroutine(newSceneName));
    }

    private IEnumerator ChangeSceneCoroutine(String newSceneName) {
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneByName(newSceneName);
        GameObject currentScene =
            GameObject.FindGameObjectWithTag("SceneContainer");

        while (!newScene.isLoaded) {
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.SetActiveScene(newScene);
        currentScene.SetActive(false);

        if (newSceneName == "Battle") {
            FillEnemiesToBattle();
            _battleController.InitializeCombat();
        }
    }

    public void FillEnemiesToBattle() {
        Transform enemyTeamTransform =
            GameObject.FindGameObjectWithTag("EnemyTeam").transform;
        Vector3 position = enemyTeamTransform.position;
        foreach (var enemy in areaEvent.enemies) {
            Instantiate(
                enemy, position, Quaternion.identity, enemyTeamTransform
            );
            position[0] += 1;
        }
    }
}