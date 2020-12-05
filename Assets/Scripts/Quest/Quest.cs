using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] private string title;

    [SerializeField] private string descripiton;

    [SerializeField] private CollectObjective[] collectObjectives;

    public QuestScript MyQuestScript { get; set; }
    public string MyTitle { get => title; set => title = value; }
    public string MyDescripiton { get => descripiton; set => descripiton = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }
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
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScript.instance.GetItemCount(item.MyTitle);
            Questlog.instance.UpdateSelected();
        }
    }
}
