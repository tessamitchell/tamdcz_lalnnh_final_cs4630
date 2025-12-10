using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    public Grid grid;
    public int[,] solution;
    public GameObject[,] cells;
    public Maze maze;
    public int gridVertical = 3;
    public int gridHorizontal = 5;

    public GameObject pipePrefab;
    public GameObject jointPrefab;
    public GameObject startPrefab;
    public GameObject goalPrefab;
    public int[] orientations = { 0, 90, 180, -90 };
    
    


    // Start is called before the first frame update
    void Start()
    {
        maze=new Maze();
        maze.makeMaze(gridVertical, gridHorizontal);
        maze.SolveMaze();
        solution=new int[gridVertical, gridHorizontal];
        LevelGen();
    }

    // Update is called once per frame
    void Update()
    {
        if (check())
        {
            Debug.Log("Solved!");
        }
    }


    void LevelGen()
    {
        cells=new GameObject[gridVertical,gridHorizontal];
        for (int i = 0; i < gridVertical; i++)
        {
            for (int j = 0; j < gridHorizontal; j++)
            {
                if(i==0 && j == 0)
                {
                    addPipe(i, j, startPrefab);
                    solution[i, j] = 0;
                }
                else if ((i==gridVertical-1) && (j == gridHorizontal - 1))
                {
                    addPipe(i, j, goalPrefab);
                    solution[i, j] = 180;
                }
                
                else if (maze.solvedMaze[i, j].above || maze.solvedMaze[i, j].below || maze.solvedMaze[i, j].right || maze.solvedMaze[i, j].left)
                {
                    blockGen(i, j);
                }
                else
                {
                    cells[i, j] = null;
                }
                
            }
        }
    }

    void blockGen(int x, int y)
    {
        Cell cell = maze.solvedMaze[x, y];
        if (cell.right && cell.above)
        {
            solution[x, y] = 0;
            addPipe(x, y, jointPrefab);
        }
        else if (cell.right && cell.below)
        {
            solution[x, y] = -90;
            addPipe(x, y, jointPrefab);
        }
        else if (cell.left && cell.above)
        {
            solution[x, y] = 90;
            addPipe(x, y, jointPrefab);
        }
        else if (cell.left && cell.below)
        {
            solution[x, y] = 180;
            addPipe(x, y, jointPrefab);
        }
        else if (cell.below && cell.above)
        {
            solution[x, y] = 0;
            addPipe(x, y, pipePrefab);
        }
        else if (cell.left && cell.right)
        {
            solution[x, y] = 90;
            addPipe(x, y, pipePrefab);
        }
        else
        {
            

        }
    }

    void addPipe(int x,int y, GameObject pipe)
    {

        GameObject gO = Instantiate(pipe);
        Vector3Int location = new Vector3Int(y, x, 0);
        Vector3 pos = grid.GetCellCenterWorld(location);
        pos.z = -1;
        gO.transform.position = pos;
        //Vector3 cellSize = grid.cellSize; // (3,3,0)
        //gO.transform.localScale = new Vector3(cellSize.x, cellSize.y, 1);

        gO.transform.Rotate(0, 0, orientations[UnityEngine.Random.Range(0, 4)]);
        cells[x, y] = gO;

    }



    bool check()
    {

        for (int i = 0; i < gridVertical; i++)
        {
            for (int j = 0; j < gridHorizontal; j++)
            {
                if (cells[i, j] != null)
                {
                    Quaternion rot = cells[i, j].transform.rotation;
                    //Debug.Log(rot.y);

                    if (rot.z == 270) rot.z = -90;
                    if (cells[i, j].CompareTag("Pipe"))
                    {
                        if (rot.z % 180 != solution[i, j])
                        {
                            return false;
                        }
                    }
                    else if (rot.z != solution[i, j])
                    {
                        return false;
                    }

                }
            }
            
        }
        return true;
    }




    public void restart()
    {
        SceneManager.LoadScene("Scene1");
    }
}
