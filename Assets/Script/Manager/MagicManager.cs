using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class MagicManager : Singleton<MagicManager>
{
    DataValidation._group group;
    string effect;
    string[] effects;
    int effectBase;
    string addEffect;
    string[] addEffects;

    public void MagicEffect(int magicID, string userID, string targetID)
    {
        group = DatabaseManager.Instance.GetMagicGroup(magicID);
        effect = DatabaseManager.Instance.GetMagicEffect(magicID);
        effects = effect.Split("+", StringSplitOptions.None);
        addEffect = DatabaseManager.Instance.GetMagicAddEffect(magicID);
        addEffects = addEffect.Split("/", StringSplitOptions.None);

        switch (group)
        {
            //攻撃魔法
            case DataValidation._group.攻撃:
                AttackMagicEffect(magicID, userID, targetID);
                break;

            //回復魔法
            case DataValidation._group.回復:
                HealingMagicEffect(userID, targetID);
                break;

            //補助魔法
            case DataValidation._group.補助:
                break;

            default:
                break;
        }

        string usePoint = DatabaseManager.Instance.GetMagicUsePoint(magicID);
        int usePointNum = DatabaseManager.Instance.GetMagicUsePointNum(magicID);

        BattleManager.Instance.UseMagic(userID, usePoint, usePointNum);
    }

    void AttackMagicEffect(int magicID, string userID, string targetID)
    {
        int target = int.Parse(targetID.Substring(1));

        DataValidation._element element = DatabaseManager.Instance.GetMagicElement(magicID);

        if (target == 25 || target == 99)
        {
            List<string> targetList = new List<string>();
            List<int> damageList = new List<int>();
            if (userID.Contains("P"))
            {
                string[] getList = BattleManager.Instance.GetEnemyList();
                targetList = getList.ToList<string>();
            }
            else if (userID.Contains("E"))
            {

            }
            if (target == 25)
            {
                int times = 0;
                bool attenuationFLG = false;

                for (int n = 0; n < addEffects.Length; n++)
                {
                    if (addEffects[n].Contains("連撃"))
                    {
                        // 文字列から数字以外の文字を削除、正規表現
                        string numericPart = Regex.Replace(addEffects[n], @"[^\d]", "");
                        times = StringToInt(numericPart);
                    }
                    if (addEffects[n].Contains("減衰")) attenuationFLG = true;
                }

                for (int n = 0; n < times; n++)
                {
                    if (attenuationFLG)
                    {
                        int _target = UnityEngine.Random.Range(0, targetList.Count);
                        effectBase = EffectCalculator_Attenuation(userID, targetList[_target], n);
                        damageList.Add(DamageCalculator(element, targetID));
                    }
                    else
                    {
                        int _target = UnityEngine.Random.Range(0, targetList.Count);
                        effectBase = EffectCalculator(userID, targetList[_target]);
                        damageList.Add(DamageCalculator(element, targetID));
                    }
                }
            }
            else if (target == 99)
            {
                for (int n = 0; n < targetList.Count; n++)
                {
                    effectBase = EffectCalculator(userID, targetList[n]);
                    damageList.Add(DamageCalculator(element, targetID));
                }
            }

            BattleManager.Instance.MultipleDamage(targetList.ToArray(), damageList.ToArray());
        }
        else
        {
            effectBase = EffectCalculator(userID, targetID);
            int damage = DamageCalculator(element, targetID);

            BattleManager.Instance.SingleDamage(targetID, damage, userID);
        }
    }

    void HealingMagicEffect(string userID,string targetID)
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
        int status = 0;
        int result = 0;
        int multiply = 1;

        if (effect.Contains("魔力"))
        {
            status = BattleManager.Instance.ListFind_MAG(userID);
            for (int no = 0; no < effects.Length; no++)
            {
                if (effects[no].Contains("魔力"))
                {
                    // 文字列から数字以外の文字を削除、正規表現
                    string numericPart = Regex.Replace(effects[no], @"[^\d]", "");
                    multiply = StringToInt(numericPart);
                    result += status * multiply;
                }
            }

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
        }
        else if (effect.Contains("精神"))
        {
            status = BattleManager.Instance.ListFind_MND(userID);
            for (int no = 0; no < effects.Length; no++)
            {
                if (effects[no].Contains("精神"))
                {
                    // 文字列から数字以外の文字を削除、正規表現
                    string numericPart = Regex.Replace(effects[no], @"[^\d]", "");
                    multiply = StringToInt(numericPart);
                    result += status * multiply;
                }
            }
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
        }
        else if (effect.Contains("対象HP"))
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

        return result;
    }

    int EffectCalculator_Attenuation(string userID, string targetID, int times)
    {
        int status = 0;
        int result = 0;
        int multiply = 1;

        if (effect.Contains("魔力"))
        {
            status = BattleManager.Instance.ListFind_MAG(userID);
            for (int no = 0; no < effects.Length; no++)
            {
                if (effects[no].Contains("魔力"))
                {
                    // 文字列から数字以外の文字を削除、正規表現
                    string numericPart = Regex.Replace(effects[no], @"[^\d]", "");
                    multiply = StringToInt(numericPart);
                    multiply = Attenuator(multiply, times);
                    result += status * multiply;
                }
            }

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
        }
        else if (effect.Contains("精神"))
        {
            status = BattleManager.Instance.ListFind_MND(userID);
            for (int no = 0; no < effects.Length; no++)
            {
                if (effects[no].Contains("精神"))
                {
                    // 文字列から数字以外の文字を削除、正規表現
                    string numericPart = Regex.Replace(effects[no], @"[^\d]", "");
                    multiply = StringToInt(numericPart);
                    multiply = Attenuator(multiply, times);
                    result += status * multiply;
                }
            }
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
        }
        else if (effect.Contains("対象HP"))
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

        return result;
    }
    int Attenuator(int multiply, int times)
    {
        int result = multiply;

        if (times > 0)
        {
            result *= (int)(0.7 * times);
        }

        return result;
    }

    int DamageCalculator(DataValidation._element magicElement, string targetID)
    {
        int result = 0;
        float damage = 0;

        int resistance = BattleManager.Instance.ListFind_Resistance(targetID, magicElement);

        damage = (float)(effectBase) * ((float)(resistance) / (float)(100));

        result = (int)damage;

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
