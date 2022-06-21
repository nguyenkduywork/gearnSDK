using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AllErc1155 : MonoBehaviour
{
    private class NFTs
    {
        public string contract { get; set; }
        public string tokenId { get; set; }
        public string uri { get; set; }
        public string balance { get; set; }
    }

    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string account = PlayerPrefs.GetString("Account");
        Debug.Log("account: " + account);
        string contract = "";
        int first = 500;
        int skip = 0;
        string response = await EVM.AllErc1155(chain, network, account, contract, first, skip);
        try
        {
            NFTs[] erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
            print(erc1155s[0].contract);
            print(erc1155s[0].tokenId);
            print(erc1155s[0].uri);
            print(erc1155s[0].balance);
        }
        catch
        {
            print("Error: " + response);
        }
    }
}