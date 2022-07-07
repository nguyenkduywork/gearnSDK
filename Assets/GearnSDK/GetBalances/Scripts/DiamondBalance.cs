using UnityEngine;
using UnityEngine.UI;

public class DiamondBalance : MonoBehaviour
{
    public int currentDiamond;
    [SerializeField] Text DiamondText;
    // Start is called before the first frame update
    void Awake()
    {
        currentDiamond = PlayerPrefs.GetInt("diamond");
        ShowDiamondBalance();
    }

    public void ShowDiamondBalance()
    {
        if(DiamondText!=null) DiamondText.text = currentDiamond.ToString();
    }
    
    public void setDiamondBalance(int amount)
    {
        currentDiamond = amount;
        PlayerPrefs.SetInt("diamond", currentDiamond);
        ShowDiamondBalance();
    }
}
