using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusDisplay : MonoBehaviour
{
    public int playerNo;
    public DataValidation._status status;
    public DataValidation._element element = DataValidation._element.ñ≥;

    int playerID;
    int currentStatus;
    int newStatus;

    TextMeshProUGUI text;

    void Start()
    {
        text = this.gameObject.GetComponent<TextMeshProUGUI>();
        playerID = PartyManager.Instance.GetPartyCharactorID(playerNo);
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
        if (status != DataValidation._status.name)
        {
            GetNewStatus();

            if (currentStatus != newStatus)
            {
                TextUpdate();
            }
        }
    }

    void GetNewStatus()
    {
        switch (status)
        {
            case DataValidation._status.Lv:
                newStatus = PartyManager.Instance.GetPartyCharactorLevel(playerID);
                break;

            case DataValidation._status.MAXHP:
                newStatus = PartyManager.Instance.GetPartyCharactorMAXHP(playerID);
                break;

            case DataValidation._status.HP:
                newStatus = PartyManager.Instance.GetPartyCharactorHP(playerID);
                break;

            case DataValidation._status.MAXMP:
                newStatus = PartyManager.Instance.GetPartyCharactorMAXMP(playerID);
                break;

            case DataValidation._status.MP:
                newStatus = PartyManager.Instance.GetPartyCharactorMP(playerID);
                break;

            case DataValidation._status.MAXSP:
                newStatus = PartyManager.Instance.GetPartyCharactorMAXSP(playerID);
                break;

            case DataValidation._status.SP:
                newStatus = PartyManager.Instance.GetPartyCharactorSP(playerID);
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

            case DataValidation._status.EXP:
                newStatus = PartyManager.Instance.GetPartyCharactorEXP(playerID);
                break;

            case DataValidation._status.nextEXP:
                newStatus = PartyManager.Instance.GetPartyCharactorNextEXP(playerID);
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
    }

    void TextUpdate()
    {
        text.text = newStatus.ToString();
        if (status == DataValidation._status.HP|| status == DataValidation._status.MP|| status == DataValidation._status.SP) GaugeUpdate();
        currentStatus = newStatus;
    }

    void GaugeUpdate()
    {
        Image gauge = GameObject.Find("CurrentHPGauge_"+ (playerNo + 1)).GetComponent<Image>();
        float percent = 0;
        if (status == DataValidation._status.HP)
        {
            percent = PartyManager.Instance.GetPartyCharactorHP_Percent(playerID);
        }
        else if (status == DataValidation._status.MP)
        {
            gauge = GameObject.Find("CurrentMPGauge_" + (playerNo + 1)).GetComponent<Image>();
            percent = PartyManager.Instance.GetPartyCharactorMP_Percent(playerID);
        }
        else if (status == DataValidation._status.SP)
        {
            gauge = GameObject.Find("CurrentSPGauge_" + (playerNo + 1)).GetComponent<Image>();
            percent = PartyManager.Instance.GetPartyCharactorSP_Percent(playerID);
        }
        gauge.fillAmount = percent;
    }
}
