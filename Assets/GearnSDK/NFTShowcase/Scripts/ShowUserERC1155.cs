using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

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
    private int i;
    NFTs[] erc1155s;

    [SerializeField] Button btn;
    private GameObject myNft;
    async void Start()
    {
        i = 0;
        btn = GetComponent<Button>();
        myNft = GameObject.FindGameObjectWithTag("nft");
        myNft.GetComponent<Renderer>().material.mainTexture = null;
        
        //Get the player's wallet address
        string account = PlayerPrefs.GetString("wallet");
        
        //Get all erc1155
        await GetErc1155ContractAndId(chain, network, account);
        Debug.Log("Done getting NFTs' information");
    }
    
    //When click the button, this function will be called
    public async void onClick()
    {
        if(i==erc1155s.Length)
        {
            i=0;
        }
        contract = erc1155s[i].contract;
        tokenId = erc1155s[i].tokenId;
        i++;

        await UpdateMaterial();
    }
    private async Task UpdateMaterial()
    {
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
        try
        {
            myNft.GetComponent<Renderer>().material.mainTexture =
                ((DownloadHandlerTexture) textureRequest.downloadHandler).texture;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private string getImageUriFromJson(Response data)
    {
        string imageUri;
        try{
            // parse json to get image uri
            imageUri = data.image;
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
            imageUri = "";
        }
        if (imageUri.StartsWith("ipfs://"))
        {
            imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
        }

        print("imageUri: " + imageUri);
        return imageUri;
    }

    private async Task<Response> getJSONfromUri(string uri)
    {
        Response data;
        // fetch json from uri
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        print("unity web request: " + webRequest);
        await webRequest.SendWebRequest();
        try
        {
            data =
                JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        return null;
    }

    private async Task<string> getTokenUri()
    {
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
        btn.gameObject.SetActive(false);
        string response = await EVM.AllErc1155(chain, network, account, "", 500, 0);
        try
        {
            erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
            //Loop through all erc1155s and get the contract and tokenId
            foreach (NFTs erc1155 in erc1155s)
            {
                contract = erc1155.contract;
                Debug.Log(contract);
                tokenId = erc1155.tokenId;
            }
            Debug.Log("You have " + erc1155s.Length + " ERC1155 NFTs");
            btn.gameObject.SetActive(true);
        }
        catch
        {
            print("Error: " + response);
            btn.gameObject.SetActive(true);
        }
    }
}