using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	//initial player Position;
	public int x = 0; 
	public int y = 0;
	public Tile currentTile; 

	// tile size to help with translation; 
	public float tileSize = 5.0f; 
	public bool canMove;
	private IEnumerator moveCoroutine;
	private TileManager tileManager; 

	// Use this for initialization
	void Start () {
		canMove = true; 
		tileManager = GameObject.Find ("TileManager").GetComponent<TileManager> ();
		Vector3 startingPosition = tileManager.getFirstTilePosition();
		transform.position = new Vector3 (startingPosition.x, startingPosition.y, transform.position.z);
		tileSize = tileManager.getTileSize ();
		updateCurrentTile (x, y);
		// Update connected neighbours
	}
	
	// Update is called once per frame
	void Update () {
		if (canMove) {
			moveCoroutine = null; 
			if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				moveCoroutine = MoveUp (); 
				canMove = false; 
			} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				moveCoroutine = MoveDown (); 
				canMove = false; 
			} else if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				moveCoroutine = MoveLeft (); 
				canMove = false; 
			} else if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				moveCoroutine = MoveRight (); 
				canMove = false; 
			}

			if (moveCoroutine != null) {
				StartCoroutine (moveCoroutine);
			}
		}
	}

	IEnumerator MoveUp() {
		if (currentTile.canMoveN() && (y > 0 || ((x == 0) && (y == 0)))) {
			y -= 1;
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3(0.0f,tileSize,0.0f); 
			print (targetPosition);
			transform.Translate (0.0f, tileSize, 0.0f);
			updateCurrentTile (x, y);
			yield return new WaitForSeconds(0.2f); 
		}
	}

	IEnumerator MoveDown() {
		if (currentTile.canMoveS() && (y < TileManager.rows || ((x == TileManager.cols) && (y == TileManager.rows)))) {
			y += 1;  
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3(0.0f,-1 *tileSize,0.0f);
			transform.Translate (0.0f,-1 *tileSize,0.0f);
			updateCurrentTile (x, y);
			yield return new WaitForSeconds(0.2f);
		}
	}

	IEnumerator MoveRight() { 
		if (currentTile.canMoveE() && (x < TileManager.cols && (y <= TileManager.rows && y >= 0))) {
			x += 1;
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3 (-tileSize, 0.0f, 0.0f);
			transform.Translate (-tileSize, 0.0f, 0.0f);
			updateCurrentTile (x, y);
			yield return new WaitForSeconds(0.2f);
		}
	}

	IEnumerator MoveLeft() {
		if (currentTile.canMoveW() && (x > 0 && (y <= TileManager.rows && y >= 0))) {
			x -= 1; 
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3 ( tileSize, 0.0f, 0.0f);
			transform.Translate (tileSize, 0.0f, 0.0f);
			updateCurrentTile (x, y);
			yield return new WaitForSeconds(0.2f);
		}
	}

	void updateCurrentTile(int x, int y) {
		currentTile = tileManager.obtainTile (y, x);
		currentTile.UpdateConnectedNeighbours ();
		currentTile.UpdateAllNeighbours ();

		//Debug.Log ("X:" + x + " Y:" + y + " " + currentTile.canMoveN () + " " + currentTile.canMoveE () + " " + currentTile.canMoveS () + " " + currentTile.canMoveW ());
	}
}
