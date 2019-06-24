using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public GameObject player;

    public GameObject dialogs;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.GetComponent<PlayerInput>().enabled = false;
            player.directionalInput = Vector2.zero;

            dialogs.GetComponent<Animation>().Play();
        }
    }
}
