using System;
using UnityEngine;
using UnityEngine.UI;

public class ETHBalance : MonoBehaviour
{
    [SerializeField] private Text myText;
    private string chain;
    private string network;
    private string account;
    private string balance;
    void Awake()
    {
        //Chain name
        chain = "ethereum";
        //Network name, this varies depending on the chain
        network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        //User's wallet address
        account = PlayerPrefs.GetString("Account");

        ShowETHBalance();
    }
    

    public async void ShowETHBalance()
    {
        balance = await EVM.BalanceOf(chain, network, account);
        //change balance to double
        double balanceDouble = double.Parse(balance);
        //Divide by 10^18
        balanceDouble = balanceDouble / 1000000000000000000;
        //Convert to string
        balance = balanceDouble.ToString();
        if(myText!=null) myText.text = balance;
    }
}