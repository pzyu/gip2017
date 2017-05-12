using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
		FindPlayer (); 
		FindEnemyList (); 
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

	void FindEnemyList() {
		enemies = GameObject.FindGameObjectsWithTag (EnemyTag);
	}

	void FindPlayer() {
		player = GameObject.FindGameObjectWithTag (PlayerTag).GetComponent<Player>();
	}

	Tile GetPlayerTile() {
		return player.currentTile;
	}

	Tile GetEnemyTile(GameObject enemy) {
		return enemy.GetComponent<Enemy> ().currentTile;
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

		for (int i = 0; i < enemies.Length; i++) {
			if (GetPlayerTile() == GetEnemyTile(enemies[i])) {
				Debug.Log ("DIED");
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}

		return allHasMove; 
	}

	public void ResetGame() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	
}
