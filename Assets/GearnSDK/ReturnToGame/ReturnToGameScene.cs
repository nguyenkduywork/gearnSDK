using UnityEngine;
using UnityEngine.SceneManagement;

namespace GearnSDK.ReturnToGame
{
    public class ReturnToGameScene : MonoBehaviour
    {
    
        //Move to next scene when click on the button
        public void MoveToGameScene()
        {
            //Load the next scene
            SceneManager.LoadScene("Game");
        }
    }
}