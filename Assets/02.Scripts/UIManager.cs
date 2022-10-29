using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ClientCountText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClientCountText.text = "Client Count = " + PlayerManager.Instance.GetPlayersCount().ToString();
    }
}
