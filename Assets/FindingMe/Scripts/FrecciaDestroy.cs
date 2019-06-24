using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrecciaDestroy : MonoBehaviour
{
    public GameObject freccia;
    public GameObject player;

    private void Update()
    {
        if (player.GetComponent<Player>().velocity.x != 0)
        {
            Destroy(freccia);
        }
    }
}
