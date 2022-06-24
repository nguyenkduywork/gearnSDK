using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ShowUserERC1155 : MonoBehaviour
{
    public class Response {
        public string image;
    }
    
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
    NFTs[] erc1155s;

    async void Start()
    {
        //Get the player's wallet address
        string account = PlayerPrefs.GetString("wallet");
        
        //Get all erc1155
        await GetErc1155ContractAndId(chain, network, account);
        
        //Get the uri of a token
        var uri = await getTokenUri();
        
        //Fetch JSON from uri 
        var data = await getJSONfromUri(uri);
        
        //Parse json to get image uri
        var imageUri = getImageUriFromJson(data);

        await displayImage(imageUri);
    }

    private async Task displayImage(string imageUri)
    {
        // fetch image and display in game
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        await textureRequest.SendWebRequest();
        this.gameObject.GetComponent<Renderer>().material.mainTexture =
            ((DownloadHandlerTexture) textureRequest.downloadHandler).texture;
    }

    private static string getImageUriFromJson(Response data)
    {
        // parse json to get image uri
        string imageUri = data.image;
        if (imageUri.StartsWith("ipfs://"))
        {
            imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }

        print("imageUri: " + imageUri);
        return imageUri;
    }

    private static async Task<Response> getJSONfromUri(string uri)
    {
        // fetch json from uri
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        print("unity web request: " + webRequest);
        await webRequest.SendWebRequest();

        Response data =
            JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        return data;
    }

    private async Task<string> getTokenUri()
    {
        contract = erc1155s[1].contract;
        tokenId = erc1155s[1].tokenId;
        // fetch uri from chain
        string uri = await ERC1155.URI(chain, network, contract, tokenId);
        if (uri.StartsWith("ipfs://"))
        {
            uri = uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }

        print("uri: " + uri);
        return uri;
    }

    private async Task GetErc1155ContractAndId(string chain, string network, string account)
    {
        string response = await EVM.AllErc1155(chain, network, account, "", 500, 0);
        try
        {
            erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
            //Loop through all erc1155s and get the contract and tokenId
            foreach (NFTs erc1155 in erc1155s)
            {
                contract = erc1155.contract;
                tokenId = erc1155.tokenId;
            }
        }
        catch
        {
            print("Error: " + response);
        }
    }
}