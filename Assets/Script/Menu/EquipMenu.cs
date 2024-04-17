using System;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.EventSystems;

public class EquipMenu : MonoBehaviour
{
    [System.NonSerialized] public int playerID = 0;

    [System.NonSerialized] public DataValidation._category equipCategory = DataValidation._category.����;
    [System.NonSerialized] public int equipNo = 0;
    [System.NonSerialized] public int selectedNo = -1;

    //�V������������A�C�e����\������E�B���h�E�̐ݒ�
    GameObject equipListWindow;
    GameObject[] itemButton;
    TextMeshProUGUI[] itemNameText;
    TextMeshProUGUI[] itemNoText;
    string[] itemName;
    public int[] itemID;
    bool buttonSetFLG = false;

    //���݂̑����󋵂̕\������E�B���h�E�̐ݒ�
    GameObject equipWindow;
    GameObject[] equipButton;
    TextMeshProUGUI[] equipNameText;
    string[] equipName;
    bool equipSetFLG = false;

    public TextMeshProUGUI effectText;
    public TextMeshProUGUI descrideText;

    void Awake()
    {
        //�V������������A�C�e����\������E�B���h�E�̐ݒ�
        #region
        equipListWindow = GameObject.Find("EquipListWindow");

        // �e�I�u�W�F�N�g�̎q�I�u�W�F�N�g�����ׂĎ擾
        int childCount = equipListWindow.transform.childCount;
        GameObject[] childObjects = new GameObject[childCount];
        GameObject[] childButton = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childObjects[i] = equipListWindow.transform.GetChild(i).gameObject;
            childButton[i] = childObjects[i].transform.Find("Button").gameObject;
        }
        itemButton = childButton;

        TextMeshProUGUI[] childNameTexts = new TextMeshProUGUI[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childNameTexts[i] = itemButton[i].transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        }
        itemNameText = childNameTexts;

        TextMeshProUGUI[] childNoTexts = new TextMeshProUGUI[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childNoTexts[i] = itemButton[i].transform.Find("NoText").GetComponent<TextMeshProUGUI>();
        }
        itemNoText = childNoTexts;
        #endregion

        //���݂̑����󋵂̕\������E�B���h�E�̐ݒ�
        #region
        equipWindow = GameObject.Find("EquipWindow");

