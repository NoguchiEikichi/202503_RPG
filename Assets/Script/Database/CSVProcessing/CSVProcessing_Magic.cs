using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class PostProcessing_Magic : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Magic.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Magic.csv", "Database/MagicDatabase.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                MagicDatabase db = AssetDatabase.LoadAssetAtPath<MagicDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<MagicDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.MagicLists = CSVSerializer.Deserialize<MagicFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif