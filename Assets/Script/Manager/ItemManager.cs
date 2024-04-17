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
            //回復アイテム
            case DataValidation._group.回復:
                HealingItemEffect(userID, targetID);
                break;

            //補助アイテム
            case DataValidation._group.補助:
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
            //使用者がプレイヤーの時の対象リスト作成
            if (userID.Contains("P"))
            {
                int[] id = PartyManager.Instance.GetActiveMemberList();
                List<string> ids = new List<string>();
                for (int n = 0; n < id.Length; n++) ids.Add("P" + id[n]);
                targetList = ids.ToArray();
            }
            //使用者が敵の時の対象リスト作成
            else if (userID.Contains("E")) targetList = BattleManager.Instance.GetEnemyList();

            for (int n = 0; n < targetList.Length; n++)
            {
                effectBase = EffectCalculator(userID, targetList[n]);
                healList.Add(effectBase);
            }

            if (addEffect.Contains("蘇生"))
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
            if (addEffect.Contains("蘇生"))
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

        if (effect.Contains("対象HP"))
        {
            int targetHP = BattleManager.Instance.ListFind_HP(targetID);
            for (int no = 0; no < effects.Length; no++)
            {
                if (effects[no].Contains("対象HP"))
                {
                    // 文字列から数字以外の文字を削除、正規表現
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
        // 文字列をintに変換
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        else
        {
            // 変換失敗時の処理
            Debug.LogError("Conversion failed.");
            return -1;
        }
    }
}
