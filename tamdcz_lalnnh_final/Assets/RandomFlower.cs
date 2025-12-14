using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomFlower : MonoBehaviour
{
    [SerializeField] public GameObject[] flowerprefabs;
    public GameObject flower;
    [SerializeField] private int numOfChildren;
    [SerializeField] private List<GameObject> children;
    [SerializeField] public float waitTime = 10f;

    private void Start()
    {
        createFlower();
        numOfChildren = flower.transform.childCount;
        children = new List<GameObject>(numOfChildren);

        for (int i = 1; i < numOfChildren; i++)
        {
            GameObject petal = flower.transform.GetChild(i).gameObject;
            children.Add(petal);
            Rigidbody rb= petal.GetComponent<Rigidbody>();
            if(rb == null)
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

        }

        //StartCoroutine(AddRig());
        Invoke("dropPetal",waitTime);
    }


    private void Update()
    {
        
    }

    private void createFlower()
    {
        flower = flowerprefabs[Random.Range(0, flowerprefabs.Length)];
        GameObject gO = Instantiate(flower);
        Vector3Int location = new Vector3Int(GameManager.game.gridHorizontal-1, GameManager.game.gridVertical-1, 0);
        Vector3 pos = GameManager.game.grid.GetCellCenterWorld(location);
        pos.z = 0;
        gO.transform.position = pos;

        
    }


    private IEnumerator AddRig()
    {
        for (int i = 1; i < children.Count; i++)
        {
            GameObject child = children[i];

            // Check if the child DOESN'T have a Rigidbody
            if (child.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = child.AddComponent<Rigidbody>();
                rb.useGravity = true;

                Debug.Log($"Added Rigidbody to child {i + 1}/{children.Count}: {child.name}");
            }

            // Wait before adding Rigidbody to next child
            yield return new WaitForSeconds(waitTime);
        }

        Debug.Log("Finished adding Rigidbodies to all children!");
    }

    private void dropPetal()
    {

        int index= Random.Range(0, children.Count);
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
            Rigidbody rb= child.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            Debug.Log("Dropped petal");
        }
        Invoke("dropPetal", waitTime);
    }
}
