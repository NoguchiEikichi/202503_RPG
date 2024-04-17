using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;

public class SaveDataManager : Singleton<SaveDataManager>
{
    //セーブデータ
    SaveDataType saveData;

    //セーブ設定
    QuickSaveSettings m_saveSettings;

    public void Start()
    {
        // QuickSaveSettingsのインスタンスを作成
        m_saveSettings = new QuickSaveSettings();
        // 暗号化の方法 
        m_saveSettings.SecurityMode = SecurityMode.Aes;
        // Aesの暗号化キー
        m_saveSettings.Password = "Password";
        // 圧縮の方法
        m_saveSettings.CompressionMode = CompressionMode.Gzip;
    }

    /// <summary>
    /// セーブデータ読み込み
    /// </summary>
    public void LoadUserData(int id)
    {
        CheckUserData(id);

        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("SaveData" + id, m_saveSettings);

        // データを読み込む
        saveData = reader.Read<SaveDataType>("SaveData");

        GameManager.Instance.LoadPosData(saveData.playerPos, saveData.playerRot);
        PartyManager.Instance.LoadPartyData(saveData.partyData);
    }

    public bool CheckUserData(int id)
    {
        //ファイルが無ければ無視
        if (FileAccess.Exists("SaveData" + id, false) == false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// データセーブ
    /// </summary>
    public void SaveUserData(int id, SaveDataType data)
    {
        Debug.Log("セーブデータ保存先:" + Application.persistentDataPath);

        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData" + id, m_saveSettings);

        // データを書き込む
        writer.Write("SaveData", data);

        // 変更を反映
        writer.Commit();
    }
}