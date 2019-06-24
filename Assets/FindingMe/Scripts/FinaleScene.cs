using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleScene : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            GetComponent<Animation>().Play();
        }
    }
}
