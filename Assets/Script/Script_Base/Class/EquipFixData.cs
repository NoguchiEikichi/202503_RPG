[System.Serializable]
public class EquipFixData
{
    public int _id;                     // ID
    public string _name;                // 名称
    public string _describe;         // 説明
    public DataValidation._element _element;            // 分類
    public DataValidation._status _status;
    public string _effect;         // 効果
    public string _addEffect;
    public int _max;
    public int _money;           // 金額
}