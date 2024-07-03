using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianManager : MonoBehaviour
{
    public float offsetY;

    public List<GameObject> terrianObjects;

    private GameObject spawnObject;

    private int lastIndex;
    //private void Start()
    //{
    //    CheckPosition(); 
    //}

    private void OnEnable()
    {
        EventHandler.GetPointEvent += OnGetPointEvent;
    }

   

    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }

    private void OnGetPointEvent(int point)
    {
        CheckPosition();
    }
    public void CheckPosition()
    {
        if(transform.position.y-Camera.main.transform.position.y<offsetY/2)
        {
            transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
            SpawnTerrian();
        }
    }
    private void SpawnTerrian()
    {
        var randomIndex=Random.Range(0, terrianObjects.Count);

        while(lastIndex==randomIndex)
        {
            randomIndex = Random.Range(0, terrianObjects.Count);
        }
        lastIndex = randomIndex;
 
        spawnObject = terrianObjects[randomIndex];

        Instantiate(spawnObject, transform.position, Quaternion.identity);
    }
}
