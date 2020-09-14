using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private Animator animator;
    protected Vector2 direction;

    [SerializeField] private float speed;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (direction.x != 0 || direction.y != 0)
        {
            AnimateMovement();
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    public void AnimateMovement()
    {

        animator.SetLayerWeight(1, 1);

        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

}