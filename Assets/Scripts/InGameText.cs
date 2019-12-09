using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameText : MonoBehaviour
{

    public float timeOnScreen = 3f;

    private Text text;
    private Text characterText;
    private Animator anim;

    private bool seen = false;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        characterText = GameObject.Find("UI").GetComponentInChildren<Text>();
        anim = GameObject.Find("UI").GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !seen) {
            seen = true;

            anim.Play("dialogueIn");

            characterText.text = text.text;

            StartCoroutine(TextFade());
        }
    }

    IEnumerator TextFade()
    {
        yield return new WaitForSeconds(timeOnScreen);

        anim.Play("dialogueOut");
    }
}