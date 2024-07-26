using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteApplier : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;

    private void OnEnable()
    {
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length - 1)];
    }
}
