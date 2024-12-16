using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviour : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected FSMEnemies fsmEnemies;
    protected Animator animator;
    protected Rigidbody2D rb;
    public abstract void Behaviour();
}
