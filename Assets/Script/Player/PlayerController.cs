using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player�̈ړ��p�����[�^")]
    public float speed = 200;    //�ړ����x

    //�ړ��Ɋւ���ϐ�
    float hor, ver;            //���͂̒l��������ϐ�

    bool inputFLG = true;

    private Rigidbody2D rb;

    GameObject talkBox;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        talkBox = gameObject.transform.GetChild(1).gameObject;
    }

    // �������Z���������ꍇ��FixedUpdate���g���̂���ʓI
    void FixedUpdate()
    {
        InputCheck();

        if (inputFLG)
        {
            Move();            //�ړ�����
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

    //�ړ�����
    private void Move()
    {
        //InputManager�Őݒ肳�ꂽ���͕��@������͂����l��ϐ��ɑ��
        hor = Input.GetAxis("Horizontal");  //�����i���E�j
        ver = Input.GetAxis("Vertical");    //�����i�㉺�j

        if (Input.GetButton("Horizontal"))
        {
            //�E���͂ŉE�����ɓ���
            if (hor > 0)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(30, 0);
            }
            //�����͂ō������ɓ���
            else if (hor < 0)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(-30, 0);
            }
        }
        //�{�^���𗣂��Ǝ~�܂�
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetButton("Vertical"))
        {
            //����͂ō������ɓ���
            if (ver > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, speed);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(0, 30);
            }
            //�����͂ō������ɓ���
            else if (ver < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -speed);
                talkBox.transform.localPosition = Vector2.zero;
                talkBox.transform.localPosition = new Vector2(0, -30);
            }
        }
        //�{�^���𗣂��Ǝ~�܂�
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    /*

    void Start()
    {
        characon = GetComponent<CharacterController>();   //CharacterController�̃R���|�[�l���g�擾

        animator = GetComponent<Animator>();  //Animator�̃R���|�[�l���g�擾
    }

    void Update()
    {
        InputCheck();

        if (inputFLG)
        {
            Move();            //�ړ�����
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

    //�ړ�����
    private void Move()
    {
        //InputManager�Őݒ肳�ꂽ���͕��@������͂����l��ϐ��ɑ��
        hor = Input.GetAxis("Horizontal");  //�����i���E�j
        ver = Input.GetAxis("Vertical");    //�����i�㉺�j

        moveForward = new Vector2(moveForward.x * hor, moveForward.y * ver);

        moveDirection = new Vector2(moveDirection.normalized.x, moveDirection.y) * moveSpeed;
    }

    //��]
    void Rotate()
    {
        //�ړ����Ă���Ƃ��̂݉�]���Ăق���
        if (hor != 0 || ver != 0)
        {
            //��]�����́H
            rotation = Quaternion.LookRotation(moveForward);

            //��]����
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            //���̊p�x, ���������p�x, ���x
        }
    }*/
}
