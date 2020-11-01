using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] private Loot[] loots;

    private List<Item> droppedItems = new List<Item>();
    private bool rolled = false;

    public void ShowLoot()
    {
        //만약 루팅창을 껏다가 다시키면은 루팅확률이 바뀌는것을 방지하기 위함
        if (!rolled)
        {
            RollLoot();
        }
        //드롭확률로 아이템이 생성시 페이지에 아이템을 생성
        LootWindow.instance.CreatePages(droppedItems);
    }

    //루팅 확률  
    private void RollLoot()
    {
        foreach (Loot loot in loots)
        {
            int roll = Random.Range(0, 100);

            if (roll <= loot.DropChance)
            {
                droppedItems.Add(loot.Item);
            }
        }
        rolled = true;
    }
}
