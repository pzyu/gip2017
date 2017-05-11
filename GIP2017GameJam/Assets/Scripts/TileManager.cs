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

    private GameObject[,] tileArray;
    private int[,] intArray;
    private float tileSize;

    private string path = "../GIP2017GameJam/Assets/Maps/sample.txt";

    public GameObject tilePrefab;

    /*
        0 - BLANK,
        1 - PIPE,
        2 - CROSS,
        3 - T,
        4 - CORNER,
        5 - DEAD
    */

    // Use this for initialization
    void Start () {
        //InitializeCharArrayFromFile();
		makeRandomLevel();
		InitializeTileArray();
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
    }

	private void InstantiateTile(int i, int j, int type) {
		int rotation = Random.Range (0, 4);
		tileArray[i, j] = Instantiate(tilePrefab, new Vector3(tileSize * i, tileSize * j, 0), Quaternion.Euler(0.0f, 0.0f, rotation * 90));
		tileArray[i, j].GetComponent<Tile>().Initialize(i, j, type, rotation);
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
}
