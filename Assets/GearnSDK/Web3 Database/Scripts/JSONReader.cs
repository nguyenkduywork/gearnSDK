using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;
    
    [System.Serializable]
    public class Contract
    {
        public string address;
        public string id;
        public string name;
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
    }

}
