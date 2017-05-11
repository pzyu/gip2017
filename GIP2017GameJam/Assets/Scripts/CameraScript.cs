using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

	public TileManager tileManager;

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
                    selectedTile.RotateRightAndUpdate();
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
                    selectedTile.RotateLeftAndUpdate();
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
										