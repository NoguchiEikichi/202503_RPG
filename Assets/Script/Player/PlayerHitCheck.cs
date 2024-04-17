using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitCheck : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GameObject enemy = other.gameObject;
            int battleNo = enemy.GetComponent<EnemyController>().no;
            GameManager.Instance.BattleSceneMove(battleNo, other.gameObject);
        }
    }
}