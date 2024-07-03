using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CkeckPosition()
    {
        if(Camera.main.transform.position.y-transform.position.y>25)
        {
            Destroy(this.gameObject); 
        }
    }
}
