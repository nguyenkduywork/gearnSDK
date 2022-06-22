using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NftScene_ButtonListener : MonoBehaviour
{
    
    //Move to next scene when click on the button
    public void MoveToNftScene()
    {
        //Load the next scene
        SceneManager.LoadScene("DetectNFT");
    }
}
