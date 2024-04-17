using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    public GameObject[] dataWindow;
    public GameObject[] windowFocus;

    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��

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

        //�t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
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
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
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
