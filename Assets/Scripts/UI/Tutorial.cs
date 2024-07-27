using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public TMP_Text header;
    public TMP_Text body;
    public GameObject tutorialObj;

    public List<TutorialLine> tutorials = new List<TutorialLine>();

    public UnityEvent onTutorialEnd;

    private Queue<TutorialLine> _tutorialQueue;

    private void Start()
    {
        DisableTutorial();
        _tutorialQueue = new Queue<TutorialLine>(tutorials);
        
        UIManager.Instance.DisableRuneShop();
        UIManager.Instance.DisableTowerShop();
        SetNextTutorial();
    }

    public void SetNextTutorial()
    {
        _tutorialQueue ??= new Queue<TutorialLine>(tutorials);
        
        if (_tutorialQueue.Count == 0)
        {
            onTutorialEnd?.Invoke();
            DisableTutorial();
            return;
        }

        var tut = _tutorialQueue.Dequeue();
        tutorialObj?.SetActive(true);
        header.text = $"Tutorial: {tut.title}";
        body.text = tut.body;
    }

    public void DisableTutorial()
    {
        tutorialObj?.SetActive(false);
    }

    public void EndTutorial()
    {
        
    }
}

[Serializable]
public struct TutorialLine
{
    public string title;
    [TextArea]
    public string body;
}