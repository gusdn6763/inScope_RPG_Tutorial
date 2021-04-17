using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private SavedGame[] saveSlots;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private Text dialogueText;

    private Chest[] chests;
    private CharButton[] equipment;
    private SavedGame current;

    private string action;

    private void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipment = FindObjectsOfType<CharButton>();
    }

    private void Start()
    {
        chests = FindObjectsOfType<Chest>();
        equipment = FindObjectsOfType<CharButton>();

        foreach (SavedGame saved in saveSlots)
        {
            ShowSavedFiles(saved);
        }

        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
            Player.instance.SetDefaultValues();
        }
    }

    //게임을 저장, 삭제, 불러오기 버튼을 누를시 다시한번 확인하는 창 생성
    public void ShowDialogue(GameObject clickButton)
    {
        action = clickButton.name;
        Debug.Log(action);
        switch (action)
        {
            case "Load":
                dialogueText.text = "게임을 불러오겠습니까?";
                break;
            case "Save":
                dialogueText.text = "게임을 저장하겠습니까?";
                break;
            case "Delete":
                dialogueText.text = "게임을 삭제하겠습니까?";
                break;
        }

        current = clickButton.GetComponentInParent<SavedGame>();
        dialogue.SetActive(true);
    }

    //다시한번 확인하는 창에서 확인을 누를시 실행
    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(current);
                break;
            case "Save":
                Save(current);
                break;
            case "Delete":
                Delete(current);
                break;
        }

        CloseDialogue();

    }

    private void LoadScene(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("Load", savedGame.MyIndex);
            SceneManager.LoadScene(data.MyScene);
        }
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    private void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideVisuals();
    }

    /// <summary>
    /// 파일이 존재할시 파일을 미리 불러와 정보를 읽음
    /// </summary>
    /// <param name="savedGame"></param>
    private void ShowSavedFiles(SavedGame savedGame)
    {
        //파일이 존재하면
        if (File.Exists(Application.persistentDataPath + "/"+savedGame.gameObject.name+".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }

   public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            //파일 생성 및 경로 저장
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name+".dat", FileMode.Create);
            //사용자 정의 타입의 데이터타입 생성
            SaveData data = new SaveData();
            //현재 씬의 이름을 불러옴
            data.MyScene = SceneManager.GetActiveScene().name;

            SaveEquipment(data);
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            SaveChests(data);
            SaveActionButtons(data);
            SaveQuests(data);
            SaveQuestGivers(data);

            //경로, 데이터
            //https://xlfksh48.tistory.com/10
            bf.Serialize(file, data);

            file.Close();

            ShowSavedFiles(savedGame);
        }
        catch (System.Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Player.instance.MyLevel,
            Player.instance.MyXp.MyCurrentValue, Player.instance.MyXp.MyMaxValue,
            Player.instance.Health.MyCurrentValue, Player.instance.Health.MyMaxValue,
            Player.instance.Mana.MyCurrentValue, Player.instance.Mana.MyMaxValue,
            Player.instance.transform.position);
    }
    //상자 안에있는 아이템들 저장
    private void SaveChests(SaveData data)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            //리스트 타입으므로
            data.MyChestData.Add(new ChestData(chests[i].name));

            foreach (Item item in chests[i].MyItems)
            {
                if (chests[i].MyItems.Count > 0)
                {
                    //아이템의 이름, 갯수, 몇번째 슬롯인지
                    data.MyChestData[i].MyItems.Add(new ItemData(item.MyTitle, item.MySlot.MyItems.Count, item.MySlot.MyIndex));
                }
            }
        }
    }

    private void SaveBags(SaveData data)
    {
        //가방의 번호는 번부터
        for (int i = 0; i < InventoryScript.instance.MyBags.Count; i++)
        {
            //슬롯이 몇개짜리 가방인지, 몇번째 위치에 있는지
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.instance.MyBags[i].MySlotCount, InventoryScript.instance.MyBags[i].MyBagButton.BagIndex));
        }
    }

    private void SaveEquipment(SaveData data)
    {
        //장비창의 갯수만큼 반복문
        foreach (CharButton charButton in equipment)
        {
            //현재 플레이어가 장착한 장비만큼 저장
            if (charButton.MyEquippedArmor != null)
            {       
                //장비 이름과 어떤장비창인지
                data.MyEquipmentData.Add(new EquipmentData(charButton.MyEquippedArmor.MyTitle, charButton.name));
            }
        }
    }

    private void SaveActionButtons(SaveData data)
    {
        //액션바 길이만큼
        for (int i = 0; i < actionButtons.Length; i++)
        {
            //액션바에 무언가 존재하면
            if (actionButtons[i].MyUseable != null)
            {
                ActionButtonData action;

                //이름과 아이템인지 아닌지 확인하는 bool타입, 액션바 번호 저장
                if (actionButtons[i].MyUseable is Spell)
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Spell).MyName, false, i);
                }
                else
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Item).MyTitle, true, i);
                }
                data.MyActionButtonData.Add(action);
            }
        }
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.instance.GetAllItems();

        foreach (SlotScript slot in slots)
        {
            //슬롯에 있는 아이템의 이름, 갯수, 슬롯 번호, 몇번째 가방인지 저장함
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    private void SaveQuests(SaveData data)
    {
        foreach (Quest quest in Questlog.instance.MyQuests)
        {
            //퀘스트의 이름, 설명, 아이템 요구, 몹죽이기 요구, 퀘스트 발행 npc 저장
            data.MyQuestData.Add(new QuestData(quest.MyTitle, quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives,quest.MyQuestGiver.MyQuestGiverID));
        }
    }

    private void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiver questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.MyQuestGiverID, questGiver.MyCompltedQuests));
        }

    }


    private void Load(SavedGame savedGame)
    {

            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            //이 경로에서 데이터를 불러들임
            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadEquipment(data);

            LoadBags(data);

            LoadInventory(data);

            LoadPlayer(data);

            LoadChests(data);

            LoadActionButtons(data);

            LoadQuests(data);

            LoadQuestGiver(data);


    }

    private void LoadPlayer(SaveData data)
    {
        Player.instance.MyLevel = data.MyPlayerData.MyLevel;
        Player.instance.UpdateLevel();
        Player.instance.Health.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);
        Player.instance.Mana.Initialize(data.MyPlayerData.MyMana, data.MyPlayerData.MyMaxMana);
        Player.instance.MyXp.Initialize(data.MyPlayerData.MyXp, data.MyPlayerData.MyMaxXP);
        Player.instance.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);
    }

    private void LoadChests(SaveData data)
    {
        foreach (ChestData chest in data.MyChestData)
        {
            Chest c = Array.Find(chests, x => x.name == chest.MyName);

            foreach (ItemData itemData in chest.MyItems)
            {
                //itemData안에 있는 아이템을 찾아 상자에 넣음
                Item item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitel));
                item.MySlot = c.MyBag.Slots.Find(x => x.MyIndex == itemData.MySlotIndex);
                c.MyItems.Add(item);
            }
        }

    }

    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {
            //items[0]는 가방이기 때문
            Bag newBag = (Bag)Instantiate(items[0]);

            //가방 슬롯의 갯수
            newBag.Initialize(bagData.MySlotCount);

            InventoryScript.instance.AddBag(newBag, bagData.MyBagIndex);
        }
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            //이름을 통해 어떠한 장비창인지 찾음
            CharButton cb = Array.Find(equipment, x => x.name == equipmentData.MyType);
            //장비창에 장비의 이름을 찾아 장착
            cb.EquipArmor(Array.Find(items, x => x.MyTitle == equipmentData.MyTitle) as Armor);
        }
    }

    //인벤토리에 저장된 아이템의 이름을 이용하므로 인벤토리를 먼저 불러와야함
    private void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData buttonData in data.MyActionButtonData)
        {
            if (buttonData.IsItem)
            {
                //액션바의 몇번째 버튼에 인벤토리의 아이템에 있는 아이템과 같은 아이템을 불러옴
                actionButtons[buttonData.MyIndex].SetUseable(InventoryScript.instance.GetUseables(buttonData.MyAction));
            }
            else
            {
                //액션바의 몇번째 버튼에 스펠북의 이름과 같은 이름을 불러움
                actionButtons[buttonData.MyIndex].SetUseable(SpellBook.instance.GetSpell(buttonData.MyAction));
            }
        }
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            Item item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitel));

            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                InventoryScript.instance.PlaceInSpecific(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }

    private void LoadQuests(SaveData data)
    {
        //단점 => 다른 씬의 퀘스트를 불러오지 못한다.
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        //데이터에 저장된 퀘스트 만큼 반복
        foreach (QuestData questData in data.MyQuestData)
        {
            //퀘스트 ID로 어떤 NPC가 퀘스트를 발행했는지 찾음
            QuestGiver qg = Array.Find(questGivers, x => x.MyQuestGiverID == questData.MyQuestGiverID);
            //퀘스트 이름으로 어떤 퀘스트인지 찾음
            Quest q = Array.Find(qg.MyQuests, x => x.MyTitle == questData.MyTitle);
            q.MyQuestGiver = qg;
            q.MyKillObjectives = questData.MyKillObjectives;
            Questlog.instance.AcceptQuest(q);
        }
    }

    private void LoadQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiverData questGiverData in data.MyQuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.MyQuestGiverID == questGiverData.MyQuestGiverID);
            questGiver.MyCompltedQuests = questGiverData.MyCompletedQuests;
            questGiver.UpdateQuestStatus();
        }
    }
}
