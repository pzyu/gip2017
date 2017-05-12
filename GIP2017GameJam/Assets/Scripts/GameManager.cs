using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {
	GameObject[] enemies; 
	Player player; 
	TileManager tileManager;
	// Use this for initialization
	string EnemyTag = "Enemy";
	string PlayerTag = "Player";
	bool isEnemyTurn; 
	bool isPlayerTurn; 

	void Start () {
		tileManager = (TileManager) GameObject.FindObjectOfType (typeof(TileManager));
		FindPlayer (); 
		FindEnemyList (); 
		isPlayerTurn = true; 
		isEnemyTurn = false;
	}	
	
	// Update is called once per frame
	void Update () {
		if (!player.canMove && isPlayerTurn) {
			checkForRelic (); 
			enableEnemyMovement ();
			isPlayerTurn = false;
			isEnemyTurn = true; 
		}

		if (isEnemyTurn && allEnemyHasMove()) {
			player.canMove = true; 
			isEnemyTurn = false; 
			isPlayerTurn = true; 
		}
	}

	void checkForRelic() {
		Tile playerTile = GetPlayerTile (); 
		if (playerTile.transform.childCount > 1) {
			//Debug.Log ("HI");
            Transform child = playerTile.transform.GetChild(1);
            Destroy(child.gameObject);
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

			if (hasPathBetween (enemies [i], player)) {
				print ("YO" + found);
			}
		}

		return allHasMove; 
	}
    
	public void ResetGame() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
    
	private bool[,] visited;
	private int[,] branchMap;
	private bool found;
	private int previousDirection;
	private int nextMove;

	public bool hasPathBetween(GameObject enemy, Player player) {
		Tile enemyTile = enemy.GetComponent<Enemy> ().currentTile;
		Tile playerTile = player.currentTile;
		visited = new bool[TileManager.rows, TileManager.cols];
		branchMap = new int[TileManager.rows, TileManager.cols];
		found = false;
		print ("PLAYER" + playerTile.getX () + " " + playerTile.getY ());
		visit (enemyTile, playerTile.getX(), playerTile.getY(), 0);
		return found;
	}

	// Call this if hasPathBetween is true
	public int getNextMoveDirection() {
		return nextMove;
	}

	public int countt = 0;

	private void visit(Tile origin, int targetX, int targetY, int count) {
		Tile tempTile;

		if (found || visited [origin.getX(), origin.getY()]) {
			return;
		}

		branchMap [origin.getX(), origin.getY()] = count;

		if (origin.getX () == targetX && origin.getY () == targetY) {
			// There is a path to the target coordinate
			found = true;
			print ("YO " + targetX + " " + targetY);

			string test = "";

			for (int i = 0; i < TileManager.rows; i++) {
				for (int j = 0; j < TileManager.cols; j++) {
					test += branchMap [i, j];
				}
				test += "\n";
			}

			print (test);

			// Retrace back steps to find the first move to take (enemy AI)
			// WARNING: 5x5 ONLY. REWRITE WHEN GENERALIZING
			/*while (true) {

				print ("YO " + targetX + " " + targetY + "N:" + tileManager.getTileArray () [targetX, targetY].GetComponent<Tile> ().canMoveN () + 
					"E:" + tileManager.getTileArray () [targetX, targetY].GetComponent<Tile> ().canMoveE () + 
					"S:" + tileManager.getTileArray () [targetX, targetY].GetComponent<Tile> ().canMoveS () + 
					"W:" + tileManager.getTileArray () [targetX, targetY].GetComponent<Tile> ().canMoveW ());

				//if (branchMap [targetY, targetX] - 1 == branchMap [targetY, targetX + 1]) {
					
				//} else if (

			}*/
		}
		print ("Visited: " + origin.getX () + " " + origin.getY ());
		visited [origin.getX(), origin.getY()] = true;


		for (int i = 0; i < 4; i++) {
			tempTile = origin.tileNeighbours [i];

			if (tempTile != null && origin.getConnectedNeighbours()[i]) {
				tempTile.printTile ();
				visit (tempTile, targetX, targetY, count + 1);
			}
		}
	}
}
