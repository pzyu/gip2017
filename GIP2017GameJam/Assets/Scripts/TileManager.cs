﻿using System.Collections;
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
    
    void PrintTileArray()
    {
        string total = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                total += tileArray[i, j].GetType() + " ";
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
                    // And first is above fourth
                    if (firstTile.getX() < secondTile.getX() && firstTile.getY() < fourthTile.getY())
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
                        tileArray[firstTile.getX(), firstTile.getY()] = tileArray[secondTile.getX(), secondTile.getY()];
                        firstTile.setX(secondTile.getX());
                        firstTile.setY(secondTile.getY());

                        Debug.Log("Replacing " + secondTile.getX() + "," + secondTile.getY() + " with " + thirdTile.getX() + "," + thirdTile.getY());
                        tileArray[secondTile.getX(), secondTile.getY()] = tileArray[thirdTile.getX(), thirdTile.getY()];
                        secondTile.setX(thirdTile.getX());
                        secondTile.setY(thirdTile.getY());

                        Debug.Log("Replacing " + thirdTile.getX() + "," + thirdTile.getY() + " with " + fourthTile.getX() + "," + fourthTile.getY());
                        tileArray[thirdTile.getX(), thirdTile.getY()] = tileArray[fourthTile.getX(), fourthTile.getY()];
                        thirdTile.setX(fourthTile.getX());
                        thirdTile.setY(fourthTile.getY());

                        Debug.Log("Replacing " + fourthTile.getX() + "," + fourthTile.getY() + " with " + tempX + "," + tempY);
                        tileArray[fourthTile.getX(), fourthTile.getY()] = tileArray[tempX, tempY];
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

	private void InstantiateTile(int i, int j, int type) {
		int rotation = 0;
		tileArray[i, j] = Instantiate(tilePrefab, new Vector3(-tileSize * j, -tileSize * i, 0), Quaternion.Euler(0.0f, 0.0f, rotation * 90));
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
