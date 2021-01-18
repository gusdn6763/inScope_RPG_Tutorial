using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public static Player instance;

    [SerializeField] private GearSocket[] gearSockets;
    [SerializeField] private Block[] blocks = null;
    [SerializeField] protected Transform[] exitPoint = null;
    [SerializeField] Animator levelUpEffect = null;
    [SerializeField] private Stat mana = null;
    [SerializeField] private Stat xpStat;
    [SerializeField] private Text levelText;

    //NPC, 몹등과 상호작용하기 위함  
    private IInteractable interactable;

    private Vector3 min, max;

    private float initMana = 50;
    private int exitIndex;

    public int MyGold { get; set; }
    public Stat MyXp { get { return xpStat; } set { xpStat = value; } }
    public IInteractable MyInteractable { get { return interactable; } set { interactable = value; } }

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
        xpStat.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
        MyGold = 100;
    }

    protected override void Update()
    {
        GetInput();

        float xMinClamp = Mathf.Clamp(transform.position.x, min.x, max.x);
        float yMinClamp = Mathf.Clamp(transform.position.y, min.y, max.y);

        transform.position = new Vector3(xMinClamp, yMinClamp, transform.position.z);
        base.Update();
    }

    public override void HandleLayers()
    {
        base.HandleLayers();

        if (IsMoving)
        {
            foreach(GearSocket g in gearSockets)
            {
                g.SetXAndY(Direction.x, Direction.y);
            }
        }
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(600);
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

        foreach (GearSocket g in gearSockets)
        {
            g.MyAnimator.SetBool("Attack", isAttacking);
        }

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
        isAttacking = false;
        Animator.SetBool("Attack", isAttacking);

        foreach (GearSocket g in gearSockets)
        {
            g.MyAnimator.SetBool("Attack", isAttacking);
        }

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            
        }
    }

    public override void ActivateLayer(LayerName layerName)
    {
        base.ActivateLayer(layerName);

        foreach (GearSocket g in gearSockets)
        {
            g.ActivateLayer(layerName);
        }
    }

    public void Interact()
    {
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    //경험치 획득
    public void GainXP(int xp)
    {
        //현재 내 경험치에서 얻을 경험치를 더함
        MyXp.MyCurrentValue += xp;
        CombatTextManager.instance.CreateText(transform.position, xp.ToString(), SCTTYPE.XP, false);

        //레벨업시
        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }
    }

    private IEnumerator LevelUp()
    {
        //이 조건문이 없을경우 한번에 여러번 레벨업시 레벨업바가 증가하는 과정을 건너뛰게된다.
        while (!MyXp.IsFull)
        {
            yield return null;
        }

        MyLevel++;
        levelUpEffect.SetTrigger("Ding");
        levelText.text = MyLevel.ToString();
        MyXp.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyXp.MyMaxValue = Mathf.Floor(MyXp.MyMaxValue);
        MyXp.MyCurrentValue = MyXp.MyOverflow;
        MyXp.Reset();

        //한번에 2업이상 레벨업을 할 경우를 대비하기위해 조건문이 맞을시 함수 재실행
        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Interactable"))
        {
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Interactable"))
        {
            if (interactable != null)
            {
                interactable.StopInteract();
                interactable = null;
            }
        }
    }
}