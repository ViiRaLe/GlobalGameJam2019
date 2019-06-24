using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Tooltip("1 Power n1, 2 Power n2, 3 Power n3")]
    public int pUp;
    public Sprite[] spriteP;
    public float c;
    private bool goingDown = false;

    [SerializeField]
    private GameObject armaturePrefab;


    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = spriteP[pUp-1];
        c = transform.position.y;
    }

    private void Update()
    {
        if (transform.position.y < c + 0.3f && !goingDown)
        {
            transform.Translate(0f, 0.002f, 0f);
        }
        else if (transform.position.y >= c + 0.3f && !goingDown)
        {
            goingDown = true;
        }
        else if (transform.position.y > c -0.3f && goingDown)
        {
            transform.Translate(0f, -0.002f, 0f);
        }
        else if (transform.position.y <= c - 0.3f && goingDown)
        {
            goingDown = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.UpgradePower(pUp, armaturePrefab);
            Destroy(gameObject);
        }
    }
}
