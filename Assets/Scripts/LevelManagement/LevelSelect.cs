using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public UnlockableLevelButton[] unlockableLevelButtons;

    private void Awake()
    {
        UpdateButtonStatus();
    }

    public void UpdateButtonStatus()
    {
        foreach (Level level in LevelManager.Instance.levelList)
        {
            if(level.isUnlocked) UnlockButton(level.levelIndex);
        }
    }
    private void UnlockButton(int index)
    {
        foreach (UnlockableLevelButton button in unlockableLevelButtons)
        {
            if (button.levelIndex == index -1) button.UnlockButton();
        }
    }

    public void UnlockAll()
    {
        LevelManager.Instance.UnlockAll();
        foreach (UnlockableLevelButton button in unlockableLevelButtons)
        {
            button.UnlockButton();
        }
    }
}
