using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Playerの移動パラメータ")]
    public float speed = 200;    //移動速度

    //移動に関する変数
    float hor, ver;            //入力の値を代入する変数

    bool inputFLG = true;

    private Rigidbody2D rb;

    GameObject talkBox;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        talkBox = gameObject.transform.GetChild(1).gameObject;
    }

    // 物理演算をしたい場合はFixedUpdateを使うのが一般的
    void FixedUpdate()
    {
        InputCheck();

        if (inputFLG)
        {
            Move();            //移動処理
        }
    }

    void InputCheck()
    {
        
        if (GameManager.Instance.playerMove)
        {
            inputFLG = true;
        }
        else
        {
            inputFLG = false;
            hor = 0;
            ver = 0;
            rb.velocity = Vector2.zero;
        }
    }

    //移動処理
    private void Move()
    {
        //InputManagerで設定された入力方法から入力した値を変数に代入
        hor = Input.GetAxis("Horizontal");  //水平（左右）
        ver = Input.GetAxis("Vertical");    //垂直（上下）

        if (Input.GetButton("Horizontal"))
        {
            //右入力で右向きに動く
            if (hor > 0)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(30, 0);
            }
            //左入力で左向きに動く
            else if (hor < 0)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(-30, 0);
            }
        }
        //ボタンを離すと止まる
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetButton("Vertical"))
        {
            //上入力で左向きに動く
            if (ver > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, speed);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(0, 30);
            }
            //下入力で左向きに動く
            else if (ver < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -speed);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(0, -30);
            }
        }
        //ボタンを離すと止まる
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    /*

    void Start()
    {
        characon = GetComponent<CharacterController>();   //CharacterControllerのコンポーネント取得

        animator = GetComponent<Animator>();  //Animatorのコンポーネント取得
    }

    void Update()
    {
        InputCheck();

        if (inputFLG)
        {
            Move();            //移動処理
        }
    }

    void InputCheck()
    {
        inputFLG = true;
        
        if (GameManager.Instance.playerMove)
        {
            inputFLG = true;
        }
        else
        {
            inputFLG = false;
            hor = 0;
            ver = 0;
            moveDirection *= 0;
        }
    }

    //移動処理
    private void Move()
    {
        //InputManagerで設定された入力方法から入力した値を変数に代入
        hor = Input.GetAxis("Horizontal");  //水平（左右）
        ver = Input.GetAxis("Vertical");    //垂直（上下）

        moveForward = new Vector2(moveForward.x * hor, moveForward.y * ver);

        moveDirection = new Vector2(moveDirection.normalized.x, moveDirection.y) * moveSpeed;
    }

    //回転
    void Rotate()
    {
        //移動しているときのみ回転してほしい
        if (hor != 0 || ver != 0)
        {
            //回転方向は？
            rotation = Quaternion.LookRotation(moveForward);

            //回転処理
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            //今の角度, 向きたい角度, 速度
        }
    }*/
}
