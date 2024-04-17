[System.Serializable]
public class LineFixData
{
    public int id;              // ID.
    public string name;         // 発言者名
    public string line;         // セリフ
    public string group;        //セリフの分類
    public int idNext_True;     // 次のセリフ
    public int idNext_False;    // 次のセリフ
}