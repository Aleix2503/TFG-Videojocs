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
    public Animator animator;
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


    private void SetRotation()
    {
        float randomRotation = Random.Range(-360f, 360f);
        spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
    }

    private void SetLocationProperties(int layer)
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        spriteRenderer.sortingOrder = layer;
    }
    
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
        GetComponentInChildren<Renderer>().material.color = color;
    }
}