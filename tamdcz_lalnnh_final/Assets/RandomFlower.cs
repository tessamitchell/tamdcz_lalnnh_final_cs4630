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
    }

    private void createFlower()
    {
        flower = flowerprefabs[Random.Range(0, flowerprefabs.Length)];
        GameObject gO = Instantiate(flower);
        Vector3Int location = new Vector3Int(GameManager.game.gridHorizontal - 1, GameManager.game.gridVertical - 1, 0);
        Vector3 pos = GameManager.game.grid.GetCellCenterWorld(location);
        pos.z = 0;
        gO.transform.position = pos;


    }

}
