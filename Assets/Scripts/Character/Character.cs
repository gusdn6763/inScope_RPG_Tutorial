using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Stat health = null;
    [SerializeField] protected Transform hitBox = null;
    private Transform target;
    protected Rigidbody2D rigi;
    protected Coroutine attackRoutine;

    private Vector2 direction;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float initHealth = 0f;
    protected bool isAttacking = false;

    public Stat Health { get { return health; } }
    public bool IsAlive { get => health.MyCurrentValue > 0; }
    public Transform Target { get => target; set => target = value; }
    public Animator Animator { get; set; }
    public Vector2 Direction { get => direction; set => direction = value; }
    public bool IsMoving { get => direction.x != 0 || direction.y != 0; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public enum LayerName
    {
        IdleLayer = 0,
        WalkLayer = 1,
        AttackLayer = 2,
        DeathLayer = 3,
    }
    protected virtual void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);
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
        if (IsAlive)
        {
            rigi.velocity = Direction.normalized * Speed;
        }
    }


    public virtual void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateLayer(LayerName.WalkLayer);
                Animator.SetFloat("X", Direction.x);
                Animator.SetFloat("Y", Direction.y);
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
        else
        {
            ActivateLayer(LayerName.DeathLayer);
        }
    }


    public virtual void ActivateLayer(LayerName layerName)
    {
        for (int i = 0; i < Animator.layerCount; i++)
        {
            Animator.SetLayerWeight(i, 0);
        }
        Animator.SetLayerWeight((int)layerName, 1);
    }

    //공격 받을시
    public virtual void TakeDamage(float damage, Transform source)
    {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {
            rigi.velocity = Vector2.zero;
            Animator.SetTrigger("Die");
        }
    }
}
