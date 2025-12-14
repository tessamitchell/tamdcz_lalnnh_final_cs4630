using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(ChangeColor(transform.GetChild(0).gameObject));
        Debug.Log(transform.GetChild(0).gameObject);
        //ChangeColor(transform.GetChild(1).gameObject);
        //ChangeColor(transform.GetChild(2).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator ChangeColor(GameObject go)
    {
        Color c2 = Color.blue;
        Renderer renderer = go.GetComponent<Renderer>();
        float elapsedTime = 0f;

        while (elapsedTime < 2.0f)
        {
            // Calculate the interpolation value (t) between 0 and 1
            // This determines how far along the transition we are
            float t = elapsedTime / 2.0f;

            // Interpolate from startColor to endColor
            renderer.material.color = Color.Lerp(renderer.material.color, c2, t);

            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;
            //Debug.Log("doing something");
            // Yield control back to Unity until the next frame
            yield return null;
        }

        // Ensure the final color is exactly the end color after the loop finishes
        renderer.material.color = c2;
    }
}
