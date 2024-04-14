using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundGenerator : MonoBehaviour
{
    public List<GameObject> GroundVariatons;
    public int RoomSize;
    public float RingDistance;
    public float TileSize;

    private void Start()
    {
        for(int i = -RoomSize/2; i < RoomSize/2; i++)
        {
            for (int j = -RoomSize/2; j < RoomSize/2; j++)
            {
                var groundInstance = Instantiate(GroundVariatons.OrderBy(o => Guid.NewGuid()).FirstOrDefault());
                groundInstance.transform.SetParent(transform);
                groundInstance.transform.position = new Vector3(i * TileSize, -0.1f, j * TileSize);
                if(Vector3.Distance(transform.position, groundInstance.transform.position) > RingDistance)
                {
                    Destroy(groundInstance.gameObject);
                }
            }
        }
    }
}