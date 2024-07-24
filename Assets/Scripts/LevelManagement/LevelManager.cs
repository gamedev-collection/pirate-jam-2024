using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public Level[] levelList;
    public AED_CanvasGroup blackFade;

    private bool isLoading = false;

    public void LoadGameLevel(int levelIndex) { LoadGameLevel(levelIndex, false); }
    public void LoadGameLevelWithTransition(int levelIndex) { LoadGameLevel(levelIndex, true); }
    private void LoadGameLevel(int levelIndex, bool withTransition = false)
    {
        if(isLoading) return;

        UnlockScene(levelIndex);
        if (withTransition) StartCoroutine(DoLoadWithTransition_Routine(levelList[levelIndex].scene.name));
        else StartCoroutine(DoSceneLoad_Routine(levelList[levelIndex].scene.name));
    }

    public void LoadScene(Scene scene) { LoadScene(scene, false); }
    public void LoadSceneWithTranstiion(Scene scene) { LoadScene(scene, true); }
    public void LoadScene(Scene scene, bool withTransition = false)
    {
        if (isLoading) return;

        if (withTransition) StartCoroutine(DoLoadWithTransition_Routine(scene.name));
        else StartCoroutine(DoSceneLoad_Routine(scene.name));
    }

    public void UnlockScene(int levelIndex)
    {
        levelList[levelIndex].isUnlocked = true;
    }

    public void UnlockAll()
    {
        foreach(Level level in levelList)
        {
            UnlockScene(level.levelIndex);
        }
    }

    private IEnumerator DoSceneLoad_Routine(string sceneName)
    {
        isLoading = true;
        Debug.Log("Loading Scene - " + sceneName);
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone) { yield return null; }
        Debug.Log("Finishid Loading Scene - " + sceneName);
        isLoading = false;
    }

    private IEnumerator DoLoadWithTransition_Routine(string sceneName)
    {
        isLoading = true;
        blackFade.AnimatedEnable();
        while (blackFade.IsEnabling) yield return null;
        
        yield return StartCoroutine(DoSceneLoad_Routine(sceneName));
        
        blackFade.AnimatedDisable();
        while (blackFade.IsDisabling) yield return null;
        isLoading = false;
    }
}

[Serializable]
public struct Level
{
    public SceneAsset scene;
    public int levelIndex;
    public bool isUnlocked;
}
