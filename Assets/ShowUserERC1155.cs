using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ShowUserERC1155 : MonoBehaviour
{
    public class Response {
        public string image;
    }

    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string contract = "0xd1d06063Dc4281f0E6ebd6CBea1AAe9E20E471ea";
        string tokenId = "10";

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
    }
}