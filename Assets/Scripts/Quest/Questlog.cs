using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour
{
    public static Questlog instance;

    [SerializeField] private GameObject questPrefab;

    [SerializeField] private Transform questParent;

    [SerializeField] private Text questDescription;

    private Quest selectedQuest;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void AcceptQuest(Quest quest)
    {
        //itemCountChangedEvent는 이벤트 함수로 아이템을 얻거나 인벤토리 슬롯의 무언가 변경이 되었을때 실행한다.  
        foreach (CollectObjective o in quest.MyCollectObjectives)
        {
            InventoryScript.instance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
        }

        GameObject go = Instantiate(questPrefab, questParent);

        QuestScript qs = go.GetComponent<QuestScript>();
        quest.MyQuestScript = qs;
        qs.MyQuest = quest;
        go.GetComponent<Text>().text = quest.MyTitle;
    }

    //UpdateItemCount함수가 실행되면 실행한다. 왜이렇게 만든거지..?  
    public void UpdateSelected()
    {
        ShowDescription(selectedQuest);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selectedQuest != null && selectedQuest != quest)
            {
                selectedQuest.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;

            selectedQuest = quest;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("<b>{0}\n\n</b><size=10>{1}\n\n</size><size=8>{2}</size>", quest.MyTitle, quest.MyDescripiton, objectives);
        }
    }
}
