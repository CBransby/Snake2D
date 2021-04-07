using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class LevelGrid
{
    //Vector to store the location of the spawned fruit.
    private Vector2Int foodGridPosition;

    //Variable to store a reference to the current foodGameObject
    public GameObject foodGameObject;

    //Variable to store a reference to Snake.cs
    private Snake snake;

    //Width of the play grid
    private static int width;

    //Height of the play grid
    private static int height;



    //Function to set the dimentions of the play area
    //called by GameHandler
    public LevelGrid(int w, int h)
    {
        width = w;
        height = h;
    }



    //Function to Setup a Reference to Snake.cs on Spawn and Call SpawnFood Function to spawn the first food.
    //Called From GameHandler
    public void Setup(Snake snake)
    {
        //Assigns the Snake.cs passed into the function to the variable on this script.
        this.snake = snake;

        //Spawns the first piece of food.
        SpawnFood();
    }

    public static void GetGridSize(int w, int h)
    {
        width = w;
        height = h;
    }


    //Function for spawning food object.
    private void SpawnFood()
    {

        /*Choses a position at random anywhere on the play grid, execpt for the edges or the space which the
         * snake head occupies.*/
        do { foodGridPosition = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1)); }
        while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);
        

        //create a new food game object, with a food sprite pulled from GameAssets class.
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;

        //Sets new GameObject "Food" to the random position selected.
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y, 0);
    }



    //Function to be triggered when the snake moves.
    //Called from Snake.cs
    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        //Check if snake's new position is the same as the food's current position.
        if (snakeGridPosition == foodGridPosition)
        {
            //If it is, destroy current food object.
            GameObject.Destroy(foodGameObject);
            GameHandler.AddScore();
            //Spawn a new piece of food at a new location
            SpawnFood();
            return true;
        }
        else {return false;}
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 1)
        {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width -1f)
        {
            gridPosition.x = 1;
        }

        if (gridPosition.y < 1)
        {
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 1;
        }

        return gridPosition;
    }
}
