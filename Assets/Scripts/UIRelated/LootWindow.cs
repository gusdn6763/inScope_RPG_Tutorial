using UnityEngine;

public class LootWindow : MonoBehaviour
{
    [SerializeField] private LootButton[] lootbuttons;

    [SerializeField] private Item[] items;

    private void Start()
    {
        // 임시 코드
        AddLoot();
    }

    private void AddLoot()
    {
        // 임시 코드
        int itemIndex = 2;

        // 아이콘 설정
        lootbuttons[itemIndex].MyIcon.sprite = items[itemIndex].Icon;

        // 활성화
        lootbuttons[itemIndex].gameObject.SetActive(true);

        // 아이템 제목 설정
        string title = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[items[itemIndex].Quality], items[itemIndex].Title);
        lootbuttons[itemIndex].MyTitle.text = title;
    }
}