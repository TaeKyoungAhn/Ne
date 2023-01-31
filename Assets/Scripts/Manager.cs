using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    
    public GameObject imageRouletteBtn;
    public Image topText;
    public GameObject replayButton;
    public GameObject captureButton;


    public void  RefreshCategory()
    {
        if(imageRouletteBtn.GetComponent<Button>().interactable == false)
        {
            imageRouletteBtn.GetComponent<Button>().interactable = true;
        }
        if(imageRouletteBtn.GetComponent<ImageRoulette>().texts.sprite != null)
            imageRouletteBtn.GetComponent< ImageRoulette>().texts.sprite = imageRouletteBtn.GetComponent<ImageRoulette>().deaultText;
        imageRouletteBtn.GetComponent<ImageRoulette>().topText.sprite = imageRouletteBtn.GetComponent<ImageRoulette>().topTextImg[0];
        if (replayButton.activeSelf == true)
        {
            replayButton.SetActive(false);
        }
        if(captureButton.activeSelf == true)
        {
            captureButton.SetActive(false);
        }
        
    }
}
