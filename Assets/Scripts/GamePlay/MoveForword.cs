using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForword : MonoBehaviour
{
    public float speed;

    public int dir;

    private Vector2 startPos;


    private void Start()
    {
        startPos=transform.position;
        transform.localScale = new Vector3(dir, 1, 1);
    }
    private void Update()
    {
        if(Mathf.Abs(transform.position.x-startPos.x)>25)
        {
            Destroy(gameObject);
        }
        Move(); 
    }
    private void Move()
    {
        transform.position += transform.right * speed *dir* Time.deltaTime;
    }
}
