using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string spellName = null; 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //스킬창에서 스킬을 클릭시 인스펙터에 입력한 스펠의 이름과 SpellBook에 있는 스펠의 이름을 찾음
            HandScript.instance.TakeMoveable(SpellBook.instance.GetSpell(spellName));
        }
    }
}
