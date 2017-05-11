using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	GameObject[] enemies; 
	Player player; 
	// Use this for initialization
	string EnemyTag = "Enemy";
	string PlayerTag = "Player";
	bool isEnemyTurn; 
	bool isPlayerTurn; 

	void Start () {
		GetPlayer (); 
		GetEnemyList (); 
		isPlayerTurn = true; 
		isEnemyTurn = false;
	}	
	
	// Update is called once per frame
	void Update () {
		if (!player.canMove && isPlayerTurn) {
			enableEnemyMovement ();
			isPlayerTurn = false; 
			isEnemyTurn = true; 
		}

		if (isEnemyTurn && allEnemyHasMove () ) {
			player.canMove = true; 
			isEnemyTurn = false; 
			isPlayerTurn = true; 
		}

	}

	void GetEnemyList() {
		enemies = GameObject.FindGameObjectsWithTag (EnemyTag);
	}

	void GetPlayer() {
		player = GameObject.FindGameObjectWithTag (PlayerTag).GetComponent<Player>();
	}

	void enableEnemyMovement() {
		for (int i = 0; i < enemies.Length; i++) {
			enemies [i].GetComponent<Enemy> ().canMove = true;
		}
	}

	bool allEnemyHasMove() {
		bool allHasMove = true;
		for (int i = 0; i < enemies.Length; i++) {
			if (enemies[i].GetComponent<Enemy> ().canMove) {
				allHasMove = false; 
				break; 
			}
		}
		return allHasMove; 
	}

}
