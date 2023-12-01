using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0f));
        }
        catch
        { 
        }
    }
}
