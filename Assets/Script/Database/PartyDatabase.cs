using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartyDatabase", menuName = "ScriptableObjects/CreatePartyParamAsset")]
public class PartyDatabase : ScriptableObject
{
    public int _money;           // ã‡äz
    public float _time;
    public int[] _partyMemberID = new int[3];
    public PartyFixData[] PartyParamData;
    public List<ItemHaveList> itemHaveList = new List<ItemHaveList>();
    public List<ItemHaveList> weaponHaveList = new List<ItemHaveList>();
    public List<ItemHaveList> accessoryHaveList = new List<ItemHaveList>();
    public List<ItemHaveList> magicHaveList = new List<ItemHaveList>();
}