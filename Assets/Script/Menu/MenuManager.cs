using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.EventSystems;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject[] menuCanvas;
    public GameObject[] menuFocus_C;
    public GameObject[] menuWindow;
    public GameObject[] menuFocus_W;

    public bool menuFLG = false;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timeText;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム

    //window、フォーカスを移動する処理用
    List<GameObject> beforeFocusList = new List<GameObject>();
    List<GameObject> currentWindow = new List<GameObject>();
    List<string> root = new List<string>();
    bool stayMenuFLG = false;

    private void Awake()
    {
        GTextUpdate(PartyManager.Instance.GetPartyMoney());
    }

    void Start()
    {
        CanvasDelete();
        WindowAllClose();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !menuFLG && GameManager.Instance.playerMove)
        {
            CanvasChenge(0);
        }

        if (Input.GetButtonDown("Cancel") && menuFLG)
        {
            if (stayMenuFLG)
            {
                CanvasDelete();
                StartCoroutine(this.DelayMethod(0.1f, MenuFLGChenge, false));
            }
            else if(root.Count > 0)
            {
                Cancel_inWindow();
            }
            else
            {
                BeforeCanvasChenge();
            }
        }

        //フォーカスが外れていないかチェック
        FocusCheck();
    }

    void CanvasDelete()
    {
        for (int i = 0; i < menuCanvas.Length; i++)
        {
            menuCanvas[i].SetActive(false);
        }
    }

    void WindowAllClose()
    {
        for (int i = 0; i < menuWindow.Length; i++)
        {
            menuWindow[i].SetActive(false);
        }
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

    public void CanvasChenge(int no)
    {
        if (menuFLG)
        {
            beforeFocusList.Add(currentFocus);
        }

        //CanvasDelete();
        menuCanvas[no].SetActive(true);
        EventSystem.current.SetSelectedGameObject(menuFocus_C[no]);

        if (no == 0) stayMenuFLG = true;
        else stayMenuFLG = false;

        if (!menuFLG)
        {
            StartCoroutine(this.DelayMethod(0.1f, MenuFLGChenge, true));
        }
    }

    public void Cancel_inWindow()
    {
        int lastNo = root.Count - 1;
        switch (root[lastNo])
        {
            case "Window":
                currentWindow[currentWindow.Count - 1].SetActive(false);
                EventSystem.current.SetSelectedGameObject(beforeFocusList[beforeFocusList.Count - 1]);
                currentWindow.RemoveAt(currentWindow.Count - 1);
                beforeFocusList.RemoveAt(beforeFocusList.Count - 1);
                break;

            case "Focus":
                EventSystem.current.SetSelectedGameObject(beforeFocusList[beforeFocusList.Count - 1]);
                beforeFocusList.RemoveAt(beforeFocusList.Count - 1);
                break;

            default:
                BeforeCanvasChenge();
                break;
        }
        root.RemoveAt(lastNo);
    }

    public void BeforeCanvasChenge()
    {
        CanvasDelete();
        menuCanvas[0].SetActive(true);
        stayMenuFLG = true;

        EventSystem.current.SetSelectedGameObject(beforeFocusList[beforeFocusList.Count - 1]);
        beforeFocusList.RemoveAt(beforeFocusList.Count - 1);
    }

    public void FocusMove(GameObject focus)
    {
        beforeFocusList.Add(currentFocus);
        EventSystem.current.SetSelectedGameObject(focus);

        root.Add("Focus");
    }

    public void WindowOpen(int i)
    {
        menuWindow[i].SetActive(true);
        currentWindow.Add(menuWindow[i]);

        beforeFocusList.Add(currentFocus);
        EventSystem.current.SetSelectedGameObject(menuFocus_W[i]);

        root.Add("Window");
    }

    void MenuFLGChenge(bool flg)
    {
        menuFLG = flg;
        GameManager.Instance.playerMove = !flg;
    }

    public void GTextUpdate(int G)
    {
        goldText.text = G.ToString() + "G";
    }

    public void TimeTextUpdate(float time)
    {
        int seconds = (int)time;
        int minutes = seconds / 60;
        int hours = minutes / 60;
        seconds = seconds % 60;
        minutes = minutes % 60;

        string hoursText = "";
        if (hours >= 10) hoursText = hours.ToString();
        else hoursText = "0" + hours.ToString();

        string minutesText = "";
        if (minutes >= 10) minutesText = minutes.ToString();
        else minutesText = "0" + minutes.ToString();

        string secondsText = "";
        if (seconds >= 10) secondsText = seconds.ToString();
        else secondsText = "0" + seconds.ToString();

        timeText.text = hoursText + ":" + minutesText + ":" + secondsText;
    }
}
