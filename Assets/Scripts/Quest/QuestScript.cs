using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    //퀘스트가 완료했는지 체크하는 변수
    private bool markedComlete = false;

    public Quest MyQuest { get; set; }

    public void Select()
    {
        GetComponent<Text>().color = Color.red;
        Questlog.instance.ShowDescription(MyQuest);
    }

    public void DeSelect()
    {
        GetComponent<Text>().color = Color.white;
    }
    public void IsComplete()
    {
        if (MyQuest.IsComplete && !markedComlete)
        {
            markedComlete = true;
            GetComponent<Text>().text = "[" + MyQuest.MyLevel + "]" + MyQuest.MyTitle + "(C)";
            MessageFeedManager.instance.WriteMessage(string.Format("\n{0} (퀘스트 완료)", MyQuest.MyTitle));
        }
        //아이템을 버렸을 경우나 완료상태에서 다시 아닐경우 완료했다는 표시를 삭제하기위함
        else if (!MyQuest.IsComplete)
        {
            markedComlete = false;
            GetComponent<Text>().text = "[" + MyQuest.MyLevel + "]" +MyQuest.MyTitle;
        }
    }
}
