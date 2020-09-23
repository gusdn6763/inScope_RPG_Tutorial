﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Stat mana = null;
    [SerializeField] private Block[] blocks = null;
    [SerializeField] protected Transform[] exitPoint = null;
    private SpellBook spellBook;
    private Vector3 min, max;

    public Transform Target { get; set; }

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
        //Executes the GetInput function
        GetInput();

        float xMinClamp = Mathf.Clamp(transform.position.x, min.x, max.x);
        float yMinClamp = Mathf.Clamp(transform.position.y, min.y, max.y);

        transform.position = new Vector3(xMinClamp, yMinClamp, transform.position.z);
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
    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = Target;
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

        if (Target != null && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
            
        }
    }

    private bool InLineOfSight()
    {
        if (Target != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Target.position, Vector2.Distance(transform.position, Target.position), 256);

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