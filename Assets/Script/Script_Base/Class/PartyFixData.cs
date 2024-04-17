using System.Collections.Generic;
[System.Serializable]
public class PartyFixData
{
    public int _id;
    public int _Lv;
    public int _EXP;
    public PlayerStatusData _statusBase;
    public PlayerStatusData _statusPlus;
    public PlayerStatusData _statusChange;
    public PlayerStatusData _statusMain;
    public StatusEffect statusEffect;
    public int _weaponID = -1;
    public int[] _accessoryIDList = new int[] {-1, -1, -1};
    public int[] _magicIDList = new int[] {-1, -1, -1, -1, -1, -1, -1, -1};
}