using UnityEngine;
using UnityEngine.SceneManagement;

//This class is used to save the wallet address and bring it to the nft showcase scene when
//the user clicks on the button "your NFTs"
namespace GearnSDK.NFTShowcase.Scripts
{
    public class NftScene_ButtonListener : MonoBehaviour
    {
        //Move to next scene when click on the button
        public void MoveToNftScene()
        {
            //Loads next scene
            SceneManager.LoadScene("DetectNFT");
        }
    }
}
