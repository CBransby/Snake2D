/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    //Variable for storing a reference to Snake
    private static GameHandler instance;

    public static int score;

    [SerializeField] private Snake snake;

    //Variable for storing a reference to LevelGrid
    private LevelGrid levelGrid;

    private void Awake()
    {
        instance = this;
        InitializeStatic();
    }

    private void Start() {

        //assign a new 20 x 20 grid to levelGrid.
        levelGrid = new LevelGrid(20, 20);

        //Call Set up function on Snake.cs and pass it the reference too LevelGrid.cs
        snake.Setup(levelGrid);

        //Call Set up function on LevelGrid.cs and pass it the reference too Snake.cs
        levelGrid.Setup(snake);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsGamePaused() == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
            
        }
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 100;
    }

    public void InitializeStatic()
    {
        score = 0;
    }

    public static void ResumeGame()
    {
        PauseWindow.HideStatic();
        Time.timeScale = 1.0f;
    }

    public static void PauseGame()
    {
        PauseWindow.ShowStatic();
        Time.timeScale = 0.0f;
    }
    
    public bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }


    

}
