using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    //--------------------------------------
    //ゲームに必要なエフェクトを管理する
    //必要な時に各スクリプトから呼び出せるようにしておく
    //--------------------------------------

    [Header("Playerの挙動に関するエフェクト")]
    public GameObject[] playerFX;

    [Header("敵やステージに関するエフェクト")]
    public GameObject[] StageFX;

    [Header("進行等に関わるエフェクト")]
    public GameObject[] otherFX;

    //個別にパーティクルのコンポーネントを取得したい時
    //public ParticleSystem fx001;


    void Start()
    {
        //fx001 = GetComponent<ParticleSystem>();
    }

}