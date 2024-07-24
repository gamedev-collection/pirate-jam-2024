using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateUIToWorldTransform : MonoBehaviour
{
    [SerializeField] private GameObject _obj;

    private void OnEnable()
    {
        SetPositionToObject();
    }

    public void SetPositionToObject()
    {
        transform.position = Camera.main.WorldToScreenPoint(_obj.transform.position);
    }
}
