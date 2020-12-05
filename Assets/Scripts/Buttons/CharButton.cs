using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GearSocket gearSocket;
    [SerializeField] private ArmorType armorType;

    // 현재 장착된 armor를 참조함
    private Armor equippedArmor;

    //현재 장착된 장비 아이템을 표시
    [SerializeField] private Image icon;

    public CharButton MyCharButton { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // HandScript 에 등록된 아이템이 Armor 라면 
            if (HandScript.instance.Dragable is Armor)
            {
                Armor tmp = (Armor)HandScript.instance.Dragable;

                // tmp의 armor 타입이 해당 슬롯의 armorType 과 같은지 확인
                if (tmp.ArmorType == armorType)
                {
                    EquipArmor(tmp);
                }
                //장비창에서 교체한 장비의 설명창UI를 바로 보여줌
                UIManager.instance.RefreshTooltip(tmp);
            }
            //드래그 한것이 없는 상태에서 장비창을 클릭하고, 무언가를 장비중일시 => 장비해제
            else if(HandScript.instance.Dragable == null && equippedArmor != null)
            {
                //아이템의 정보를 드래그에 넘겨줌
                HandScript.instance.TakeMoveable(equippedArmor);
                CharacterPanel.instance.MyCharButton = this;
                icon.color = Color.grey;
            }
        }
    }

    // 장착
    public void EquipArmor(Armor armor)
    {
        //원래 입던 장비 해제
        armor.Remove();

        //장비를 착용중이라면
        if (equippedArmor != null)
        {
            if (equippedArmor != armor)
            {
                //원래입던 장비를 가방에 추가함
                armor.MySlot.AddItem(equippedArmor);
            }
            //원래 입었던 장비의 설명창UI를 보여줌
            UIManager.instance.RefreshTooltip(equippedArmor);
        }
        else
        {
            UIManager.instance.HideTooltip();
        }
        // 비활성화된 유니티상의 인스펙터에서 Image컴포넌트를 활성화
        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        this.equippedArmor = armor;
        icon.color = Color.white;
        this.equippedArmor.MyCharButton = this;

        if (HandScript.instance.Dragable == (armor as IMoveable))
        {
            HandScript.instance.Drop();
        }

        //gearSocket이 널값이 아닌지 체크하는 이유 : 보조장비, 장갑같은경우 장비를 해도 플레이어가 장비한 장비를 보여주지 않음  
        if (gearSocket != null && equippedArmor.MyAnimationClips != null)
        {
            gearSocket.Equip(equippedArmor.MyAnimationClips);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equippedArmor != null)
        {
            //플레이어가 착용한 장비의 정보를 보여줌
            UIManager.instance.ShowTooltip(new Vector2(0, 0), transform.position, equippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (equippedArmor != null)
        {
            //플레이어가 착용한 장비의 정보를 보여줌
            UIManager.instance.HideTooltip();
        }
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;

        if (gearSocket != null && equippedArmor.MyAnimationClips != null)
        {
            gearSocket.Dequip();
        }
        equippedArmor.MyCharButton = null;
        equippedArmor = null;
    }
}