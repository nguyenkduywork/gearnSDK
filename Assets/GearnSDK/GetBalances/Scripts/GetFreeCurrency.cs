using UnityEngine;

namespace GearnSDK.GetBalances.Scripts
{
    public class GetFreeCurrency : MonoBehaviour
    {
    
        [SerializeField] InGameBalance inGameBalance;
    
        public void onClick()
        {
            //if transaction is successful, add 100 in-game money
            inGameBalance.setInGameBalance(inGameBalance.currentInGameBalance + 100);
                
            //This variable is used to send back to the game scene the new amount of in-game currency the user has
            //More explanation can be found in the GoToBalanceScene class
            var transactionPassed = true;
                
            //Save the transaction's status to the player prefs
            PlayerPrefs.SetInt("transactionPassed", transactionPassed?1:0);
        }
    }
}
