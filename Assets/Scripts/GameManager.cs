using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    private PlayerControl player;
    public GameObject checkpoint;


    void Start ()
    {
        SpawnPlayer();
        Camera.main.GetComponent<CameraControl>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }



	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Keypad0))
            RespawnPlayer();
	}


    void SpawnPlayer ()
    {
        GameObject playerObject = Instantiate(playerPrefab, checkpoint.transform.position, Quaternion.identity) as GameObject;
        player = playerObject.GetComponent<PlayerControl>();
    }

    public void RespawnPlayer ()
    {
        player.transform.position = checkpoint.transform.position;
    }
}
