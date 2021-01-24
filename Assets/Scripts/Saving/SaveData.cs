using System;
using System.Collections.Generic;
using UnityEngine;

//여러가지를 저장하기위한 생성한 데이터 타입을 묶어두는 클래스
[Serializable]
public class SaveData
{
    public List<ChestData> MyChestData { get; set; }        //상자는 맵상에 여러개가 존재하므로 리스트로 저장
    public List<EquipmentData> MyEquipmentData { get; set; }   //플레이어 장비창의 장비슬롯은 여러개이므로 리스트로 저장
    public List<ActionButtonData> MyActionButtonData { get; set; } //플레이어 액션바
    public List<QuestData> MyQuestData { get; set; }        //퀘스트
    public List<QuestGiverData> MyQuestGiverData { get; set; }
    public PlayerData MyPlayerData { get; set; }            //플레이어 정보
    public InventoryData MyInventoryData { get; set; }      //인벤토리 정보
    public DateTime MyDateTime { get; set; }                //저장 슬롯에 날짜를 보여주기위함
    public string MyScene { get; set; }                     //저장 슬롯에 현재 위치를 보여주기 위함

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyChestData = new List<ChestData>();
        MyActionButtonData = new List<ActionButtonData>();
        MyEquipmentData = new List<EquipmentData>();
        MyQuestData = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
        MyDateTime = DateTime.Now;
    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }
    public float MyXp { get; set; }
    public float MyMaxXP { get; set; }
    public float MyHealth { get; set; }
    public float MyMaxHealth { get; set; }
    public float MyMana { get; set; }
    public float MyMaxMana { get; set; }
    public float MyX { get; set; }
    public float MyY { get; set; }

    public PlayerData(int level, float xp, float maxXp, float health, float maxHealth, float mana, float maxMana, Vector2 position)
    {
        this.MyLevel = level;
        this.MyXp = xp;
        this.MyMaxXP = maxXp;
        this.MyHealth = health;
        this.MyMaxHealth = maxHealth;
        this.MyMana = mana;
        this.MyMaxMana = maxMana;
        this.MyX = position.x;
        this.MyY = position.y;
    }
}

[Serializable]
public class ItemData
{
    //아이템 이름
    public string MyTitel { get; set; }
    public int MyStackCount { get; set; }
    public int MySlotIndex { get; set; }
    public int MyBagIndex { get; set; }

    public ItemData(string titel, int stackCount = 0, int slotIndex = 0, int bagIndex = 0)
    {
        MyTitel = titel;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
        MyBagIndex = bagIndex;
    }
}

//상자에 들어있는 아이템 저장
[Serializable]
public class ChestData
{
    //인스펙터의 상자 이름 => 상자는 여러개이므로 이름을 통해 어떠한 상자에 저장했는지 파악
    //단점 : 이름이 중복되어선 안됌
    //단점 : 씬 이동시에 상자만 다시 불러와야함 => 다른씬의 상자는 저장되지 않으므로
    public string MyName { get; set; }
    public List<ItemData> MyItems { get; set; }

    public ChestData(string name)
    {
        MyName = name;
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }           //가방
    public List<ItemData> MyItems { get; set; }         //가방안에 저장되어있는 아이템들

    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class BagData
{
    public int MySlotCount { get; set; }        //슬롯의 갯수가 몇개인지 확인
    public int MyBagIndex { get; set; }         //몇번째 가방인지 확인

    public BagData(int count, int index)
    {
        MySlotCount = count;
        MyBagIndex = index;
    }
}

[Serializable]
public class EquipmentData
{
    public string MyTitle { get; set; }
    public string MyType { get; set; }

    public EquipmentData(string title, string type)
    {
        MyTitle = title;
        MyType = type;
    }

}

[Serializable]
public class ActionButtonData
{
    public string MyAction { get; set; }
    public bool IsItem { get; set; }
    public int MyIndex { get; set; }

    public ActionButtonData(string action, bool isItem, int index)
    {
        this.MyAction = action;
        this.IsItem = isItem;
        this.MyIndex = index;
    }
}

//퀘스트들에 대한 정보 => 설명, 클리어 요구, npc, 퀘스트 id, 퀘스트 이름 다 있다...
[Serializable]
public class QuestData
{
    public CollectObjective[] MyCollectObjectives { get; set; }
    public KillObjective[] MyKillObjectives { get; set; }

    public string MyTitle { get; set; }
    public string MyDescription { get; set; }
    public int MyQuestGiverID { get; set; }

    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives, int questGiverID)
    {
        MyTitle = title;
        MyDescription = description;
        MyCollectObjectives = collectObjectives;
        MyKillObjectives = killObjectives;
        MyQuestGiverID = questGiverID;
    }
}

[Serializable]
public class QuestGiverData
{
    public List<string> MyCompletedQuests { get; set; }
    public int MyQuestGiverID { get; set; }

    public QuestGiverData(int questGiverID, List<string> completedQuests)
    {
        this.MyQuestGiverID = questGiverID;
        MyCompletedQuests = completedQuests;
    }
}
