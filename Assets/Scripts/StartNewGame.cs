using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class StartNewGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.Find("StartButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Loader.Load(Loader.Scene.GameScene);
        };
        
        transform.Find("ExitButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Application.Quit();
        };

    }
}
