using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer _spriteRenderer;

    private void OnEnable() {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    private void OnDisable() {
        UpdateOutline(false);
    }

    private  void Update() {
        UpdateOutline(true);
    }

    private void UpdateOutline(bool outline) {
        var mpb = new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        _spriteRenderer.SetPropertyBlock(mpb);
    }
}