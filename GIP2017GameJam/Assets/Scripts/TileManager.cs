using System;
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

    private ArrayList selectionList = new ArrayList();

    private bool canRotate = true;

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
                tileArray[i][j] = Instantiate(tilePrefab, new Vector3(tileSize * j, -tileSize * i, 0), Quaternion.identity);
                tileArray[i][j].GetComponent<Tile>().Initialize(j, i, i + j, int.Parse(cur.ToString()));
            }
        }
    }

    void PrintTileArray()
    {
        String total = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                total += tileArray[i][j].GetType() + " ";
            }
            total += "\n";
        }
        Debug.Log(total);
    }

    public void RotateSelection()
    {
        // If selection is not full, then reset
        if (selectionList.Count < 4)
        {
            ResetSelection();
        } else
        {
            if (canRotate)
            {
                canRotate = false;
                Debug.Log("Rotating selection");

                // Otherwise, rotate based on order
                // Rotate 0 to 1 to 2 to 3
                // Check first two to determine rotation
                Tile firstTile = (Tile)selectionList[0];
                Tile secondTile = (Tile)selectionList[1];
                Tile thirdTile = (Tile)selectionList[2];
                Tile fourthTile = (Tile)selectionList[3];

                Vector3 tempPos = new Vector3(firstTile.transform.position.x, firstTile.transform.position.y, firstTile.transform.position.z);
                int tempX = firstTile.getX();
                int tempY = firstTile.getY();

                Debug.Log("First: " + firstTile.getX() + "," + firstTile.getY());
                Debug.Log("Second: " + secondTile.getX() + "," + secondTile.getY());

                // If both Y's are same level
                if (firstTile.getY() == secondTile.getY())
                {
                    // If first is left of second, then rotate clockwise
                    if (firstTile.getX() < secondTile.getX())
                    {
                        // 1 in 2
                        firstTile.transform.position = secondTile.transform.position;

                        // 2 in 3
                        secondTile.transform.position = thirdTile.transform.position;

                        // 3 in 4
                        thirdTile.transform.position = fourthTile.transform.position;

                        // 4 in 1
                        fourthTile.transform.position = tempPos;

                        Debug.Log("Replacing " + firstTile.getX() + "," + firstTile.getY() + " with " + secondTile.getX() + "," + secondTile.getY());
                        tileArray[firstTile.getX()][firstTile.getY()] = tileArray[secondTile.getX()][secondTile.getY()];
                        firstTile.setX(secondTile.getX());
                        firstTile.setY(secondTile.getY());

                        Debug.Log("Replacing " + secondTile.getX() + "," + secondTile.getY() + " with " + thirdTile.getX() + "," + thirdTile.getY());
                        tileArray[secondTile.getX()][secondTile.getY()] = tileArray[thirdTile.getX()][thirdTile.getY()];
                        secondTile.setX(thirdTile.getX());
                        secondTile.setY(thirdTile.getY());

                        Debug.Log("Replacing " + thirdTile.getX() + "," + thirdTile.getY() + " with " + fourthTile.getX() + "," + fourthTile.getY());
                        tileArray[thirdTile.getX()][thirdTile.getY()] = tileArray[fourthTile.getX()][fourthTile.getY()];
                        thirdTile.setX(fourthTile.getX());
                        thirdTile.setY(fourthTile.getY());

                        Debug.Log("Replacing " + fourthTile.getX() + "," + fourthTile.getY() + " with " + tempX + "," + tempY);
                        tileArray[fourthTile.getX()][fourthTile.getY()] = tileArray[tempX][tempY];
                        fourthTile.setX(tempX);
                        fourthTile.setY(tempY);
                    }

                    else
                    {
                        // Otherwise, rotate anti clockwise
                    }
                }
                else if (firstTile.getX() == secondTile.getX())
                {
                    // If first is top of second, then rotate clockwise
                    if (firstTile.getY() < secondTile.getY())
                    {

                    }

                    else
                    {
                        // Otherwise, rotate anti clockwise
                    }
                }

                // Now reset
                ResetSelection();
            }
        }


    }

    public void SelectTile(Tile tile)
    {
        if (selectionList.Count < 4)
        {
            // TODO: Connectivity check
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
}
