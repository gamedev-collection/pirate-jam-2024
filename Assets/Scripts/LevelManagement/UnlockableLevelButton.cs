using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockableLevelButton : MonoBehaviour
{
    public int levelIndex;
    public bool isUnlocked = false;
    public Button loadLevelButton;
    public TextMeshProUGUI textField;

    private void Awake()
    {
        loadLevelButton.interactable = isUnlocked;
        textField.text = levelIndex.ToString();
        if (loadLevelButton) loadLevelButton.onClick.AddListener(OnLevelButtonClicked);
    }

    private void OnDestroy()
    {
        if (loadLevelButton) loadLevelButton.onClick.RemoveListener(OnLevelButtonClicked);
    }

    private void OnLevelButtonClicked()
    {
        LevelManager.Instance.LoadGameLevelWithTransition(levelIndex);
    }

    public void UnlockButton()
    {
        isUnlocked = true;
        loadLevelButton.interactable = isUnlocked;
    }
}
