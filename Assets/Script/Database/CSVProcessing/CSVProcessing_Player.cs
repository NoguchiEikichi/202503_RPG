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
            //　IndexOfの引数は"/(読み込ませたいファイル名)"とする。
            if (str.IndexOf("/Player_0.csv") != -1)
            {
                //　エディタ内で読み込むならResource.Loadではなくこちらを使うこともできる。
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //　同名のScriptableObjectファイルを読み込む。ない場合は新たに作る。
                string assetfile = str.Replace("Player_0.csv", "Database/PlayerDatabase_0.asset");
                //　※"LineDataBase"はScriptableObjectのクラス名に合わせて変更する。
                PlayerDatabase db = AssetDatabase.LoadAssetAtPath<PlayerDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<PlayerDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //　※FixData部分はScriptableObjectに入れるデータのクラス名に合わせて変更。
                db.playerID = 0;
                db.datas = CSVSerializer.Deserialize<PlayerFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }

        foreach (string str in importedAssets)
        {
            //　IndexOfの引数は"/(読み込ませたいファイル名)"とする。
            if (str.IndexOf("/Player_1.csv") != -1)
            {
                //　エディタ内で読み込むならResource.Loadではなくこちらを使うこともできる。
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //　同名のScriptableObjectファイルを読み込む。ない場合は新たに作る。
                string assetfile = str.Replace("Player_1.csv", "Database/PlayerDatabase_1.asset");
                //　※"LineDataBase"はScriptableObjectのクラス名に合わせて変更する。
                PlayerDatabase db = AssetDatabase.LoadAssetAtPath<PlayerDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<PlayerDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //　※FixData部分はScriptableObjectに入れるデータのクラス名に合わせて変更。
                db.playerID = 1;
                db.datas = CSVSerializer.Deserialize<PlayerFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }

        foreach (string str in importedAssets)
        {
            //　IndexOfの引数は"/(読み込ませたいファイル名)"とする。
            if (str.IndexOf("/Player_2.csv") != -1)
            {
                //　エディタ内で読み込むならResource.Loadではなくこちらを使うこともできる。
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //　同名のScriptableObjectファイルを読み込む。ない場合は新たに作る。
                string assetfile = str.Replace("Player_2.csv", "Database/PlayerDatabase_2.asset");
                //　※"LineDataBase"はScriptableObjectのクラス名に合わせて変更する。
                PlayerDatabase db = AssetDatabase.LoadAssetAtPath<PlayerDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<PlayerDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //　※FixData部分はScriptableObjectに入れるデータのクラス名に合わせて変更。
                db.playerID = 2;
                db.datas = CSVSerializer.Deserialize<PlayerFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif