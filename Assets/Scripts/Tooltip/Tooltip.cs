using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int characterWrapLimit;
    [SerializeField] private Vector2 _positionOffset;
    [SerializeField] private GameObject _headerDeco;

    [Header("Textfields")]
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _bodyText;

    public virtual void SetText(string body, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            _headerText.gameObject.SetActive(false);
            if(_headerDeco) _headerDeco.gameObject.SetActive(false);
        }
        else
        {
            _headerText.gameObject.SetActive(true);
            _headerText.text = header;
        }

        if (_bodyText) _bodyText.text = body;

        int headerLength = _headerText.text.Length;
        int bodyLength = _bodyText.text.Length;

        _layoutElement.enabled = (headerLength > characterWrapLimit || bodyLength > characterWrapLimit) ? true : false;
    }
    private void Update()
    {
        Vector2 position = Input.mousePosition;

        float actualPivotX = 0f;
        float actualPivotY = 0f;
        Vector2 actualPositionOffset = _positionOffset;

        if (position.x / Screen.width > 0.5f) { actualPivotX = 1f; actualPositionOffset.x *= -1f; }
        if (position.y / Screen.width > 0.5f) { actualPivotY = 1f; actualPositionOffset.y *= -1f; }

        _rectTransform.pivot = new Vector2(actualPivotX, actualPivotY);
        transform.position = position + actualPositionOffset;
    }
}
