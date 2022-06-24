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
        public string uri { get; set; }
        public string balance { get; set; }
    }

    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string contract = "0xd1d06063Dc4281f0E6ebd6CBea1AAe9E20E471ea";
        string tokenId = "10";
        string account = "0x5146DcCC1b386BC26BE120Ea41b1f4A27f796e89";
        Debug.Log(account);
        //Get all erc1155
        
        string response = await EVM.AllErc1155(chain, network, account, "", 500, 0);
        try
        {
            NFTs[] erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
            //loop through all erc1155 and print out the contract
            foreach (NFTs erc1155 in erc1155s)
            {
                Debug.Log(erc1155.contract);
            }
        }
        catch
        {
            print("Error: " + response);
        }
        /*
        // fetch uri from chain
        string uri = await ERC1155.URI(chain, network, contract, tokenId);
        if (uri.StartsWith("ipfs://"))
        {
            uri = uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }
        print("uri: " + uri);

        // fetch json from uri
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        print("unity web request: " + webRequest);
        await webRequest.SendWebRequest();
    
        Response data = JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
      
        // parse json to get image uri
        string imageUri = data.image;
        if (imageUri.StartsWith("ipfs://"))
        {
            imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }
        print("imageUri: " + imageUri);

        // fetch image and display in game
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        await textureRequest.SendWebRequest();
        this.gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
        */
    }
}