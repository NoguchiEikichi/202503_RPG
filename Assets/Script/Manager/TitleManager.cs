using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    public GameObject[] dataWindow;
    public GameObject[] windowFocus;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム

    void Start()
    {
        dataWindow[0].SetActive(false);
        dataWindow[1].SetActive(false);
        EventSystem.current.SetSelectedGameObject(windowFocus[0]);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && dataWindow[1].activeSelf)
        {
            dataWindow[1].SetActive(false);
            EventSystem.current.SetSelectedGameObject(windowFocus[1]);
        }
        else if (Input.GetButtonDown("Cancel") && dataWindow[0].activeSelf)
        {
            dataWindow[0].SetActive(false);
            EventSystem.current.SetSelectedGameObject(windowFocus[3]);
        }

        //フォーカスが外れていないかチェック
        FocusCheck();
    }

    public void WindowOpen(int n)
    {
        dataWindow[n].SetActive(true);
        EventSystem.current.SetSelectedGameObject(windowFocus[n + 1]);
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
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
