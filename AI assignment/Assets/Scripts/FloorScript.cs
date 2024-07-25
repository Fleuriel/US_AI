using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;




    // Start is called before the first frame update
    void Start()
    {
        Player.transform.position = SpawnPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