        // �e�I�u�W�F�N�g�̎q�I�u�W�F�N�g�����ׂĎ擾
        childCount = equipWindow.transform.childCount;
        childObjects = new GameObject[childCount];
        childButton = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childObjects[i] = equipWindow.transform.GetChild(i).gameObject;
            childButton[i] = childObjects[i].transform.Find("Button").gameObject;
        }
        equipButton = childButton;

        childNameTexts = new TextMeshProUGUI[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childNameTexts[i] = equipButton[i].transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        }
        equipNameText = childNameTexts;
        #endregion
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (equipListWindow.transform.parent.gameObject.activeSelf && !buttonSetFLG)
        {
            ButtonReset();
            ButtonSet();

            buttonSetFLG = true;
        }
        else if(!equipListWindow.transform.parent.gameObject.activeSelf)
        {
            buttonSetFLG = false;
            selectedNo = -1;
        }

        if (equipWindow.transform.parent.gameObject.activeSelf && !equipSetFLG)
        {
            ButtonSet_Equip();
            equipSetFLG = true;
        }
        else if (!equipWindow.transform.parent.gameObject.activeSelf)
        {
            equipSetFLG = false;
        }
    }

    void ButtonReset()
    {
        for (int n = 0; n < itemButton.Length; n++)
        {
            itemButton[n].transform.parent.gameObject.SetActive(false);
        }
    }

    void ButtonSet()
    {
        switch (equipCategory)
        {
            case DataValidation._category.����:
                ButtonSet_Weapon();
                break;

            case DataValidation._category.����:
                ButtonSet_Accessory();
                break;

            case DataValidation._category.���@:
                ButtonSet_Magic();
                break;

            default:
                break;
        }
    }

    void ButtonSet_Weapon()
    {
        int haveItemNo = 0;
        itemName = new string[0];
        int[] itemNo = new int[0];
        itemID = new int[0];

        haveItemNo = PartyManager.Instance.GetHaveWeaponCount() + 1;
        itemName = PartyManager.Instance.GetHaveWeaponNameArray();
        itemNo = PartyManager.Instance.GetHaveWeaponNoArray();
        itemID = PartyManager.Instance.GetHaveWeaponIDArray();

        itemButton[0].transform.parent.gameObject.SetActive(true);

        for (int n = 1; n < haveItemNo; n++)
        {
            itemButton[n].transform.parent.gameObject.SetActive(true);
            itemNameText[n].text = itemName[n - 1];
            itemNoText[n].text = itemNo[n - 1] + "��";
        }
    }

    void ButtonSet_Accessory()
    {
        int haveItemNo = 0;
        itemName = new string[0];
        int[] itemNo = new int[0];
        itemID = new int[0];

        haveItemNo = PartyManager.Instance.GetHaveAccessoryCount() + 1;
        itemName = PartyManager.Instance.GetHaveAccessoryNameArray();
        itemNo = PartyManager.Instance.GetHaveAccessoryNoArray();
        itemID = PartyManager.Instance.GetHaveAccessoryIDArray();

        itemButton[0].transform.parent.gameObject.SetActive(true);

        for (int n = 1; n < haveItemNo; n++)
        {
            itemButton[n].transform.parent.gameObject.SetActive(true);
            itemNameText[n].text = itemName[n - 1];
            itemNoText[n].text = itemNo[n - 1] + "��";
        }
    }

    void ButtonSet_Magic()
    {
        int haveItemNo = 0;
        itemName = new string[0];
        int[] itemNo = new int[0];
        itemID = new int[0];

        haveItemNo = PartyManager.Instance.GetHaveMagicCount() + 1;
        itemName = PartyManager.Instance.GetHaveMagicNameArray();
        itemNo = PartyManager.Instance.GetHaveMagicNoArray();
        itemID = PartyManager.Instance.GetHaveMagicIDArray();

        itemButton[0].transform.parent.gameObject.SetActive(true);

        for (int n = 1; n < haveItemNo; n++)
        {
            itemButton[n].transform.parent.gameObject.SetActive(true);
            itemNameText[n].text = itemName[n - 1];
            itemNoText[n].text = itemNo[n - 1] + "��";
        }
    }

    void ButtonSet_Equip()
    {
        //�ϐ��̐錾
        #region
        equipName = new string[12];
        int equipWeaponID = PartyManager.Instance.GetPartyCharactorWeapon(playerID);
        string equipWeaponName = "";
        int[] equipAccessoryID = PartyManager.Instance.GetPartyCharactorAccessoryArray(playerID);
        string[] equipAccessoryName = new string[3];
        int[] equipMagicID = PartyManager.Instance.GetPartyCharactorMagicArray(playerID);
        string[] equipMagicName = new string[8];
        #endregion

        //�����A�C�e���̖��O�̎擾
        #region
        if (equipWeaponID >= 0) equipWeaponName = DatabaseManager.Instance.GetWeaponName(equipWeaponID);
        else equipWeaponName = "����";

        for (int n = 0; n < equipAccessoryID.Length; n++)
        {
            if (equipAccessoryID[n] >= 0) 
                equipAccessoryName[n] = DatabaseManager.Instance.GetAccessoryName(equipAccessoryID[n]);
            else equipAccessoryName[n] = "����";
        }

        for (int n = 0; n < equipMagicID.Length; n++)
        {
            if (equipMagicID[n] >= 0)
                equipMagicName[n] = DatabaseManager.Instance.GetMagicName(equipMagicID[n]);
            else equipMagicName[n] = "����";
        }
        #endregion

        equipName[0] = equipWeaponName;
        Array.Copy(equipAccessoryName, 0, equipName, 1, equipAccessoryName.Length);
        Array.Copy(equipMagicName, 0, equipName, equipAccessoryName.Length + 1, equipMagicName.Length);

        for (int n = 0; n < 12; n++)
        {
            equipNameText[n].text = equipName[n];
        }
    }

    public void PlayerIDSetting(int id)
    {
        int playerMax = PartyManager.Instance.GetPartyMemberCount();
        if(id >= 0 && id < playerMax) playerID = id;
    }

    public void ChoseEquip(int no)
    {
        if (no == 0)
        {
            equipCategory = DataValidation._category.����;
            equipNo = no;
        }
        else if(no >= 1 && no <= 3)
        {
            equipCategory = DataValidation._category.����;
            equipNo = no - 1;
        }
        else
        {
            equipCategory = DataValidation._category.���@;
            equipNo = no - 4;
        }
    }

    public void SelectedEquip(int no)
    {
        selectedNo = no;
    }

    public void DecidedEquip()
    {
        equipSetFLG = false;
        buttonSetFLG = false;

        int id = 0;
        switch (equipCategory)
        {
            case DataValidation._category.����:
                if (selectedNo == 99)
                {
                    PartyManager.Instance.RemoveWeapon(playerID);
                    break;
                }
                id = PartyManager.Instance.GetHaveWeaponID(selectedNo);
                PartyManager.Instance.EquipWeapon(playerID, id);
                break;

            case DataValidation._category.����:
                if (selectedNo == 99)
                {
                    PartyManager.Instance.RemoveAccessory(playerID, equipNo);
                    break;
                }
                id = PartyManager.Instance.GetHaveAccessoryID(selectedNo);
                PartyManager.Instance.EquipAccessory(playerID, id, equipNo);
                break;

            case DataValidation._category.���@:
                if (selectedNo == 99) 
                { 
                    PartyManager.Instance.RemoveMagic(playerID, equipNo);
                    break;
                }
                id = PartyManager.Instance.GetHaveMagicID(selectedNo);
                PartyManager.Instance.EquipMagic(playerID, id, equipNo);
                break;

            default:
                break;
        }
    }
}
