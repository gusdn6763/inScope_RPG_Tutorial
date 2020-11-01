using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour
{
    private AnimatorOverrideController animatorOverrideController;
    protected SpriteRenderer spriteRenderer;
    private Animator parentAnimator;

    public Animator MyAnimator { get; set; }

    public enum LayerName
    {
        IdleLayer = 0,
        WalkLayer = 1,
        AttackLayer = 2,
        DeathLayer = 3,
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }

    public virtual void SetXAndY(float x, float y)
    {
        MyAnimator.SetFloat("X", x);
        MyAnimator.SetFloat("Y", y);
    }

    //플레이어의 상태에 따라 레이어를 바꿈
    public void ActivateLayer(Player.LayerName layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight((int)layerName, 1);
    }

public void Equip(AnimationClip[] animationClips)
    {
        //애니메이션 컨트롤러는 해당장비에 맞게 애니메이션 클립값들을 얻는다.  
        spriteRenderer.color = Color.white;

        Debug.Log("asd");
        animatorOverrideController["Wizard_Attack_Back"] = animationClips[0];
        animatorOverrideController["Wizard_Attack_Front"] = animationClips[1];
        animatorOverrideController["Wizard_Attack_Left"] = animationClips[2];
        animatorOverrideController["Wizard_Attack_Right"] = animationClips[3];

        animatorOverrideController["Wizard_Idle_Back"] = animationClips[4];
        animatorOverrideController["Wizard_Idle_Front"] = animationClips[5];
        animatorOverrideController["Wizard_Idle_Left"] = animationClips[6];
        animatorOverrideController["Wizard_Idle_Right"] = animationClips[7];

        animatorOverrideController["Wizard_Walk_Back"] = animationClips[8];
        animatorOverrideController["Wizard_Walk_Front"] = animationClips[9];
        animatorOverrideController["Wizard_Walk_Left"] = animationClips[10];
        animatorOverrideController["Wizard_Walk_Right"] = animationClips[11];
    }

    public void Dequip()
    {
        animatorOverrideController["Wizard_Attack_Back"] = null;
        animatorOverrideController["Wizard_Attack_Front"] = null;
        animatorOverrideController["Wizard_Attack_Left"] = null;
        animatorOverrideController["Wizard_Attack_Right"] = null;

        animatorOverrideController["Wizard_Idle_Back"] = null;
        animatorOverrideController["Wizard_Idle_Front"] = null;
        animatorOverrideController["Wizard_Idle_Left"] = null;
        animatorOverrideController["Wizard_Idle_Right"] = null;

        animatorOverrideController["Wizard_Walk_Back"] = null;
        animatorOverrideController["Wizard_Walk_Front"] = null;
        animatorOverrideController["Wizard_Walk_Left"] = null;
        animatorOverrideController["Wizard_Walk_Right"] = null;

        //장비 해제시 장비를 투명하게 만들어 보여주지 않음  
        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }
}
