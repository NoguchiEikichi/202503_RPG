using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class BattleManager : Singleton<BattleManager>
{
    List<string> commandSet = new List<string>();
    List<string> targetSet = new List<string>();

    public bool isTurnFLG = false;
    bool startFLG = false;
    bool battleFinishFLG = false;
    bool escapeFLG = false;

    public List<List<string>> sortLists = new List<List<string>>();
    public List<string> turnLists = new List<string>();
    public List<int> enemyIDLists = new List<int>();

    public float delayTime = 2f;

    public TextMeshProUGUI messageText;
    int dropEXP;
    int dropG;

    public GameObject enemyPos;     //�G�̐����ʒu
    public List<EnemyGroupData> enemySelections;    //�o������G�̌��
    public List<BattleEnemyData> enemyLists = new List<BattleEnemyData>();   //���ۂɏo�������G

    public CommandManager commandManager;

    private void Awake()
    {
        int enemyNo = Random.Range(0, enemySelections.Count);
        GameObject enemy = Instantiate(enemySelections[enemyNo].enemyImage);
        enemy.transform.SetParent(enemyPos.transform, false);
        enemy.transform.localPosition *= 0;

        for (int n = 0; n < enemySelections[enemyNo].enemyID.Count; n++)
        {
            EnemyFixData addData = DatabaseManager.Instance.GetEnemyStatus(enemySelections[enemyNo].enemyID[n]);
            enemyLists.Add(EnemyDataConverter(addData));
            enemyIDLists.Add(addData._id);
        }

        GameObject child = enemy.transform.Find("EnemyCursol").gameObject;
        commandManager.commandWindow.Add(child);
        GameObject cursol = child.transform.Find("Cursol-1").gameObject;
        commandManager.defaultFocus.Add(cursol);
    }

    void Start()
    {
        dropEXP = 0;
        dropG = 0;

        isTurnFLG = true;

        List<string> startText = new List<string>();
        startText.Add("");
        startText.Add("");
        startText.Add("");

        for (int n = 0; n < enemyLists.Count; n++)
        {
            if (n <= 2)
            {
                startText[0] += enemyLists[n]._name + "�����ꂽ�I\n";
            }
            else if (n <= 5)
            {
                startText[1] += enemyLists[n]._name + "�����ꂽ�I\n";
            }
            else
            {
                startText[2] += enemyLists[n]._name + "�����ꂽ�I\n";
            }
        }

        TextOutput_List(startText);
        StartCoroutine(this.DelayMethod(delayTime, TurnEnd));
    }

    void Update()
    {
        if (battleFinishFLG)
        {
            if (Input.anyKeyDown)
            {
                GameManager.Instance.BattleFinishSceneMove(1);
            }
        }
        if (escapeFLG)
        {
            if (Input.anyKeyDown)
            {
                GameManager.Instance.BattleEscapeSceneMove(1);
            }
        }
    }

    void Sort_Turn()
    {
        sortLists.Clear();
        turnLists.Clear();

        int[] activeMenberList = PartyManager.Instance.GetActiveMemberList();

        for (int n = 0; n < PartyManager.Instance.GetActiveMemberCount(); n++)
        {
            List<string> addList = new List<string>();
            addList.Add("P" + activeMenberList[n]) ;
            addList.Add(PartyManager.Instance.GetPartyCharactorAGI(activeMenberList[n]).ToString());
            sortLists.Add(addList);
        }
        for (int n = 0; n < enemyLists.Count; n++)
        {
            List<string> addList = new List<string>();
            if (enemyLists[n]._HP > 0)
            {
                addList.Add("E" + enemyLists[n]._id);
                addList.Add(enemyLists[n]._AGI.ToString());
                sortLists.Add(addList);
            }
        }

        int listCount = sortLists.Count;
        for (int n = 0; n < listCount;)
        {
            int bace = int.Parse(sortLists[n][1]);
            int no = n;

            for (int m = 0; m < sortLists.Count; m++)
            {
                if (bace < int.Parse(sortLists[m][1]))
                {
                    bace = int.Parse(sortLists[m][1]);
                    no = m;
                }
                if (bace == int.Parse(sortLists[m][1]))
                {
                    int random = Random.Range(0, 2);
                    if (random < 1)
                    {
                        bace = int.Parse(sortLists[m][1]);
                        no = m;
                    }
                }
            }

            turnLists.Add(sortLists[no][0]);
            sortLists.RemoveAt(no);
            listCount--;
        }

        for (int n = 0; n < turnLists.Count; n++)
        {
            Debug.Log(turnLists[n]);
        }
    }

    //�L�����̃X�e�[�^�X���̊m�F�A�ҏW
    #region
    bool ListCheck(string id)
    {
        if (id.Contains("P"))
        {
            return true;
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public string[] GetEnemyList()
    {
        List<int> enemyList = enemyIDLists;
        List<string> inTheMiddle = new List<string>();
        string[] result;

        for (int n = 0; n < enemyList.Count; n++)
        {
            int hp = ListFind_HP("E" + enemyList[n].ToString());
            if (hp > 0) inTheMiddle.Add("E" + enemyList[n]);
        }

        result = inTheMiddle.ToArray();

        return result;
    }

    public int ListFind_HP(string id)
    {
        int hp = 0;

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            hp = PartyManager.Instance.GetPartyCharactorHP(playerID);
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    hp = enemyLists[n]._HP;
                }
            }
        }

        return hp;
    }

    public void ListSet_HP(string id, int newHP)
    {
        if (id.Contains("P"))
        {
            int currentHP = ListFind_HP(id);
            id = id.Replace("P", "");
            int playerID = int.Parse(id);
            int update = newHP - currentHP;

            PartyManager.Instance.ChangePCStatus_HP(playerID, update);
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);
            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    if (newHP > 0)
                    {
                        enemyLists[n]._HP = newHP;
                    }
                    else
                    {
                        dropG += enemyLists[n]._dropG;
                        dropEXP += enemyLists[n]._dropEXP;
                        enemyLists.RemoveAt(n);
                    }
                }
            }
        }
    }

    public int ListFind_MP(string id)
    {
        int mp = 0;

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            mp = PartyManager.Instance.GetPartyCharactorMP(playerID);
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    mp = enemyLists[n]._MP;
                }
            }
        }

        return mp;
    }

    public void ListSet_MP(string id, int useMP)
    {
        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            PartyManager.Instance.ChangePCStatus_MP(playerID, useMP);
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            int currentMP = ListFind_MP(id);
            int newMP = currentMP - useMP;

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    if (newMP >= 0)
                    {
                        enemyLists[n]._MP = newMP;
                    }
                    else
                    {
                        enemyLists[n]._MP = 0;
                    }
                }
            }
        }
    }

    public int ListFind_SP(string id)
    {
        int sp = 0;

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            sp = PartyManager.Instance.GetPartyCharactorSP(playerID);
        }

        return sp;
    }

    public void ListSet_SP(string id, int useSP)
    {
        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            PartyManager.Instance.ChangePCStatus_SP(playerID, useSP);
        }
    }

    public void SingleDamage(string targetID, int damage, string userID)
    {
        int HP = ListFind_HP(targetID);
        string name = ListFind_Name(userID);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
        }
        TextOutput(name + "�̍U���I\n" + damage + "�̃_���[�W�B�c��HP:" + HP);

        ListSet_HP(targetID, HP);
    }

    public void MultipleDamage(string[] targetID, int[] damage)
    {
        List<string> outputList = new List<string>();
        for (int n = 0; n < targetID.Length; n++)
        {
            int HP = ListFind_HP(targetID[n]);

            HP -= damage[n];
            if (HP <= 0)
            {
                HP = 0;
            }
            outputList.Add(name + "�̍U���I\n" + damage[n] + "�̃_���[�W�B�c��HP:" + HP);

            ListSet_HP(targetID[n], HP);
        }
        TextOutput_List(outputList);
    }
    
    public void SingleHeal(string targetID, int heal, string userID)
    {
        int HP = ListFind_HP(targetID);
        string name = ListFind_Name(userID);

        HP += heal;
        TextOutput(name + "�̉񕜖��@�I\n" + heal + "�̉񕜁B�c��HP:" + HP);

        ListSet_HP(targetID, HP);
    }

    public void MultipleHeal(string[] targetID, int[] heal)
    {
        List<string> outputList = new List<string>();
        for (int n = 0; n < targetID.Length; n++)
        {
            int HP = ListFind_HP(targetID[n]);

            HP += heal[n];
            outputList.Add(name + "�̉񕜖��@�I\n" + heal[n] + "�̉񕜁B�c��HP:" + HP);

            ListSet_HP(targetID[n], HP);
        }
        TextOutput_List(outputList);
    }

    public void SingleResurrection(string targetID, int heal)
    {
        int HP = ListFind_HP(targetID);
        int playerID = int.Parse(targetID.Replace("P",""));
        Debug.Log(playerID);

        HP += heal;
        TextOutput(name + "�̉񕜖��@�I\n" + heal + "�̉񕜁B�c��HP:" + HP);

        PartyManager.Instance.Resurrection(playerID);
        ListSet_HP(targetID, HP);
    }

    public void MultipleResurrection(string[] targetID, int[] heal)
    {
        List<string> outputList = new List<string>();
        List<int> playerID = new List<int>();
        for (int n = 0; n < targetID.Length; n++) playerID.Add(int.Parse(targetID[n].Replace("P", "")));
        outputList.Add(name + "�̉񕜖��@�I\n");

        for (int n = 0; n < targetID.Length; n++)
        {
            int HP = ListFind_HP(targetID[n]);

            HP += heal[n];
            outputList.Add(heal[n] + "�̉񕜁B�c��HP:" + HP + "\n");

            PartyManager.Instance.Resurrection(playerID[n]);
            ListSet_HP(targetID[n], HP);
        }
        TextOutput_List(outputList);
    }

    public void UseMagic(string userID, string usePoint, int usePointNum)
    {
        switch (usePoint)
        {
            case "MP":
                ListSet_MP(userID ,-usePointNum);
                break;

            case "SP":
                ListSet_SP(userID, -usePointNum);
                break;

            default:
                break;
        }
    }

    public int ListFind_MAG(string id)
    {
        int mag = 0;

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            mag = PartyManager.Instance.GetPartyCharactorMAG(playerID);
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    mag = enemyLists[n]._MAG;
                }
            }
        }

        return mag;
    }

    public int ListFind_MND(string id)
    {
        int mnd = 0;

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            mnd = PartyManager.Instance.GetPartyCharactorMND(playerID);
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    mnd = enemyLists[n]._MND;
                }
            }
        }

        return mnd;
    }

    public int ListFind_Resistance(string id, DataValidation._element element)
    {
        int resistance = 100;

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            switch (element)
            {
                case DataValidation._element.��:
                    resistance = PartyManager.Instance.GetPCResistance_Fi(playerID);
                    break;

                case DataValidation._element.��:
                    resistance = PartyManager.Instance.GetPCResistance_Wa(playerID);
                    break;

                case DataValidation._element.��:
                    resistance = PartyManager.Instance.GetPCResistance_Wi(playerID);
                    break;

                case DataValidation._element.�y:
                    resistance = PartyManager.Instance.GetPCResistance_Ea(playerID);
                    break;

                default:
                    break;
            }
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    switch (element)
                    {
                        case DataValidation._element.��:
                            resistance = enemyLists[n]._resistanceFire;
                            break;

                        case DataValidation._element.��:
                            resistance = enemyLists[n]._resistanceWater;
                            break;

                        case DataValidation._element.��:
                            resistance = enemyLists[n]._resistanceWind;
                            break;

                        case DataValidation._element.�y:
                            resistance = enemyLists[n]._resistanceEarth;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        return resistance;
    }

    public string ListFind_Name(string id)
    {
        string name = "";

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            name = DatabaseManager.Instance.GetPlayerName(playerID);
        }
        else if (id.Contains("E"))
        {
            id = id.Replace("E", "");
            int enemyID = int.Parse(id);

            for (int n = 0; n < enemyLists.Count; n++)
            {
                if (enemyID == enemyLists[n]._id)
                {
                    name = enemyLists[n]._name;
                }
            }
        }

        return name;
    }

    public string ListFind_Target(string id)
    {
        id = id.Replace("P", "");
        string targetID = targetSet[int.Parse(id)];
        int target = int.Parse(targetID.Substring(1));

        if (target == 99 || target == 25) 
        { 
            return targetID;
        }

        if (targetID.Contains("E"))
        {
            if (target >= enemyIDLists.Count) target = 0;
            int targetBase = target;
            targetID = "E" + enemyIDLists[target];

            for (; ; )
            {
                int targetHP = ListFind_HP(targetID);
                if (targetHP > 0)
                {
                    break;
                }
                target++;
                if (target >= enemyIDLists.Count) target = 0;

                if (target == targetBase) BattleFinish(true);

                targetID = "E" + enemyIDLists[target];
                Debug.Log(target);
            }
        }
        else if(targetID.Contains("P"))
        {
        }

        return targetID;
    }

    public StatusEffect GetEnemyStatusEffect(string id)
    {
        StatusEffect result = new StatusEffect();

        id = id.Replace("E", "");
        int enemyID = int.Parse(id);

        for (int n = 0; n < enemyLists.Count; n++)
        {
            if (enemyID == enemyLists[n]._id)
            {
                result = enemyLists[n]._effect;
            }
        }

        return result;
    }

    public StatusEffect GetStatusEffect(string id)
    {
        StatusEffect result = new StatusEffect();

        if (id.Contains("P"))
        {
            id = id.Replace("P", "");
            int playerID = int.Parse(id);

            result = PartyManager.Instance.GetPartyCharactorAddEffect(playerID);
        }
        else if(id.Contains("E"))
        {
            result = GetEnemyStatusEffect(id);
        }

        return result;
    }
    #endregion

    //�R�}���h�̓��͎�t
    #region
    public void Escape()
    {
        if (PartyManager.Instance.GetPartyMemberCount() >= 2)
        {
            TextOutput(DatabaseManager.Instance.GetPlayerName(0) + "�����͓����؂����I");
        }
        else
        {
            TextOutput(DatabaseManager.Instance.GetPlayerName(0) + "�͓����؂����I");
        }
        escapeFLG = true;
    }

    public void AttackSet(int target)
    {
        commandSet.Add("Attack");
        targetSet.Add("E" + target);
    }

    public void MagicSet(int magicID, string target)
    {
        commandSet.Add("Magic"+magicID);
        targetSet.Add(target);
    }

    public void ItemSet(int itemID, string target)
    {
        commandSet.Add("Item"+itemID);
        targetSet.Add(target);
    }

    public void DefenceSet()
    {
        commandSet.Add("Defence");
        targetSet.Add("P" + 99);
    }

    public void CommandCansel()
    {
        commandSet.RemoveAt(commandSet.Count -1);
        targetSet.RemoveAt(targetSet.Count -1);
    }
    #endregion

    public void TurnStart()
    {
        isTurnFLG = true;
        StartCoroutine(Turn());
    }

    //�^�[���̊�{�I�ȗ���
    IEnumerator Turn()
    {
        Sort_Turn();

        for (int n = 0; n < turnLists.Count; n++)
        {
            if (turnLists[n].Contains("P"))
            {
                PlayerTurn(n);
            }
            else if (turnLists[n].Contains("E"))
            {
                EnemyTurn(n);
            }

            bool playerAlive = PartyManager.Instance.GetPartyAlive();

            bool enemyAlive = false;
            if (enemyLists.Count > 0)
            {
                enemyAlive = true;
            }

            yield return new WaitForSeconds(delayTime);

            if (!playerAlive)
            {
                BattleFinish(false);
                yield break;
            }

            if (!enemyAlive)
            {
                BattleFinish(true);
                yield break;
            }
        }

        TurnEnd();
    }

    //PC�̃^�[������
    #region
    void PlayerTurn(int i)
    {
        string id = turnLists[i];
        id = id.Replace("P","");
        int playerID = int.Parse(id);

        switch (commandSet[playerID])
        {
            case "Attack":
                PlayerAttack(i);
                break;

            case "Defence":
                PlayerDefence(i);
                break;

            default://�f�t�H���g���疂�@�R�}���h�Ɠ���R�}���h�����ʂ���
                if (commandSet[playerID].Contains("Magic"))
                {
                    PlayerMagic(i);
                }
                else if (commandSet[playerID].Contains("Item"))
                {
                    PlayerItem(i);
                }
                break;
        }
    }

    void PlayerAttack(int i)
    {
        string id = turnLists[i];
        string name = ListFind_Name(id);
        int MAG = ListFind_MAG(id);

        string targetID = ListFind_Target(id);

        int HP = ListFind_HP(targetID);

        HP -= MAG;
        if (HP <= 0)
        {
            HP = 0;
        }
        TextOutput(name + "�̍U���I\n" + MAG + "�̃_���[�W�B�c��HP:" + HP);

        ListSet_HP(targetID, HP);
    }

    void PlayerMagic(int i)
    {
        //Player��ID�̃Z�b�g
        string id = turnLists[i];
        string pcID = id.Replace("P", "");
        int playerID = int.Parse(pcID);

        //�^�[�Q�b�g��ID�̃Z�b�g
        string targetID = ListFind_Target(id);
        Debug.Log(targetID);

        // �����񂩂琔���ȊO�̕������폜�A���K�\��
        string numericPart = Regex.Replace(commandSet[playerID], @"[^\d]", "");
        int magicID = StringToInt(numericPart);

        MagicManager.Instance.MagicEffect(magicID, id, targetID);
    }

    void PlayerItem(int i)
    {
        //Player��ID�̃Z�b�g
        string id = turnLists[i];
        string pcID = id.Replace("P", "");
        int playerID = int.Parse(pcID);

        //�^�[�Q�b�g��ID�̃Z�b�g
        string targetID = ListFind_Target(id);
        Debug.Log(targetID);

        // �����񂩂琔���ȊO�̕������폜�A���K�\��
        string numericPart = Regex.Replace(commandSet[playerID], @"[^\d]", "");
        int itemID = StringToInt(numericPart);

        MagicManager.Instance.MagicEffect(itemID, id, targetID);
    }

    void PlayerDefence(int i)
    {
        
    }
    #endregion

    //Enemy�̃^�[������
    #region
    void EnemyTurn(int i)
    {
        int n = Random.Range(0,10);


        Attack(i);
    }

    void Attack(int i)
    {
        string id = turnLists[i];

        if (!ListCheck(id))
        {
            return;
        }

        string name = ListFind_Name(id);
        int ATK = ListFind_MAG(id);

        int randomTarget = Random.Range(0, 3);

        string targetID = "P" + randomTarget;

        int damage = ATK;
        if (damage <= 0)
        {
            damage = 0;
        }

        SingleDamage(targetID, damage, id);
    }
    #endregion

    //�^�[���I�����̏���
    #region
    void TurnEnd()
    {
        commandSet.Clear();
        targetSet.Clear();

        if (startFLG)
        {
            for (int n = 0; n < PartyManager.Instance.GetPartyMemberCount(); n++)
            {
                TurnEndEffect("P", n);
            }

            for (int n = 0; n < enemyLists.Count; n++)
            {
                TurnEndEffect("E", n);
            }
        }
        else
        {
            startFLG = true;
        }

        TextOutput("�ǂ�����H");
        isTurnFLG = false;
    }

    void TurnEndEffect(string a, int no)
    {
        StatusEffect effect = GetStatusEffect(a + no);
        if (!effect.deadFLG)
        {
            if (a.Contains("P"))
            {
                PartyManager.Instance.ChangePCStatus_SP(no, 10);
            }
        }
    }
    #endregion

    //Battle�I����̏���
    #region
    void BattleFinish(bool win)
    {
        if (win)
        {
            BattleWin();
        }
        else
        {
            BattleLose();
        }
    }

    void BattleWin()
    {
        TextOutput("������|�����I\n" + dropG + "G�A" + dropEXP + "EXP����ɓ��ꂽ�B");
        PartyManager.Instance.AddPartyMoney(dropG);
        PartyManager.Instance.AddPartyEXP(dropEXP);
        battleFinishFLG = true;
    }

    void BattleLose()
    {
        if (PartyManager.Instance.GetPartyMemberCount() >= 2)
        {
            TextOutput(DatabaseManager.Instance.GetPlayerName(0) + "�����͑S�ł����c�c�B");
        }
        else
        {
            TextOutput(DatabaseManager.Instance.GetPlayerName(0) + "�͑S�ł����c�c�B");
        }
        battleFinishFLG = true;
    }
    #endregion

    //�ėp����
    #region
    BattleEnemyData EnemyDataConverter(EnemyFixData input)
    {
        BattleEnemyData result = new BattleEnemyData();

        result._id = input._id;
        result._name = input._name;
        result._describe = input._describe;
        result._HP = input._HP;
        result._MP = input._MP;
        result._MAG = input._MAG;
        result._MND = input._MND;
        result._AGI = input._AGI;
        result._HIT = input._HIT;
        result._DEX = input._DEX;
        result._element = input._element;
        result._resistanceFire = input._resistanceFire;
        result._resistanceWater = input._resistanceWater;
        result._resistanceWind = input._resistanceWind;
        result._resistanceEarth = input._resistanceEarth;
        result._dropEXP = input._dropEXP;
        result._dropG = input._dropG;
        result._dropItem1 = input._dropItem1;
        result._dropChance1 = input._dropChance1;
        result._dropItem2 = input._dropItem2;
        result._dropChance2 = input._dropChance2;

        return result;
    }

    EnemyFixData EnemyDataConverter(BattleEnemyData input)
    {
        EnemyFixData result = new EnemyFixData();

        result._id = input._id;
        result._name = input._name;
        result._describe = input._describe;
        result._HP = input._HP;
        result._MP = input._MP;
        result._MAG = input._MAG;
        result._MND = input._MND;
        result._AGI = input._AGI;
        result._HIT = input._HIT;
        result._DEX = input._DEX;
        result._element = input._element;
        result._resistanceFire = input._resistanceFire;
        result._resistanceWater = input._resistanceWater;
        result._resistanceWind = input._resistanceWind;
        result._resistanceEarth = input._resistanceEarth;
        result._dropEXP = input._dropEXP;
        result._dropG = input._dropG;
        result._dropItem1 = input._dropItem1;
        result._dropChance1 = input._dropChance1;
        result._dropItem2 = input._dropItem2;
        result._dropChance2 = input._dropChance2;

        return result;
    }

    void TextOutput(string text)
    {
        messageText.text = text;
    }

    void TextOutput_List(List<string> text)
    {
        TextOutput(text[0]);
        for (int n = 1; n < text.Count; n++)
        {
            if (text[n] == "" || text[n] == null)
            {
                break;
            }
            StartCoroutine(this.DelayMethod(delayTime, TextOutput, text[n]));
        }
    }

    int StringToInt(string input)
    {
        // �������int�ɕϊ�
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        else
        {
            // �ϊ����s���̏���
            Debug.LogError("Conversion failed.");
            return -1;
        }
    }
    #endregion
}