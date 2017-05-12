using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	//initial player Position;
	public int x = 0; 
	public int y = 0;
	public Tile currentTile; 
	// tile size to help with translation; 
	public float tileSize = 5.0f;
	public bool canMove;
	public float moveDelay = 0.5f;
	private IEnumerator moveCoroutine;
	private TileManager tileManager;

    private Vector3 targetPos;
    private float speed = 1000.0f;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
		canMove = false; 
		tileManager = GameObject.Find ("TileManager").GetComponent<TileManager> ();
		Vector3 startingPosition = tileManager.getTilePosition(x,y);
		transform.position = new Vector3 (startingPosition.x, startingPosition.y, transform.position.z);
		tileSize = tileManager.getTileSize ();
		updateCurrentTile (x, y);
        targetPos = transform.position;

        audioSource = GetComponent<AudioSource>();
    }


	// Update is called once per frame
	void Update () {
		if (canMove) {
			int moveType = Random.Range (0, 4);
			moveCoroutine = null; 
			if (moveType == 0 && currentTile.canMoveN()) {
				moveCoroutine = MoveUp (); 
				canMove = false; 
			} else if (moveType == 1 && currentTile.canMoveS()) {
				moveCoroutine = MoveDown (); 
				canMove = false; 
			} else if (moveType == 2 && currentTile.canMoveW()) {
				moveCoroutine = MoveLeft (); 
				canMove = false; 
			} else if (moveType == 3 && currentTile.canMoveE()) {
				moveCoroutine = MoveRight (); 
				canMove = false; 
			}
			if (moveCoroutine != null) {
				StartCoroutine (moveCoroutine);
			}
		}
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * 0.01f * Time.deltaTime);
    }

	IEnumerator MoveUp() {
		if (y > 0 || ((x == 0) && (y == 0))) {
			y -= 1;
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3(0.0f,tileSize,0.0f); 
			print (targetPosition);
            //transform.Translate (0.0f, tileSize, 0.0f);
            targetPos = targetPosition;
            updateCurrentTile (x, y);
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.Play();
            yield return new WaitForSeconds(moveDelay); 
		}
	}

	IEnumerator MoveDown() {
		if (y < TileManager.rows || ((x == TileManager.cols) && (y == TileManager.rows))) {
			y += 1;  
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3(0.0f,-1 *tileSize,0.0f);
            //transform.Translate (0.0f,-1 *tileSize,0.0f);
            targetPos = targetPosition;
            updateCurrentTile (x, y);
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.Play();
            yield return new WaitForSeconds(moveDelay);
		}
	}

	IEnumerator MoveRight() { 
		if (x < TileManager.cols && (y <= TileManager.rows && y >= 0)) {
			x += 1;
			print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3 (-tileSize, 0.0f, 0.0f);
            //transform.Translate (-tileSize, 0.0f, 0.0f);
            targetPos = targetPosition;
            updateCurrentTile (x, y);
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.Play();
            yield return new WaitForSeconds(moveDelay);
		}
	}

	IEnumerator MoveLeft() {
		if (x > 0 && (y <= TileManager.rows && y >= 0)) {
			x -= 1; 
			//print (x + ", " + y);
			Vector3 targetPosition = transform.position + new Vector3 ( tileSize, 0.0f, 0.0f);
            //transform.Translate (tileSize, 0.0f, 0.0f);
            targetPos = targetPosition;
            updateCurrentTile (x, y);
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.Play();
            yield return new WaitForSeconds(moveDelay);
		}
	}

	void updateCurrentTile(int x, int y) {
		currentTile = tileManager.obtainTile (y, x);
        tileManager.enemyTile = currentTile;
        
        tileManager.RefreshAllTiles();

        //currentTile.UpdateConnectedNeighbours ();
		//currentTile.UpdateAllNeighbours ();
		//Debug.Log ("X:" + x + " Y:" + y + " " + currentTile.canMoveN () + " " + currentTile.canMoveE () + " " + currentTile.canMoveS () + " " + currentTile.canMoveW ());
	}
}
