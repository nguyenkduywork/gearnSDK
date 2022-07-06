using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshButton : MonoBehaviour
{
    [SerializeField] Button refreshButton;
    ETHBalance ethBalance;
    ERC20BalanceGearn erc20Balance;
    DiamondBalance diamondBalance;
    void Start()
    {
        diamondBalance = FindObjectOfType<DiamondBalance>();
        ethBalance = FindObjectOfType<ETHBalance>();
        erc20Balance = FindObjectOfType<ERC20BalanceGearn>();
        Button btn = refreshButton.GetComponent<Button>();
        btn.onClick.AddListener(Refresh);
    }
    
    public void Refresh()
    {
        diamondBalance.ShowDiamondBalance();
        ethBalance.ShowETHBalance();
        erc20Balance.ShowERC20Balance();
    }
}
