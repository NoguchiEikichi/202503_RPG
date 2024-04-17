using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchController : MonoBehaviour
{
    public bool talkFLG = false;
    int talkNo = -1;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (talkFLG && !TalkManager.Instance.talkCanvas.activeSelf && !TalkManager.Instance.onTalkFLG)
            {
                TalkManager.Instance.TalkText(talkNo);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC" && talkNo == -1)
        {
            talkFLG = true;
            talkNo = other.GetComponent<MobController>().no;
        }

        if (other.tag == "Chest" && talkNo == -1)
        {
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            talkFLG = false;
            talkNo = -1;
        }
    }
}