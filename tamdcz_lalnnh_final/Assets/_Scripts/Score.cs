using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    static private int score;
    

    static public int SCORE
    {
        get { return score; }
        set { score = value; }
    }

}
