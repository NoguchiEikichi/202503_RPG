using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class PostProcessing_Line : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
            if (str.IndexOf("/Line.csv") != -1)
            {
                //�@�G�f�B�^���œǂݍ��ނȂ�Resource.Load�ł͂Ȃ���������g�����Ƃ��ł���B
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
                string assetfile = str.Replace("Line.csv", "Database/LineDatabase.asset");
                //�@��"LineDataBase"��ScriptableObject�̃N���X���ɍ��킹�ĕύX����B
                LineDatabase db = AssetDatabase.LoadAssetAtPath<LineDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<LineDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //�@��FixData������ScriptableObject�ɓ����f�[�^�̃N���X���ɍ��킹�ĕύX�B
                db.datas = CSVSerializer.Deserialize<LineFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif