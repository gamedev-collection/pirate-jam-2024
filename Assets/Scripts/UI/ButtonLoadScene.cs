using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLoadScene : MonoBehaviour
{
    public void Load(int index)
    {
        LevelManager.Instance.LoadGameLevelWithTransition(index);
    }

    public void LoadNext()
    {
        LevelManager.Instance.LoadNextGameLevel();
    }

    public void ReloadCurrent()
    {
        LevelManager.Instance.ReloadCurrentGameLevel();
    }

    public void Menu()
    {
        LevelManager.Instance.LoadSceneWithTranstiion(0);
    }
}
