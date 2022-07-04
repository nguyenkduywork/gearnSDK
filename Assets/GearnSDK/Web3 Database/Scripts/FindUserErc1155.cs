using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class FindUserErc1155 : MonoBehaviour
{
    private class NFTs
    {
        public string contract { get; set; }
        public string tokenId { get; set; }
    }
    
    //Initialize the variables
    string chain = "ethereum";
    string network = "rinkeby";
    string contract;
    string tokenId;
    private int i;
    NFTs[] erc1155s;

    async void Start()
    {
        i = 0;
        
        //Get the player's wallet address
        string account = PlayerPrefs.GetString("wallet");
        
        //Get all erc1155
        await GetErc1155ContractAndId(chain, network, account);

        JSONReader.Contract[] contractInDatabase = this.GetComponentInParent<JSONReader>().getMyContractList();
        Debug.Log("contractInDatabase.Length: " + contractInDatabase.Length);

    }
    
    //Loop through all erc1155s and get the contract and tokenId
    //Use the function balanceOf to find the balance of each nft
    //if the balance is 0, then the nft is deleted from the array erc1155s
    private async Task GetErc1155ContractAndId(string chain, string network, string account)
    {
        string response = await EVM.AllErc1155(chain, network, account, "", 500, 0);
        try
        {
            erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
            for (int i = 0; i < erc1155s.Length; i++)
            {
                contract = erc1155s[i].contract;
                tokenId = erc1155s[i].tokenId;
                BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, erc1155s[i].contract, account, erc1155s[i].tokenId);
                
                //if the balance is 0, then delete it from the array and reduce the length of the array
                if (balanceOf == 0)
                {
                    Debug.Log("Contract address: " + erc1155s[i].contract + ", token ID: " + erc1155s[i].tokenId 
                              + " has a balance of " + balanceOf + " --> deleting it from the array");
                    erc1155s = erc1155s.Where(x => x.tokenId != erc1155s[i].tokenId).ToArray();
                    i--;
                }
                else
                {
                    Debug.Log("Contract address: " + erc1155s[i].contract + ", token ID: " 
                              + erc1155s[i].tokenId + " has a balance of " + balanceOf);
                }
            }
            Debug.Log("You have " + erc1155s.Length + " ERC1155 NFTs");
        }
        catch
        {
            print("Error: " + response);
        }
    }
    
    
}
