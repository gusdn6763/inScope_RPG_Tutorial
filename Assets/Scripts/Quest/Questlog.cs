using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//플레이어 퀘스트 UI  
public class Questlog : MonoBehaviour
{
    public static Questlog instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questParent;
    [SerializeField] private Text questDescription;     //플레이어 퀘스트UI의 설명창
    [SerializeField] private Text questCountTxt;        //현재 받은 퀘스트 갯수
    [SerializeField] private int maxCount;              //받을 수 있는 퀘스트 최대 갯수
    //플레이어가 소지한 퀘스트들
    private List<QuestScript> questScripts = new List<QuestScript>();
    //플레이어가 수락한 퀘스트들 => NPC퀘스트중에서 수락한 퀘스트가 있는지 체크하기위함  
    private List<Quest> quests = new List<Quest>();
    //플레이어가 선택하고있는 퀘스트
    private Quest selectedQuest;
    private int currentCount = 0;

    public List<Quest> MyQuests { get => quests; set => quests = value; }

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
        //받을 수 있는 최대 퀘스트 갯수보다 많으면 받지 않음
        if (currentCount < maxCount)
        {
            //갯수 증가
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;
            //itemCountChangedEvent는 이벤트 함수로 아이템을 얻거나 인벤토리 슬롯의 무언가 변경이 되었을때 실행한다.
            //아이템을 얻을시 퀘스트에 필요한 아이템이면 갯수를 증가시키기 위함    
            foreach (CollectObjective o in quest.MyCollectObjectives)
            {
                InventoryScript.instance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);

                o.UpdateItemCount();
            }

            foreach (KillObjective o in quest.MyKillObjectives)
            {
                GameManager.instance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);
            }

            MyQuests.Add(quest);

            GameObject go = Instantiate(questPrefab, questParent);
            QuestScript qs = go.GetComponent<QuestScript>();
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;
            go.GetComponent<Text>().text = quest.MyTitle;

            questScripts.Add(qs);

            //퀘스트를 수락 후 인벤토리에 아이템이 있는지 체크  
            CheckCompletion();
        }
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
            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("<b>{0}\n\n</b><size=10>{1}\n\n</size><size=8>{2}</size>", quest.MyTitle, quest.MyDescription, objectives);
        }
    }
    //퀘스트들이 완료됬는지 하나씩 확인한다.  
    public void CheckCompletion()
    {
        foreach (QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }


    //퀘스트 포기
    public void AbandonQuest()
    {
        foreach (CollectObjective o in selectedQuest.MyCollectObjectives)
        {
            InventoryScript.instance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
            o.Complete();
        }

        foreach (KillObjective o in selectedQuest.MyKillObjectives)
        {
            GameManager.instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
        }
        RemoveQuest(selectedQuest.MyQuestScript);
    }

    //퀘스트 포기 및 퀘스트 완료
    public void RemoveQuest(QuestScript qs)
    {
        //수락한 퀘스트들 삭제
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        MyQuests.Remove(qs.MyQuest);

        questDescription.text = string.Empty;
        selectedQuest = null;
        currentCount--;
        questCountTxt.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return MyQuests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}
