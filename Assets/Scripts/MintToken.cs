using System;
using System.Collections;
using System.Collections.Generic;
using Jint.Parser.Ast;
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
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(retireMonnaie);
    }
    


    public void retireMonnaie()
    {
        database = Singleton<DataManager>.Instance.database;

        if (!(database.cash > 0))
        {
            Debug.Log("You don't have enough money");
            return;
        }
        
        database.cash = 0;
        //Update in game cash's text to new values
        GameUtilities.String.ToText(mainScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
        GameUtilities.String.ToText(shopScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
    }
}
