using UnityEngine;
using UnityEngine.UI;

public class GetWalletAddress : MonoBehaviour
{
    [SerializeField] private Text myText;

    void Start()
    {
        myText.text = PlayerPrefs.GetString("Account");
    }
    
}