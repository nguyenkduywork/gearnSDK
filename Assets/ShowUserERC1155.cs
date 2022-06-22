using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class ShowUserERC1155 : MonoBehaviour
{
    private class Response {
        public string image;
    }
    private class NFTs
    {
        public string contract { get; set; }
        public string tokenId { get; set; }
        public string uri { get; set; }
        public string balance { get; set; }
    }
    
    Canvas canvas;
    private NFTs[] erc1155s;

    private void Awake()
    {
        //find object with the tag Canvas
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
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
            erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
            print(erc1155s[0].contract);
            print(erc1155s[0].tokenId);
            print(erc1155s[0].uri);
            print(erc1155s[0].balance);
        }
        catch
        {
            print("Error: " + response);
        }
        
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = canvas.transform.position;
        string uri = await ERC1155.URI(chain, network, erc1155s[0].contract, erc1155s[0].tokenId);
        // fetch json from uri
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();
        Response data = JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

        // parse json to get image uri
        string imageUri = data.image;
        print("imageUri: " + imageUri);

        // fetch image and display in game
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        await textureRequest.SendWebRequest();
        cube.GetComponent<MeshRenderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
