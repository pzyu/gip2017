using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileManager : MonoBehaviour {

    private int rows = 5;
    private int cols = 5;

    private Tile[][] tileArray;
    private char[][] charArray;

    private string path = "../GIP2017GameJam/Assets/Maps/sample.txt";

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

    void InitializeTileArray() {
        tileArray = new Tile[rows][];
        
    }
}
