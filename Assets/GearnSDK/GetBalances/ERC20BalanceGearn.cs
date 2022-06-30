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
    void Start()
    {
        //Chain name
        chain = "ethereum";
        //Network name, this varies depending on the chain
        network = "rinkeby";
        //ERC20 contract address
        contract = "0x9B7E19548fb3E11CF24cd2140FBE9271ef6E61EF";
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
        CoinText.text = balanceOfDivided.ToString();
    }
}