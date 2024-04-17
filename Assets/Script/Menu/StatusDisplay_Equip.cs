using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusDisplay_Equip : MonoBehaviour
{
    public bool nextFLG = false;
    public DataValidation._status status;
    public DataValidation._element element = DataValidation._element.ñ≥;

    int playerID;
    int baseStatus;
    int currentStatus;
    int newStatus;
    int selectNo = -1;

    TextMeshProUGUI text;

    EquipMenu equipMenuManager;

    void Start()
    {
        equipMenuManager = GameObject.Find("EquipMenu").GetComponent<EquipMenu>();
        text = this.gameObject.GetComponent<TextMeshProUGUI>();
        playerID = equipMenuManager.playerID;
        if (status != DataValidation._status.name)
        {
            GetNewStatus();
            TextUpdate();
        }
        else if (status == DataValidation._status.name)
        {
            string name = DatabaseManager.Instance.GetPlayerName(playerID);
            text.text = name;
        }
    }

    void Update()
    {
        if (playerID != equipMenuManager.playerID)
        {
            playerID = equipMenuManager.playerID;
            if (status != DataValidation._status.name)
            {
                GetNewStatus();
                TextUpdate();
            }
            else if (status == DataValidation._status.name)
            {
                string name = DatabaseManager.Instance.GetPlayerName(playerID);
                text.text = name;
            }
        }

        if (status != DataValidation._status.name)
        {
            if (nextFLG)
            {
                if (selectNo != equipMenuManager.selectedNo)
                {
                    selectNo = equipMenuManager.selectedNo;
                    if (selectNo >= 0)
                    {
                        GetNewStatus_Next();
                    }
                    else if(selectNo == 99)
                    {

                    }
                    else
                    {
                        GetNewStatus();
                    }
                }
            }
            else
            {
                GetNewStatus();
            }

            if (currentStatus != newStatus)
            {
                TextUpdate();
            }
        }
    }

    void GetNewStatus_Next()
    {
        int current;
        int defaultStatus;
        int id = 0;
        int plus = 0;
        if (selectNo != 99)
        {
            id = GetNewStatus_EquipID();
            plus = GetNewStatus_Equip(id);
        }
        if (GetNewStatus_NextFLG(id))
        {
            switch (status)
            {
                case DataValidation._status.MAXHP:
                    current = PartyManager.Instance.GetPartyCharactorMAXHP(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.MAXMP:
                    current = PartyManager.Instance.GetPartyCharactorMAXMP(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.MAXSP:
                    current = PartyManager.Instance.GetPartyCharactorMAXSP(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.MAG:
                    current = PartyManager.Instance.GetPartyCharactorMAG(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.MND:
                    current = PartyManager.Instance.GetPartyCharactorMND(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.AGI:
                    current = PartyManager.Instance.GetPartyCharactorAGI(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.HIT:
                    current = PartyManager.Instance.GetPartyCharactorHIT(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.DEX:
                    current = PartyManager.Instance.GetPartyCharactorDEX(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.CRI:
                    current = PartyManager.Instance.GetPartyCharactorCRI(playerID);
                    defaultStatus = current - GetCurrentStatus_Equip();
                    newStatus = defaultStatus + plus;
                    break;

                case DataValidation._status.ìKê´:
                    switch (element)
                    {
                        case DataValidation._element.âŒ:
                            current = PartyManager.Instance.GetPCAptitude_Fi(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        case DataValidation._element.êÖ:
                            current = PartyManager.Instance.GetPCAptitude_Wa(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        case DataValidation._element.ïó:
                            current = PartyManager.Instance.GetPCAptitude_Wi(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        case DataValidation._element.ìy:
                            current = PartyManager.Instance.GetPCAptitude_Ea(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        default:
                            break;
                    }
                    break;

                case DataValidation._status.ëœê´:
                    switch (element)
                    {
                        case DataValidation._element.âŒ:
                            current = PartyManager.Instance.GetPCResistance_Fi(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        case DataValidation._element.êÖ:
                            current = PartyManager.Instance.GetPCResistance_Wa(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        case DataValidation._element.ïó:
                            current = PartyManager.Instance.GetPCResistance_Wi(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        case DataValidation._element.ìy:
                            current = PartyManager.Instance.GetPCResistance_Ea(playerID);
                            defaultStatus = current - GetCurrentStatus_Equip();
                            newStatus = defaultStatus + plus;
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }
        else
        {
            if (baseStatus != currentStatus) newStatus = baseStatus;
        }
    }

    int GetNewStatus_EquipID()
    {
        int id = 0;

        switch (equipMenuManager.equipCategory)
        {
            case DataValidation._category.ïêäÌ:
                id = PartyManager.Instance.GetHaveWeaponID(selectNo);
                break;

            case DataValidation._category.ëïè¸:
                id = PartyManager.Instance.GetHaveAccessoryID(selectNo);
                break;

            case DataValidation._category.ñÇñ@:
                id = PartyManager.Instance.GetHaveMagicID(selectNo);
                break;

            default:
                break;
        }
        return id;
    }

    int GetNewStatus_Equip(int id)
    {
        int result = 5;

        switch (equipMenuManager.equipCategory)
        {
            case DataValidation._category.ïêäÌ:
                result = DatabaseManager.Instance.GetWeaponEffect(id);
                break;

            case DataValidation._category.ëïè¸:
                result = DatabaseManager.Instance.GetAccessoryEffect(id);
                break;

            case DataValidation._category.ñÇñ@:
                break;

            default:
                break;
        }
        return result;
    }

    int GetCurrentStatus_Equip()
    {
        int result = 0;
        int id = 0;

        switch (equipMenuManager.equipCategory)
        {
            case DataValidation._category.ïêäÌ:
                id = PartyManager.Instance.GetPartyCharactorWeapon(playerID);
                if (id < 0) break;
                if(status == DatabaseManager.Instance.GetWeaponStatus(id) && element == DatabaseManager.Instance.GetWeaponElement(id))
                    result = DatabaseManager.Instance.GetWeaponEffect(id);
                break;

            case DataValidation._category.ëïè¸:
                id = PartyManager.Instance.GetPartyCharactorAccessory(playerID, equipMenuManager.equipNo);
                if (id < 0) break;
                if (status == DatabaseManager.Instance.GetAccessoryStatus(id) && element == DatabaseManager.Instance.GetAccessoryElement(id))
                    result = DatabaseManager.Instance.GetAccessoryEffect(id);
                break;

            case DataValidation._category.ñÇñ@:
                break;

            default:
                break;
        }

        return result;
    }

    bool GetNewStatus_NextFLG(int id)
    {
        int currentID;
        bool result = false; 
        bool flg1 = false;
        bool flg2 = false;

        switch (equipMenuManager.equipCategory)
        {
            case DataValidation._category.ïêäÌ:
                currentID = PartyManager.Instance.GetPartyCharactorWeapon(playerID);
                if (currentID >= 0)
                    if (status == DatabaseManager.Instance.GetWeaponStatus(currentID)) flg2 = true;

                if (status == DatabaseManager.Instance.GetWeaponStatus(id)) flg1 = true;

                if (flg1 || flg2) result = true;
                break;

            case DataValidation._category.ëïè¸:
                currentID = PartyManager.Instance.GetPartyCharactorAccessory(playerID, equipMenuManager.equipNo);
                if (currentID >= 0)
                    if (status == DatabaseManager.Instance.GetAccessoryStatus(currentID)) flg2 = true;

                if (status == DatabaseManager.Instance.GetAccessoryStatus(id)) flg1 = true;

                if(flg1 || flg2) result = true;
                break;

            case DataValidation._category.ñÇñ@:
                currentID = PartyManager.Instance.GetPartyCharactorAccessory(playerID, equipMenuManager.equipNo);
                if (currentID >= 0)
                    if (status == DatabaseManager.Instance.GetAccessoryStatus(currentID)) flg2 = true;

                if (status == DatabaseManager.Instance.GetAccessoryStatus(id)) flg1 = true;

                if (flg1 || flg2) result = true;
                break;

            default:
                break;
        }
        return result;
    }

    void GetNewStatus()
    {
        switch (status)
        {
            case DataValidation._status.MAXHP:
                newStatus = PartyManager.Instance.GetPartyCharactorMAXHP(playerID);
                break;

            case DataValidation._status.MAXMP:
                newStatus = PartyManager.Instance.GetPartyCharactorMAXMP(playerID);
                break;

            case DataValidation._status.MAXSP:
                newStatus = PartyManager.Instance.GetPartyCharactorMAXSP(playerID);
                break;

            case DataValidation._status.MAG:
                newStatus = PartyManager.Instance.GetPartyCharactorMAG(playerID);
                break;

            case DataValidation._status.MND:
                newStatus = PartyManager.Instance.GetPartyCharactorMND(playerID);
                break;

            case DataValidation._status.AGI:
                newStatus = PartyManager.Instance.GetPartyCharactorAGI(playerID);
                break;

            case DataValidation._status.HIT:
                newStatus = PartyManager.Instance.GetPartyCharactorHIT(playerID);
                break;

            case DataValidation._status.DEX:
                newStatus = PartyManager.Instance.GetPartyCharactorDEX(playerID);
                break;

            case DataValidation._status.CRI:
                newStatus = PartyManager.Instance.GetPartyCharactorCRI(playerID);
                break;

            case DataValidation._status.ìKê´:
                switch (element)
                {
                    case DataValidation._element.âŒ:
                        newStatus = PartyManager.Instance.GetPCAptitude_Fi(playerID);
                        break;

                    case DataValidation._element.êÖ:
                        newStatus = PartyManager.Instance.GetPCAptitude_Wa(playerID);
                        break;

                    case DataValidation._element.ïó:
                        newStatus = PartyManager.Instance.GetPCAptitude_Wi(playerID);
                        break;

                    case DataValidation._element.ìy:
                        newStatus = PartyManager.Instance.GetPCAptitude_Ea(playerID);
                        break;

                    default:
                        break;
                }
                break;

            case DataValidation._status.ëœê´:
                switch (element)
                {
                    case DataValidation._element.âŒ:
                        newStatus = PartyManager.Instance.GetPCResistance_Fi(playerID);
                        break;

                    case DataValidation._element.êÖ:
                        newStatus = PartyManager.Instance.GetPCResistance_Wa(playerID);
                        break;

                    case DataValidation._element.ïó:
                        newStatus = PartyManager.Instance.GetPCResistance_Wi(playerID);
                        break;

                    case DataValidation._element.ìy:
                        newStatus = PartyManager.Instance.GetPCResistance_Ea(playerID);
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
        baseStatus = newStatus;
    }

    void TextUpdate()
    {
        text.text = newStatus.ToString();
        currentStatus = newStatus;
    }
}
