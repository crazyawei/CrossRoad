using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int Direction;

    public List<GameObject> spawnObject;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0.2f, Random.Range(5f, 7f));
    }
    private void Spawn()
    {
       var index= Random.Range(0,spawnObject.Count);
       var target= Instantiate(spawnObject[index], transform.position, Quaternion.identity, transform);
        target.GetComponent<MoveForword>().dir=Direction; 
    }
}
