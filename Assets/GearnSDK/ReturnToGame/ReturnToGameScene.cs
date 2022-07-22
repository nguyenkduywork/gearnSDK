using UnityEngine;
using UnityEngine.SceneManagement;

namespace GearnSDK.ReturnToGame
{
    public class ReturnToGameScene : MonoBehaviour
    {
    
        //Move to next scene when click on the button
        public void MoveToGameScene()
        {
            //load the first scene, please change the index to the first scene you want to load
            SceneManager.LoadScene(1);
        }
    }
}