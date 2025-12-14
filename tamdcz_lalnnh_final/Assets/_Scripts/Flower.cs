using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Flower : MonoBehaviour
{

    public GameObject flower;
    [SerializeField] private int numOfChildren;
    [SerializeField] private List<GameObject> children;
    [SerializeField] public float waitTime = 10f;
    public float timeElapsed = 0;
    private void Start()
    {
        flower = gameObject;
        
        numOfChildren = flower.transform.childCount;
        children = new List<GameObject>(numOfChildren);

        for (int i = 1; i < numOfChildren; i++)
        {
            GameObject petal = flower.transform.GetChild(i).gameObject;
            children.Add(petal);
            Rigidbody rb = petal.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = petal.AddComponent<Rigidbody>();

            }
            rb.isKinematic = true;
            rb.useGravity = true;
            rb.mass = 0.1f;
            rb.drag = 0.2f;
            rb.angularDrag = 0.05f;
            //Debug.Log(rb);
            //Debug.Log($"Rigidbody ready on {petal.name}, isKinematic={rb.isKinematic}");
            //if (rb != null)
            //{
            //    DestroyImmediate(rb);
            //}
        }

        //StartCoroutine(AddRig());
        Invoke("dropPetal",1f);
    }


    private void Update()
    {
        timeElapsed += Time.deltaTime;
        //if ((timeElapsed >25))
        //{
        //    waitTime = 1f;
        //}
        //else if (timeElapsed > 20)
        //{
        //    waitTime = 2f;
        //}
        //else if ((timeElapsed > 10))
        //{
        //    waitTime = 3f;
        //}
        
    }

    private void dropPetal()
    {

        int index = Random.Range(0, children.Count);
        GameObject child = children[index];
        //if (child.GetComponent<Rigidbody>() == null)
        //{
        //    Rigidbody rb = child.AddComponent<Rigidbody>();
        //    rb.useGravity = true;
        //    children.RemoveAt(index);
        //    Debug.Log($"Added Rigidbody to child {index + 1}/{children.Count}: {child.name}");
        //}

        if (child.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            //Debug.Log("Dropped petal");
            children.RemoveAt(index);
            child.GetComponent<Collider>().isTrigger = true;

        }
        if (children.Count > 0) {
            waitTime = Mathf.Max(1f, waitTime - 1f);
            Invoke("dropPetal", waitTime);
        }
        
    }
}
