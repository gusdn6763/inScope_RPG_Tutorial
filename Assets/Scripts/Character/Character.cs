using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Stat health = null;
    [SerializeField] private Transform hitBox = null;
    [SerializeField] protected float initHealth = 0f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private string type;
    [SerializeField] private int level;

    private Character target;
    protected Rigidbody2D rigi;
    protected Coroutine attackRoutine;

    private Vector2 direction;
    protected bool isAttacking = false;

    public Stat Health { get { return health; } }
    public Character Target { get => target; set => target = value; }
    public Transform MyHitbox { get { return hitBox; } set { hitBox = value; } }
    public Animator Animator { get; set; }
    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }
    public string MyType { get => type;}
    public int MyLevel { get => level; set => level = value; }
    public bool IsAlive { get => health.MyCurrentValue > 0; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsMoving { get => direction.x != 0 || direction.y != 0; }

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
    public virtual void TakeDamage(float damage, Character source)
    {
        health.MyCurrentValue -= damage;
        CombatTextManager.instance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE, true);
        if (health.MyCurrentValue <= 0)
        {
            rigi.velocity = Vector2.zero;
            //이벤트에 저장된 함수가 있을시 실행
            GameManager.instance.OnKillConfirmed(this);
            Animator.SetTrigger("Die");
        }
    }

    //플레이어의 체력 회복
    public void GetHealth(int health)
    {
        Health.MyCurrentValue += health;
        CombatTextManager.instance.CreateText(transform.position, health.ToString(), SCTTYPE.HEAL, false);
    }


}
