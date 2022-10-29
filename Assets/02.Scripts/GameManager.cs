using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool serverConnected = false;


    void Start()
    {
        Instance = gameObject.GetComponent<GameManager>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
