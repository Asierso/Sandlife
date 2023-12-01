using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridModifier : MonoBehaviour
{
    public List<GameObject> GridObj = new List<GameObject>();
    public Sprite GridSprite;
    public void Grid(bool Show)
    {
        GridObj = gameObject.GetComponent<Grid>().gridObject;
        if(Show == false)
        {
            GridObj.ForEach((Obj) => { Obj.GetComponent<SpriteRenderer>().sprite = null; Obj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0); });
        }
        else
        {
            GridObj.ForEach((Obj) => { Obj.GetComponent<SpriteRenderer>().sprite = GridSprite; Obj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255); });
        }
    }
}
