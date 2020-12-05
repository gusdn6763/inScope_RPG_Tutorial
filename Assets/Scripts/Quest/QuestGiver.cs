using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private Quest[] quests;

    //디버깅용
    [SerializeField] private Questlog tmpLog;

    private void Awake()
    {
        //디버깅용
        tmpLog.AcceptQuest(quests[0]);
        tmpLog.AcceptQuest(quests[1]);
    }
}
