using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TMP_Text header;
    public TMP_Text body;
    public GameObject tutorialObj;
    public GameObject clickArrow;
    public float delay = 1;
    public UnityEvent onTutorialStart;

    public List<TutorialLine> tutorials = new List<TutorialLine>();

    public UnityEvent onTutorialEnd;

    private Queue<TutorialLine> _tutorialQueue;
    private TutorialLine _lastTut;
    private bool routineRunning = false;

    private void Start()
    {
        DisableTutorial();
        _tutorialQueue = new Queue<TutorialLine>(tutorials);

        onTutorialStart?.Invoke();

        UIManager.Instance.DisableRuneShop();
        UIManager.Instance.DisableTowerShop();
        SetNextTutorial();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_lastTut.continueOnClick) SetNextTutorial();
        }
    }

    public void SetNextTutorial()
    {
        if (!routineRunning) StartCoroutine(SetNextTutorial_Routine());
    }

    public IEnumerator SetNextTutorial_Routine()
    {
        routineRunning = true;
        _tutorialQueue ??= new Queue<TutorialLine>(tutorials);

        if (_lastTut.objectsToDisableAfterStepDone != null)
        {
            foreach (GameObject obj in _lastTut.objectsToDisableAfterStepDone)
            {
                obj.SetActive(false);
            }
        }

        if (_tutorialQueue.Count == 0)
        {
            onTutorialEnd?.Invoke();
            DisableTutorial();
            yield break;
        }

        yield return new WaitForSeconds(delay);

        _lastTut = _tutorialQueue.Dequeue();
        tutorialObj?.SetActive(true);
        if (_lastTut.continueOnClick) clickArrow?.SetActive(true);

        if (_lastTut.objectsToEnableDuringStep != null)
        {
            foreach (GameObject obj in _lastTut.objectsToEnableDuringStep)
            {
                obj.SetActive(true);
            }
        }

        _lastTut.onItem?.Invoke();

        if (_lastTut.title == "")
        {
            header.gameObject.SetActive(false);
        }
        else
        {
            header.gameObject.SetActive(true);
            header.text = _lastTut.title;
        }
        body.text = _lastTut.body;
        LayoutRebuilder.ForceRebuildLayoutImmediate(tutorialObj.GetComponent<RectTransform>());
        routineRunning = false;
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
    public GameObject[] objectsToEnableDuringStep;
    public GameObject[] objectsToDisableAfterStepDone;
    public UnityEvent onItem;
    public bool continueOnClick;
}