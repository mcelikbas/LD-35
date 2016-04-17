using UnityEngine;
using System.Collections;

public class PoundDestroy : MonoBehaviour {

    PlayerControl player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (player.isPounding)
        {
            Destroy(gameObject);
        }
    }
}
