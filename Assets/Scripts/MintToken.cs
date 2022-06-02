using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MintToken : MonoBehaviour
{
    public Button yourButton;
    
    private Database database;
    
    [SerializeField]
    private Text mainScreenCash;
    
    [SerializeField]
    private Text shopScreenCash;
    // Start is called before the first frame update
    void Start()
    {
        database = Singleton<DataManager>.Instance.database;
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(retireMonnaie);
    }
    

    public void retireMonnaie()
    {
        database.cash = 0;
        GameUtilities.String.ToText(mainScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
        GameUtilities.String.ToText(shopScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
    }
}
