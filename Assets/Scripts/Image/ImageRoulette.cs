using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRoulette : MonoBehaviour
{
    public Sprite[] textImg;
    public float timer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void OnDoButtonClick()
    {
        StartCoroutine(RouletteText());
    }

    IEnumerator RouletteText()
    {
        while(true)
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(timer);
            Debug.Log(timer);
        }
        
        
        
    }

}
