using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedManager : MonoBehaviour
{
    public static MessageFeedManager instance;

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

    public void WriteMessage(string message)
    {
        GameObject go = Instantiate(messagePrefab, transform);
        go.GetComponent<Text>().text = message;
    }
}
