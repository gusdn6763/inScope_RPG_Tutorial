using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmorType { Helmet, Shoulders, Chest, Gloves, Pants, Boots, MainHand, Offhand, TwoHand }

// 상단 메뉴에 명령버튼 추가.
[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
    [SerializeField] private AnimationClip[] animationClips;

    [SerializeField] private ArmorType armorType;

    // 힘
    [SerializeField] private int strength;

    // 내구력
    [SerializeField] private int stamina;

    // 지력
    [SerializeField] private int intellect;

    internal ArmorType ArmorType { get => armorType; }
    public AnimationClip[] MyAnimationClips { get => animationClips; }

    public override string GetDescription()
    {
        string stats = string.Empty;

        if (strength > 0)
        {
            stats += string.Format("\n +{0} 힘", strength);
        }
        if (stamina > 0)
        {
            stats += string.Format("\n +{0} 내구력", stamina);
        }
        if (intellect > 0)
        {
            stats += string.Format("\n +{0} 지력", intellect);
        }

        // 원래 설명글 + 아이템 능력치를 반환
        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.instance.EquipArmor(this);
    }
}