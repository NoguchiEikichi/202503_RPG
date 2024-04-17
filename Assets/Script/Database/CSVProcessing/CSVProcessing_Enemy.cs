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
            //　IndexOfの引数は"/(読み込ませたいファイル名)"とする。
            if (str.IndexOf("/Enemy.csv") != -1)
            {
                //　エディタ内で読み込むならResource.Loadではなくこちらを使うこともできる。
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //　同名のScriptableObjectファイルを読み込む。ない場合は新たに作る。
                string assetfile = str.Replace("Enemy.csv", "Database/EnemyDatabase.asset");
                //　※"LineDataBase"はScriptableObjectのクラス名に合わせて変更する。
                EnemyDatabase db = AssetDatabase.LoadAssetAtPath<EnemyDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<EnemyDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //　※FixData部分はScriptableObjectに入れるデータのクラス名に合わせて変更。
                db.EnemyParamList = CSVSerializer.Deserialize<EnemyFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif