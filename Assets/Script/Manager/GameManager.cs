using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameStartFLG = false;

    public GameObject player;
    public Vector3 playerPos;
    public Quaternion playerRot;
    CinemachineVirtualCamera vcMain;

    GameObject pc;

    public bool playerMove = false;

    public float sceneMoveTime = 0.5f;

    public bool battleFinishFLG = false;
    public bool escapeFLG = false;
    List<string> battleEnemy = new List<string>();

    void Awake()
    {
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;

        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (gameStartFLG) SceneStart();
    }

    public void SceneStart()
    {
        pc = Instantiate(player, playerPos, playerRot);
        vcMain = GameObject.Find("VC_Main").GetComponent<CinemachineVirtualCamera>();
        vcMain.Follow = pc.transform;

        if (battleFinishFLG)
        {
            if (escapeFLG)
            {
                GameObject enemy = GameObject.Find(battleEnemy[battleEnemy.Count - 1]).gameObject;
                enemy.GetComponent<EnemyController>().NonContact();
                battleEnemy.RemoveAt(battleEnemy.Count-1);
            }
            for (int n = 0; n < battleEnemy.Count; n++)
            {
                if (GameObject.Find(battleEnemy[n]))
                {
                    GameObject enemy = GameObject.Find(battleEnemy[n]).gameObject;
                    Destroy(enemy);
                }
            }

            battleFinishFLG = false;
        }
        else
        {
            battleEnemy.Clear();
        }

        playerMove = true;
    }

    void Update()
    {
        
    }

    void SceneMove(int sceneNo)
    {
        playerMove = false;

        FadeManager.Instance.LoadSceneIndex(sceneNo, sceneMoveTime);
    }

    public void StartSceneMove()
    {
        playerMove = false;
        gameStartFLG = true;

        PartyManager.Instance.DefaultSettings();

        SceneMove(1);
    }

    public void StageSceneMove(int sceneNo)
    {
        playerMove = false;

        SceneMove(sceneNo);
    }

    public void BattleSceneMove(int sceneNo, GameObject enemy)
    {
        playerMove = false;

        battleEnemy.Add(enemy.name);

        playerPos = pc.transform.position;
        playerRot = pc.transform.rotation;

        FadeManager.Instance.LoadSceneIndex_Battle(sceneNo, sceneMoveTime);
    }

    public void BattleFinishSceneMove(int sceneNo)
    {
        playerMove = false;
        battleFinishFLG = true;

        SceneMove(sceneNo);
    }

    public void BattleEscapeSceneMove(int sceneNo)
    {
        playerMove = false;
        battleFinishFLG = true;
        escapeFLG = true;

        SceneMove(sceneNo);
    }

    public void LoadPosData(Vector3 pos, Quaternion rot)
    {
        playerPos = pos;
        playerRot = rot;
    }

    public Vector3 GetPlayerPos()
    {
        Vector3 result = player.transform.position;
        return result;
    }

    public Quaternion GetPlayerRotation()
    {
        Quaternion result = player.transform.rotation;
        return result;
    }
}
