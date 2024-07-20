using System;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HideTile()
    {
        if (_spriteRenderer)
        {
            _spriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }
    
    public void ShowTile()
    {
        if (_spriteRenderer)
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }
}