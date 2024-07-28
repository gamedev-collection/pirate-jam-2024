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
    public AnimatedEnableDisable blackFade;
    public AnimatedEnableDisable levelFade;

    private bool isLoading = false;
    private int currentIndex = 0;

    public void LoadNextGameLevel()
    {
        LoadGameLevelWithTransition(currentIndex + 1);
    }
    public void ReloadCurrentGameLevel()
    {
        LoadGameLevelWithTransition(currentIndex);
    }
    public void LoadGameLevel(int levelIndex) { LoadGameLevel(levelIndex, false); }
    public void LoadGameLevelWithTransition(int levelIndex) { LoadGameLevel(levelIndex, true); }
    private void LoadGameLevel(int levelIndex, bool withTransition = false)
    {
        if(isLoading) return;
        currentIndex = levelIndex;
        UnlockScene(levelIndex);
        if (withTransition) StartCoroutine(DoLoadWithFadeTransition_Routine(levelList[levelIndex].scene.name));
        else StartCoroutine(DoSceneLoad_Routine(levelList[levelIndex].scene.name));
    }


    public void LoadScene(Scene scene) { LoadScene(scene, false); }
    public void LoadSceneWithTranstiion(Scene scene) { LoadScene(scene, true); }
    public void LoadScene(Scene scene, bool withTransition = false)
    {
        if (isLoading) return;
        if (withTransition) StartCoroutine(DoLoadWithFadeTransition_Routine(scene.name));
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

    private IEnumerator DoLoadWithFadeTransition_Routine(string sceneName)
    {
        isLoading = true;
        blackFade.AnimatedEnable();
        while (blackFade.IsEnabling) yield return null;
        
        yield return StartCoroutine(DoSceneLoad_Routine(sceneName));
        
        blackFade.AnimatedDisable();
        while (blackFade.IsDisabling) yield return null;
        isLoading = false;
    }

    private IEnumerator DoLoadWithLevelTransition_Routine(string sceneName)
    {
        isLoading = true;
        levelFade.AnimatedEnable();
        while (levelFade.IsEnabling) yield return null;

        yield return StartCoroutine(DoSceneLoad_Routine(sceneName));

        levelFade.AnimatedDisable();
        while (levelFade.IsDisabling) yield return null;
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
