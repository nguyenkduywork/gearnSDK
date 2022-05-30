using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getWeb3Address : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text myText = GameObject.Find("Web3Canvas/MainScreen/TaskbarTop/Web3Address/Address").GetComponent<Text>();
        myText.text = PlayerPrefs.GetString("webAddress");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
