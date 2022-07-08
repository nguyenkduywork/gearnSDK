using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GearnSDK.GetBalances.Scripts
{
    public class CallSolidityFuncForButton : MonoBehaviour
    {
        public DiamondBalance diamondBalance;
        
        private string transaction;
        
        private string txConfirmed;
        
        RefreshButton refreshButton;
        
        //The button to mint a token
        public Button yourButton;
        //The user's wallet address
        string address;
        //Function name in the smart contract
        string method = "mint";
        
        //Smart contract's abi
        private string abi =
            "[ { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Approval\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" } ], \"name\": \"allowance\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"approve\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"burn\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"burnFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"claimByAddress\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"decimals\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"subtractedValue\", \"type\": \"uint256\" } ], \"name\": \"decreaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"addedValue\", \"type\": \"uint256\" } ], \"name\": \"increaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"mint\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"transfer\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"transferFrom\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";
        
        //Contract's address
        string contract = "0x9B7E19548fb3E11CF24cd2140FBE9271ef6E61EF";
        
        //Value in wei to be sent to the contract, here 0
        string value = "0";
        
        //Gas limit to be used for the transaction
        string gasLimit = "";
        string gasPrice = "";
        
        //Arguments for the function call
        string args;
        
        //Chain ID of the network the contract is deployed on, here Rinkeby so 4
        string chainId= "4";
        
        //The data to be sent to the contract
        private string data;
        
        string chain = "ethereum";
        string network = "rinkeby";
        void Start()
        {
            diamondBalance = FindObjectOfType<DiamondBalance>();
            refreshButton = FindObjectOfType<RefreshButton>();
            //Initialize the button to mint tokens using
            Button mint = yourButton.GetComponent<Button>();
            mint.onClick.AddListener(mintButton);
        }

        //Create the transaction to the blockchain,
        //the transaction variable is the transaction address and the amount of diamonds in Uint256 format
        private async Task SendTransactionWeb3()
        {
            //Get the user's wallet address
            address = PlayerPrefs.GetString("wallet");
            
            //Added 18 zeros after the mint value to make it a Uint256 format
            string mintValue = diamondBalance.currentDiamond.ToString();
            string traillingZeros = "000000000000000000";
            string concatenated = mintValue + traillingZeros;

            //Arguments used for the smart contract function call
            args = string.Format("[\"{0}\", \"{1}\"]", address, concatenated);
            
            //args = string.Format("[\"{0}\"]", concatenated);
        
            //Create the data to be sent to the contract
            data = await EVM.CreateContractData(abi, method, args);
        
            transaction = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
        }
    
    
        //Get the transaction's status
        async void getStatus()
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
            
                //if transaction is successful, take all of the in-game diamond
                diamondBalance.setDiamondBalance(0);
                
                //This variable is used to send back to the game scene the new amount of diamonds the user has
                //More explanation can be found in the GoToBalanceScene class
                var transactionPassed = true;
                
                //Save the transaction's status to the player prefs
                PlayerPrefs.SetInt("transactionPassed", transactionPassed?1:0);
            }
            //reenable the button
            yourButton.enabled = true;
            refreshButton.Refresh();
        }
    
        //Function called when the button is clicked,
        private async void mintButton()
        {
            if (diamondBalance.currentDiamond > 0)
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
                Invoke("getStatus", 20);
            }
        }
    }
}
    
