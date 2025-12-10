using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    private void Start()
    {

        
        
    }

    private void Update()
    {
        Vector3 mousePos = inputManager.GetSelectedPos();
        mousePos.z= 1;
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        //mouseIndicator.transform.position = mousePos;
   
        Vector3 snappedPos = grid.CellToWorld(gridPos);
        snappedPos += (grid.cellSize / 2f);
        snappedPos.z = 0f;
        
        
        cellIndicator.transform.position = snappedPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x <5; x++)
            for (int y = 0; y <3; y++)
            {
                Vector3 p = grid.CellToWorld(new Vector3Int(x, y, 0));
                p.z = 0;
                Gizmos.DrawWireCube(p + grid.cellSize / 2f, grid.cellSize);
            }
    }
}
