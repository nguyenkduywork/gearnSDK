using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NftScene_ButtonListener : MonoBehaviour
{
    DataManager dataManager;
    private void Awake()
    {
        //assign dataManager
        dataManager = FindObjectOfType<DataManager>();
    }

    //Move to next scene when click on the button
    public void MoveToNftScene()
    {
        //Saves database
        dataManager.SaveDatabase();
        //Loads next scene
        SceneManager.LoadScene("DetectNFT");
    }
}
