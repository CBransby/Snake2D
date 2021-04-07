using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour
{
    
    private void Awake()
    {
        transform.Find("RetryButton").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.GameScene);
        };
        transform.Find("MenuButton").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.MainMenu);
        };
    }
}
