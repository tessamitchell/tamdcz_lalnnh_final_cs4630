using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public int x, y;
    public bool isSolution = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnMouseOver()
    {
        //Debug.Log("over");
        if (Input.GetMouseButtonDown(0)) {
            //Debug.Log("click");
            this.transform.Rotate(new Vector3(0, 0, -90));
            if (isSolution)
            {
                
                GameManager.game.pipeOri[x, y] = (GameManager.game.pipeOri[x, y] + 1) % 4;
                //Debug.Log($"x: {x} y: {y} ori: {GameManager.game.pipeOri[x, y]}");
                GameManager.game.check();
            }
        }
    }
}
