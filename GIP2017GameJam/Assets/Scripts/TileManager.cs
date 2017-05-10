using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileManager : MonoBehaviour {

    private int rows = 5;
    private int cols = 5;

    private GameObject[][] tileArray;
    private char[][] charArray;
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
        InitializeCharArrayFromFile();
        InitializeTileArray();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Reads text file and initializes it into charArray
    // TODO: Verify rows and cols with file
    void InitializeCharArrayFromFile() {
        string[] result = File.ReadAllLines(path);
        charArray = new char[result[0].Length][];
        for (int i = 0; i < result.Length; i++) {
            char[] tempArr = result[i].ToCharArray();
            charArray[i] = tempArr;
            Debug.Log(new string(tempArr));
        }
    }

    // Reads from charArray and converts each one to the corresponding tile
    void InitializeTileArray() {
        tileArray = new GameObject[rows][];

        tileSize = tilePrefab.GetComponent<Renderer>().bounds.size.x;

        for (int i = 0; i < rows; i++) {
            tileArray[i] = new GameObject[cols];
            for (int j = 0; j < cols; j++) {
                char cur = charArray[i][j];
                
                // Instantiate then initialize
                tileArray[i][j] = Instantiate(tilePrefab, new Vector3(tileSize * i, tileSize * j, 0), Quaternion.identity);
                tileArray[i][j].GetComponent<Tile>().Initialize(i, j, i + j, int.Parse(cur.ToString()));
            }
        }
    }
}
