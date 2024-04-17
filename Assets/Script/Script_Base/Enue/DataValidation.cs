[System.Serializable]
public class DataValidation
{
    public enum _status
    {
        name,
        Lv,
        HP,
        MAXHP,
        MP,
        MAXMP,
        SP,
        MAXSP,
        MAG,
        MND,
        AGI,
        HIT,
        DEX,
        CRI,
        適性,
        耐性,
        EXP,
        nextEXP,
    }

    public enum _element
    {
        火,
        水,
        風,
        土,
        無,
    }

    public enum _group
    {
        攻撃,
        回復,
        補助,
        強化,
        無し,
    }

    public enum _target
    {
        味方単体,
        味方全体,
        敵単体,
        敵全体,
        敵ランダム,
        無し,
    }

    public enum _timing
    {
        常時,
        戦闘,
        移動,
        無し,
    }

    public enum _category
    {
        武器,
        装飾,
        魔法,
        無し,
    }
}
