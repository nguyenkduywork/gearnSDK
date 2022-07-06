using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This class is used to save the wallet address and bring it to the nft showcase scene when
//the user clicks on the button "your NFTs"
public class GoToBalanceScene : MonoBehaviour
{
    DataManager dataManager;
    private void Awake()
    {
        //assign dataManager
        dataManager = FindObjectOfType<DataManager>();
    }

    private void Start()
    {
        //This is used to send the wallet address to the NFT showcase scene
        string walletAddress = PlayerPrefs.GetString("Account");
        PlayerPrefs.SetString("wallet", walletAddress);
        
        //Get the current diamond in the database, this depends on the game and how it is implemented
        int currentDiamond = Singleton<DataManager>.Instance.database.diamond;
        PlayerPrefs.SetInt("diamond", currentDiamond);
    }

    //Move to next scene when click on the button
    public void MoveToBalanceScene()
    {
        //Saves database
        dataManager.SaveDatabase();
        //Loads next scene
        SceneManager.LoadScene("TokenBalances");
    }
}