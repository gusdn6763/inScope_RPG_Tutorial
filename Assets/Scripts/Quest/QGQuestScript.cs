using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QGQuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }

    //퀘스트클릭시 퀘스트의 정보를 보여줌
    public void Select()
    {
        QuestGiverWindow.instance.ShowQuestInfo(MyQuest);
    }
}
