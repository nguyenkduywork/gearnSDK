using UnityEngine;
using UnityEngine.SceneManagement;

//This class is used to save the wallet address and bring it to the nft showcase scene when
//the user clicks on the button "your NFTs"
namespace GearnSDK.Web3_Database.Scripts
{
    public class MoveToSkinScene : MonoBehaviour
    {

        //Move to next scene when click on the button
        public void MoveToNewScene()
        {
            //Loads next scene
            SceneManager.LoadScene("NFTassets");
        }
    }
}