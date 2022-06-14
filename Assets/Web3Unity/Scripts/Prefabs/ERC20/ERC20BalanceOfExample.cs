using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string contract = "0xD40d1f9854e989225c88935E79d2EF0033d4369c";
        string account = PlayerPrefs.GetString("account");

        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account);
        string name = await ERC20.Name(chain, network, contract);

        print(balanceOf); 
    }
}
