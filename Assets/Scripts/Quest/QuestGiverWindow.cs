using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    public static QuestGiverWindow instance;

    //퀘스트 수락, 뒤로 돌아가기 버튼 
    [SerializeField] private GameObject backBtn, AcceptBtn, questDescription, completeBtn;

    //showQuests함수로 NPC의 퀘스트의 갯수만큼 생성
    [SerializeField] private GameObject questPrefab;

    //NPC퀘스트창 위치
    [SerializeField] private Transform questArea;

    //퀘스트 종류를 저장하기위한 리스트(퀘스트가 중복생성되는것을 막음)
    private List<GameObject> quests = new List<GameObject>();

    //퀘스트를 주는 NPC
    private QuestGiver questGiver;

    //퀘스트 선택시 선택된 퀘스트
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

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (GameObject go in quests)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                //퀘스트 생성
                GameObject go = Instantiate(questPrefab, questArea);
                //퀘스트 수락 가능할 시
                go.GetComponent<Text>().text = "[" + quest.MyLevel + "]" + quest.MyTitle + "<color=#ffbb04> <size=14>!</size></color>";
                go.GetComponent<QGQuestScript>().MyQuest = quest;

                //다시 삭제하기위해 리스트에 저장
                quests.Add(go);

                //퀘스트를 클리어했을시 표현
                if (Questlog.instance.HasQuest(quest) && quest.IsComplete)
                {
                    //퀘스트 완료시 회색으로
                    go.GetComponent<Text>().text = quest.MyTitle + "<color=#ffbb04> <size=14> ?</size></color>";
                }
                //퀘스트를 수락중인 상태시 반투명으로 표현  
                else if (Questlog.instance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;

                    c.a = 0.5f;

                    go.GetComponent<Text>().color = c;
                    //퀘스트 수락중인 표시
                    go.GetComponent<Text>().text = quest.MyTitle + "<color=#c0c0c0ff> <size=14> ?</size></color>";
                }
            }
        }
    }
    public override void Open(NPC npc)
    {
        ShowQuests((npc as QuestGiver));
        base.Open(npc);
    }

    //퀘스트 이름 클릭시 퀘스트를 수락할지, 뒤로 돌아갈지(다른 퀘스트를 보기위함), 퀘스트 설명을 표현
    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        //이미 퀘스트를 클릭했을시 수락버튼을 비활성화 시킴  
        if (Questlog.instance.HasQuest(quest) && quest.IsComplete)
        {
            AcceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!Questlog.instance.HasQuest(quest))
        {
            AcceptBtn.SetActive(true);
        }

        backBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = "\n필요 아이템";

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        questDescription.GetComponent<Text>().text = string.Format("<b>{0}\n\n</b><size=11>{1}\n\n</size><size=8>{2}</size>", quest.MyTitle, quest.MyDescription, objectives);
    }

    public void Back()
    {
        backBtn.SetActive(false);
        AcceptBtn.SetActive(false);
        ShowQuests(questGiver);
        completeBtn.SetActive(false);
    }

    public void Accept()
    {
        Questlog.instance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completeBtn.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        //퀘스트를 클리어시 NPC의 퀘스트들중에 찾아서 비활성화
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompltedQuests.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }
            foreach (CollectObjective o in selectedQuest.MyCollectObjectives)
            {
                InventoryScript.instance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
                o.Complete();
            }

            foreach (KillObjective o in selectedQuest.MyKillObjectives)
            {
                GameManager.instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
            }

            //함수 오버로딩 ->퀘스트와 몹변수를 받을 수 있음
            Player.instance.GainXP(XPManager.CalculateXP((selectedQuest)));
            Questlog.instance.RemoveQuest(selectedQuest.MyQuestScript);
            Back();
        }
    }
}
