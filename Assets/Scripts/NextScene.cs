using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NextScene : MonoBehaviour
{
    AuthenticationKit authenticationKit;
    [SerializeField] GameObject authenticationKitPrefab;
    private int nextSceneIndex;
   
    private void Awake()
    {
        authenticationKit = authenticationKitPrefab.GetComponent<AuthenticationKit>();
    }
    

    public void NextLevel()
    {
        /*
        if (!authenticationKit.isConnected)
        {
            Debug.Log("Not connected");
            Debug.Log(authenticationKit.isConnected);
            return;
        }
        */
        SceneManager.LoadScene(1);
    }
    
}
