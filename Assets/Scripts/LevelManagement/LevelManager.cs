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
        if (withTransition) StartCoroutine(DoLoadWithFadeTransition_Routine(levelList[levelIndex].levelIndex));
        else StartCoroutine(DoSceneLoad_Routine(levelList[levelIndex].levelIndex));
    }


    public void LoadScene(int index) { LoadScene(index, false); }
    public void LoadSceneWithTranstiion(int index) { LoadScene(index, true); }
    public void LoadScene(int index, bool withTransition = false)
    {
        if (isLoading) return;
        if (withTransition) StartCoroutine(DoLoadWithFadeTransition_Routine(index));
        else StartCoroutine(DoSceneLoad_Routine(index));
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

    private IEnumerator DoSceneLoad_Routine(int index)
    {
        isLoading = true;
        Debug.Log("Loading Scene - " + index);
        var asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        while (!asyncLoad.isDone) { yield return null; }
        Debug.Log("Finishid Loading Scene - " + index);
        isLoading = false;
    }

    private IEnumerator DoLoadWithFadeTransition_Routine(int index)
    {
        isLoading = true;
        blackFade.AnimatedEnable();
        while (blackFade.IsEnabling) yield return null;
        
        yield return StartCoroutine(DoSceneLoad_Routine(index));
        
        blackFade.AnimatedDisable();
        while (blackFade.IsDisabling) yield return null;
        isLoading = false;
    }

    private IEnumerator DoLoadWithLevelTransition_Routine(int index)
    {
        isLoading = true;
        levelFade.AnimatedEnable();
        while (levelFade.IsEnabling) yield return null;

        yield return StartCoroutine(DoSceneLoad_Routine(index));

        levelFade.AnimatedDisable();
        while (levelFade.IsDisabling) yield return null;
        isLoading = false;
    }
}

[Serializable]
public struct Level
{
    //public SceneAsset scene;
    public int levelIndex;
    public bool isUnlocked;
}
