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
    //variable for custom map sizing. --# means that belongs to this set of variables 
    public int h;
    public int w;
    private GameObject gridBackground;
    public Camera mainCamera;


    //Variable for storing a reference to Snake
    private static GameHandler instance;

    public static int score;

    [SerializeField] private Snake snake;

    private WallGrid wallGrid;

    //Variable for storing a reference to LevelGrid
    private LevelGrid levelGrid;

    private static bool gameIsPaused = false;

    private void Awake()
    {
        wallGrid = new WallGrid();
        mainCamera = Camera.main;
        instance = this;
        LevelGrid.GetGridSize(w, h); //--#
        gridBackground = GameObject.FindGameObjectWithTag("Background");//--#
        SetupPlayArea();//--#
        InitializeStatic();
        wallGrid.SpawnWalls();

    }

    private void Start() {

        
        //assign a new 20 x 20 grid to levelGrid.
        levelGrid = new LevelGrid(w, h);

        //Call Set up function on Snake.cs and pass it the reference too LevelGrid.cs
        snake.Setup(levelGrid, wallGrid);

        //Call Set up function on LevelGrid.cs and pass it the reference too Snake.cs
        levelGrid.Setup(snake, wallGrid);

        wallGrid.Setup(snake, levelGrid);


        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused != false)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void SetupPlayArea()//--#
    {
        gridBackground.GetComponent<RectTransform>().localScale = new Vector3(w, h);
        gridBackground.GetComponent<RectTransform>().position = new Vector3(w / 2, h / 2);
        mainCamera.transform.position = new Vector3(w / 2, h / 2, -10);
        mainCamera.orthographicSize = h / 2 + 2;
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

    public static void PauseGame()
    {
        Time.timeScale = 0;
        PauseWindow.ShowStatic();
        gameIsPaused = true;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
        PauseWindow.HideStatic();
        gameIsPaused = false;
    }


    

}
