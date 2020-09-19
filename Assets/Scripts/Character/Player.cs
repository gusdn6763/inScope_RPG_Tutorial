using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Stat mana = null;
    [SerializeField] private Block[] blocks = null;
    [SerializeField] protected Transform[] exitPoint = null;
    private SpellBook spellBook;


    public Transform MyTarget { get; set; }

    private float initMana = 50;
    private int exitIndex;

    protected override void Awake()
    {
        base.Awake();
        spellBook = GetComponent<SpellBook>();
    }

    protected override void Start()
    {
        base.Start();
        mana.Initialize(initMana, initMana);
    }


    protected override void Update()
    {
        GetInput();
        base.Update();
    }

    private void GetInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        if (IsMoving)
        {
            if (direction.y > 0) 
                exitIndex = 0;         // 위쪽
            else if (direction.y < 0) 
                exitIndex = 2;    // 아래
            else if (direction.x > 0) 
                exitIndex = 1;    // 오른족
            else 
                exitIndex = 3;       // 왼쪽
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget;
        Spell newSpell = spellBook.CastSpell(spellIndex);

        isAttacking = true;
        animator.SetBool("Attack", isAttacking);

        yield return new WaitForSeconds(newSpell.CastTime);

        if (currentTarget != null & InLineOfSight())
        {
            Vector3 exitPosition = exitPoint[exitIndex].position;
            Quaternion exitQuaternion = Quaternion.identity;

            SpellScript s = Instantiate(newSpell.SpellGameObject, exitPosition, exitQuaternion).GetComponent<SpellScript>();
            s.Initialize(currentTarget, newSpell.Damage);
        }
        StopAttack();
    }


    public void CastSpell(int spellIndex)
    {
        Block();

        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
            
        }
    }

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, MyTarget.position, Vector2.Distance(transform.position, MyTarget.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }

    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.activate(false);
        }

        blocks[exitIndex].activate(true);
    }

    public override void StopAttack()
    {
        base.StopAttack();
        spellBook.StopCasting();
    }
}