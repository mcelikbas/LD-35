using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    private GameManager gm;

    void Start ()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(tag == "LevelEnd")
            {
                gm.NextLevel();
            }
            gm.checkpoint = transform.gameObject;
        }
    }
}
