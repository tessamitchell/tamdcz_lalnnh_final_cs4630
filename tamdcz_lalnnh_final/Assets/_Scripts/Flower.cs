using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flower : MonoBehaviour
{

    public GameObject flower;
    [SerializeField] private int numOfChildren;
    [SerializeField] private List<GameObject> children;
    [SerializeField] public float waitTime = 10f;
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
            Debug.Log(rb);
            Debug.Log($"Rigidbody ready on {petal.name}, isKinematic={rb.isKinematic}");
            //if (rb != null)
            //{
            //    DestroyImmediate(rb);
            //}
        }

        //StartCoroutine(AddRig());
        Invoke("dropPetal", waitTime);
    }


    private void Update()
    {

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
            Debug.Log("Dropped petal");
        }
        Invoke("dropPetal", waitTime);
    }
}
