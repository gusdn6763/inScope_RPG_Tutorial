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
    private Vector3 min, max;

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
        Vector2 moveVector;

        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");
        Direction = moveVector;
        if (IsMoving)
        {
            if (Direction.y > 0) 
                exitIndex = 0;         // 위쪽
            else if (Direction.y < 0) 
                exitIndex = 2;    // 아래
            else if (Direction.x > 0) 
                exitIndex = 1;    // 오른족
            else 
                exitIndex = 3;       // 왼쪽
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
        if (IsMoving)
        {
            StopAttack();
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
        Animator.SetBool("Attack", isAttacking);

        yield return new WaitForSeconds(newSpell.CastTime);

        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.SpellGameObject, exitPoint[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.Damage, transform);
        }
        StopAttack();
    }


    public void CastSpell(int spellIndex)
    {
        Block();

        if (Target != null && Target.GetComponentInParent<Character>().IsAlive && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
            
        }
    }

    private bool InLineOfSight()
    {
        if (Target != null)
        {
            Vector3 targetDirection = (Target.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.position), 256);
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
            b.BlockCheck(false);
        }

        blocks[exitIndex].BlockCheck(true);
    }

    public void StopAttack()
    {
        spellBook.StopCasting();
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            Animator.SetBool("Attack", isAttacking);
        }
    }
}