using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToGameScene : MonoBehaviour
{
    
    //Move to next scene when click on the button
    public void MoveToGameScene()
    {
        //Load the next scene
        SceneManager.LoadScene("Game");
    }
}