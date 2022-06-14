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
    
    [SerializeField]
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(retireMonnaie);
    }
    
    public void retireMonnaie()
    {
        //Get the current cash in the database
        double currentCash = Singleton<DataManager>.Instance.database.cash;
        if (currentCash <= 185000)
        {
            Notification.instance.Warning("Minimum coin required: 185000");
            Singleton<SoundManager>.Instance.Play("Notification");
            return;
        }
        Notification.instance.Confirm(delegate
        {
            //Take all cash from user
            gameManager.SetCash(-currentCash);
            Singleton<SoundManager>.Instance.Play("Purchased");
        }, "Mint 185 IRC tokens for all of your current coins?");
    }
}
