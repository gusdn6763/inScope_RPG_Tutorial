using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigi;
    protected Coroutine attackRoutine;

    [SerializeField] private float speed = 1f;
    protected Vector2 direction;


    protected bool isAttacking = false;


    public enum LayerName
    {
        IdleLayer = 0,
        WalkLayer = 1,
        AttackLayer = 2,
    }

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }


    protected virtual void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rigi.velocity = direction.normalized * speed;
    }


    public void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer(LayerName.WalkLayer);
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
            StopAttack();
        }
        else if (isAttacking)
        {
            ActivateLayer(LayerName.AttackLayer);
        }
        else
        {
            ActivateLayer(LayerName.IdleLayer);
        }
    }


    public void ActivateLayer(LayerName layerName)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight((int)layerName, 1);
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            animator.SetBool("Attack", isAttacking);
        }
    }
}
