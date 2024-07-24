using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRawImage : MonoBehaviour
{
    [SerializeField] private RawImage _imageToScroll;
    [SerializeField] private float _x, _y;

    void Update()
    {
        _imageToScroll.uvRect = new Rect(_imageToScroll.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _imageToScroll.uvRect.size);
    }
}
