using System.Collections; using System.Collections.Generic; using UnityEngine;

public class Trace : MonoBehaviour
{
    public enum SplatLoacation
    {
        Foreground,
        Background,
    }

    public float minSizeMod = 0.8f;
    public float maxSizeMod = 1.5f;
    public Sprite[] sprites;
    private SplatLoacation splatLocation;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Initialize(SplatLoacation splatLocation, int layer, Color color)
    {
        this.splatLocation = splatLocation;
        SetSprite();
        SetSize();
        SetRotation();
        SetColor(color);
        SetLocationProperties(layer);
        GetComponent<Animator>().SetTrigger("Init");
    }

    private void SetSprite()
    {
        if (sprites.Length == 0) return;
        int randomIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomIndex];
    }

    private void SetSize()
    {
        float sizeMod = Random.Range(minSizeMod, maxSizeMod);
        transform.localScale *= sizeMod;
    }


    private void SetRotation()
    {
        float randomRotation = Random.Range(-360f, 360f);
        spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
    }

    private void SetLocationProperties(int layer)
    {
        switch (splatLocation)
        {
            case SplatLoacation.Background:
                //spriteRenderer.color = Color.black*0.1f;
                spriteRenderer.sortingOrder = layer;
                break;
            case SplatLoacation.Foreground:
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                spriteRenderer.sortingOrder = layer;
                break;
        }
    }
    
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
        /*
        Color initialColor = color;
        // Generate random offsets for each RGB component within the specified range
        float rOffset = Random.Range(minColorRange, maxColorRange);
        float gOffset = Random.Range(minColorRange, maxColorRange);
        float bOffset = Random.Range(minColorRange, maxColorRange);

        // Apply the offsets to the initial color
        Color newColor = new Color(
            Mathf.Clamp01(initialColor.r + rOffset),
            Mathf.Clamp01(initialColor.g + gOffset),
            Mathf.Clamp01(initialColor.b + bOffset),
            initialColor.a
        );

        spriteRenderer.color = newColor;*/
    }
}