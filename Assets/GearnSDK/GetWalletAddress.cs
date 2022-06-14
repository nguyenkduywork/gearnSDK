using UnityEngine;
using UnityEngine.UI;
using System;
public class GetWalletAddress : MonoBehaviour
{
    [SerializeField] private Text myText;
    void Start()
    {
        myText.text = PlayerPrefs.GetString("Account");
    }
    
}