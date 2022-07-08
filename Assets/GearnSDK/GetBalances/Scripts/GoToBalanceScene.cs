using UnityEngine;
using UnityEngine.SceneManagement;

//This class is used to save the wallet address and bring it to the nft showcase scene when
//the user clicks on the button "your NFTs"
public class GoToBalanceScene : MonoBehaviour
{
    DataManager dataManager;
    private void Awake()
    {
        //assign dataManager
        dataManager = FindObjectOfType<DataManager>();
    }
    
    private void Start()
    {
        //If the diamond amount has been changed when we are in the TokenBalances scene,
        //we need to update the diamond amount in the dataManager
        var txConfirmed =  PlayerPrefs.GetInt("transactionPassed")==1;
        if (txConfirmed)
        {
            //Set the diamond amount to the new value, saved from DiamondBalance class, function
            //SetDiamondBalance()
            dataManager.database.diamond = PlayerPrefs.GetInt("diamond");
            
            var transactionPassed = false;
            //Reinitialize the transactionPassed variable to false
            PlayerPrefs.SetInt("transactionPassed", transactionPassed?1:0);
        }
        
    }

    //Move to next scene when click on the button
    public void MoveToBalanceScene()
    {
        //Get the current diamond in the database, this depends on the game and how it is implemented
        int currentDiamond = Singleton<DataManager>.Instance.database.diamond;
        PlayerPrefs.SetInt("diamond", currentDiamond);
        //Saves database
        dataManager.SaveDatabase();
        //Loads next scene
        SceneManager.LoadScene("TokenBalances");
    }
}