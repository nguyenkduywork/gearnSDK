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
        contract = "0xD40d1f9854e989225c88935E79d2EF0033d4369c";
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