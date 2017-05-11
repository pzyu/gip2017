using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	//initial player Position;
	public int x = 0; 
	public int y = 0;

	// tile size to help with translation; 
	public float tileSize = 5.0f; 
	private bool canMove;
	private IEnumerator moveCoroutine;
	private TileManager tileManager; 

	// Use this for initialization
	void Start () {
		canMove = true; 
		tileManager = GameObject.Find ("TileManager").GetComponent<TileManager> ();
		Vector3 startingPosition = tileManager.getFirstTilePosition();
		transform.position = new Vector3 (startingPosition.x, startingPosition.y, transform.position.z);
		tileSize = tileManager.getTileSize ();
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
		if ( x > 0 || ((x == 0) && (y == 0))) {
			x -= 1;
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3(0.0f,tileSize,0.0f); 
			print (targetPosition);
			transform.Translate (0.0f, tileSize, 0.0f);
			yield return new WaitForSeconds(0.2f); 
		}
		canMove = true;
	}

	IEnumerator MoveDown() {
		if (x < 4 || ((x == 4) && (y == 4))) {
			x += 1;  
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3(0.0f,-1 *tileSize,0.0f);
			transform.Translate (0.0f,-1 *tileSize,0.0f);
			yield return new WaitForSeconds(0.2f);
		}
		canMove = true;
	}

	IEnumerator MoveRight() { 
		if (y < 4 && (x <= 4 && x >= 0)) {
			y += 1;
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3 (tileSize, 0.0f, 0.0f);
			transform.Translate (tileSize, 0.0f, 0.0f);
			yield return new WaitForSeconds(0.2f);
		}
		canMove = true;
	}

	IEnumerator MoveLeft() {
		if (y > 0 && (x <= 4 && x >= 0)) {
			y -= 1; 
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3 (-1 * tileSize, 0.0f, 0.0f);
			transform.Translate (-1 * tileSize, 0.0f, 0.0f);
			yield return new WaitForSeconds(0.2f);
		}
		canMove = true;
	}

}
