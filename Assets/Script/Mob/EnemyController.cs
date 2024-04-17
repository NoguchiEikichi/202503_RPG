using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MobController
{
    bool contactFLG = true;
    public float nonContactTime = 2f;
    float currentTime = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (!contactFLG)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= nonContactTime)
            {
                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public void NonContact()
    {
        contactFLG = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
