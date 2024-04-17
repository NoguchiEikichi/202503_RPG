using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class PostProcessing_Enemy : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Enemy.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Enemy.csv", "Database/EnemyDatabase.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                EnemyDatabase db = AssetDatabase.LoadAssetAtPath<EnemyDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<EnemyDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.EnemyParamList = CSVSerializer.Deserialize<EnemyFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif