using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyGroupData", menuName = "ScriptableObjects/CreateEnemyGroupParamAsset")]
public class EnemyGroupData : ScriptableObject
{
    public List<int> enemyID;
    public GameObject enemyImage;
}