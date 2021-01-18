using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{ 
    [SerializeField] private Quest[] quests;

    [SerializeField] private Sprite question, questionSilver, exclamation;

    [SerializeField] private SpriteRenderer statusRenderer;

    public Quest[] MyQuests { get { return quests; } }

    private void Start()
    {
        //NPC에 퀘스트가 있을경우 그 퀘스트가 어떠한 NPC에게 있는지 저장하기위함
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    //퀘스트의 상태에 따라 NPC의 기호 변경  
    public void UpdateQuestStatus()
    {
        int count = 0;

        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                //퀘스트를 클리어가능하고, 퀘스트를 보유시
                if (quest.IsComplete && Questlog.instance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    break;
                }
                //퀘스트를 가지고 있을시
                else if (!Questlog.instance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    break;
                }
                //퀘스트를 아직 클리어 못할시
                else if (!quest.IsComplete && Questlog.instance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                }
            }
            else
            {
                count++;

                if (count == quests.Length)
                {
                    statusRenderer.enabled = false;
                }
            }
        }
    }
}
