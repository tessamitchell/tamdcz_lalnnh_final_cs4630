using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Maze
{
    public int height; //number of vertical cells
    public int width; // number of horizontal
    public Cell[,] maze;

    public int[,] directions = {{-1, 0 }, {1, 0 }, {0,-1 },{0, 1 }};
    
    public void makeMaze(int height,int weight)
    {
        


        UnionFind uf = new UnionFind(height*width);
        int entrance = getIndex(0,0);
        int exit = getIndex(width-1,height-1);
        /*
        while(find(entrance) != find(exit){
            cell1 = random cell
            cell2 = random adjacent cell
            if(find(cell1) != find(cell2){
                union(cell1,cell2)
            }
        }
        */
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




    public int[,] usableMaze()
    {

        return null;
    }

    public int getIndex(int y,int x)
    {
        return y * width + x;
    }
    
}

public class Cell
{
    int xi;
    int yi;
    public bool above = false;
    public bool below = false;
    public bool left = false;
    public bool right = false;
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
        if (parent[x] != x)
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


