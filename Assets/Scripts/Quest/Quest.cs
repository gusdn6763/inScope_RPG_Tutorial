using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] private string title;

    [SerializeField] private string descripiton;

    //아이템 수집
    [SerializeField] private CollectObjective[] collectObjectives;

    //몹죽이기
    [SerializeField] private KillObjective[] killObjectives;

    //누구한테 퀘스트를 받았는지 저장하기위함
    public QuestGiver MyQuestGiver { get; set; }

    public QuestScript MyQuestScript { get; set; }
    public string MyTitle { get => title; set => title = value; }
    public string MyDescription { get => descripiton; set => descripiton = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }
    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }

    [SerializeField] private int level;
    [SerializeField] private int xp;

    //퀘스트에서 요구하는 아이템들을 체크함
    public bool IsComplete
    {
        get
        {
            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            foreach (Objective o in MyKillObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public int MyLevel { get => level; set => level = value; }
    public int MyXp { get => xp; set => xp = value; }
}

//퀘스트에서 요구하는 아이템
[System.Serializable]
public abstract class Objective
{
    [SerializeField] private int amount;

    private int currentAmount;

    [SerializeField] private string type;

    public int MyAmount { get => amount; set => amount = value; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }

    public string MyType { get => type; set => type = value; }

    //퀘스트를 완료하기에 알맞은 양인지 확인
    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    //아이템을 얻을때마다 요구하는 아이템의 수량이 변경되어있는지 체크한다. 
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScript.instance.GetItemCount(item.MyTitle);
            if (MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.instance.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }
            Questlog.instance.UpdateSelected();
            Questlog.instance.CheckCompletion();
        }
    }

    //함수 오버로딩, 
    //퀘스트 수락전에 아이템이 이미 있거나 아이템을 소비 및 버릴경우를 대비해 
    //퀘스트에서 요구하는 아이템 수량이 바뀌었는지 체크
    public void UpdateItemCount()
    {
        MyCurrentAmount = InventoryScript.instance.GetItemCount(MyType);

        Questlog.instance.CheckCompletion();
        Questlog.instance.UpdateSelected();
    }

    //퀘스트 완료시 아이템 삭제
    public void Complete()
    {
        Stack<Item> items = InventoryScript.instance.GetItems(MyType, MyAmount);

        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}


[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if (MyType == character.MyType)
        {
            if (MyCurrentAmount < MyAmount)
            {
                MyCurrentAmount++;
                MessageFeedManager.instance.WriteMessage(string.Format("{0}: {1}/{2}", character.MyType, MyCurrentAmount, MyAmount));
                Questlog.instance.CheckCompletion();
                Questlog.instance.UpdateSelected();
            }
        }
    }
}


