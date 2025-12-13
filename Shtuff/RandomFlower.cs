using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomFlower : MonoBehaviour
{
    [SerializeField] public GameObject flower;
    [SerializeField] private int numOfChildren;
    [SerializeField] private GameObject[] children;
    [SerializeField] public float waitTime = 10f;

    private void Start()
    {
        numOfChildren = flower.transform.childCount;
        children = new GameObject[numOfChildren];

        for (int i = 0; i < numOfChildren; i++)
        {
            children[i] = flower.transform.GetChild(i).gameObject;
        }

        //StartCoroutine(AddRig());
        dropPetal();
    }


    private void Update()
    {

    }

    private IEnumerator AddRig()
    {
        for (int i = 1; i < children.Length; i++)
        {
            GameObject child = children[i];

            // Check if the child DOESN'T have a Rigidbody
            if (child.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = child.AddComponent<Rigidbody>();
                rb.useGravity = true;

                Debug.Log($"Added Rigidbody to child {i + 1}/{children.Length}: {child.name}");
            }

            // Wait before adding Rigidbody to next child
            yield return new WaitForSeconds(waitTime);
        }

        Debug.Log("Finished adding Rigidbodies to all children!");
    }

    private void dropPetal()
    {

        int index= Random.Range(1, children.Length);
        GameObject child = children[index];
        if (child.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = child.AddComponent<Rigidbody>();
            rb.useGravity = true;

            Debug.Log($"Added Rigidbody to child {index + 1}/{children.Length}: {child.name}");
        }
        Invoke("dropPetal", waitTime);
    }
}
