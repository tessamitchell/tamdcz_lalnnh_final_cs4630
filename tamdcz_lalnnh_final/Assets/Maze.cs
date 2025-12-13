using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Maze
{
    public int height; //number of vertical cells
    public int width; // number of horizontal
    public Cell[,] maze;
    public Cell[,] solvedMaze;

    public int[,] directions = {{-1, 0 }, {1, 0 }, {0,-1 },{0, 1 }};
    public List<Vector2Int> path;


    public void makeMaze(int height,int width)
    {
        this.height = height;
        this.width = width;
        maze=new Cell[height,width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                maze[i, j] = new Cell();
            }
        }

        UnionFind uf = new UnionFind(height*width);
        int entrance = getIndex(0,0);
        int exit = getIndex(height-1, width - 1);
        
        while(uf.Find(entrance) != uf.Find(exit)){
            int x1=Random.Range(0,width);
            int y1 = Random.Range(0,height);



            int ind = Random.Range(0, 4);
            int dx = directions[ind, 1];
            int dy = directions[ind, 0];

            int x2 = x1 + dx;
            int y2 = y1 + dy;

            if (y2 < 0 || y2 >= height || x2 < 0 || x2 >= width)
            {
                continue;
            }
            int idx1 = getIndex(y1, x1);
            int idx2 = getIndex(y2, x2);

            
            if (uf.Find(idx1) != uf.Find(idx2))
            {
                uf.Union(idx1, idx2);
                RemoveWall(y1,x1,y2,x2);
            }
        }
    }

    public void RemoveWall(int y1,int x1, int y2, int x2)
    {
        if (y1 == y2)
        {
            if (x1 < x2)        // cell2 is to the right
            {
                maze[y1, x1].right = true;
                maze[y2, x2].left = true;
            }
            else                // cell2 is to the left
            {
                maze[y1, x1].left = true;
                maze[y2, x2].right = true;
            }
        }
        // Same column  vertical neighbors
        else
        {
            if (y1 < y2)        // cell2 is below
            {
                maze[y1, x1].below = true;
                maze[y2, x2].above = true;
            }
            else                // cell2 is above
            {
                maze[y1, x1].above = true;
                maze[y2, x2].below = true;
            }
        }
    }


    public int getIndex(int y,int x)
    {
        return y * width + x;
    }


    public void SolveMaze()
    {
        path = MazeSolver.FindPath(this);

    
        solvedMaze = new Cell[height, width];
  
        

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                solvedMaze[y, x] = new Cell
                {
                    above = maze[y, x].above,
                    below = maze[y, x].below,
                    left = maze[y, x].left,
                    right = maze[y, x].right,
                    isPath = false
                };
            }
        }

        // 3. Mark the solution path
        foreach (var pos in path)
        {
            solvedMaze[pos.y, pos.x].isPath = true;
        }

        


        // 4. Prune all non-path connections
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!solvedMaze[y, x].isPath)
                {
                    // Remove all walls for cells not on the path
                    solvedMaze[y, x].above = false;
                    solvedMaze[y, x].below = false;
                    solvedMaze[y, x].left = false;
                    solvedMaze[y, x].right = false;
                }
                else
                {
                    // For path cells, remove walls that lead to non-path neighbors
                    if (y > 0 && !solvedMaze[y - 1, x].isPath) solvedMaze[y, x].above = false;
                    if (y < height - 1 && !solvedMaze[y + 1, x].isPath) solvedMaze[y, x].below = false;
                    if (x > 0 && !solvedMaze[y, x - 1].isPath) solvedMaze[y, x].left = false;
                    if (x < width - 1 && !solvedMaze[y, x + 1].isPath) solvedMaze[y, x].right = false;
                }
            }
        }
        //if (path.Count > 1)
        //{
        //    Vector2Int goal = path[path.Count - 1];
        //    Vector2Int prev = path[path.Count - 2];

        //    Cell c = solvedMaze[goal.y, goal.x];

        //    // Close everything
        //    c.above = c.below = c.left = c.right = false;

        //    // Open only the direction back toward the path
        //    if (prev.y == goal.y - 1) c.above = true;
        //    else if (prev.y == goal.y + 1) c.below = true;
        //    else if (prev.x == goal.x - 1) c.left = true;
        //    else if (prev.x == goal.x + 1) c.right = true;
        //}


        //if (path.Count > 1)
        //{
        //    // Enforce exactly one exit at the start
        //    Vector2Int start = path[0];
        //    Vector2Int next = path[1];

        //    Cell startCell = solvedMaze[start.y, start.x];
        //    Cell nextCell = solvedMaze[next.y, next.x];

        //    // Close all start exits
        //    startCell.above = startCell.below = startCell.left = startCell.right = false;

        //    // Open only the correct one
        //    if (next.x == start.x) 
        //    {
        //        startCell.below = true;
        //    }
        //    else
        //    {
        //        startCell.right = true;
        //    }

        //}
    }





}

public class Cell
{
    public bool above = false;
    public bool below = false;
    public bool left = false;
    public bool right = false;
    public bool isPath = false;
}


public class UnionFind
{
    private int[] parent;
    private int[] rank;

    public UnionFind(int size)
    {
        parent= new int[size];
        rank = new int[size];
        for (int i = 0; i<size; i++)
        {
            parent[i] = i;
        }
    }

    public int Find(int x)
    {
        //Debug.Log(x);
        if (x<parent.Length && parent[x] != x)
        {
            parent[x]=Find(parent[x]);

        }
        return parent[x];
    }
    public bool Union (int a, int b)
    {
        int ra=Find(a);
        int rb=Find(b);

        if (ra == rb)
        {
            return false;
        }
        if (rank[ra] < rank[rb])
            parent[ra] = rb;
        else if (rank[rb] < rank[ra])
            parent[rb] = ra;
        else
        {
            parent[rb] = ra;
            rank[ra]++;
        }

        return true;
    }
}

public class MazeSolver
{
    public static List<Vector2Int> FindPath(Maze maze)
    {
        int height = maze.height;
        int width = maze.width;

        Vector2Int start = new Vector2Int(0, 0);
        Vector2Int end = new Vector2Int(width - 1, height - 1);

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] visited = new bool[height, width];
        Vector2Int[,] parent = new Vector2Int[height, width];

        queue.Enqueue(start);
        visited[start.y, start.x] = true;
        parent[start.y, start.x] = new Vector2Int(-1, -1); // mark start parent as invalid

        int[,] directions = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == end)
                break;

            Cell cell = maze.maze[current.y, current.x];

            // check neighbors
            for (int i = 0; i < 4; i++)
            {
                int ny = current.y + directions[i, 0];
                int nx = current.x + directions[i, 1];

                if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                    continue;

                if (visited[ny, nx])
                    continue;

                // Check if wall is open in the direction
                bool canMove = false;
                if (i == 0 && cell.above) canMove = true; // up
                if (i == 1 && cell.below) canMove = true; // down
                if (i == 2 && cell.left) canMove = true;  // left
                if (i == 3 && cell.right) canMove = true; // right

                if (!canMove) continue;

                visited[ny, nx] = true;
                parent[ny, nx] = current;
                queue.Enqueue(new Vector2Int(nx, ny));
            }
        }

        // reconstruct path
        List<Vector2Int> path = new List<Vector2Int>();
        if (!visited[end.y, end.x])
        {
            Debug.Log("No path found!");
            return path; // empty path
        }

        Vector2Int step = end;
        while (step.x != -1 && step.y != -1)
        {
            path.Add(step);
            step = parent[step.y, step.x];
        }
        path.Reverse();
        return path;
    }
}