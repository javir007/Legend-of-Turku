﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour {

	private Transform player;

	private NavMeshAgent nav;
	private Animator anim;
	private EnemyHealth enemyHealth;


	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		enemyHealth = GetComponent<EnemyHealth> ();
		player = GameManager.instance.Player.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.GameOver && enemyHealth.IsAlive) {
			nav.SetDestination (player.position);
		} else if ((!GameManager.instance.GameOver || GameManager.instance.GameOver) && !enemyHealth.IsAlive) {
			nav.enabled = false;
		} else {
			nav.enabled = false;
			anim.Play ("Idle");
		}
	}
}