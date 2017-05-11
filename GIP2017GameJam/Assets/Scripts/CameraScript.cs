using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

	public TileManager tileManager;

	public Player player; 
	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider != null)
				{
					Debug.DrawLine(transform.position, hit.point, Color.red);
					//Debug.Log(hit.transform.GetComponent<Tile>().getType());
					//Debug.Log("Hits at: " + hit.point);

					Tile selectedTile = hit.transform.GetComponent<Tile>();

					// If tile is not selected, send it to tile manager
					if (!selectedTile.isSelected)
					{
						tileManager.SelectTile(hit.transform.GetComponent<Tile>());
					}

				}
			}
		}
		else if (!Input.GetMouseButton(0))
		{
			tileManager.RotateSelection();
		}
		//left mouse click 
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider != null)
				{
					Debug.DrawLine(transform.position, hit.point, Color.green);
					//Debug.Log(hit.transform.GetComponent<Tile>().getType());
					//Debug.Log("Hits at: " + hit.point);

					Tile selectedTile = hit.transform.GetComponent<Tile>();
					// TODO: Check the player is within vicinity 
					int playerX = player.y; 
					int playerY = player.x;

					int tileX = selectedTile.getX ();
					int tileY = selectedTile.getY (); 

					// do check with the current tile 
					int diffX = tileX - playerX; 
					int diffY = tileY - playerY; 
					Debug.Log ("playerX: " + playerX);
					Debug.Log ("playerY: " + playerY);
					Debug.Log ("tileX: " + tileX);
					Debug.Log ("tileY: " + tileY);
					Debug.Log ("diffX: " + diffX);
					Debug.Log ("diffY: " + diffY);

					// north - up  
					if (diffX == -1 && diffY == 0) {
						//check connectivity 
						if(selectedTile.canMoveS()){
							player.RunMoveUp ();
						} 

					}

					//south - down
					if (diffX == 1 && diffY == 0) {
						//check connectivity, opposite direction of the player intended direction 
						if(selectedTile.canMoveN()){
							player.RunMoveDown ();
						} 

					}

					//west - left
					if (diffX == 0 && diffY == -1) {
						//check connectivity, opposite direction of the player intended direction 
						if(selectedTile.canMoveE()){
							player.RunMoveLeft ();
						} 

					}
					//east - right
					if (diffX == 0 && diffY == 1) {
						//check connectivity, opposite direction of the player intended direction 
						if(selectedTile.canMoveW()){
							player.RunMoveRight ();
						} 

					}
				}
			}
		}
		// Right mouse click
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider != null)
				{
					Debug.DrawLine(transform.position, hit.point, Color.green);
					//Debug.Log(hit.transform.GetComponent<Tile>().getType());
					//Debug.Log("Hits at: " + hit.point);

					Tile selectedTile = hit.transform.GetComponent<Tile>();
					// TODO: Check if player is on tile
					selectedTile.ChooseTile();
				}
			}
		}
	}

}
										