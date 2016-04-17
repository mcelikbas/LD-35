using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    private PlayerControl player;
    public GameObject checkpoint;

    public static int level = 1;
    private int lastLevel = 4;

    void Start ()
    {
        SpawnPlayer();
        Camera.main.GetComponent<CameraControl>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }



	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.R))
            RespawnPlayer();
	}


    public void NextLevel()
    {
        level++;
        if (level < lastLevel + 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(level);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
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
