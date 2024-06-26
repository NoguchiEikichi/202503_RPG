[System.Serializable]
public class ItemFixData
{
    public int _id;                     // ID
    public string _name;                // 名称
    public string _describe;         // 説明
    public DataValidation._group _group;            // 分類
    public string _effect;         // 効果
    public string _addEffect;
    public DataValidation._target _target;
    public DataValidation._timing _timing;
    public int _max;
    public int _money;           // 金額
}