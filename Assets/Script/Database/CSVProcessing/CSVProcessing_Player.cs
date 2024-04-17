using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class CSVProcessing_Player : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Player_0.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Player_0.csv", "Database/PlayerDatabase_0.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                PlayerDatabase db = AssetDatabase.LoadAssetAtPath<PlayerDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<PlayerDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.playerID = 0;
                db.datas = CSVSerializer.Deserialize<PlayerFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }

        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Player_1.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Player_1.csv", "Database/PlayerDatabase_1.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                PlayerDatabase db = AssetDatabase.LoadAssetAtPath<PlayerDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<PlayerDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.playerID = 1;
                db.datas = CSVSerializer.Deserialize<PlayerFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }

        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Player_2.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Player_2.csv", "Database/PlayerDatabase_2.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                PlayerDatabase db = AssetDatabase.LoadAssetAtPath<PlayerDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<PlayerDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.playerID = 2;
                db.datas = CSVSerializer.Deserialize<PlayerFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif