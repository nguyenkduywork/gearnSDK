using UnityEngine;
using UnityEngine.UI;

namespace GearnSDK.GetBalances.Scripts
{
    public class InGameBalance : MonoBehaviour
    {
        public int currentInGameBalance;
        [SerializeField] Text InGameBalanceText;
        
        void Awake()
        {
            currentInGameBalance = PlayerPrefs.GetInt("InGameMoney");
            ShowInGameBalance();
        }

        public void ShowInGameBalance()
        {
            if(InGameBalanceText!=null) InGameBalanceText.text = currentInGameBalance.ToString();
        }

        public void setInGameBalance(int amount)
        {
            currentInGameBalance = amount;
            PlayerPrefs.SetInt("InGameMoney", currentInGameBalance);
            ShowInGameBalance();
        }
    }
}
