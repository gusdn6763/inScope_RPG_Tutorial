using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] private Loot[] loots;

    public List<Drop> MyDroppedItems { get; set; }

    private bool rolled = false;

    public List<Drop> GetLoot()
    {
        //만약 루팅창을 껏다가 다시키면은 루팅확률이 바뀌는것을 방지하기 위함
        if (!rolled)
        {
            MyDroppedItems = new List<Drop>();
            RollLoot();
        }
        return MyDroppedItems;
    }

    //루팅 확률  
    private void RollLoot()
    {
        foreach (Loot loot in loots)
        {
            int roll = Random.Range(0, 100);

            if (roll <= loot.DropChance)
            {
                MyDroppedItems.Add(new Drop(loot.Item, this));
            }
        }
        rolled = true;
    }
}
