using UnityEngine;
using UnityEngine.UI;

namespace GearnSDK.GetUserAddress
{
    public class GetWalletAddress : MonoBehaviour
    {
        //The text field that will display the address
        [SerializeField] private Text myText;
        void Start()
        {
            //Get the address from the wallet and display it
            myText.text = PlayerPrefs.GetString("Account");
        }
    
    }
}