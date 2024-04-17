using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour
{
    GameObject itemMenuWindow;
    GameObject[] itemButton;
    TextMeshProUGUI[] itemNameText;
    TextMeshProUGUI[] itemNoText;
    string[] itemName;
    public int[] itemID;
    bool buttonSetFLG = false;

    int itemSelect = -1;

    public TextMeshProUGUI effectText;
    public TextMeshProUGUI descrideText;

    void Awake()
    {
        itemMenuWindow = GameObject.Find("ItemListWindow");

        // 親オブジェクトの子オブジェクトをすべて取得
        int childCount = itemMenuWindow.transform.childCount;
        GameObject[] childObjects = new GameObject[childCount];
        GameObject[] childButton = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childObjects[i] = itemMenuWindow.transform.GetChild(i).gameObject;
            childButton[i] = childObjects[i].transform.Find("Button").gameObject;
        }
        itemButton = childButton;

        TextMeshProUGUI[] childNameTexts = new TextMeshProUGUI[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childNameTexts[i] = itemButton[i].transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>();
        }
        itemNameText = childNameTexts;

        TextMeshProUGUI[] childNoTexts = new TextMeshProUGUI[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childNoTexts[i] = itemButton[i].transform.Find("ItemNoText").GetComponent<TextMeshProUGUI>();
        }
        itemNoText = childNoTexts;
    }

    void Start()
    {
    }

    void Update()
    {
        if (itemMenuWindow.transform.parent.gameObject.activeSelf && !buttonSetFLG)
        {
            ButtonReset();
            ButtonSet();

            buttonSetFLG = true;
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
        int haveItemNo = 0;
        itemName = new string[0];
        int[] itemNo = new int[0];
        itemID = new int[0];

        haveItemNo = PartyManager.Instance.GetHaveItemCount();
        itemName = PartyManager.Instance.GetHaveItemNameArray();
        itemNo = PartyManager.Instance.GetHaveItemNoArray();
        itemID = PartyManager.Instance.GetHaveItemIDArray();

        Debug.Log(haveItemNo);

        for (int n = 0; n < haveItemNo; n++)
        {
            itemButton[n].transform.parent.gameObject.SetActive(true);
            itemNameText[n].text = itemName[n];
            itemNoText[n].text = itemNo[n] + "個";
        }
    }

    public void ItemSelect(int i)
    {
        itemSelect = i;
    }

    public void ItemUse()
    {
        if (itemSelect < 0)
        {
            return;
        }

        int useItemID = itemID[itemSelect];
        PartyManager.Instance.UseHaveItem(useItemID);
        MenuManager.Instance.Cancel_inWindow();

        ButtonReset();
        ButtonSet();
    }
}
