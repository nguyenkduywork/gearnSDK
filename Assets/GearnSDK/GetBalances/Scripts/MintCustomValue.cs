using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GearnSDK.GetBalances.Scripts
{
    public class MintCustomValue : MonoBehaviour
    {
        private string MintValue;
        private int MintValueInt;
        
        public GameObject inputField;

        //The user's wallet address
        string address;
        //Function name in the smart contract
        string method = "mint";
        
        //Smart contract's abi
        private string abi;
        
        //Contract's address
        string contract;
        
        //Value in wei to be sent to the contract, here 0
        string value = "0";
        
        //Gas limit to be used for the transaction
        string gasLimit = "";
        string gasPrice = "";
        
        //Arguments for the function call
        string args;
        
        //Chain ID of the network the contract is deployed on, here Rinkeby so 4
        string chainId;
        
        //The data to be sent to the contract
        private string data;
        
        string chain;
        string network;
        
        [SerializeField] InGameBalance inGameBalance;
        
        private string transaction;
        
        private string txConfirmed;
        
        [SerializeField] RefreshButton.RefreshButton refreshButton;
        
        //The button to mint custom value
        [SerializeField] Button yourButton;

        private void Awake()
        {
            chain = Environment.GetEnvironmentVariable("chain");
            chainId = Environment.GetEnvironmentVariable("chainId");
            network = Environment.GetEnvironmentVariable("network");
            abi = Environment.GetEnvironmentVariable("abi");
            contract = Environment.GetEnvironmentVariable("contract");
        }
        
        private void Start()
        {
            //Get the user's wallet address
            address = PlayerPrefs.GetString("Account");
        }
        
        //Create the transaction to the blockchain,
        //the transaction variable is the transaction address and the amount of diamonds in Uint256 format
        private async Task SendTransactionWeb3()
        {
            
            //Added 18 zeros after the mint value to make it a Uint256 format
            string mintValue = MintValue;
            string traillingZeros = "000000000000000000";
            string concatenated = mintValue + traillingZeros;

            //Arguments used for the smart contract function call
            args = $"[\"{address}\", \"{concatenated}\"]";
            
            //args = string.Format("[\"{0}\"]", concatenated);
        
            //Create the data to be sent to the contract
            data = await EVM.CreateContractData(abi, method, args);
        
            transaction = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
        }
        
        //Get the transaction's status
        private async void getStatus()
        {
            try{
                txConfirmed = await EVM.TxStatus(chain, network, transaction);
                //reinitialize the transaction variable
                transaction = null;
            }
            catch(Exception)
            {
                txConfirmed = "error";
            }
        
            //Send a notification to the screen, this can be changed freely to fit the needs of the game
            if(!txConfirmed.Equals("success"))
            {
                Debug.Log("Transaction failed");
            }
            else
            {
                Debug.Log("Transaction successful");
            
                //if transaction is successful, deduct the in-game currency
                inGameBalance.setInGameBalance(inGameBalance.currentInGameBalance - MintValueInt);
                
                //This variable is used to send back to the game scene the new amount of in-game currency the user has
                //More explanation can be found in the GoToBalanceScene class
                var transactionPassed = true;
                
                //Save the transaction's status to the player prefs
                PlayerPrefs.SetInt("transactionPassed", transactionPassed?1:0);
            }
            //reenable the button
            yourButton.enabled = true;
            refreshButton.Refresh();
        }
        
        public async void CustomMintButton()
        {
            MintValue = inputField.GetComponent<Text>().text;
            
            //try to convert to int
            if (int.TryParse(MintValue, out MintValueInt))
            {
                //if it works, send the value to the SDK
                Debug.Log("Minting " + MintValueInt + " tokens");
            }
            else
            {
                //if it doesn't work, tell the user
                Debug.Log("Invalid value");
                return;
            }
            
            //if the amount is negative error
            if (MintValueInt < 0)
            {
                Debug.Log("Negative value is not accepted");
                return;
            }
            
            if (inGameBalance.currentInGameBalance > 0 && MintValueInt > 0 && MintValueInt <= inGameBalance.currentInGameBalance)
            {
                //Disable the button to prevent multiple clicks
                yourButton.enabled = false;

                //Send the transaction to the blockchain
                try
                {
                    await SendTransactionWeb3();
                }
                catch
                {
                    getStatus();
                    return;
                }

                //Wait for 20 seconds for the transaction to be confirmed on the blockchain
                Invoke(nameof(getStatus), 20);
            }
            else
            {
                Debug.Log("You don't have enough in-game currency");
            }
            
        }
        
        
        
    }
}
