using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CommandManager : MonoBehaviour
{
    public List<GameObject> defaultFocus = new List<GameObject>();

    public List<GameObject> commandWindow = new List<GameObject>();

    public GameObject buttonPrefab;

    int partyMenberCount = 0;
    int commandCount = 0;
    int[] activeMenber;
    bool isCommand = false;

    string currentCommand = "";
    string targetOR = "";

    int magicID = -1;
    int itemID = -1;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム

    //window、フォーカスを移動する処理用
    List<GameObject> beforeFocusList = new List<GameObject>();
    List<GameObject> currentWindow = new List<GameObject>();
    List<string> root = new List<string>();
    public bool minusFLG = false;

    void Start()
    {
        WindowDelete();

        partyMenberCount = PartyManager.Instance.GetPartyMemberCount();
        commandCount = 0;
    }

    void Update()
    {
        if (!isCommand && !BattleManager.Instance.isTurnFLG)
        {
            WindowOpen(0);
            isCommand = true;
            partyMenberCount = PartyManager.Instance.GetActiveMemberCount();
            activeMenber = PartyManager.Instance.GetActiveMemberList();
            commandCount = 0;

            MagicButtonDelete();
            MagicButtonSet();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (root.Count > 1)
            {
                Cancel_inWindow();
            }
        }

        //フォーカスが外れていないかチェック
        FocusCheck();
    }

    //フォーカスが外れていないかチェック
    void FocusCheck()
    {
        //現在のフォーカスを格納
        currentFocus = EventSystem.current.currentSelectedGameObject;

        //もし前回までのフォーカスと同じなら即終了
        if (currentFocus == previousFocus) return;

        //もしフォーカスが外れていたら前フレームのフォーカスに戻す
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        //残された条件から、フォーカスが存在するのは確定
        //前フレームのフォーカスを更新
        previousFocus = EventSystem.current.currentSelectedGameObject;
    }

    public void WindowDelete()
    {
        for (int i = 0; i < commandWindow.Count; i++)
        {
            commandWindow[i].SetActive(false);
        }
        root.Clear();
    }

    public void Cancel_inWindow()
    {
        if (minusFLG && commandCount > 0)
        {
            commandCount--;
            BattleManager.Instance.CommandCansel();
        }

        int lastNo = root.Count - 1;
        switch (root[lastNo])
        {
            case "Window":
                currentWindow[currentWindow.Count - 1].SetActive(false);
                currentWindow[currentWindow.Count - 2].SetActive(true);
                EventSystem.current.SetSelectedGameObject(beforeFocusList[beforeFocusList.Count - 1]);
                currentWindow.RemoveAt(currentWindow.Count - 1);
                beforeFocusList.RemoveAt(beforeFocusList.Count - 1);

                if (currentWindow[currentWindow.Count - 1] == commandWindow[2]) minusFLG = true;
                else if (currentWindow[currentWindow.Count - 1] == commandWindow[3]) minusFLG = true;
                break;

            case "Focus":
                EventSystem.current.SetSelectedGameObject(beforeFocusList[beforeFocusList.Count - 1]);
                beforeFocusList.RemoveAt(beforeFocusList.Count - 1);
                break;

            default:
                break;
        }
        root.RemoveAt(lastNo);
    }

    public void FocusMove(GameObject focus)
    {
        beforeFocusList.Add(currentFocus);
        EventSystem.current.SetSelectedGameObject(focus);

        root.Add("Focus");
    }

    public void WindowOpen(int i)
    {
        commandWindow[i].SetActive(true);
        currentWindow.Add(commandWindow[i]);

        beforeFocusList.Add(currentFocus);
        EventSystem.current.SetSelectedGameObject(defaultFocus[i]);

        root.Add("Window");
        if (i == 2 || i == 3)
        {
            minusFLG = true;
        }
        else
        {
            minusFLG = false;
        }
    }

    public void MagicButtonDelete()
    {
        int no = 4;

        for (int n = 0; n < 3; n++)
        {
            GameObject child = commandWindow[no + n].transform.Find("ListWindow").gameObject;
            for (int i = 0; i < child.transform.childCount; i++)
            {
                child.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void MagicButtonSet()
    {
        int no = 4;

        for (int n = 0; n < 3; n++)
        {
            GameObject child = commandWindow[no + n].transform.Find("ListWindow").gameObject;

            int buttonCount = PartyManager.Instance.GetPartyCharactorMagicNo(n);

            for (int i = 0; i < buttonCount; i++)
            {
                child.transform.GetChild(i).gameObject.SetActive(true);
                GameObject button = child.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject;
                int magicID = PartyManager.Instance.GetPartyCharactorMagic(n, i);
                DataValidation._element element = DatabaseManager.Instance.GetMagicElement(magicID);

                //名前
                #region
                TextMeshProUGUI nameText = button.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
                nameText.text = DatabaseManager.Instance.GetMagicName(magicID);
                #endregion

                //属性に関する記述
                #region
                TextMeshProUGUI elementText = button.transform.Find("ElementText").GetComponent<TextMeshProUGUI>();
                elementText.text = element.ToString() + "術";
                Image elementImage = button.transform.Find("Image").GetComponent<Image>();
                string colorString = "";
                Color newColor;
                switch (element)
                {
                    case DataValidation._element.火:
                        colorString = "#fa8072"; // 赤色の16進数文字列
                        ColorUtility.TryParseHtmlString(colorString, out newColor); // 新しくColorを作成
                        elementText.color = newColor;
                        elementImage.sprite = Resources.Load<Sprite>("Sprite/Icons/Icons/Fire");
                        elementImage.color = newColor;
                        break;

                    case DataValidation._element.水:
                        colorString = "#87ceeb"; // 水色の16進数文字列
                        ColorUtility.TryParseHtmlString(colorString, out newColor); // 新しくColorを作成
                        elementText.color = newColor;
                        elementImage.sprite = Resources.Load<Sprite>("Sprite/Icons/Icons/Water");
                        elementImage.color = newColor;
                        break;

                    case DataValidation._element.風:
                        colorString = "#90ee90"; // 黄緑色の16進数文字列
                        ColorUtility.TryParseHtmlString(colorString, out newColor); // 新しくColorを作成
                        elementText.color = newColor;
                        elementImage.sprite = Resources.Load<Sprite>("Sprite/Icons/Icons/Wind");
                        elementImage.color = newColor;
                        break;

                    case DataValidation._element.土:
                        colorString = "#f0e68c"; // 黄色の16進数文字列
                        ColorUtility.TryParseHtmlString(colorString, out newColor); // 新しくColorを作成
                        elementText.color = newColor;
                        elementImage.sprite = Resources.Load<Sprite>("Sprite/Icons/Icons/Rock");
                        elementImage.color = newColor;
                        break;

                    default:
                        elementText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        elementImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        break;
                }
                #endregion

                //消費MP、SPに関する記述
                #region
                TextMeshProUGUI pointText = button.transform.Find("UsePointText").GetComponent<TextMeshProUGUI>();
                pointText.text = "消費\n" + DatabaseManager.Instance.GetMagicUsePoint(magicID);
                TextMeshProUGUI pointNumber = button.transform.Find("UsePointNum").GetComponent<TextMeshProUGUI>();
                pointNumber.text = DatabaseManager.Instance.GetMagicUsePointNum(magicID).ToString();
                #endregion
            }
        }
    }

    public void Command_Battle()
    {
        commandWindow[0].SetActive(false);
        WindowOpen(activeMenber[0] + 1);
    }

    public void Command_Escape()
    {
        commandWindow[0].SetActive(false);
        BattleManager.Instance.Escape();
    }

    public void Command_Attack()
    {
        if (BattleManager.Instance.enemyLists.Count <= 1)
        {
            BattleManager.Instance.AttackSet(0);
            commandCount++;
            FinishJudge();
        }
        else
        {
            currentCommand = "Attack";
            commandWindow[activeMenber[commandCount+1] + 1].SetActive(false);
            WindowOpen(commandWindow.Count-1);
        }
    }

    public void Command_Magic()
    {
        commandWindow[activeMenber[commandCount] + 1].SetActive(false);
        WindowOpen(activeMenber[commandCount] + 4);
    }

    public void MagicSelect(int no)
    {
        magicID = PartyManager.Instance.GetPartyCharactorMagic(commandCount, no);
        string usePoint = DatabaseManager.Instance.GetMagicUsePoint(magicID);

        if (usePoint == "MP")
        {
            if (DatabaseManager.Instance.GetMagicUsePointNum(magicID) <= PartyManager.Instance.GetPartyCharactorMP(commandCount))
            {
                DataValidation._target target = DatabaseManager.Instance.GetMagicTarget(magicID);

                switch (target)
                {
                    case DataValidation._target.味方単体:
                        #region
                        currentCommand = "Magic";
                        targetOR = "P";
                        commandWindow[activeMenber[commandCount] + 1].SetActive(false);
                        commandWindow[activeMenber[commandCount] + 4].SetActive(false);
                        WindowOpen(commandWindow.Count - 2);
                        break;
                    #endregion

                    case DataValidation._target.敵単体:
                        #region
                        if (BattleManager.Instance.enemyLists.Count <= 1)
                        {
                            BattleManager.Instance.MagicSet(magicID, "E0");
                            commandCount++;
                            FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        }
                        else
                        {
                            currentCommand = "Magic";
                            targetOR = "E";
                            commandWindow[activeMenber[commandCount] + 1].SetActive(false);
                            commandWindow[activeMenber[commandCount] + 4].SetActive(false);
                            WindowOpen(commandWindow.Count - 1);
                        }
                        break;
                    #endregion

                    case DataValidation._target.敵ランダム:
                        #region
                        BattleManager.Instance.MagicSet(magicID, "E25");
                        commandCount++;
                        FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        break;
                    #endregion

                    case DataValidation._target.敵全体:
                        #region
                        BattleManager.Instance.MagicSet(magicID, "E99");
                        commandCount++;
                        FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        break;
                    #endregion

                    case DataValidation._target.味方全体:
                        #region
                        BattleManager.Instance.MagicSet(magicID, "P99");
                        commandCount++;
                        FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        break;
                    #endregion

                    default:
                        break;
                }
            }
        }
        else if (usePoint == "SP")
        {

            if (DatabaseManager.Instance.GetMagicUsePointNum(magicID) <= PartyManager.Instance.GetPartyCharactorSP(commandCount))
            {
                DataValidation._target target = DatabaseManager.Instance.GetMagicTarget(magicID);

                switch (target)
                {
                    case DataValidation._target.味方単体:
                        #region
                        if (activeMenber.Length <= 1)
                        {
                            BattleManager.Instance.MagicSet(magicID, "P0");
                            commandCount++;
                            FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        }
                        else
                        {
                            currentCommand = "Magic";
                            targetOR = "P";
                            commandWindow[activeMenber[commandCount] + 1].SetActive(false);
                            commandWindow[activeMenber[commandCount] + 4].SetActive(false);
                            WindowOpen(commandWindow.Count - 2);
                        }
                        break;
                    #endregion

                    case DataValidation._target.敵単体:
                        #region
                        if (BattleManager.Instance.enemyLists.Count <= 1)
                        {
                            BattleManager.Instance.MagicSet(magicID, "E0");
                            commandCount++;
                            FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        }
                        else
                        {
                            currentCommand = "Magic";
                            targetOR = "E";
                            commandWindow[activeMenber[commandCount] + 1].SetActive(false);
                            commandWindow[activeMenber[commandCount] + 4].SetActive(false);
                            WindowOpen(commandWindow.Count - 1);
                        }
                        break;
                    #endregion

                    case DataValidation._target.敵ランダム:
                        #region
                        BattleManager.Instance.MagicSet(magicID, "E25");
                        commandCount++;
                        FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        break;
                    #endregion

                    case DataValidation._target.敵全体:
                        #region
                        BattleManager.Instance.MagicSet(magicID, "E99");
                        commandCount++;
                        FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        break;
                    #endregion

                    case DataValidation._target.味方全体:
                        #region
                        BattleManager.Instance.MagicSet(magicID, "P99");
                        commandCount++;
                        FinishJudge(activeMenber[commandCount - 1] + 1, activeMenber[commandCount - 1] + 4);
                        break;
                    #endregion

                    default:
                        break;
                }
            }
        }
    }

    public void Command_Item()
    {
        commandWindow[activeMenber[commandCount] + 1].SetActive(false);
        WindowOpen(commandWindow.Count -3);
    }

    public void ItemSelect(int no)
    {
        itemID = PartyManager.Instance.GetHaveItemID(no);
        DataValidation._target target = DatabaseManager.Instance.GetItemTarget(itemID);

        switch (target)
        {
            case DataValidation._target.味方単体:
                #region
                currentCommand = "Item";
                targetOR = "P";
                commandWindow[activeMenber[commandCount] + 1].SetActive(false);
                WindowOpen(commandWindow.Count - 2);
                break;
            #endregion

            case DataValidation._target.敵単体:
                #region
                if (BattleManager.Instance.enemyLists.Count <= 1)
                {
                    BattleManager.Instance.ItemSet(itemID, "E0");

                    FinishJudge();
                }
                else
                {
                    currentCommand = "Item";
                    targetOR = "E";
                    commandWindow[activeMenber[commandCount] + 1].SetActive(false);
                    WindowOpen(commandWindow.Count - 1);
                }
                break;
            #endregion

            case DataValidation._target.味方全体:
                # region
                BattleManager.Instance.ItemSet(itemID, "P99");
                commandCount++;
                FinishJudge();
                break;
                #endregion

            case DataValidation._target.敵全体:
                # region
                BattleManager.Instance.ItemSet(itemID, "E99");
                commandCount++;
                FinishJudge();
                break;
            #endregion

            default:
                break;
        }
    }

    public void Command_Defense()
    {
        BattleManager.Instance.DefenceSet();
        commandCount++;

        FinishJudge();
    }

    public void PlayerSelect(int no)
    {
        switch (currentCommand)
        {
            case "Attack":
                BattleManager.Instance.AttackSet(no);
                break;
                
            case "Magic":
                BattleManager.Instance.MagicSet(magicID,targetOR + no);
                break;
                
            case "Item":
                BattleManager.Instance.ItemSet(itemID, targetOR + no);
                break;

            default:
                break;
        }
        commandCount++;

        FinishJudge(commandWindow.Count - 2);
    }
    
    public void EnemySelect(int no)
    {
        switch (currentCommand)
        {
            case "Attack":
                BattleManager.Instance.AttackSet(no);
                break;
                
            case "Magic":
                BattleManager.Instance.MagicSet(magicID, targetOR + no);
                break;
                
            case "Item":
                BattleManager.Instance.ItemSet(itemID, targetOR + no);
                break;

            default:
                break;
        }
        commandCount++;

        FinishJudge(commandWindow.Count - 1);
    }

    void FinishJudge()
    {
        if (commandCount < partyMenberCount)
        {
            commandWindow[activeMenber[commandCount - 1] + 1].SetActive(false);
            WindowOpen(activeMenber[commandCount] + 1);
        }
        else
        {
            WindowDelete();
            isCommand = false;
            BattleManager.Instance.TurnStart();
        }
    }
    void FinishJudge(int windowNo)
    {
        if (commandCount < partyMenberCount)
        {
            commandWindow[windowNo].SetActive(false);
            WindowOpen(activeMenber[commandCount] + 1);
        }
        else
        {
            WindowDelete();
            isCommand = false;
            BattleManager.Instance.TurnStart();
        }
    }
    void FinishJudge(int windowNo1, int windowNo2)
    {
        if (commandCount < partyMenberCount)
        {
            commandWindow[windowNo1].SetActive(false);
            commandWindow[windowNo2].SetActive(false);
            WindowOpen(activeMenber[commandCount] + 1);
        }
        else
        {
            WindowDelete();
            isCommand = false;
            BattleManager.Instance.TurnStart();
        }
    }
}