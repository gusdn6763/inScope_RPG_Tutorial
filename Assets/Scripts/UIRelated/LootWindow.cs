using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LootWindow : MonoBehaviour
{
    public static LootWindow instance;

    [SerializeField] private LootButton[] lootbuttons;
    [SerializeField] private Item[] items;
    [SerializeField] private GameObject previousBtn;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private Text pageNumber;
    private CanvasGroup canvasGroup;

    //모든페이지 포함하는 변수, 페이지마다 아이템의 리스트를 표현
    private List<List<Item>> pages = new List<List<Item>>();
    private List<Item> droppedLoot = new List<Item>();
 
 
    // 현재 열려있는 상태인지 확인

    //페이지의 번호
    private int pageIndex = 0;

    //몹에서 나오는 루팅 아이템을 1번으로만 나오게 하기위해
    public bool IsOpen { get { return canvasGroup.alpha > 0; } }


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
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //하이템 페이지 리스트를 표현
    public void CreatePages(List<Item> items)
    {
        //몹의 루팅UI에서 아이템이 계속 생성되는것을 방지하기위함
        if (!IsOpen)
        {
            //받은 아이템이 10개라면 3개의 페이지를 만듬
            List<Item> page = new List<Item>();

            droppedLoot = items;

            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);

                if (page.Count == 4 || i == items.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Item>();
                }
            }
            AddLoot();
            Open();
        }
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;

            previousBtn.SetActive(pageIndex > 0);

            nextBtn.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);

            //몇번째 페이지에 있는 아이템의 갯수만큼 -> 아이템이 총 10개라면 1페이지 4개, 2페이지4개, 3페이지의 아이템은 2개
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    // 아이콘 설정
                    lootbuttons[i].MyIcon.sprite = pages[pageIndex][i].MyIcon;

                    lootbuttons[i].MyLoot = pages[pageIndex][i];

                    // 활성화
                    lootbuttons[i].gameObject.SetActive(true);

                    // 아이템 제목 설정
                    string title = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[pages[pageIndex][i].MyQuality], pages[pageIndex][i].MyTitle);
                    lootbuttons[i].MyTitle.text = title;
                }
            }
        }
    }
    public void ClearButtons()
    {
        foreach (LootButton btn in lootbuttons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        //현재 페이지보다 큰 페이지가 있는지 확인
        //그렇지만 버튼 AddLoot함수에서 비활성화 하니 없어도 문제 없다.
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        //현재 페이지보다 작은 페이지가 있는지 확인
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    //얻은 아이템을 루팅UI에서 삭제 및 droppedLoot에서도 삭제해 루팅UI를 껏다켜도 다시 아이템이 생성되지 않게함
    public void TakeLoot(Item item)
    {
        pages[pageIndex].Remove(item);
        droppedLoot.Remove(item);

        //현재 페이지에 아이템이 없으면
        if (pages[pageIndex].Count == 0)
        {
            //페이지를 삭제
            pages.Remove(pages[pageIndex]);
            //만약 현재 마지막 페이지 이거나 0보다 클때 
            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }
            AddLoot();
        }
    }

    public void Open()
    {

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }


    public void Close()
    {
        //루팅UI를 껏다키면 아이템이 계속 생성되는것을 방지하기위해 루팅UI를 끌시 아이템을 전부 삭제시킨다.
        //루팅UI를 키면은 루팅한 아이템을 다시 생성하지만 droppedLoot변수로 아이템을 관리하기때문에 아이템을 루팅시 
        //droppedLoot변수에서 드랍한 아이템을 삭제한다. 
        pages.Clear();
        ClearButtons();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}