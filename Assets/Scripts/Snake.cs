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

    //Enum to store movement Direction
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;

    
    public LevelGrid levelGrid; //Variable for storing a Reference to the LevelGrid script.
    [SerializeField] private int snakeBodySize; //Variable for storing the current size of the snake
    private List<SnakeMovePosition> snakeMovePositionList; //A list of Vector2int to record the snake's past movement.
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
        gridMoveDirection = Direction.Right; 

        snakeMovePositionList = new List<SnakeMovePosition>();//Initialise new list
        snakeBodyPartList = new List<SnakeBodyPart>();//Initialise new list
    }

    private void Update() {
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)) {
            if (gridMoveDirection != Direction.Down) {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            if (gridMoveDirection != Direction.Up) {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (gridMoveDirection != Direction.Right) {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }

    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition ,gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition); //Insert new position into snakeMovePositionList.

            Vector2Int gridMoveDirectionVector;
            switch(gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }
            gridPosition += gridMoveDirectionVector; //Set Snake's new position.

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
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            UpdateSnakeBodyParts();
            

            
        }
    }

    private void UpdateSnakeBodyParts()
    {
        //Update Snake Body Parts Location
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
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
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }



    //I Think this should be in another script
    //A class for SnakeBodyParts
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        private Vector2Int gridPosition;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y, 0);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left:
                            angle = 180 + 45;
                            transform.position += new Vector3(.25f, .25f, 0f);
                            break;
                        case Direction.Right:
                            angle = 180 - 45;
                            transform.position += new Vector3(-.25f, .25f, 0f);
                            break;
                    }
                    break;

                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left:
                            angle = 180 - 45;
                            transform.position += new Vector3(.25f, -.25f, 0f);
                            break;
                        case Direction.Right:
                            angle = 180 + 45;
                            transform.position += new Vector3(-0.25f, -.25f, 0f);
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down:
                            angle = -45;
                            transform.position += new Vector3(-.25f, .25f, 0f);
                            break;
                        case Direction.Up:
                            angle = 45;
                            transform.position += new Vector3(-0.25f, -.25f, 0f);
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 90;
                            break;
                        case Direction.Down:
                            angle = 45;
                            transform.position += new Vector3(0.25f, .25f, 0f);
                            break;
                        case Direction.Up:
                            angle = -45;
                            transform.position += new Vector3(.25f, -.25f, 0f);
                            break;
                    }
                    break;

            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }



    /* 
    * Handles one Move Position from the snake. Again as it is a seperate class, I feel like this should be in a seperate script.
    */
    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }
    }
}
