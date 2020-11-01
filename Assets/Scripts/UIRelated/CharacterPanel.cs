using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    public static CharacterPanel instance;

    [SerializeField] private CharButton helmet, shoulders, chest, gloves, pants, boots, main, off;

    public CharButton MyCharButton { get; set;  }

    private CanvasGroup canvasGroup;

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

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }

        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void EquipArmor(Armor armor)
    {
        switch (armor.ArmorType)
        {
            case ArmorType.Helmet:
                helmet.EquipArmor(armor);
                break;
            case ArmorType.Shoulders:
                shoulders.EquipArmor(armor);
                break;
            case ArmorType.Chest:
                chest.EquipArmor(armor);
                break;
            case ArmorType.Gloves:
                gloves.EquipArmor(armor);
                break;
            case ArmorType.Pants:
                pants.EquipArmor(armor);
                break;
            case ArmorType.Boots:
                boots.EquipArmor(armor);
                break;
            case ArmorType.MainHand:
                main.EquipArmor(armor);
                break;
            case ArmorType.Offhand:
                off.EquipArmor(armor);
                break;
        }
    }
}