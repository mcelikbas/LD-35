﻿using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    public GameManager gm;

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            gm.checkpoint = transform.gameObject;
        }
    }
}