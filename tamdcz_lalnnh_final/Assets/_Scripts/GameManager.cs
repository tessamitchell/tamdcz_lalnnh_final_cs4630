using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    public static GameManager game;

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
    public RandomFlower flower;
    public int[] orientations = { 0, -90,180, 90};

    public int[,] pipeOri;

    
    bool solved = false;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI solvedText;
    float timeLeft = 30f;

    public float prefabScale = 0.5f;
    public float prob = .1f;

    public int level = 1;



    // Start is called before the first frame update
    void Start()
    {
        game = this;
        maze=new Maze();
        maze.makeMaze(gridVertical, gridHorizontal);
        maze.SolveMaze();
        solution=new int[gridVertical, gridHorizontal];
        pipeOri = new int[gridVertical, gridHorizontal];
        initArrays(solution);
        LevelGen();
        levelText.text = "Level: " + Score.LEVEL;
        solvedText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (solved)
        {
            Debug.Log("Solved!");
            StartCoroutine(solvedMessage("Solved"));
            Debug.Log(Score.LEVEL);
            solved = false;

            if (Score.LEVEL < 3)
            {
                Invoke("NextLevel", 2f);
                Debug.Log(Score.LEVEL);
            }
            else
            {
                Score.DIFFICULTY += 0.1f;
                Invoke("restart", 2f);
            }
            Score.LEVEL++;
        }
        else if (timeLeft <= 0)
        {
      
            SceneManager.LoadScene("EndScreen");
  
        }
        else
        {
            timeLeft -= Time.deltaTime;
            int seconds = (int)timeLeft;
            timerText.text = "Time Left: " + seconds.ToString();
            scoreText.text = "Score: " + Score.SCORE.ToString();
        }
    }

    void initArrays(int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for(int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = -1;
            }
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
                    Debug.Log($"Start above {maze.solvedMaze[i, j].above} below {maze.solvedMaze[i, j].below} left {maze.solvedMaze[i, j].left} right {maze.solvedMaze[i, j].right}");
                    pipeOri[i,j]=addPipe(i, j, startPrefab);
                    if (maze.solvedMaze[i, j].right)
                    {
                        solution[i, j] = 1;
                    }
                    else if (maze.solvedMaze[i, j].below)
                    {
                        solution[i, j] = 0;
                    }
                    
                }
                else if ((i==gridVertical-1) && (j == gridHorizontal - 1))
                {
                    Debug.Log($"End above {maze.solvedMaze[i, j].above} below {maze.solvedMaze[i, j].below} left {maze.solvedMaze[i, j].left} right {maze.solvedMaze[i, j].below}");
                    pipeOri[i, j] = addPipe(i, j, goalPrefab);
                    if (maze.solvedMaze[i, j].left)
                    {
                        solution[i, j] = 3;
                    }
                    else if (maze.solvedMaze[i,j].above)
                    {
                        solution[i, j] = 2;
                    }
                    
                }
                
                else if (maze.solvedMaze[i, j].above || maze.solvedMaze[i, j].below || maze.solvedMaze[i, j].right || maze.solvedMaze[i, j].left)
                {
                    blockGen(i, j);
                }
                else if ((UnityEngine.Random.value <= Score.DIFFICULTY)  && (maze.maze[i, j].above || maze.maze[i, j].below || maze.maze[i, j].right || maze.maze[i, j].left))
                {
                    nonPathBlockGen(i, j);
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
            solution[x, y] = 1;
            pipeOri[x,y]= addPipe(x, y, jointPrefab);
        }
        else if (cell.right && cell.below)
        {
            solution[x, y] = 0;
            pipeOri[x, y] = addPipe(x, y, jointPrefab);
        }
        else if (cell.left && cell.above)
        {
            solution[x, y] = 2;
            pipeOri[x, y] = addPipe(x, y, jointPrefab);
        }
        else if (cell.left && cell.below)
        {
            solution[x, y] = 3;
            pipeOri[x, y] = addPipe(x, y, jointPrefab);
        }
        else if (cell.below && cell.above)
        {
            solution[x, y] = 0;
            pipeOri[x, y] = addPipe(x, y, pipePrefab);
        }
        else if (cell.left && cell.right)
        {
            solution[x, y] = 3;
            pipeOri[x, y] = addPipe(x, y, pipePrefab);
        }
        
    }

    void nonPathBlockGen(int x, int y)
    {
        
        switch (UnityEngine.Random.Range(0, 2))
        {
                case 0:
                    addPipe(x, y, jointPrefab,false); 
                    break;
                case 1:
                    addPipe(x, y, pipePrefab,false);
                    break;
        }
        
    }

    int addPipe(int x,int y, GameObject pipe,bool isSolution=true)
    {

        GameObject gO = Instantiate(pipe);
        Vector3Int location = new Vector3Int(y, x, 0);
        Vector3 pos = grid.GetCellCenterWorld(location);
        pos.z = -1;
        gO.transform.position = pos;
        Pipe pipeScript = gO.GetComponent<Pipe>();
        if( pipeScript != null)
        {
            pipeScript.x = x;
            pipeScript.y = y;
            pipeScript.isSolution = isSolution;
        }

        //Vector3 cellSize = grid.cellSize; // (3,3,0)
        gO.transform.localScale = new Vector3(prefabScale, prefabScale, 1);
        int ori = UnityEngine.Random.Range(0, 4);
        gO.transform.Rotate(0, 0, ori*(-90));
        cells[x, y] = gO;
        return ori;
    }



    public bool check()
    {

        for (int i = 0; i < gridVertical; i++)
        {
            for (int j = 0; j < gridHorizontal; j++)
            {
                if (solution[i,j] !=-1)
                {

                    Debug.Log($"x: {i} y: {j} solution: {solution[i, j]} ori: {pipeOri[i, j]}");
                    //Debug.Log($"x: {i} y: {j} solution: {solution[i, j] % 2} ori: {pipeOri[i, j] % 2}");
                    if (cells[i, j].CompareTag("Pipe") && (solution[i, j]%2 == pipeOri[i, j]%2))
                    {
                        continue;
                    }
                    else if (solution[i, j] != pipeOri[i, j])
                    {
                        return false;
                    }

                }
            }
            
        }
        Score.SCORE += (int)timeLeft;
        solved = true;
        color();
        return true;
    }


    public bool color()
    {
        List<Vector2Int> path = maze.path;
        foreach (Vector2Int pos in path)
        {
            changeColor(cells[pos.y, pos.x]);
        }

        return false;
    }
    IEnumerator changeColor(GameObject go)
    {
        Color c2 = Color.blue;
        Renderer renderer = go.GetComponent<Renderer>();
        float elapsedTime = 0f;

        while (elapsedTime < 1.0f)
        {
            // Calculate the interpolation value (t) between 0 and 1
            // This determines how far along the transition we are
            float t = elapsedTime / 2.0f;

            // Interpolate from startColor to endColor
            renderer.material.color = Color.Lerp(renderer.material.color, c2, t);

            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Yield control back to Unity until the next frame
            yield return null;
        }

        // Ensure the final color is exactly the end color after the loop finishes
        renderer.material.color = c2;
    }
    private IEnumerator solvedMessage(string msg)
    {
        solvedText.text = msg;
        solvedText.enabled = true;

        yield return new WaitForSeconds(1);

        solvedText.enabled = false;
    }



    public void restart()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current);
    }

    public void NextLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;
        SceneManager.LoadScene(next);
       
    }
    public void EndGame()
    {
        SceneManager.LoadScene("EndScreen");
    }
}
