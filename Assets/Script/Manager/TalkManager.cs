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

    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��

    void Start()
    {
        talkCanvas.SetActive(false);
        branchWindow.SetActive(false);
    }

    void Update()
    {
        //�t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
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

        if (lineData.group == "�I����")
        {
            branchWindow.SetActive(true);
            shopWindow.SetActive(false);
            EventSystem.current.SetSelectedGameObject(focusNext[1]);
        }
        else if (lineData.group == "�X")
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

    //�t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
    void FocusCheck()
    {
        //���݂̃t�H�[�J�X���i�[
        currentFocus = EventSystem.current.currentSelectedGameObject;

        //�����O��܂ł̃t�H�[�J�X�Ɠ����Ȃ瑦�I��
        if (currentFocus == previousFocus) return;

        //�����t�H�[�J�X���O��Ă�����O�t���[���̃t�H�[�J�X�ɖ߂�
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        //�c���ꂽ��������A�t�H�[�J�X�����݂���̂͊m��
        //�O�t���[���̃t�H�[�J�X���X�V
        previousFocus = EventSystem.current.currentSelectedGameObject;
    }
}