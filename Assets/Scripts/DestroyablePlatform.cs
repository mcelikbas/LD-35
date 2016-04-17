using UnityEngine;
using System.Collections;

public class DestroyablePlatform : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip explosion;


    void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.GetComponent<PlayerControl>().isPounding)
            {
                audioSource.PlayOneShot(explosion, 0.5f);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(gameObject, 1.0f);
            }
        }
    }
}
