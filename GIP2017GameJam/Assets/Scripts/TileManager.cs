using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public static int rows = 5;
    public static int cols = 5;

	public int numRelics = 1;
	public int numEnemies = 1;
	public int numBlanks = 0;

	public int[] tileDistribution = {1, 1, 1, 1, 4, 4, 4, 4, 3, 3, 3, 2};

    public GameObject[,] tileArray;
    private int[,] intArray;
    private float tileSize;

    private string path = "../GIP2017GameJam/Assets/Maps/sample.txt";

    public GameObject tilePrefab;
    public GameObject pivotPrefab;

    private GameObject[,] pivotArray;

	private ArrayList selectionList = new ArrayList();
	private bool canRotate = true;

    /*
        0 - BLANK,
        1 - PIPE,
        2 - CROSS,
        3 - T,
        4 - CORNER,
        5 - DEAD
    */

    // Use this for initialization
    void Awake () {
        //InitializeCharArrayFromFile();
		makeRandomLevel();
		InitializeTileArray();
        InitializePivots();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Reads text file and initializes it into charArray
    // TODO: Verify rows and cols with file
	void InitializeCharArrayFromFile() {
		string[] result = File.ReadAllLines(path);
		intArray = new int[result.Length, result[0].Length];
		for (int i = 0; i < result.Length; i++) {
			char[] tempArr = result[i].ToCharArray();
			for (int j = 0; j < result [i].Length; j++) {
				intArray[i, j] = int.Parse(tempArr[j].ToString());
			}
		}
	}

    // Reads from charArray and converts each one to the corresponding tile
    void InitializeTileArray() {
        tileArray = new GameObject[rows, cols];
        tileSize = tilePrefab.GetComponent<Renderer>().bounds.size.x;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
				InstantiateTile(i, j, intArray[i, j]);
            }
        }

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				if (i - 1 < 0) {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[0] = null;
				} else {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[0] = tileArray[i - 1, j].GetComponent<Tile>();
				}

				if (j + 1 > cols - 1) {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[1] = null;
				} else {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[1] = tileArray[i, j + 1].GetComponent<Tile>();
				}

				if (i + 1 > rows - 1) {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[2] = null;
				} else {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[2] = tileArray[i + 1, j].GetComponent<Tile>();
				}

				if (j - 1 < 0) {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[3] = null;
				} else {
					tileArray[i, j].GetComponent<Tile>().tileNeighbours[3] = tileArray[i, j - 1].GetComponent<Tile>();
				}
			}
		}
    }

	private void InstantiateTile(int i, int j, int type) {
		int rotation = 0;
		tileArray[i, j] = Instantiate(tilePrefab, new Vector3(-tileSize * j, -tileSize * i, 0), Quaternion.Euler(0.0f, 0.0f, rotation * 90));
		tileArray[i, j].GetComponent<Tile>().Initialize(i, j, type, rotation);
	}

    private void InitializePivots()
    {
        pivotArray = new GameObject[4, 4];
        for (int i = 1; i < 5; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                pivotArray[i - 1, j] = Instantiate(pivotPrefab, new Vector3(-tileSize/2 + (-tileSize * j), 
                    tileSize/2 + (-tileSize * i), 0.1f), Quaternion.Euler(0.0f, 0.0f, 90));
                pivotArray[i - 1, j].GetComponent<Pivot>().Initialize(i - 1, j);
            }
        }
    }

    public void RotateLeft(int j, int i)
    {

        Tile firstTile = tileArray[i, j + 1].GetComponent<Tile>();
        Tile secondTile = tileArray[i, j].GetComponent<Tile>();
        Tile thirdTile = tileArray[i + 1, j].GetComponent<Tile>();
        Tile fourthTile = tileArray[i + 1, j + 1].GetComponent<Tile>();

        GameObject temp = tileArray[i + 1, j + 1];

        Vector3 tempPos = new Vector3(fourthTile.transform.position.x, fourthTile.transform.position.y, fourthTile.transform.position.z);

        // 4 go to 1
        fourthTile.transform.position = firstTile.transform.position;

        // 1 go to 2
        firstTile.transform.position = secondTile.transform.position;

        // 2 go to 3
        secondTile.transform.position = thirdTile.transform.position;

        // 3 go to 4
        thirdTile.transform.position = tempPos;
        
        
        Debug.Log("Before rotation");
        Debug.Log(secondTile.getX() + "," + secondTile.getY() + " | " + firstTile.getX() + "," + firstTile.getY());
        Debug.Log(thirdTile.getX() + "," + thirdTile.getY() + " | " + fourthTile.getX() + "," + fourthTile.getY());
        
        // Put 3 in 4
        tileArray[i + 1, j + 1] = tileArray[i + 1, j];

        // Put 2 in 3
        tileArray[i + 1, j] = tileArray[i, j];

        // Put 1 in 2
        tileArray[i, j] = tileArray[i, j + 1];

        // Put 4 in 1
        tileArray[i, j + 1] = temp;


        firstTile = tileArray[i, j + 1].GetComponent<Tile>();
        secondTile = tileArray[i, j].GetComponent<Tile>();
        thirdTile = tileArray[i + 1, j].GetComponent<Tile>();
        fourthTile = tileArray[i + 1, j + 1].GetComponent<Tile>();

        Debug.Log("After rotation");
        Debug.Log(secondTile.getX() + "," + secondTile.getY() + " | " + firstTile.getX() + "," + firstTile.getY());
        Debug.Log(thirdTile.getX() + "," + thirdTile.getY() + " | " + fourthTile.getX() + "," + fourthTile.getY());

		int tempX = firstTile.getX();
		int tempY = firstTile.getY();

		firstTile.setX (secondTile.getX ());
		firstTile.setY (secondTile.getY ());
		secondTile.setX (thirdTile.getX ());
		secondTile.setY (thirdTile.getY ());
		thirdTile.setX (fourthTile.getX ());
		thirdTile.setY (fourthTile.getY ());
		fourthTile.setX (tempX);
		fourthTile.setY (tempY);

		foreach (Tile tile in getTileNeighbours(firstTile)) {
			if (tile != null) {
				
			}
		}

		// Extremely inefficient
		foreach (Tile tile in getTileNeighbours(firstTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}
		foreach (Tile tile in getTileNeighbours(secondTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}
		foreach (Tile tile in getTileNeighbours(thirdTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}
		foreach (Tile tile in getTileNeighbours(fourthTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}

		int count = 0;
		secondTile.printTile ();
		foreach (Tile tile in getTileNeighbours(secondTile)) {
			if (tile != null) {
				tile.printTile ();
			}
		}

    }

	public Tile[] getTileNeighbours(Tile tile) {
		int x = tile.getX ();
		int y = tile.getY ();

		Tile[] tileNeighbours = new Tile[4];

		if (x == 0) {
			tileNeighbours [3] = null;
			tileNeighbours [1] = tileArray[x + 1, y].GetComponent<Tile>();
		} else if (x == cols - 1) {
			tileNeighbours [1] = null;
			tileNeighbours [3] = tileArray[x - 1, y].GetComponent<Tile>();
		} else {
			tileNeighbours [1] = tileArray[x - 1, y].GetComponent<Tile>();
			tileNeighbours [3] = tileArray[x + 1, y].GetComponent<Tile>();
		}

		if (y == 0) {
			tileNeighbours [0] = null;
			tileNeighbours [2] = tileArray[x, y + 1].GetComponent<Tile>();
		} else if (y == rows - 1) {
			tileNeighbours [2] = null;
			tileNeighbours [0] = tileArray[x, y - 1].GetComponent<Tile>();
		} else {
			tileNeighbours [0] = tileArray[x, y - 1].GetComponent<Tile>();
			tileNeighbours [2] = tileArray[x, y + 1].GetComponent<Tile>();
		}

		return tileNeighbours;
	}

    public void RotateRight(int j, int i)
    {
        //Debug.Log("Rotating from " + i + ", " + j);

        Tile firstTile = tileArray[i, j].GetComponent<Tile>();
        Tile secondTile = tileArray[i, j + 1].GetComponent<Tile>();
        Tile thirdTile = tileArray[i + 1, j + 1].GetComponent<Tile>();
        Tile fourthTile = tileArray[i + 1, j].GetComponent<Tile>();

        GameObject temp = tileArray[i + 1, j];

        Vector3 tempPos = new Vector3(firstTile.transform.position.x, firstTile.transform.position.y, firstTile.transform.position.z);
        int tempX = firstTile.getX();
        int tempY = firstTile.getY();

        // 1 go to 2
        firstTile.transform.position = secondTile.transform.position;

        // 2 go to 3
        secondTile.transform.position = thirdTile.transform.position;

        // 3 go to 4
        thirdTile.transform.position = fourthTile.transform.position;

        // 4 go to 1
        fourthTile.transform.position = tempPos;

        /*
        Debug.Log("Before rotation");
        Debug.Log(firstTile.getX() + "," + firstTile.getY() + " | " + secondTile.getX() + "," + secondTile.getY());
        Debug.Log(fourthTile.getX() + "," + fourthTile.getY() + " | " + thirdTile.getX() + "," + thirdTile.getY());
        */

        // Put 3 in 4
        tileArray[i + 1, j] = tileArray[i + 1, j + 1];

        // Put 2 in 3
        tileArray[i + 1, j + 1] = tileArray[i, j + 1];

        // Put 1 in 2
        tileArray[i, j + 1] = tileArray[i, j];

        // Put 4 in 1
        tileArray[i, j] = temp;

        
        firstTile = tileArray[i, j].GetComponent<Tile>();
        secondTile = tileArray[i, j + 1].GetComponent<Tile>();
        thirdTile = tileArray[i + 1, j + 1].GetComponent<Tile>();
        fourthTile = tileArray[i + 1, j].GetComponent<Tile>();

        Debug.Log("After rotation");
        Debug.Log(firstTile.getX() + "," + firstTile.getY() + " | " + secondTile.getX() + "," + secondTile.getY());
        Debug.Log(fourthTile.getX() + "," + fourthTile.getY() + " | " + thirdTile.getX() + "," + thirdTile.getY());
        
		foreach (Tile tile in getTileNeighbours(firstTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}
		foreach (Tile tile in getTileNeighbours(secondTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}
		foreach (Tile tile in getTileNeighbours(thirdTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}
		foreach (Tile tile in getTileNeighbours(fourthTile)) {
			if (tile != null) tile.UpdateTileNeighbours (getTileNeighbours (tile));
		}
    }

    public void SelectTile(Tile tile) {
		if (selectionList.Count < 4) {
			// TODO: Check tile selection and player
			selectionList.Add(tile);
			tile.Highlight();
		}
	}

	public void ResetSelection()
	{
		canRotate = true;
		for (int i = 0; i < selectionList.Count; i++)
		{
			Tile tile = (Tile)selectionList[i];
			selectionList.RemoveAt(i);
			tile.Unhighlight();
		}
	}



	public void makeRandomLevel() {
		int x = 0, y = 0;
		int totalLength = rows + cols - 2;

		int[,] path = new int[rows, cols];
		intArray = new int[rows, cols];

		// Traverse from a corner to the opposite corner randomly (distance is always decreasing)
		while (x + y < totalLength) {
			path[y, x] = 1;

			if (Random.value < 0.5) {
				x++;
			} else {
				y++;
			}

			if (x >= rows - 1) {
				for (int i = y; i < rows; i++) {
					// Non-empty tile: 2 to 4 connections
					path[i, x] = 1;
				}
				break;
			}

			if (y >= cols - 1) {
				for (int j = x; j < cols; j++) {
					// Non-empty tile: 2 to 4 connections
					path[y, j] = 1;
				}
				break;
			}
		}

		for (y = 0; y < rows; y++) {
			for (x = 0; x < cols; x++) {
				if (path [y, x] == 0) {
					intArray [y, x] = tileDistribution[Random.Range(0, tileDistribution.Length)];
				} else if ((y > 0 && y < rows - 1 && path[y - 1, x] == 1 && path[y + 1, x] == 1) ||
					(x > 0 && x < cols - 1 && path[y, x - 1] == 1 && path[y, x + 1] == 1)) {
					// If 3 consecutive tiles are in a straight line 
					// choose from PIPE / CROSS / T (1,2,3)
					intArray[y, x] = Random.Range (1, 4);
				} else {
					// Else choose from CORNER / CROSS / T (2,3,4)
					intArray[y, x] = Random.Range (2, 5);
				}
			}
		}
	}

	public Vector3 getFirstTilePosition(){
		return tileArray[0, 0].transform.position;
	}

	public Vector3 getTilePosition(int x, int y){
		return tileArray[x, y].transform.position;
	}

	public float getTileSize() {
		return tileSize;
	}

	public Tile obtainTile(int x, int y){
		return tileArray[x, y].GetComponent<Tile>();
	}
}
