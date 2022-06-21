using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MintToken : MonoBehaviour
{
    //The button to mint a token
    public Button yourButton;
    
    //GameManager object, used here to deduct in game coins
    [SerializeField]
    private GameManager gameManager;
    
    //The user's wallet address
    string address;
    //Function name in the smart contract
    string method = "mint";
    //Smart contract's abi
    string abi = "[ { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Approval\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" } ], \"name\": \"allowance\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"approve\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"claimByAddress\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"decimals\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"subtractedValue\", \"type\": \"uint256\" } ], \"name\": \"decreaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"addedValue\", \"type\": \"uint256\" } ], \"name\": \"increaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"mint\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"transfer\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"transferFrom\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";
    //Contract's address
    string contract = "0xD40d1f9854e989225c88935E79d2EF0033d4369c";
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
    //User's current in game cash
    private double currentCash;

    private string transaction;
    
    string chain = "ethereum";
    string network = "rinkeby";
    private string txConfirmed;
    private string errorMess;
    async void Start()
    {
        //Initialize the button to mint tokens using
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(mintButton);
    }
    
    //Retrieve all in-game money from the user (in the database)
    public void retireMonnaie()
    {
        //Remove all in-game money from the user
        gameManager.SetCash(-currentCash);
    }

    //Create the transaction to the blockchain,
    //the transaction variable is the transaction address
    private async Task SendTransactionWeb3()
    {
        //Get the user's wallet address, when using ChainSafe,
        //the address is in PlayerPrefs, with the key "Account"
        address = PlayerPrefs.GetString("Account");
        
        //Arguments used for the function call,
        //here we need the wallet address and the amount of IRC to be sent
        args = string.Format("[\"{0}\", \"{1}\"]", address, "185000000000000000000");
        
        //Create the data to be sent to the contract
        data = await EVM.CreateContractData(abi, method, args);
        try{
            transaction = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
            Debug.Log(transaction);
        }
        catch(Exception e)
        {
            errorMess = e.Message;
            GUIUtility.systemCopyBuffer = errorMess;
        }
    }

    //Method used to return the current cash if the transaction failed
    void ReturnMoney()
    {
        gameManager.SetCash(currentCash);
    }
    
    //Get the transaction's status
    async void getStatus()
    {
        try{
            txConfirmed = await EVM.TxStatus(chain, network, transaction);
        }
        catch(Exception e)
        {
            txConfirmed = "error";
        }
        
        if(txConfirmed.Equals("error") || txConfirmed.Equals("fail"))
        {
            Notification.instance.Warning("Transaction failed or unrealized");
            Singleton<SoundManager>.Instance.Play("Notification");
            ReturnMoney();
        }
        else if(txConfirmed.Equals("success"))
        {
            Notification.instance.Warning("Transaction successful");
            Singleton<SoundManager>.Instance.Play("Notification");
        }
    }
    
    //Function called when the button is clicked,
    //if the user has more than 185000 in game cash
    private async void mintButton()
    {
        //Get the current cash in the database
        currentCash = Singleton<DataManager>.Instance.database.cash;
        if (currentCash <= 185000)
        {
            Notification.instance.Warning("Minimum coin required: 185000");
            Singleton<SoundManager>.Instance.Play("Notification");
            return;
        }
        //Take all in-game money from the user
        retireMonnaie();
        //Send the transaction to the blockchain
        await SendTransactionWeb3();
        if(GUIUtility.systemCopyBuffer.Equals(errorMess))
        {
            Notification.instance.Warning("Transaction canceled");
            Singleton<SoundManager>.Instance.Play("Notification");
            ReturnMoney();
            return;
        }
        
        Notification.instance.Warning("Processing transaction");
        Singleton<SoundManager>.Instance.Play("Notification");
        //Test, should the transaction failed, return the in-game money
        Invoke("getStatus",30);
    }
}
    
