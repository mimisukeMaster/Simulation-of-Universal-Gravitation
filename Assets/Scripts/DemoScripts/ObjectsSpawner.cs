using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectsSpawner : MonoBehaviour
{
    public int Count;
    public Vector3 SpawnBoundingBox;
    public GameObject SpawnObject;
    
    void Awake()
    {
        float midX = SpawnBoundingBox.x / 2;
        float midY = SpawnBoundingBox.y / 2;
        float midZ = SpawnBoundingBox.z / 2;
        for (int i = 0; i < Count; i++)
        {
            float posX = Random.Range(-midX, midX);
            float posY = Random.Range(-midY, midY);
            float posZ = Random.Range(-midZ, midZ);
            Vector3 pos = new Vector3(posX, posY, posZ);

            float rotX = Random.Range(-180, 180);
            float rotY = Random.Range(-180, 180);
            float rotZ = Random.Range(-180, 180);
            Quaternion rot = Quaternion.Euler(rotX, rotY, rotZ);
            
            Instantiate(SpawnObject, pos, rot);
        }
    }

}
