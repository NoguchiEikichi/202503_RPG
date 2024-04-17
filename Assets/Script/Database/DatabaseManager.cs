using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : Singleton<DatabaseManager>
{
    [SerializeField] LineDatabase lineData;
    [SerializeField] EnemyDatabase enemyData;
    [SerializeField] ItemDatabase itemData;
    [SerializeField] EquipDatabase weaponData;
    [SerializeField] EquipDatabase accessoryData;
    [SerializeField] MagicDatabase magicData;
    [SerializeField] PlayerDatabase[] playerData = new PlayerDatabase[3];

    //敵のステータスの取得
    #region
    public EnemyFixData GetEnemyStatus(int enemyID)
    {
        EnemyFixData getData;

        if (enemyID >= 100)
        {
            for (; enemyID >= 100;)
            {
                enemyID -= 100;
            }
        }

        for (int n = 0; n < enemyData.EnemyParamList.Length; n++)
        {
            if (enemyID == enemyData.EnemyParamList[n]._id)
            {
                getData = enemyData.EnemyParamList[n];
                return getData;
            }
        }
        return null;
    }

    public string GetEnemyName(int enemyID)
    {
        if (enemyID >= 100)
        {
            for (; enemyID >= 100;)
            {
                enemyID -= 100;
            }
        }

        for (int n = 0; n < enemyData.EnemyParamList.Length; n++)
        {
            if (enemyID == enemyData.EnemyParamList[n]._id)
            {
                return enemyData.EnemyParamList[n]._name;
            }
        }
        return "";
    }

    public int GetEnemyHP(int enemyID)
    {
        if (enemyID >= 100)
        {
            for (; enemyID >= 100;)
            {
                enemyID -= 100;
            }
        }

        for (int n = 0; n < enemyData.EnemyParamList.Length; n++)
        {
            if (enemyID == enemyData.EnemyParamList[n]._id)
            {
                return enemyData.EnemyParamList[n]._HP;
            }
        }
        return 0;
    }

    public int GetEnemyMAG(int enemyID)
    {
        if (enemyID >= 100)
        {
            for (; enemyID >= 100;)
            {
                enemyID -= 100;
            }
        }

        for (int n = 0; n < enemyData.EnemyParamList.Length; n++)
        {
            if (enemyID == enemyData.EnemyParamList[n]._id)
            {
                return enemyData.EnemyParamList[n]._MAG;
            }
        }
        return 0;
    }

    public int GetEnemyMND(int enemyID)
    {
        if (enemyID >= 100)
        {
            for (; enemyID >= 100;)
            {
                enemyID -= 100;
            }
        }

        for (int n = 0; n < enemyData.EnemyParamList.Length; n++)
        {
            if (enemyID == enemyData.EnemyParamList[n]._id)
            {
                return enemyData.EnemyParamList[n]._MND;
            }
        }
        return 0;
    }

    public int GetEnemyDEX(int enemyID)
    {
        if (enemyID >= 100)
        {
            for (; enemyID >= 100;)
            {
                enemyID -= 100;
            }
        }

        for (int n = 0; n < enemyData.EnemyParamList.Length; n++)
        {
            if (enemyID == enemyData.EnemyParamList[n]._id)
            {
                return enemyData.EnemyParamList[n]._AGI;
            }
        }
        return 0;
    }
    #endregion

    //アイテムの情報取得
    #region
    public string GetItemName(int id)
    {
        for (int n = 0; n < itemData.ItemParamList.Length; n++)
        {
            if (id == itemData.ItemParamList[n]._id)
            {
                return itemData.ItemParamList[n]._name;
            }
        }
        return "";
    }

    public DataValidation._group GetItemGroup(int id)
    {
        DataValidation._group target = DataValidation._group.無し;
        for (int n = 0; n < itemData.ItemParamList.Length; n++)
        {
            if (id == itemData.ItemParamList[n]._id)
            {
                target = itemData.ItemParamList[n]._group;
            }
        }
        return target;
    }

    public string GetItemEffect(int id)
    {
        string effect = "";
        for (int n = 0; n < itemData.ItemParamList.Length; n++)
        {
            if (id == itemData.ItemParamList[n]._id)
            {
                effect = itemData.ItemParamList[n]._effect;
            }
        }
        return effect;
    }

    public string GetItemAddEffect(int id)
    {
        string addEffect = "";
        for (int n = 0; n < itemData.ItemParamList.Length; n++)
        {
            if (id == itemData.ItemParamList[n]._id)
            {
                addEffect = itemData.ItemParamList[n]._addEffect;
            }
        }
        return addEffect;
    }

    public DataValidation._target GetItemTarget(int id)
    {
        DataValidation._target target = DataValidation._target.無し;
        for (int n = 0; n < itemData.ItemParamList.Length; n++)
        {
            if (id == itemData.ItemParamList[n]._id)
            {
                target = itemData.ItemParamList[n]._target;
            }
        }
        return target;
    }

    public DataValidation._timing GetItemTiming(int id)
    {
        DataValidation._timing target = DataValidation._timing.無し;
        for (int n = 0; n < itemData.ItemParamList.Length; n++)
        {
            if (id == itemData.ItemParamList[n]._id)
            {
                target = itemData.ItemParamList[n]._timing;
            }
        }
        return target;
    }
    #endregion

    //PCのステータス取得
    #region
    public PlayerFixData GetPlayerStatus(int playerID, int currentEXP)
    {
        PlayerFixData getData;
        int playerIndex = GetPlayerStatusIndex(playerID);

        for (int n = 0; n < playerData[playerIndex].datas.Length; n++)
        {
            if (currentEXP == playerData[playerIndex].datas[n]._EXP)
            {
                getData = playerData[playerIndex].datas[n];
                return getData;
            }
            else if (currentEXP < playerData[playerIndex].datas[n]._EXP)
            {
                getData = playerData[playerIndex].datas[n - 1];
                return getData;
            }
        }

        getData = playerData[playerIndex].datas[playerData[playerIndex].datas.Length - 1];
        return getData;
    }

    public string GetPlayerName(int playerID)
    {
        string name = "";

        for (int n = 0; n < playerData.Length; n++)
        {
            if (playerID == playerData[n].playerID)
            {
                name = playerData[n].playerName;
                return name;
            }
        }
        return "";
    }

    public int GetPlayerHP(int playerID, int currentEXP)
    {
        int playerHP = 0;
        int playerIndex = GetPlayerStatusIndex(playerID);

        for (int n = 0; n < playerData[playerIndex].datas.Length; n++)
        {
            if (currentEXP < playerData[playerIndex].datas[n]._EXP)
            {
                n--;
                playerHP = playerData[playerIndex].datas[n]._HP;
                break;
            }
        }

        return playerHP;
    }

    public int GetPlayerMP(int playerID, int currentEXP)
    {
        int playerMP = 0;
        int playerIndex = GetPlayerStatusIndex(playerID);

        for (int n = 0; n < playerData[playerIndex].datas.Length; n++)
        {
            if (currentEXP < playerData[playerIndex].datas[n]._EXP)
            {
                n--;
                playerMP = playerData[playerIndex].datas[n]._MP;
                break;
            }
        }

        return playerMP;
    }

    public int GetPlayerMAG(int playerID, int currentEXP)
    {
        int playerMAG = 0;
        int playerIndex = GetPlayerStatusIndex(playerID);

        for (int n = 0; n < playerData[playerIndex].datas.Length; n++)
        {
            if (currentEXP < playerData[playerIndex].datas[n]._EXP)
            {
                n--;
                playerMAG = playerData[playerIndex].datas[n]._MAG;
                break;
            }
        }

        return playerMAG;
    }

    public int GetPlayerMND(int playerID, int currentEXP)
    {
        int playerMND = 0;
        int playerIndex = GetPlayerStatusIndex(playerID);

        for (int n = 0; n < playerData[playerIndex].datas.Length; n++)
        {
            if (currentEXP < playerData[playerIndex].datas[n]._EXP)
            {
                n--;
                playerMND = playerData[playerIndex].datas[n]._MND;
                break;
            }
        }

        return playerMND;
    }

    public int GetPlayerDEX(int playerID, int currentEXP)
    {
        int playerDEX = 0;
        int playerIndex = GetPlayerStatusIndex(playerID);

        for (int n = 0; n < playerData[playerIndex].datas.Length; n++)
        {
            if (currentEXP < playerData[playerIndex].datas[n]._EXP)
            {
                n--;
                playerDEX = playerData[playerIndex].datas[n]._AGI;
                break;
            }
        }

        return playerDEX;
    }

    int GetPlayerStatusIndex(int playerID)
    {
        int index = 0;

        for (int n = 0; n < playerData.Length; n++)
        {
            if (playerID == playerData[n].playerID)
            {
                index = n;
                break;
            }
        }

        return index;
    }

    public int GetPlayerNextEXP(int playerID, int currentEXP)
    {
        int nextEXP = 0;
        int playerIndex = GetPlayerStatusIndex(playerID);

        for (int n = 0; n < playerData[playerIndex].datas.Length; n++)
        {
            if (currentEXP < playerData[playerIndex].datas[n]._EXP)
            {
                nextEXP = playerData[playerIndex].datas[n]._EXP;
                break;
            }
        }

        return nextEXP;
    }
    #endregion

    //魔法の情報取得
    #region
    public string GetMagicName(int id)
    {
        string magicName = "";
        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                magicName = magicData.MagicLists[n]._name;
            }
        }
        return magicName;
    }

    public string GetMagicUsePoint(int id)
    {
        string usePoint = "MP";

        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                if (magicData.MagicLists[n]._useSP > 0)
                {
                    usePoint = "SP";
                    break;
                }
            }
        }

        return usePoint;
    }

    public int GetMagicUsePointNum(int id)
    {
        int usePoint = 0;

        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                if (magicData.MagicLists[n]._useMP > 0)
                {
                    usePoint = magicData.MagicLists[n]._useMP;
                    return usePoint;
                }
                else if (magicData.MagicLists[n]._useSP > 0)
                {
                    usePoint = magicData.MagicLists[n]._useSP;
                    return usePoint;
                }
            }
        }
        return usePoint;
    }

    public DataValidation._element GetMagicElement(int id)
    {
        DataValidation._element element = DataValidation._element.無;
        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                element = magicData.MagicLists[n]._element;
            }
        }
        return element;
    }

    public DataValidation._group GetMagicGroup(int id)
    {
        DataValidation._group group = DataValidation._group.無し;
        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                group = magicData.MagicLists[n]._group;
            }
        }
        return group;
    }

    public string GetMagicEffect(int id)
    {
        string effect = "";
        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                effect = magicData.MagicLists[n]._effect;
            }
        }
        return effect;
    }

    public string GetMagicAddEffect(int id)
    {
        string effect = "";
        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                effect = magicData.MagicLists[n]._addEffect;
            }
        }
        return effect;
    }

    public DataValidation._target GetMagicTarget(int id)
    {
        DataValidation._target target = DataValidation._target.無し;
        for (int n = 0; n < magicData.MagicLists.Length; n++)
        {
            if (id == magicData.MagicLists[n]._id)
            {
                target = magicData.MagicLists[n]._target;
            }
        }
        return target;
    }
    #endregion

    //武器の情報取得
    #region
    public string GetWeaponName(int id)
    {
        string name = weaponData.datas[id]._name;
        return name;
    }

    public DataValidation._element GetWeaponElement(int id)
    {
        DataValidation._element element = weaponData.datas[id]._element;
        return element;
    }

    public DataValidation._status GetWeaponStatus(int id)
    {
        DataValidation._status status = weaponData.datas[id]._status;

        return status;
    }

    public int GetWeaponEffect(int id)
    {
        int effect = 0;

        effect += int.Parse(weaponData.datas[id]._effect);

        return effect;
    }
    #endregion

    //装飾の情報取得
    #region
    public string GetAccessoryName(int id)
    {
        string name = accessoryData.datas[id]._name;
        return name;
    }

    public DataValidation._element GetAccessoryElement(int id)
    {
        DataValidation._element element = accessoryData.datas[id]._element;
        return element;
    }

    public DataValidation._status GetAccessoryStatus(int id)
    {
        DataValidation._status status = accessoryData.datas[id]._status;
        return status;
    }

    public int GetAccessoryEffect(int id)
    {
        int effect = 0;

        effect += int.Parse(accessoryData.datas[id]._effect);

        return effect;
    }
    #endregion

    public LineFixData GetLineData(int id)
    {
        return lineData.datas[id];
    }
}
