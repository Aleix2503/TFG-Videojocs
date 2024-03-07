using System.Collections; using System.Collections.Generic; using UnityEngine;

public class Splat : MonoBehaviour
{
    public enum SplatLoacation
    {
        Foreground,
        Background,
    }

    public Color backgroundTint;
    public float minSizeMod = 0.8f;
    public float maxSizeMod = 1.5f;
    public float minColorRange = -0.2f;
    public float maxColorRange = 0.2f; 
    public Sprite[] sprites;
    public Animator animator;
    private SplatLoacation splatLocation;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Initialize(SplatLoacation splatLocation, int layer, Color color, Vector3 normal)
    {
        this.splatLocation = splatLocation;
        SetSprite();
        SetSize();
        SetNormal(normal);
        SetRotation();
        SetColor(color);
        SetLocationProperties(layer);
        GetComponent<Animator>().SetTrigger("Init");
        StartCoroutine(DisableAnimatorAfterAnimation());
    }

    private IEnumerator DisableAnimatorAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.enabled = false;
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

    private void SetNormal(Vector3 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        transform.rotation = rotation;
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
                spriteRenderer.color = backgroundTint;
                spriteRenderer.sortingOrder = 0;
                break;
            case SplatLoacation.Foreground:
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                spriteRenderer.sortingOrder = layer;
                break;
        }
    }
    
    public void SetColor(Color color)
    {   
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

        GetComponentInChildren<Renderer>().material.color = newColor;
        spriteRenderer.color = newColor;
    }
}