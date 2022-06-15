using UnityEngine;
using UnityEngine.UI;

public class ETHBalance : MonoBehaviour
{
    [SerializeField] private Text myText;
    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string account = PlayerPrefs.GetString("Account");

        string balance = await EVM.BalanceOf(chain, network, account);
        //change balance to double
        double balanceDouble = double.Parse(balance);
        //Divide by 10^18
        balanceDouble = balanceDouble / 1000000000000000000;
        //Convert to string
        balance = balanceDouble.ToString();
        myText.text = balance;
    }
}