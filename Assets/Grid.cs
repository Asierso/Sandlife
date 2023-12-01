using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    #region Grid definition
    public GameObject cube;
    private int rows = 100; 
    private int tileSize = 40;
    private int cols = 100;
    public List<GameObject> gridObject = new List<GameObject>();
    #endregion
    #region Create grid
    void Start()
    {
        gridObject.Clear();
        for(int row = 0; row < rows;row++)
        {
            for(int col = 0;col < cols; col++)
            {
                gridObject.Add(Instantiate(cube, new Vector3((col * tileSize), gameObject.transform.position.y * 2 + (row * -tileSize), cube.transform.position.z), cube.transform.rotation));
            }
        }
    }
    #endregion
}
