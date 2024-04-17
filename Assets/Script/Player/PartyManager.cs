using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    public PartyDatabase partyData;

    void Awake()
    {
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;

        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    void Update()
    {

    }

    //経過時間の更新
    private void FixedUpdate()
    {
        if (GameManager.Instance.gameStartFLG)
        {
            partyData._time += Time.deltaTime;
            if (GameObject.Find("MenuManager")) MenuManager.Instance.TimeTextUpdate(partyData._time);
        }
    }

    //パーティキャラクターのステータス設定
    #region
    void SetPCStatus_ALL()
    {
        for(int i = 0; i < partyData.PartyParamData.Length; i++)
        {
            int id = partyData.PartyParamData[i]._id;
            SetPCStatus_One(id);
        }
    }
    void SetPCStatus_One(int id)
    {
        int index = 0;

        for (int i = 0; i < partyData.PartyParamData.Length; i++)
        {
            if (id == partyData.PartyParamData[i]._id)
            {
                index = i;
                break;
            }
        }

        SetPCStatus_HP(index);
        SetPCStatus_MP(index);
        SetPCStatus_SP(index);
        SetPCStatus_MAG(index);
        SetPCStatus_MND(index);
        SetPCStatus_AGI(index);
    }

    //ステータスの確定
    #region
    void SetPCStatus_HP(int index)
    {
        int sum;
        int max = partyData.PartyParamData[index]._statusBase._HP + partyData.PartyParamData[index]._statusPlus._HP;
        sum = max + partyData.PartyParamData[index]._statusChange._HP;

        if (sum <= 0)
        {
            partyData.PartyParamData[index]._statusChange._HP = -max;
            sum = 0;
            partyData.PartyParamData[index].statusEffect.deadFLG = true;
        }
        else if (partyData.PartyParamData[index].statusEffect.deadFLG)
        {
            partyData.PartyParamData[index]._statusChange._HP = -max;
            sum = 0;
        }

        if (sum > max)
        {
            partyData.PartyParamData[index]._statusChange._HP = 0;
            sum = max;
        }

        partyData.PartyParamData[index]._statusMain._HP = sum;
    }

    void SetPCStatus_MP(int index)
    {
        int sum = 0;
        sum = partyData.PartyParamData[index]._statusBase._MP + partyData.PartyParamData[index]._statusPlus._MP + partyData.PartyParamData[index]._statusChange._MP;
        if (sum <= 0) sum = 0;

        partyData.PartyParamData[index]._statusMain._MP = sum;
    }

    void SetPCStatus_SP(int index)
    {
        int sum = 0;
        sum = partyData.PartyParamData[index]._statusBase._SP + partyData.PartyParamData[index]._statusPlus._SP + partyData.PartyParamData[index]._statusChange._SP;
        if (sum <= 0) sum = 0;
        partyData.PartyParamData[index]._statusMain._SP = sum;
    }

    void SetPCStatus_MAG(int index)
    {
        int sum = 0;
        sum = partyData.PartyParamData[index]._statusBase._MAG + partyData.PartyParamData[index]._statusPlus._MAG + partyData.PartyParamData[index]._statusChange._MAG;

        partyData.PartyParamData[index]._statusMain._MAG = sum;
    }

    void SetPCStatus_MND(int index)
    {
        int sum = 0;
        sum = partyData.PartyParamData[index]._statusBase._MND + partyData.PartyParamData[index]._statusPlus._MAG + partyData.PartyParamData[index]._statusChange._MAG;

        partyData.PartyParamData[index]._statusMain._MND = sum;
    }

    void SetPCStatus_AGI(int index)
    {
        int sum = 0;
        sum = partyData.PartyParamData[index]._statusBase._AGI + partyData.PartyParamData[index]._statusPlus._AGI + partyData.PartyParamData[index]._statusChange._AGI;

        partyData.PartyParamData[index]._statusMain._AGI = sum;
    }
    #endregion

    //ステータスの値の一時加算
    #region
    public void ChangePCStatus_HP(int id, int add)
    {
        int index = 0;

        for (int i = 0; i < partyData.PartyParamData.Length; i++)
        {
            if (id == partyData.PartyParamData[i]._id)
            {
                index = i;
                break;
            }
        }

        partyData.PartyParamData[index]._statusChange._HP += add;
        SetPCStatus_HP(index);
    }

    public void ChangePCStatus_MP(int id, int add)
    {
        int index = 0;

        for (int i = 0; i < partyData.PartyParamData.Length; i++)
        {
            if (id == partyData.PartyParamData[i]._id)
            {
                index = i;
                break;
            }
        }

        partyData.PartyParamData[index]._statusChange._MP += add;
        SetPCStatus_MP(index);
    }

    public void ChangePCStatus_SP(int id, int add)
    {
        int index = 0;

        for (int i = 0; i < partyData.PartyParamData.Length; i++)
        {
            if (id == partyData.PartyParamData[i]._id)
            {
                index = i;
                break;
            }
        }

        partyData.PartyParamData[index]._statusChange._SP += add;
        SetPCStatus_SP(index);
    }
    #endregion

    //ステータスの値の永続加算
    #region
    void AddPCStatus_HP(int id, int add)
    {
        int index = 0;

        for (int i = 0; i < partyData.PartyParamData.Length; i++)
        {
            if (id == partyData.PartyParamData[i]._id)
            {
                index = i;
                break;
            }
        }

        if (partyData.PartyParamData[index]._statusMain._HP <= 0)
        {
            return;
        }

        partyData.PartyParamData[index]._statusPlus._HP += add;
        SetPCStatus_HP(index);
    }
    #endregion

    //アイテムの装備関連
    #region
    //共通
    void StatusChange_Equip(int playerID, DataValidation._element element, DataValidation._status status, int effect)
    {
        switch (status)
        {
            case DataValidation._status.HP:
                partyData.PartyParamData[playerID]._statusPlus._HP += effect;
                break;

            case DataValidation._status.MP:
                partyData.PartyParamData[playerID]._statusPlus._MP += effect;
                break;

            case DataValidation._status.MAG:
                partyData.PartyParamData[playerID]._statusPlus._MAG += effect;
                break;

            case DataValidation._status.MND:
                partyData.PartyParamData[playerID]._statusPlus._MND += effect;
                break;

            case DataValidation._status.AGI:
                partyData.PartyParamData[playerID]._statusPlus._AGI += effect;
                break;

            case DataValidation._status.HIT:
                partyData.PartyParamData[playerID]._statusPlus._HIT += effect;
                break;

            case DataValidation._status.DEX:
                partyData.PartyParamData[playerID]._statusPlus._DEX += effect;
                break;

            case DataValidation._status.CRI:
                partyData.PartyParamData[playerID]._statusPlus._CRI += effect;
                break;

            case DataValidation._status.適性:
                switch (element)
                {
                    case DataValidation._element.火:
                        partyData.PartyParamData[playerID]._statusPlus._aptitudeFire += effect;
                        break;

                    case DataValidation._element.水:
                        partyData.PartyParamData[playerID]._statusPlus._aptitudeWater += effect;
                        break;

                    case DataValidation._element.風:
                        partyData.PartyParamData[playerID]._statusPlus._aptitudeWind += effect;
                        break;

                    case DataValidation._element.土:
                        partyData.PartyParamData[playerID]._statusPlus._aptitudeEarth += effect;
                        break;

                    default:
                        break;
                }
                break;

            case DataValidation._status.耐性:
                switch (element)
                {
                    case DataValidation._element.火:
                        partyData.PartyParamData[playerID]._statusPlus._resistanceFire += effect;
                        break;

                    case DataValidation._element.水:
                        partyData.PartyParamData[playerID]._statusPlus._resistanceWater += effect;
                        break;

                    case DataValidation._element.風:
                        partyData.PartyParamData[playerID]._statusPlus._resistanceWind += effect;
                        break;

                    case DataValidation._element.土:
                        partyData.PartyParamData[playerID]._statusPlus._resistanceEarth += effect;
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
        SetPCStatus_ALL();
    }

    //武器
    public void EquipWeapon(int playerID, int weaponID)
    {
        if (SearchHaveWeaponID(weaponID))
        {
            if (partyData.PartyParamData[playerID]._weaponID >= 0) RemoveWeapon(playerID);

            partyData.PartyParamData[playerID]._weaponID = weaponID;

            DataValidation._status weaponStatus = DatabaseManager.Instance.GetWeaponStatus(weaponID); 
            DataValidation._element weaponElement = DatabaseManager.Instance.GetWeaponElement(weaponID);
            int weaponEffect = DatabaseManager.Instance.GetWeaponEffect(weaponID);
            UseHaveWeaponNo(weaponID);

            StatusChange_Equip(playerID, weaponElement, weaponStatus, weaponEffect);
        }
    }

    public void RemoveWeapon(int playerID)
    {
        int weaponID = partyData.PartyParamData[playerID]._weaponID;
        if (weaponID >= 0)
        {
            DataValidation._status weaponStatus = DatabaseManager.Instance.GetWeaponStatus(weaponID);
            DataValidation._element weaponElement = DatabaseManager.Instance.GetWeaponElement(weaponID);
            int weaponEffect = DatabaseManager.Instance.GetWeaponEffect(weaponID);

            StatusChange_Equip(playerID, weaponElement, weaponStatus, -weaponEffect);

            partyData.PartyParamData[playerID]._weaponID = -1;
            GetWeapon(weaponID);
        }
    }

    //装飾品
    public void EquipAccessory(int playerID, int accessoryID, int accessoryNo)
    {
        if (accessoryNo < partyData.PartyParamData[playerID]._accessoryIDList.Length)
        {
            if (partyData.PartyParamData[playerID]._accessoryIDList[accessoryNo] >= 0)
            {
                RemoveAccessory(playerID, accessoryNo);
            }
            partyData.PartyParamData[playerID]._accessoryIDList[accessoryNo] = accessoryID;

            DataValidation._status accessoryStatus = DatabaseManager.Instance.GetAccessoryStatus(accessoryID);
            DataValidation._element accessoryElement = DatabaseManager.Instance.GetAccessoryElement(accessoryID);
            int accessoryEffect = DatabaseManager.Instance.GetAccessoryEffect(accessoryID);
            UseHaveAccessoryNo(accessoryID);

            StatusChange_Equip(playerID, accessoryElement, accessoryStatus, accessoryEffect);
        }
    }

    public void RemoveAccessory(int playerID, int accessoryNo)
    {
        if (accessoryNo < partyData.PartyParamData[playerID]._accessoryIDList.Length)
        {
            int accessoryID = partyData.PartyParamData[playerID]._accessoryIDList[accessoryNo];
            if (accessoryID >= 0)
            {
                DataValidation._status accessoryStatus = DatabaseManager.Instance.GetAccessoryStatus(accessoryID);
                DataValidation._element accessoryElement = DatabaseManager.Instance.GetAccessoryElement(accessoryID);
                int accessoryEffect = DatabaseManager.Instance.GetAccessoryEffect(accessoryID);

                StatusChange_Equip(playerID, accessoryElement, accessoryStatus, -accessoryEffect);

                partyData.PartyParamData[playerID]._accessoryIDList[accessoryNo] = -1;

                GetAccessory(accessoryID);
            }
        }
    }

    //魔法
    public void EquipMagic(int playerID, int magicID, int magicNo)
    {
        if (magicNo < partyData.PartyParamData[playerID]._magicIDList.Length)
        {
            if (partyData.PartyParamData[playerID]._magicIDList[magicNo] >= 0)
            {
                RemoveMagic(playerID, magicNo);
            }
            partyData.PartyParamData[playerID]._magicIDList[magicNo] = magicID;
            UseHaveMagicNo(magicID);
        }
    }

    public void RemoveMagic(int playerID, int magicNo)
    {
        if (magicNo < partyData.PartyParamData[playerID]._magicIDList.Length)
        {
            int magicID = partyData.PartyParamData[playerID]._magicIDList[magicNo];
            if (partyData.PartyParamData[playerID]._magicIDList[magicNo] >= 0)
            {
                partyData.PartyParamData[playerID]._magicIDList[magicNo] = -1;

                GetMagic(magicID);
            }
        }
    }
    #endregion

    //パーティキャラクターの状態変化
    #region
    public void Resurrection(int id)
    {
        if (partyData.PartyParamData[id].statusEffect.deadFLG)
        {
            partyData.PartyParamData[id].statusEffect.deadFLG = false;
        }
    }
    #endregion
    #endregion

    //パーティの情報取得、編集
    #region
    public int GetPartyMemberCount()
    {
        return partyData.PartyParamData.Length;
    }

    public int GetActiveMemberCount()
    {
        int partyMenberCount = GetPartyMemberCount();
        for (int n = 0; n < partyMenberCount; n++)
        {
            if (partyData.PartyParamData[n].statusEffect.deadFLG)
            {
                partyMenberCount--;
            }
        }
        return partyMenberCount;
    }
    
    public int[] GetActiveMemberList()
    {
        int partyMenberCount = GetPartyMemberCount();
        List<int> activeMenberList = new List<int>();

        for (int n = 0; n < partyMenberCount; n++)
        {
            if (!partyData.PartyParamData[n].statusEffect.deadFLG)
            {
                activeMenberList.Add(partyData.PartyParamData[n]._id);
            }
        }

        int[] result = activeMenberList.ToArray();
        return result;
    }

    public int GetPartyMoney()
    {
        return partyData._money;
    }

    public void AddPartyMoney(int getG)
    {
        partyData._money += getG;
    }

    public void UsePartyMoney(int useG)
    {
        partyData._money -= useG;
    }

    public void AddPartyEXP(int addEXP)
    {
        for (int n = 0; n < partyData.PartyParamData.Length; n++)
        {
            if (partyData.PartyParamData[n]._statusMain._HP > 0)
            {
                int id = partyData.PartyParamData[n]._id;
                AddPartyCharactorEXP(id, addEXP);
            }
        }
    }

    public void AddPartyCharactorEXP(int playerID, int addEXP)
    {
        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                partyData.PartyParamData[m]._EXP += addEXP;
            }
        }

        DefaultSettings();
    }

    public bool GetPartyAlive()
    {
        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (!partyData.PartyParamData[m].statusEffect.deadFLG)
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    //パーティキャラクターの情報取得、編集
    #region
    public int GetPartyCharactorID(int index)
    {
        return partyData.PartyParamData[index]._id;
    }

    public int GetPartyCharactorLevel(int playerID)
    {
        int lv = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                lv = partyData.PartyParamData[m]._Lv;
                return lv;
            }
        }

        return lv;
    }

    public int GetPartyCharactorHP(int playerID)
    {
        int hp = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                hp = partyData.PartyParamData[m]._statusMain._HP;
                return hp;
            }
        }
        return hp;
    }

    public int GetPartyCharactorMAXHP(int playerID)
    {
        int hp = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                hp = partyData.PartyParamData[m]._statusBase._HP + partyData.PartyParamData[m]._statusPlus._HP;
                return hp;
            }
        }
        return 0;
    }

    public float GetPartyCharactorHP_Percent(int playerID)
    {
        float percent = 0;
        int hp = GetPartyCharactorHP(playerID);
        int maxHP = GetPartyCharactorMAXHP(playerID);

        percent = (float)(hp) / (float)(maxHP);

        return percent;
    }

    public int GetPartyCharactorMP(int playerID)
    {
        int mp = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                mp = partyData.PartyParamData[m]._statusMain._MP;
                return mp;
            }
        }
        return 0;
    }

    public int GetPartyCharactorMAXMP(int playerID)
    {
        int mp = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                mp = partyData.PartyParamData[m]._statusBase._MP + partyData.PartyParamData[m]._statusPlus._MP;
                return mp;
            }
        }
        return 0;
    }

    public float GetPartyCharactorMP_Percent(int playerID)
    {
        float percent = 0;
        int mp = GetPartyCharactorMP(playerID);
        int maxMP = GetPartyCharactorMAXMP(playerID);

        percent = (float)(mp) / (float)(maxMP);

        return percent;
    }

    public int GetPartyCharactorSP(int playerID)
    {
        int mp = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                mp = partyData.PartyParamData[m]._statusMain._SP;
                return mp;
            }
        }
        return 0;
    }

    public int GetPartyCharactorMAXSP(int playerID)
    {
        int mp = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                mp = partyData.PartyParamData[m]._statusBase._SP + partyData.PartyParamData[m]._statusPlus._SP;
                return mp;
            }
        }
        return 0;
    }

    public float GetPartyCharactorSP_Percent(int playerID)
    {
        float percent = 0;
        int sp = GetPartyCharactorSP(playerID);
        int maxSP = GetPartyCharactorMAXSP(playerID);

        percent = (float)(sp) / (float)(maxSP);

        return percent;
    }

    public int GetPartyCharactorMAG(int playerID)
    {
        int it = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                it = partyData.PartyParamData[m]._statusMain._MAG;
                return it;
            }
        }
        return 0;
    }

    public int GetPartyCharactorMND(int playerID)
    {
        int mnd = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                mnd = partyData.PartyParamData[m]._statusMain._MND;
                return mnd;
            }
        }
        return 0;
    }

    public int GetPartyCharactorAGI(int playerID)
    {
        int agi = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                agi = partyData.PartyParamData[m]._statusMain._AGI;
                return agi;
            }
        }
        return 0;
    }

    public int GetPartyCharactorHIT(int playerID)
    {
        int hit = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                hit = partyData.PartyParamData[m]._statusMain._HIT;
                return hit;
            }
        }
        return 0;
    }

    public int GetPartyCharactorDEX(int playerID)
    {
        int dex = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                dex = partyData.PartyParamData[m]._statusMain._DEX;
                return dex;
            }
        }
        return 0;
    }

    public int GetPartyCharactorCRI(int playerID)
    {
        int cri = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                cri = partyData.PartyParamData[m]._statusMain._CRI;
                return cri;
            }
        }
        return 0;
    }

    public int GetPCAptitude_Fi(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._aptitudeFire;
                return result;
            }
        }
        return 100;
    }

    public int GetPCAptitude_Wa(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._aptitudeWater;
                return result;
            }
        }
        return 100;
    }

    public int GetPCAptitude_Wi(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._aptitudeWind;
                return result;
            }
        }
        return 100;
    }

    public int GetPCAptitude_Ea(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._aptitudeEarth;
                return result;
            }
        }
        return 100;
    }

    public int GetPCResistance_Fi(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._resistanceFire;
                return result;
            }
        }
        return 100;
    }

    public int GetPCResistance_Wa(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._resistanceWater;
                return result;
            }
        }
        return 100;
    }

    public int GetPCResistance_Wi(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._resistanceWind;
                return result;
            }
        }
        return 100;
    }

    public int GetPCResistance_Ea(int playerID)
    {
        int result = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                result = partyData.PartyParamData[m]._statusMain._resistanceEarth;
                return result;
            }
        }
        return 100;
    }

    public int GetPartyCharactorEXP(int playerID)
    {
        int exp = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                exp = partyData.PartyParamData[m]._EXP;
                return exp;
            }
        }
        return 0;
    }

    public int GetPartyCharactorNextEXP(int playerID)
    {
        int currentEXP = 0;
        int nextEXP = 0;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                currentEXP = partyData.PartyParamData[m]._EXP;
                nextEXP = DatabaseManager.Instance.GetPlayerNextEXP(playerID, currentEXP) - currentEXP;
                return nextEXP;
            }
        }
        return 0;
    }

    public int GetPartyCharactorWeapon(int playerID)
    {
        int weaponID = -1;

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                weaponID = partyData.PartyParamData[m]._weaponID;
                return weaponID;
            }
        }
        return weaponID;
    }

    public int GetPartyCharactorAccessory(int playerID, int accessoryNo)
    {
        int accessoryID = -1;

        if (accessoryNo >= 3)
        {
            return accessoryID;
        }

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                accessoryID = partyData.PartyParamData[m]._accessoryIDList[accessoryNo];
                return accessoryID;
            }
        }
        return accessoryID;
    }

    public int[] GetPartyCharactorAccessoryArray(int playerID)
    {
        int[] accessoryID = new int[GetPartyCharactorAccessoryNo(playerID)];

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                accessoryID = partyData.PartyParamData[m]._accessoryIDList;
                return accessoryID;
            }
        }
        return accessoryID;
    }

    public int GetPartyCharactorAccessoryNo(int playerID)
    {
        int no = 0;

        for (int n = 0; n < partyData.PartyParamData[playerID]._accessoryIDList.Length; n++)
        {
            if (partyData.PartyParamData[playerID]._accessoryIDList[n] >= 0) no++;
        }

        return no;
    }

    public int GetPartyCharactorMagic(int playerID, int magicNo)
    {
        int magicID = -1;

        if (magicNo >= 8)
        {
            return magicID;
        }

        for (int m = 0; m < partyData.PartyParamData.Length; m++)
        {
            if (playerID == partyData.PartyParamData[m]._id)
            {
                magicID = partyData.PartyParamData[m]._magicIDList[magicNo];
                return magicID;
            }
        }
        return magicID;
    }

    public int[] GetPartyCharactorMagicArray(int playerID)
    {
        int[] magicID = new int[GetPartyCharactorMagicNo(playerID)];

        for (int n = 0; n < partyData.PartyParamData.Length; n++)
        {
            if (playerID == partyData.PartyParamData[n]._id)
            {
                magicID = partyData.PartyParamData[n]._magicIDList;
                return magicID;
            }
        }
        return magicID;
    }

    public int GetPartyCharactorMagicNo(int playerID)
    {
        int no = 0;

        for (int n = 0; n < partyData.PartyParamData[playerID]._magicIDList.Length; n++)
        {
            if (partyData.PartyParamData[playerID]._magicIDList[n] >= 0) no++;
        }

        return no;
    }

    public StatusEffect GetPartyCharactorAddEffect(int playerID)
    {
        StatusEffect effect = partyData.PartyParamData[playerID].statusEffect;

        return effect;
    }
    #endregion

    //所持アイテムの情報取得、編集
    #region
    public void GetItem(int id)
    {
        GetItems(id, 1);
    }

    public void GetItems(int id, int no)
    {
        if (SearchHaveItemID(id))
        {
            int element = GetHaveItemElement(id);

            partyData.itemHaveList[element]._no += no;
        }
        else
        {
            ItemHaveList getItem = new ItemHaveList();
            getItem._id = id;
            getItem._no = no;
            partyData.itemHaveList.Add(getItem);

            Sort_Item();
        }
    }

    public void UseHaveItem(int id)
    {
        for (int n = 0; n < partyData.itemHaveList.Count; n++)
        {
            if (id == partyData.itemHaveList[n]._id)
            {
                partyData.itemHaveList[n]._no--;

                if (partyData.itemHaveList[n]._no <= 0)
                {
                    partyData.itemHaveList.RemoveAt(n);
                }
            }
        }
    }

    public int GetHaveItemCount()
    {
        return partyData.itemHaveList.Count;
    }

    public int GetHaveItemNo(int id)
    {
        for (int n = 0; n < partyData.itemHaveList.Count; n++)
        {
            if (id == partyData.itemHaveList[n]._id)
            {
                return partyData.itemHaveList[n]._no;
            }
        }
        return 0;
    }

    public int[] GetHaveItemNoArray()
    {
        int haveItem = GetHaveItemCount();
        int[] itemNoArray = new int[haveItem];

        for (int n = 0; n < haveItem; n++)
        {
            itemNoArray[n] = partyData.itemHaveList[n]._no;
        }
        return itemNoArray;
    }

    public int GetHaveItemID(int i)
    {
        return partyData.itemHaveList[i]._id;
    }

    public int[] GetHaveItemIDArray()
    {
        int haveItem = GetHaveItemCount();
        int[] haveItemID = new int[haveItem];

        for (int n = 0; n < haveItem; n++)
        {
            haveItemID[n] = GetHaveItemID(n);
        }

        return haveItemID;
    }

    public string GetHaveItemName(int id)
    {
        return DatabaseManager.Instance.GetItemName(id);
    }

    public string[] GetHaveItemNameArray()
    {
        int haveItem = GetHaveItemCount();
        string[] haveItemList = new string[haveItem];

        for (int n = 0; n < haveItem; n++)
        {
            int itemID = GetHaveItemID(n);
            haveItemList[n] = GetHaveItemName(itemID);
        }

        return haveItemList;
    }
    
    public int GetHaveItemElement(int id)
    {
        int itemElement = 0;

        for (int n = 0; n < partyData.itemHaveList.Count; n++)
        {
            if (id == partyData.itemHaveList[n]._id)
            {
                itemElement = n;
                return itemElement;
            }
        }
        return itemElement;
    }

    public bool SearchHaveItemID(int id)
    {
        bool haveFLG = false;

        for (int i = 0; i < partyData.itemHaveList.Count; i++)
        {
            if (id == partyData.itemHaveList[i]._id)
            {
                haveFLG = true;
                return haveFLG;
            }
        }
        return haveFLG;
    }

    void Sort_Item()
    {
        List<ItemHaveList> sortList = partyData.itemHaveList;
        partyData.itemHaveList.Clear();

        int listCount = sortList.Count;
        for (int n = 0; n < listCount;)
        {
            int bace = sortList[n]._id;
            int no = n;

            for (int m = 0; m < sortList.Count; m++)
            {
                if (bace > sortList[m]._id)
                {
                    bace = sortList[m]._id;
                    no = m;
                }
            }

            partyData.itemHaveList.Add(sortList[no]);
            sortList.RemoveAt(no);
            listCount--;
        }
    }
    #endregion

    //所持武器の情報取得、編集
    #region
    public void GetWeapon(int id)
    {
        GetWeapons(id, 1);
    }

    public void GetWeapons(int id, int no)
    {
        if (SearchHaveWeaponID(id))
        {
            int element = GetHaveWeaponElement(id);

            partyData.weaponHaveList[element]._no += no;
        }
        else
        {
            ItemHaveList getItem = new ItemHaveList();
            getItem._id = id;
            getItem._no = no;
            partyData.weaponHaveList.Add(getItem);

            Sort_Weapon();
        }
    }

    public int GetHaveWeaponCount()
    {
        return partyData.weaponHaveList.Count;
    }

    public int GetHaveWeaponNo(int id)
    {
        for (int n = 0; n < partyData.weaponHaveList.Count; n++)
        {
            if (id == partyData.weaponHaveList[n]._id)
            {
                return partyData.weaponHaveList[n]._no;
            }
        }
        return 0;
    }

    public int[] GetHaveWeaponNoArray()
    {
        int haveWeapon = GetHaveWeaponCount();
        int[] weaponNoArray = new int[haveWeapon];

        for (int n = 0; n < haveWeapon; n++)
        {
            weaponNoArray[n] = partyData.weaponHaveList[n]._no;
        }
        return weaponNoArray;
    }

    public void UseHaveWeaponNo(int id)
    {
        for (int n = 0; n < partyData.weaponHaveList.Count; n++)
        {
            if (id == partyData.weaponHaveList[n]._id)
            {
                partyData.weaponHaveList[n]._no--;

                if (partyData.weaponHaveList[n]._no <= 0)
                {
                    partyData.weaponHaveList.RemoveAt(n);
                }
            }
        }
    }

    public int GetHaveWeaponID(int i)
    {
        return partyData.weaponHaveList[i]._id;
    }

    public int[] GetHaveWeaponIDArray()
    {
        int haveWeapon = GetHaveWeaponCount();
        int[] haveWeaponID = new int[haveWeapon];

        for (int n = 0; n < haveWeapon; n++)
        {
            haveWeaponID[n] = GetHaveWeaponID(n);
        }

        return haveWeaponID;
    }

    public string GetHaveWeaponName(int id)
    {
        return DatabaseManager.Instance.GetWeaponName(id);
    }

    public string[] GetHaveWeaponNameArray()
    {
        int haveWeapon = GetHaveWeaponCount();
        string[] haveWeaponList = new string[haveWeapon];

        for (int n = 0; n < haveWeapon; n++)
        {
            int weaponID = GetHaveWeaponID(n);
            haveWeaponList[n] = GetHaveWeaponName(weaponID);
        }

        return haveWeaponList;
    }

    public int GetHaveWeaponElement(int id)
    {
        int itemElement = 0;

        for (int n = 0; n < partyData.weaponHaveList.Count; n++)
        {
            if (id == partyData.weaponHaveList[n]._id)
            {
                itemElement = n;
                return itemElement;
            }
        }
        return itemElement;
    }

    public bool SearchHaveWeaponID(int id)
    {
        bool haveFLG = false;

        for (int i = 0; i < partyData.weaponHaveList.Count; i++)
        {
            if (id == partyData.weaponHaveList[i]._id)
            {
                haveFLG = true;
                return haveFLG;
            }
        }

        return haveFLG;
    }
    
    void Sort_Weapon()
    {
        List<ItemHaveList> sortList = partyData.weaponHaveList;
        List<ItemHaveList> overwrite = new List<ItemHaveList>();

        int listCount = sortList.Count;
        for (int n = 0; n < listCount;)
        {
            int bace = sortList[0]._id;
            int no = n;

            for (int m = 0; m < sortList.Count; m++)
            {
                if (bace > sortList[m]._id)
                {
                    bace = sortList[m]._id;
                    no = m;
                }
            }

            overwrite.Add(sortList[no]);
            sortList.RemoveAt(no);
            listCount--;
        }

        partyData.weaponHaveList = overwrite;
    }
    #endregion

    //所持装飾品の情報取得、編集
    #region
    public void GetAccessory(int id)
    {
        GetAccessories(id, 1);
    }

    public void GetAccessories(int id, int no)
    {
        if (SearchHaveAccessoryID(id))
        {
            int element = GetHaveAccessoryElement(id);

            partyData.accessoryHaveList[element]._no += no;
        }
        else
        {
            ItemHaveList getItem = new ItemHaveList();
            getItem._id = id;
            getItem._no = no;
            partyData.accessoryHaveList.Add(getItem);

            Sort_Accessory();
        }
    }

    public int GetHaveAccessoryCount()
    {
        return partyData.accessoryHaveList.Count;
    }

    public int GetHaveAccessoryNo(int id)
    {
        for (int n = 0; n < partyData.accessoryHaveList.Count; n++)
        {
            if (id == partyData.accessoryHaveList[n]._id)
            {
                return partyData.accessoryHaveList[n]._no;
            }
        }
        return 0;
    }

    public int[] GetHaveAccessoryNoArray()
    {
        int haveAccessory = GetHaveAccessoryCount();
        int[] accessoryNoArray = new int[haveAccessory];

        for (int n = 0; n < haveAccessory; n++)
        {
            accessoryNoArray[n] = partyData.accessoryHaveList[n]._no;
        }
        return accessoryNoArray;
    }

    public void UseHaveAccessoryNo(int id)
    {
        for (int n = 0; n < partyData.accessoryHaveList.Count; n++)
        {
            if (id == partyData.accessoryHaveList[n]._id)
            {
                partyData.accessoryHaveList[n]._no--;

                if (partyData.accessoryHaveList[n]._no <= 0)
                {
                    partyData.accessoryHaveList.RemoveAt(n);
                }
            }
        }
    }

    public int GetHaveAccessoryID(int i)
    {
        return partyData.accessoryHaveList[i]._id;
    }

    public int[] GetHaveAccessoryIDArray()
    {
        int haveAccessory = GetHaveAccessoryCount();
        int[] haveAccessoryID = new int[haveAccessory];

        for (int n = 0; n < haveAccessory; n++)
        {
            haveAccessoryID[n] = GetHaveAccessoryID(n);
        }

        return haveAccessoryID;
    }

    public string GetHaveAccessoryName(int id)
    {
        return DatabaseManager.Instance.GetAccessoryName(id);
    }

    public string[] GetHaveAccessoryNameArray()
    {
        int haveAccessory = GetHaveAccessoryCount();
        string[] haveAccessoryList = new string[haveAccessory];

        for (int n = 0; n < haveAccessory; n++)
        {
            int accessoryID = GetHaveAccessoryID(n);
            haveAccessoryList[n] = GetHaveAccessoryName(accessoryID);
        }

        return haveAccessoryList;
    }

    public int GetHaveAccessoryElement(int id)
    {
        int accessoryElement = 0;

        for (int n = 0; n < partyData.accessoryHaveList.Count; n++)
        {
            if (id == partyData.accessoryHaveList[n]._id)
            {
                accessoryElement = n;
                return accessoryElement;
            }
        }
        return accessoryElement;
    }

    public bool SearchHaveAccessoryID(int id)
    {
        bool haveFLG = false;

        for (int i = 0; i < partyData.accessoryHaveList.Count; i++)
        {
            if (id == partyData.accessoryHaveList[i]._id)
            {
                haveFLG = true;
                return haveFLG;
            }
        }

        return haveFLG;
    }

    void Sort_Accessory()
    {
        List<ItemHaveList> sortList = partyData.accessoryHaveList;
        List<ItemHaveList> overwrite = new List<ItemHaveList>();

        int listCount = sortList.Count;
        for (int n = 0; n < listCount;)
        {
            int bace = sortList[0]._id;
            int no = n;

            for (int m = 0; m < sortList.Count; m++)
            {
                if (bace > sortList[m]._id)
                {
                    bace = sortList[m]._id;
                    no = m;
                }
            }

            overwrite.Add(sortList[no]);
            sortList.RemoveAt(no);
            listCount--;
        }

        partyData.accessoryHaveList = overwrite;
    }
    #endregion

    //所持魔法の情報取得、編集
    #region
    public void GetMagic(int id)
    {
        GetMagic(id, 1);
    }

    public void GetMagic(int id, int no)
    {
        if (SearchHaveMagicID(id))
        {
            int element = GetHaveMagicElement(id);

            partyData.magicHaveList[element]._no += no;
        }
        else
        {
            ItemHaveList getItem = new ItemHaveList();
            getItem._id = id;
            getItem._no = no;
            partyData.magicHaveList.Add(getItem);

            Sort_Magic();
        }
    }

    public int GetHaveMagicCount()
    {
        return partyData.magicHaveList.Count;
    }

    public int GetHaveMagicNo(int id)
    {
        for (int n = 0; n < partyData.magicHaveList.Count; n++)
        {
            if (id == partyData.magicHaveList[n]._id)
            {
                return partyData.magicHaveList[n]._no;
            }
        }
        return 0;
    }

    public int[] GetHaveMagicNoArray()
    {
        int haveMagic = GetHaveMagicCount();
        int[] magicNoArray = new int[haveMagic];

        for (int n = 0; n < haveMagic; n++)
        {
            magicNoArray[n] = partyData.magicHaveList[n]._no;
        }
        return magicNoArray;
    }

    public void UseHaveMagicNo(int id)
    {
        for (int n = 0; n < partyData.magicHaveList.Count; n++)
        {
            if (id == partyData.magicHaveList[n]._id)
            {
                partyData.magicHaveList[n]._no--;

                if (partyData.magicHaveList[n]._no <= 0)
                {
                    partyData.magicHaveList.RemoveAt(n);
                }
            }
        }
    }

    public int GetHaveMagicID(int i)
    {
        return partyData.magicHaveList[i]._id;
    }

    public int[] GetHaveMagicIDArray()
    {
        int haveMagic = GetHaveMagicCount();
        int[] haveMagicID = new int[haveMagic];

        for (int n = 0; n < haveMagic; n++)
        {
            haveMagicID[n] = GetHaveMagicID(n);
        }

        return haveMagicID;
    }

    public string GetHaveMagicName(int id)
    {
        return DatabaseManager.Instance.GetMagicName(id);
    }

    public string[] GetHaveMagicNameArray()
    {
        int haveMagic = GetHaveMagicCount();
        string[] haveMagicList = new string[haveMagic];

        for (int n = 0; n < haveMagic; n++)
        {
            int magicID = GetHaveMagicID(n);
            haveMagicList[n] = GetHaveMagicName(magicID);
        }

        return haveMagicList;
    }

    public int GetHaveMagicElement(int id)
    {
        int magicElement = 0;

        for (int n = 0; n < partyData.magicHaveList.Count; n++)
        {
            if (id == partyData.magicHaveList[n]._id)
            {
                magicElement = n;
                return magicElement;
            }
        }
        return magicElement;
    }

    public bool SearchHaveMagicID(int id)
    {
        bool haveFLG = false;

        for (int i = 0; i < partyData.magicHaveList.Count; i++)
        {
            if (id == partyData.magicHaveList[i]._id)
            {
                haveFLG = true;
                return haveFLG;
            }
        }

        return haveFLG;
    }

    void Sort_Magic()
    {
        List<ItemHaveList> sortList = partyData.magicHaveList;
        List<ItemHaveList> overwrite = new List<ItemHaveList>();

        int listCount = sortList.Count;
        for (int n = 0; n < listCount;)
        {
            int bace = sortList[0]._id;
            int no = n;

            for (int m = 0; m < sortList.Count; m++)
            {
                if (bace > sortList[m]._id)
                {
                    bace = sortList[m]._id;
                    no = m;
                }
            }

            overwrite.Add(sortList[no]);
            sortList.RemoveAt(no);
            listCount--;
        }

        partyData.magicHaveList = overwrite;
    }
    #endregion

    public void LoadPartyData(PartyDatabase data)
    {
        partyData._money = data._money;
        partyData._time = data._time;
        partyData._partyMemberID = data._partyMemberID;
        partyData.PartyParamData = data.PartyParamData;
        partyData.itemHaveList = data.itemHaveList;
        partyData.weaponHaveList = data.weaponHaveList;
        partyData.accessoryHaveList = data.accessoryHaveList;
        partyData.magicHaveList = data.magicHaveList;
    }

    public PartyDatabase GetPartyData()
    {
        return partyData;
    }

    public void DefaultSettings()
    {
        for (int i = 0; i < partyData.PartyParamData.Length; i++)
        {
            int id = partyData.PartyParamData[i]._id;
            int exp = partyData.PartyParamData[i]._EXP;
            PlayerFixData statusData = DatabaseManager.Instance.GetPlayerStatus(id, exp);
            partyData.PartyParamData[i]._Lv = statusData._level;
            partyData.PartyParamData[i]._statusBase._HP = statusData._HP;
            partyData.PartyParamData[i]._statusBase._MP = statusData._MP;
            partyData.PartyParamData[i]._statusBase._MAG = statusData._MAG;
            partyData.PartyParamData[i]._statusBase._MND = statusData._MND;
            partyData.PartyParamData[i]._statusBase._AGI = statusData._AGI;
        }
        SetPCStatus_ALL();
    }
}