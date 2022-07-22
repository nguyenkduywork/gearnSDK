using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace GearnSDK.Web3_Database.Scripts
{
    [SuppressMessage("ReSharper", "LocalVariableHidesMember")]
    public class FindUserGameNft : MonoBehaviour
    {
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class NFTs
        {
            public string contract { get; set; }
            public string tokenId { get; set; }
        }
    
        //Initialize the variables
        private string chain;
        private string network;
        
        private string account;
    
        //User's ERC1155 NFTs
        NFTs[] erc1155s;
    
        //GameObjects to display the skin depends on NFT owned and on the database
        GameObject skin;
        
        //Button to change skin (if user has at least one NFT)
        [SerializeField] Button changeSkin;

        private JSONReader.Contract[] contractInDatabase;

        private List<int> indices;
        
        private void Awake()
        {
            chain = Environment.GetEnvironmentVariable("chain");
            network = Environment.GetEnvironmentVariable("network");
        }
        async void Start()
        {
        
            //find object with tag "skin" and assign it to skin variable
            skin = GameObject.FindGameObjectWithTag("skin");
            //Get the player's wallet address
            account = PlayerPrefs.GetString("Account");
        
            //Get all erc1155
            await GetErc1155ContractAndId();
        
            //Get the list of NFTs from Json file
            contractInDatabase = GetComponentInParent<JSONReader>().getMyContractList();
        
            //make a list of indices of NFTs in the database that are also in erc1155s
            indices = ListOfIndices(contractInDatabase);
        
            if(indices.Count == 0)
            {
                Debug.Log("User has no NFTs");
                return;
            }
            changeSkin.gameObject.SetActive(true);
            
        }
        
        //Counter for onClick function

        int i;
        public void onClick()
        {
            try
            {
                if(i < indices.Count - 1)
                {
                    i++;
                } else { i = 0; }
                //change the material of the skin
                skin.GetComponent<Renderer>().material = Resources.Load<Material>(contractInDatabase[indices[i]].path);
            }
            catch (Exception e)
            { 
                Debug.Log(e);
            }
        }

        //Get the list of indices in the Json file that are also in erc1155s
        private List<int> ListOfIndices(JSONReader.Contract[] databaseContracts)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < databaseContracts.Length; i++)
            {
                for (int j = 0; j < erc1155s.Length; j++)
                {
                    if (databaseContracts[i].contract == erc1155s[j].contract &&
                        databaseContracts[i].tokenId == erc1155s[j].tokenId)
                    {
                        //if the indices are already in the list, don't add them again
                        if (!indices.Contains(i))
                        {
                            indices.Add(i);
                        }
                    }
                }
            }

            //print the list of indices
            foreach (int i in indices)
            {
                Debug.Log(i);
            }

            return indices;
        }

        //Loop through all erc1155s and get the contract and tokenId
        //Use the function balanceOf to find the balance of each nft
        //if the balance is 0, then the nft is deleted from the array erc1155s
        private async Task GetErc1155ContractAndId()
        {
            try
            {
                string response = await EVM.AllErc1155(chain, network, account);
                erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
                for (int i = 0; i < erc1155s.Length; i++)
                {
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
            catch(Exception e)
            {
                print("Error: " + e);
            }
        }
    }
}
