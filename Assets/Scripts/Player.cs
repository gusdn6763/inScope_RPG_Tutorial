using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Stat health = null;
    [SerializeField] private Stat mana = null;
    [SerializeField] private Block[] blocks = null;
    [SerializeField] protected Transform[] exitPoint = null;
    [SerializeField] protected GameObject[] spellPrefab = null;

    public Transform MyTarget { get; set; }

    private float initHealth = 100;
    private float initMana = 50;
    private int exitIndex;

    private void Start()
    {
        health.Initialize(initHealth, initHealth);
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
        isAttacking = true;
        animator.SetBool("Attack", isAttacking);
        yield return new WaitForSeconds(1);
        Instantiate(spellPrefab[spellIndex], exitPoint[exitIndex].position, quaternion.identity);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, MyTarget.position, Vector2.Distance(transform.position, MyTarget.position), 256);

        if (hit.collider == null)
        {
            return true;
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
}