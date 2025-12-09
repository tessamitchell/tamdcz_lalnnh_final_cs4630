using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int[] grid;
    public int[,] solution;
    public Maze maze = new Maze();
    public int gridVertical = 5;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void LevelGen()
    {

    }

    void blockGen(int x, int y)
    {
        Cell cell = maze.maze[x, y];
        if (cell.below && cell.above)
        {
            solution[x, y] = 0;
        }
        else if (cell.left && cell.right)
        {
            solution[x, y] = 90;
        }
        else if (cell.right && cell.left) { }
        else if (cell.left && cell.right)
        {

        }
        else if (cell.right && cell.left) { }
        else if (c)
        {

        }
    }
}
