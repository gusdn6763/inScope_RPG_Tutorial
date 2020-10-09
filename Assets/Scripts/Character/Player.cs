using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    public static Player instance;

    [SerializeField] private Stat mana = null;
    [SerializeField] private Block[] blocks = null;
    [SerializeField] protected Transform[] exitPoint = null;
    private Vector3 min, max;

    private float initMana = 50;
    private int exitIndex;

    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        base.Awake();
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
        Direction = Vector2.zero;

        ///THIS IS USED FOR DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }


        // KeybindManager에 설정된 버튼으로 이동하기
        if (Input.GetKey(KeybindManager.instance.keybinds["UP"])) //Moves up
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeybindManager.instance.keybinds["LEFT"])) //Moves left
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeybindManager.instance.keybinds["DOWN"])) //Moves down
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeybindManager.instance.keybinds["RIGHT"])) //Moves right
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }
        if (IsMoving)
        {
            StopAttack();
        }

        // KeybindManager에 설정된 버튼으로 스킬 사용
        foreach (string action in KeybindManager.instance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.instance.ActionBinds[action]))
            {
                UIManager.instance.ClickActionButton(action);
            }
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(string spellName)
    {
        Transform currentTarget = Target;
        Spell newSpell = SpellBook.instance.CastSpell(spellName);

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


    public void CastSpell(string spellName)
    {
        Block();

        if (Target != null && Target.GetComponentInParent<Character>().IsAlive && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellName));
            
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
        SpellBook.instance.StopCasting();
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            Animator.SetBool("Attack", isAttacking);
        }
    }
}