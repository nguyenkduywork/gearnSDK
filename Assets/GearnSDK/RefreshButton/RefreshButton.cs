using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshButton : MonoBehaviour
{
    [SerializeField] Button refreshButton;
    ETHBalance ethBalance;
    ERC20BalanceGearn ircBalance;
    void Start()
    {
        ethBalance = FindObjectOfType<ETHBalance>();
        ircBalance = FindObjectOfType<ERC20BalanceGearn>();
        Button btn = refreshButton.GetComponent<Button>();
        btn.onClick.AddListener(Refresh);
    }
    
    void Refresh()
    {
        ethBalance.ShowETHBalance();
        ircBalance.ShowERC20Balance();
    }
}
