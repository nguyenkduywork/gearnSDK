using UnityEngine;
using UnityEngine.SceneManagement;

namespace GearnSDK.GetBalances.Scripts
{
    public class GoToBalanceScene : MonoBehaviour
    {
        private void Start()
        {
            //If the in-game money amount has been changed when we are in the TokenBalances scene,
            //we need to update the in-game amount in the dataManager
            var txConfirmed =  PlayerPrefs.GetInt("transactionPassed")==1;
            if (txConfirmed)
            {
                //Set the in-game money amount to the new value, saved from InGameBalance class, function
                //SetInGameBalance()
                
                //Change this accordingly to your project
                TheDataManager.THE_PLAYER_DATA.GEM = PlayerPrefs.GetInt("InGameMoney");
            
                var transactionPassed = false;
                
                //Reinitialize the transactionPassed variable to false
                PlayerPrefs.SetInt("transactionPassed", transactionPassed?1:0);
            }
        
        }

        //Move to next scene when click on the button
        public void MoveToBalanceScene()
        {
            //Get the build index of the current scene
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("currentSceneIndex", currentSceneIndex);
            
            //Get the current in-game balance in the database, this depends on the game and how it is implemented
            
            int currentInGameBalance = TheDataManager.THE_PLAYER_DATA.GEM;
            PlayerPrefs.SetInt("InGameMoney", currentInGameBalance);
            
            //Loads next scene
            SceneManager.LoadScene("WalletLogin");
        }
    }
}