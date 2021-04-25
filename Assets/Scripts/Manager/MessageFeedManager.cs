using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 정보창을 나타내는 스크립트
/// </summary>
public class MessageFeedManager : MonoBehaviour
{
    public static MessageFeedManager instance;

    [SerializeField] private Transform position;
    [SerializeField] private GameObject messagePrefab;

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

    /// <summary>
    /// 퀘스트의 정보상태를 생성
    /// </summary>
    /// <param name="message">어떠한 메세지인지</param>
    public void WriteMessage(string message)
    {
        GameObject go = Instantiate(messagePrefab, position);
        go.GetComponent<Text>().text = message;
    }
}
