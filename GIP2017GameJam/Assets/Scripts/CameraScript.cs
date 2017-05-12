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
	}

	// Update is called once per frame
	void Update()
	{
		/*if (Input.GetMouseButton(0))
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
		}*/

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
				}

                if (hit.collider.tag == "Tile")
                {
                    Tile selectedTile = hit.transform.GetComponent<Tile>();
                    if (selectedTile == tileManager.playerTile)
                    {
                        selectedTile.RotateRightAndUpdate();
                        tileManager.RefreshAllTiles();
                    }
                }


                if (hit.collider.tag == "Pivot")
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    Pivot selectedPivot = hit.transform.GetComponent<Pivot>();
                    selectedPivot.RotateRight();
                }
            }
		}

        // Left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile selectedTile = hit.transform.GetComponent<Tile>();
					if (selectedTile == tileManager.playerTile) {
						selectedTile.RotateLeftAndUpdate ();
						tileManager.RefreshAllTiles ();
					} else {
						// compare distance
						int selectedX = selectedTile.getX(); 
						int selectedY = selectedTile.getY (); 
						int playerX = tileManager.playerTile.getX ();
						int playerY = tileManager.playerTile.getY (); 

						int diffX = selectedX - playerX; 
						int diffY = selectedY - playerY; 

						// move right
						if (diffX == 0 && diffY == 1) {
							Debug.Log ("X: " + diffX + ",Y: " + diffY);
							player.runMoveRight ();
						} else if (diffX == 0 && diffY == -1) { // move left
							Debug.Log ("X: " + diffX + ",Y: " + diffY);
							player.runMoveLeft ();
						} else if (diffX == 1 && diffY == 0) { // move Down
							Debug.Log ("X: " + diffX + ",Y: " + diffY);
							player.runMoveDown ();
						} else if (diffX == -1 && diffY == 0) {// move Up
							Debug.Log ("X: " + diffX + ",Y: " + diffY);
							player.runMoveUp ();
						}
					}
                }

                if (hit.collider.tag == "Pivot")
                {
                    Debug.DrawLine(transform.position, hit.point, Color.yellow);
                    Pivot selectedPivot = hit.transform.GetComponent<Pivot>();
                    selectedPivot.RotateLeft();
                }
            }
        }
    }

}
										