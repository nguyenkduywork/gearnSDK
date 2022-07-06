using UnityEngine;
using UnityEngine.UI;

public class DiamondBalance : MonoBehaviour
{
    private int currentDiamond;
    [SerializeField] Text DiamondText;
    // Start is called before the first frame update
    void Start()
    {
        ShowDiamondBalance();
    }

    public async void ShowDiamondBalance()
    {
        currentDiamond = PlayerPrefs.GetInt("diamond");
        if(DiamondText!=null) DiamondText.text = currentDiamond.ToString();
    }
}
