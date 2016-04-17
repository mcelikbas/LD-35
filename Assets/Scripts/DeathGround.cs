using UnityEngine;
using System.Collections;

public class DeathGround : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip death;
    public GameManager gm;

    void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(death, 0.2f);
            gm.RespawnPlayer();
        }
    }
}
