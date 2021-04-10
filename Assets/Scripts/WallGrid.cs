using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class WallGrid 
{
    public  List<GameObject> walls;
    public  List<Vector3> wallPositionList;
    public  GameObject wallGameObject;
    private Snake snake;
    private LevelGrid levelGrid;

    public void Setup(Snake _snake, LevelGrid _levelGrid)
    {
        snake = _snake;
        levelGrid = _levelGrid;
    }

    private static WallGrid wg;
    public void SpawnWalls()
    {
        wallPositionList = new List<Vector3>();
        walls = new List<GameObject>();
        walls.AddRange(GameObject.FindGameObjectsWithTag("Wall"));

        foreach(GameObject i in walls)
        {
            wallPositionList.Add(new Vector3(i.transform.position.x, i.transform.position.y, 0));
        }
        
        foreach(Vector3 pos in wallPositionList)
        {
            wallGameObject = new GameObject("Wall", typeof(SpriteRenderer));
            wallGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.wallSprite;
            wallGameObject.transform.position = new Vector3(pos.x, pos.y);
        }
    }
    public bool WallCheck(Vector3 foodObject)
    {
        if(wallPositionList.Contains(foodObject))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckSnakeHitWall(Vector2Int snakeGridPosition)
    {
        if(wallPositionList.Contains(new Vector3(snakeGridPosition.x, snakeGridPosition.y,0)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
