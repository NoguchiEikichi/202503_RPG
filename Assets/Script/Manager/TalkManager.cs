using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TalkManager : Singleton<TalkManager>
{
    //UI
    public GameObject talkCanvas;
    public TextMeshProUGUI nameSpace;
    public TextMeshProUGUI textSpace;
    [SerializeField] GameObject[] focusNext;
    public GameObject branchWindow;
    public GameObject shopWindow;

    public bool onTalkFLG;

    LineFixData lineData;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム

    void Start()
    {
        talkCanvas.SetActive(false);
        branchWindow.SetActive(false);
    }

    void Update()
    {
        //フォーカスが外れていないかチェック
        FocusCheck();
    }

    public void TalkText(int no)
    {
        if (no == 625)
        {

        }

        lineData = DatabaseManager.Instance.GetLineData(no);
        nameSpace.text = lineData.name;
        if (lineData.line.Contains("<>"))
        {
            lineData.line = lineData.line.Replace("<>", "\n");
        }
        textSpace.text = lineData.line;

        if (!talkCanvas.activeSelf)
        {
            talkCanvas.SetActive(true);
            OnTalkFLGChenge(true);
        }

        if (lineData.group == "選択肢")
        {
            branchWindow.SetActive(true);
            shopWindow.SetActive(false);
            EventSystem.current.SetSelectedGameObject(focusNext[1]);
        }
        else if (lineData.group == "店")
        {
            branchWindow.SetActive(false);
            shopWindow.SetActive(true);
            EventSystem.current.SetSelectedGameObject(focusNext[2]);
        }
        else
        {
            branchWindow.SetActive(false);
            shopWindow.SetActive(false);
            EventSystem.current.SetSelectedGameObject(focusNext[0]);
        }
    }

    public void NextText()
    {
        if (lineData.idNext_True == -1)
        {
            talkCanvas.SetActive(false);

            StartCoroutine(this.DelayMethod(0.1f, OnTalkFLGChenge, false));

            return;
        }

        TalkText(lineData.idNext_True);
    }

    public void NextText_Branch(bool flg)
    {
        if (flg)
        {
            TalkText(lineData.idNext_True);
        }
        else
        {
            TalkText(lineData.idNext_False);
        }
    }

    void OnTalkFLGChenge(bool flg)
    {
        onTalkFLG = flg;
        GameManager.Instance.playerMove = !flg;
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
}