using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    int id = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SelectData(int no)
    {
        id = no;
    }

    public void SaveCommand()
    {
        SaveDataType saveData = new SaveDataType();

        saveData.playerPos = GameManager.Instance.GetPlayerPos();
        saveData.playerRot = GameManager.Instance.GetPlayerRotation();
        saveData.partyData = PartyManager.Instance.GetPartyData();

        SaveDataManager.Instance.SaveUserData(id, saveData);
    }
}
