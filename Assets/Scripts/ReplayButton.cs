using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
    public GameObject SelectButtons; // 누르는 버튼
    public ImageRoulette imageRoulette;


    private void OnEnable()
    {
        imageRoulette = SelectButtons.GetComponent<ImageRoulette>();
    }


    public void Replay()
    {
        imageRoulette.timer = 3f;
        imageRoulette.texts.sprite = imageRoulette.deaultText;
        imageRoulette.captureBtn.SetActive(false);
        
        imageRoulette.topText.sprite = imageRoulette.topTextImg[0];
        SelectButtons.GetComponent<Button>().interactable = true;
        GetComponent<Transform>().gameObject.SetActive(false);
    }

}
