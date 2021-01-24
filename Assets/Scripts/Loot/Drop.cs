using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop
{
    public Item MyItem { get; set; }
    public LootTable MyLootTabe { get; set; }

    public Drop(Item item, LootTable lootTable)
    {
        MyLootTabe = lootTable;
        MyItem = item;
    }

    //몹이 죽을시 루팅창에서 아이템을 드랍하지만 중첩되어 죽었을시 루팅창이 1개밖에 보이지가 않는다.  
    //LootWindow스크립트에서 중첩되어있을시 여러개의 루팅창을 하나의 루팅창으로 
    //만들어버리고 Remove()함수를 이용해 다른 루팅창의 아이템을 삭제
    public void Remove()
    {
        MyLootTabe.MyDroppedItems.Remove(this);
    }
}
