using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class PostProcessing_Equip : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Weapon.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Weapon.csv", "Database/WeaponDatabase.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                EquipDatabase db = AssetDatabase.LoadAssetAtPath<EquipDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<EquipDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.datas = CSVSerializer.Deserialize<EquipFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Accessory.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Accessory.csv", "Database/AccessoryDatabase.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                EquipDatabase db = AssetDatabase.LoadAssetAtPath<EquipDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<EquipDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.datas = CSVSerializer.Deserialize<EquipFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif