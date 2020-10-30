using UnityEngine;
using UnityEngine.UI;

public class LootButton : MonoBehaviour
{
    [SerializeField] private Image icon;

    [SerializeField] private Text title;

    public Image MyIcon { get { return icon; } }

    public Text MyTitle { get { return title; } }
}
