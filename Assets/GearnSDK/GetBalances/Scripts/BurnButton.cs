using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GearnSDK.GetBalances.Scripts
{
    public class BurnButton : MonoBehaviour
    {
        //The user's wallet address
        string address;
        
        //Function name in the smart contract
        //string method = "mint";
        string method = "burn";
        
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
        
        //The button to burn tokens
        [SerializeField] Button burnButton;
        
        [SerializeField] InGameBalance inGameBalance;
        
        [SerializeField] RefreshButton.RefreshButton refreshButton;
        
        private string transaction;
        
        private string txConfirmed;
        
        string balanceOfString;

        private void Awake()
        {
            chain = Environment.GetEnvironmentVariable("chain");
            chainId = Environment.GetEnvironmentVariable("chainId");
            network = Environment.GetEnvironmentVariable("network");
            abi = Environment.GetEnvironmentVariable("abi");
            contract = Environment.GetEnvironmentVariable("contract");
        }

        void Start()
        {
            //Get the user's wallet address
            address = PlayerPrefs.GetString("Account");
           
        }

        //Create the transaction to the blockchain,
        private async Task SendTransactionWeb3()
        {
            //Get the balance of ERC20
            BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, address);
            
            /****************************/
            
            //Use this if you want to burn a particular amount of tokens
            
            //string burnValue = "100";
            //string traillingZeros = "000000000000000000";
            //string concatenated = burnValue + traillingZeros;
            
            //turn balanceOf to a string
            balanceOfString = balanceOf.ToString();

            //Arguments used for the smart contract function call

            args = $"[\"{balanceOfString}\"]";
        
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
            
                //if transaction is successful, take all of the in-game currency
                
                //take away the last 18 characters in the balanceOfString string
                balanceOfString = balanceOfString.Substring(0, balanceOfString.Length - 18);
                
                //change balanceOfString to integer
                int balanceOfInt = int.Parse(balanceOfString);
                
                //change the in-game currency balance to the new balance
                inGameBalance.setInGameBalance(inGameBalance.currentInGameBalance + balanceOfInt);
                
                
                //This variable is used to send back to the game scene the new amount of in-game currency the user has
                //More explanation can be found in the GoToBalanceScene class
                var transactionPassed = true;
                
                //Save the transaction's status to the player prefs
                PlayerPrefs.SetInt("transactionPassed", transactionPassed?1:0);
            }
            //reenable the button
            burnButton.enabled = true;
            refreshButton.Refresh();
        }
    
        //Function called when the button is clicked
        public async void burn()
        {
            //Disable the button to prevent multiple clicks
            burnButton.enabled = false;

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
            Invoke("getStatus",20);
        }
    }
}
    
