using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class PostProcessing_Item : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //　IndexOfの引数は"/(読み込ませたいファイル名)"とする。
            if (str.IndexOf("/Item.csv") != -1)
            {
                //　エディタ内で読み込むならResource.Loadではなくこちらを使うこともできる。
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //　同名のScriptableObjectファイルを読み込む。ない場合は新たに作る。
                string assetfile = str.Replace("Item.csv", "Database/ItemDatabase.asset");
                //　※"LineDataBase"はScriptableObjectのクラス名に合わせて変更する。
                ItemDatabase db = AssetDatabase.LoadAssetAtPath<ItemDatabase>(assetfile);
                if (db == null)
                {
                    db = ScriptableObject.CreateInstance<ItemDatabase>();
                    AssetDatabase.CreateAsset(db, assetfile);
                }
                //　※FixData部分はScriptableObjectに入れるデータのクラス名に合わせて変更。
                db.ItemParamList = CSVSerializer.Deserialize<ItemFixData>(textasset.text);
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif