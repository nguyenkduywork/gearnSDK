using GearnSDK.GetBalances.Scripts;
using UnityEngine;

namespace GearnSDK.RefreshButton
{
    public class RefreshButton : MonoBehaviour
    {
        ETHBalance ethBalance;
        ERC20BalanceGearn erc20Balance;
        InGameBalance inGameBalance;
        void Start()
        {
            inGameBalance = FindObjectOfType<InGameBalance>();
            ethBalance = FindObjectOfType<ETHBalance>();
            erc20Balance = FindObjectOfType<ERC20BalanceGearn>();
        }
    
        public void Refresh()
        {
            Debug.Log("Refresh");
            if(inGameBalance != null)
            {
                inGameBalance.ShowInGameBalance();
            }
            if(ethBalance != null)
            {
                ethBalance.ShowETHBalance();
            }
            if(erc20Balance != null)
            {
                erc20Balance.ShowERC20Balance();
            }
        }
    }
}
