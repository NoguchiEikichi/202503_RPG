using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;

public class SaveDataManager : Singleton<SaveDataManager>
{
    //�Z�[�u�f�[�^
    SaveDataType saveData;

    //�Z�[�u�ݒ�
    QuickSaveSettings m_saveSettings;

    public void Start()
    {
        // QuickSaveSettings�̃C���X�^���X���쐬
        m_saveSettings = new QuickSaveSettings();
        // �Í����̕��@ 
        m_saveSettings.SecurityMode = SecurityMode.Aes;
        // Aes�̈Í����L�[
        m_saveSettings.Password = "Password";
        // ���k�̕��@
        m_saveSettings.CompressionMode = CompressionMode.Gzip;
    }

    /// <summary>
    /// �Z�[�u�f�[�^�ǂݍ���
    /// </summary>
    public void LoadUserData(int id)
    {
        CheckUserData(id);

        // QuickSaveReader�̃C���X�^���X���쐬
        QuickSaveReader reader = QuickSaveReader.Create("SaveData" + id, m_saveSettings);

        // �f�[�^��ǂݍ���
        saveData = reader.Read<SaveDataType>("SaveData");

        GameManager.Instance.LoadPosData(saveData.playerPos, saveData.playerRot);
        PartyManager.Instance.LoadPartyData(saveData.partyData);
    }

    public bool CheckUserData(int id)
    {
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData" + id, false) == false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData(int id, SaveDataType data)
    {
        Debug.Log("�Z�[�u�f�[�^�ۑ���:" + Application.persistentDataPath);

        // QuickSaveWriter�̃C���X�^���X���쐬
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData" + id, m_saveSettings);

        // �f�[�^����������
        writer.Write("SaveData", data);

        // �ύX�𔽉f
        writer.Commit();
    }
}