using System;
using System.Numerics;
using UnityEngine.UI;
using UnityEngine;
public class ERC20BalanceGearn : MonoBehaviour
{
    [SerializeField] Text CoinText;
    private string chain;
    private string network;
    private string contract;
    private string account;
    void Awake()
    {
        //Chain name
        chain = Environment.GetEnvironmentVariable("chain");
        
        //Network name, this varies depending on the chain
        network = Environment.GetEnvironmentVariable("network");
        
        //ERC20 contract address
        contract = Environment.GetEnvironmentVariable("contract");
        
        //User's wallet address
        account = PlayerPrefs.GetString("Account");
        
        ShowERC20Balance();
    }

    public async void ShowERC20Balance()
    {
        //Get the balance of ERC20
        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account);
        //divide by 10^18
        BigInteger balanceOfDivided = balanceOf / BigInteger.Pow(10, 18);
        //Display the balance
        if(CoinText!=null) CoinText.text = balanceOfDivided.ToString();
    }
}