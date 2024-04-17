using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class ItemManager : Singleton<ItemManager>
{
    DataValidation._group group;
    string effect;
    string[] effects;
    int effectBase;
    string addEffect;
    string[] addEffects;

    public void ItemEffect(int itemID, string userID, string targetID)
    {
        group = DatabaseManager.Instance.GetItemGroup(itemID);
        effect = DatabaseManager.Instance.GetItemEffect(itemID);
        effects = effect.Split("+", StringSplitOptions.None);
        addEffect = DatabaseManager.Instance.GetItemAddEffect(itemID);
        addEffects = addEffect.Split("/", StringSplitOptions.None);

        switch (group)
        {
            //�񕜃A�C�e��
            case DataValidation._group.��:
                HealingItemEffect(userID, targetID);
                break;

            //�⏕�A�C�e��
            case DataValidation._group.�⏕:
                break;

            default:
                break;
        }
    }

    void HealingItemEffect(string userID, string targetID)
    {
        int target = int.Parse(targetID.Substring(1));

        List<int> healList = new List<int>();

        if (target == 99)
        {
            string[] targetList = new string[3];
            //�g�p�҂��v���C���[�̎��̑Ώۃ��X�g�쐬
            if (userID.Contains("P"))
            {
                int[] id = PartyManager.Instance.GetActiveMemberList();
                List<string> ids = new List<string>();
                for (int n = 0; n < id.Length; n++) ids.Add("P" + id[n]);
                targetList = ids.ToArray();
            }
            //�g�p�҂��G�̎��̑Ώۃ��X�g�쐬
            else if (userID.Contains("E")) targetList = BattleManager.Instance.GetEnemyList();

            for (int n = 0; n < targetList.Length; n++)
            {
                effectBase = EffectCalculator(userID, targetList[n]);
                healList.Add(effectBase);
            }

            if (addEffect.Contains("�h��"))
            {
                BattleManager.Instance.MultipleResurrection(targetList, healList.ToArray());
            }
            else
            {
                BattleManager.Instance.MultipleHeal(targetList, healList.ToArray());
            }
        }
        else
        {
            effectBase = EffectCalculator(userID, targetID);
            if (addEffect.Contains("�h��"))
            {
                BattleManager.Instance.SingleResurrection(targetID, effectBase);
            }
            else
            {
                BattleManager.Instance.SingleHeal(targetID, effectBase, userID);
            }
        }
    }

    int EffectCalculator(string userID, string targetID)
    {
        int result = 0;
        int multiply = 1;

        if (effect.Contains("�Ώ�HP"))
        {
            int targetHP = BattleManager.Instance.ListFind_HP(targetID);
            for (int no = 0; no < effects.Length; no++)
            {
                if (effects[no].Contains("�Ώ�HP"))
                {
                    // �����񂩂琔���ȊO�̕������폜�A���K�\��
                    string numericPart = Regex.Replace(effects[no], @"[^\d]", "");
                    multiply = StringToInt(numericPart);
                    result += targetHP * multiply;
                }
            }
        }
        else
        {
            result = StringToInt(effect);
        }

        return result;
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
}
