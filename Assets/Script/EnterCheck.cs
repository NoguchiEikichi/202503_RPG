using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCheck : MonoBehaviour
{
    public int sceneNo = 0;

    void Start()
    {
        
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.StageSceneMove(sceneNo);
        }
    }
}
