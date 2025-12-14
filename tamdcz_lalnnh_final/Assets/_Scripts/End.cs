using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI level;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "You finished with " + Score.SCORE + " points";
        level.text = "on level " + Score.LEVEL;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("StartScreen");
    }

}
