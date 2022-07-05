using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;
    
    [System.Serializable]
    public class Contract
    {
        public string contract;
        public string tokenId;
    }
    
    [System.Serializable]
    public class ContractList
    {
        public Contract[] contract;
    }
    
    public ContractList myContractList = new ContractList();
    // Start is called before the first frame update
    void Start()
    {
        myContractList = JsonUtility.FromJson<ContractList>(textJSON.text);
        
        //set the contract and tokenId to lowercase
        for (int i = 0; i < myContractList.contract.Length; i++)
        {
            myContractList.contract[i].contract = myContractList.contract[i].contract.ToLower();
            myContractList.contract[i].tokenId = myContractList.contract[i].tokenId.ToLower();
        }
    }
    
    public Contract[] getMyContractList()
    {
        return myContractList.contract;
    }

}
