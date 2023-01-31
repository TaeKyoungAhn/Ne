using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRoulette : MonoBehaviour
{
    public Image texts;
    public Sprite deaultText;
    public Image topText;
    public Sprite[] textImg;
    public Sprite[] topTextImg;
    public float timer = 3f;

    public List<string> selectedTexts = new List<string>();

    public GameObject replayBtn;
    public GameObject captureBtn;

    // Start is called before the first frame update
    void Start()
    {
        texts = GetComponent<Image>();
    }


    public void OnDoButtonClick()
    {
        StartCoroutine(RouletteText());
    }

    IEnumerator RouletteText()
    {
        topText.sprite = topTextImg[1];
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            if(timer > 0.3f)
            {
                texts.sprite = textImg[Random.Range(0, textImg.Length)];
            }
            else if(timer < 0.3f)
            {
                topText.sprite = topTextImg[2];
                texts.sprite = textImg[Random.Range(0, textImg.Length)];
            }
            Debug.Log(timer);
        }
        GetComponent<Button>().interactable = false;

        replayBtn.SetActive(true);
        captureBtn.SetActive(true);
    }

    public void OnSelectButtonClick()
    {
        StartCoroutine(RouletteSelectedText());
    }


    IEnumerator RouletteSelectedText()
    {
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            if (timer > 0.3f)
            {
                
            }
            else if (timer < 0.3f)
            {
              
            }
            Debug.Log(timer);
        }
        replayBtn.SetActive(true);
        captureBtn.SetActive(true);
    }
}
