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
using System;

public class Snake : MonoBehaviour {

    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;

    
    public LevelGrid levelGrid; //Variable for storing a Reference to the LevelGrid script.
    [SerializeField] private int snakeBodySize; //Variable for storing the current size of the snake
    private List<Vector2Int> snakeMovePositionList; //A list of Vector2int to record the snake's past movement.
    private List<SnakeBodyPart> snakeBodyPartList; //A list of Transforms to record the snakes location 

    //Function to Setup a Reference to LevelGrid.cs on Spawn.
    //Called from GameHandler
    public void Setup(LevelGrid levelGrid)
    {
        //Assigns the level grid passed into the function to the variable on this script.
        this.levelGrid = levelGrid;
    }
    private void Awake() {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = .25f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeMovePositionList = new List<Vector2Int>();//Initialise new list
        snakeBodyPartList = new List<SnakeBodyPart>();//Initialise new list
    }

    private void Update() {
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (gridMoveDirection.y != -1) {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (gridMoveDirection.y != +1) {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (gridMoveDirection.x != +1) {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (gridMoveDirection.x != -1) {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }
        }
    }

    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition); //Insert new position into snakeMovePositionList.
            
            gridPosition += gridMoveDirection; //Set Snake's new position.

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition); //Call TrySnakeEatFood Function from LevelGrid.cs and pass in gridPosition variable.
            {
                if (snakeAteFood) //If TrySnakeEatFood = true
                { //then Grow Snake.
                    snakeBodySize++;
                    CreateSnakeBody();
                }
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)//If snakeMovePositionList is larger than or equal to the snakeBodySize + 1.
            { // then remove the last item from the list.
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }



            /*/TESTING############ Snake body by adding white square using code monkey utils
            for(int i=0; i<snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];
                World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y, 0), Vector3.one * .5f, Color.white);
                FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            }*/

            //Update Snake Head Location and Rotation. 
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);

            UpdateSnakeBodyParts();
            

            
        }
    }

    private void UpdateSnakeBodyParts()
    {
        //Update Snake Body Parts Location
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }


    private float GetAngleFromVector(Vector2Int dir) {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }



    //A Function to return the position of the snake.
    //Called by LevelGrid.cs
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    //A Function that returns the full list of positions occupied by the snake: Head + Body.
    //called by LevelGrid.cs
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }

    //I Think this should be in another script
    //A class for SnakeBodyParts
    private class SnakeBodyPart
    {
        private Transform transform;
        private Vector2Int gridPosition;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyObject.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
        }
    }
}
