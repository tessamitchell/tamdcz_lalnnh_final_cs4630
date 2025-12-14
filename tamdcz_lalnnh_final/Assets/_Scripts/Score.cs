using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    static private int score = 0;
    static private int level = 1;
    static private float difficulty = 0.1f;
    

    static public int SCORE
    {
        get { return score; }
        set { score = value; }
    }

    static public int LEVEL
    {
        get { return level; }
        set {  level = value; }
    }

    static public float DIFFICULTY 
    { get { return difficulty; }
        set { difficulty = value; }
    }

  

}
