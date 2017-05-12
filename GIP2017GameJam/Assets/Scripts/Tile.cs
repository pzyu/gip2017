using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int x, y;
    private byte orientation;
    
    public float rotation = 0.0f;
    public Quaternion to = Quaternion.identity;
    private Vector3 targetPos;
    private float speed = 1000.0f;

	public bool hasNorthPath;
	public bool hasSouthPath;
	public bool hasEastPath;
	public bool hasWestPath;

	public bool isSelected;
    public AudioSource audioSource;

    public enum TYPE {
        BLANK,
        PIPE,
        CROSS,
        T,
        CORNER,
        DEAD
    }
    
    private TYPE tileType = TYPE.BLANK;

    private SpriteRenderer sr;
	public Tile[] tileNeighbours = new Tile[4]; // NESW
	private bool[] connectedNeighbours = new bool[4]; // NESW

    public Sprite[] spriteArray = new Sprite[6];

    private TextMesh debugText;
    
    // Constructor
    public void Initialize (int x, int y, int type, int rot) {
        debugText = transform.GetChild(0).GetComponent<TextMesh>();
        debugText.text = "X: " + x + " Y: " + y + "\n" + hasNorthPath + ", " + hasEastPath + ", " + hasSouthPath + ", " + hasWestPath;

        targetPos = transform.position;

        //Debug.Log("Initializing new tile: " + x + " " + y + " " + (TYPE)type);
        this.x = x;
        this.y = y;
		this.to = Quaternion.Euler (0.0f, 0.0f, rot * 90);
		this.rotation = rot * 90;

		SetType((TYPE)type);
		updatePaths ();

        sr = GetComponent<SpriteRenderer>();
        //Debug.Log(sr);
    }

    // Use this for initialization
    void Start () {
        //Debug.Log(getN() + " " + getE() + " " + getS() + " " + getW());
        audioSource = GetComponent<AudioSource>();

        targetPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, to, speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * 0.005f * Time.deltaTime);
    }

	void updatePaths() {
		hasNorthPath = (orientation & 0xC0) == 0xC0;
		hasEastPath = (orientation & 0x30) == 0x30;
		hasSouthPath = (orientation & 0x0C) == 0x0C;
		hasWestPath = (orientation & 0x03) == 0x03;
	}

    // Sets type in a less cryptic manner
    public void SetType(TYPE type) {
        tileType = type;
        //Debug.Log("Current sprite: " + sr);
        this.GetComponent<SpriteRenderer>().sprite = spriteArray[(int)type];

        switch(type) {
            case TYPE.BLANK:
                orientation = 0x00; //00000000
                break;
            case TYPE.PIPE:
                orientation = 0x33; //00110011
                break;
            case TYPE.CROSS:
                orientation = 0xFF; //11111111
                break;
            case TYPE.T:
				orientation = 0x3F; //00111111
                break;
            case TYPE.CORNER:
				orientation = 0xF0; //11110000
                break;
            case TYPE.DEAD:
				orientation = 0x0C; //00001100
                break;
            default:
                Debug.Log("Invalid type");
                break;
        }
    }

	// Updates the byte representation of the tile type
	// according to its orientation
	// Anti-clockwise
	void RotateLeft(byte b, bool animate) {
		bool temp = hasNorthPath;

		hasNorthPath = hasEastPath;
		hasEastPath = hasSouthPath;
		hasSouthPath = hasWestPath;
		hasWestPath = temp;

		if (animate) {
			rotation -= 90;
			if (rotation < 0)
				rotation += 360;
			to = Quaternion.Euler (0.0f, 0.0f, rotation);
		}
		//updatePaths ();
        //audioSource.Play();
    }

	// Updates the byte representation of the tile type
	// according to its orientation
	// Clockwise
	void RotateRight(byte b, bool animate) {
		bool temp = hasNorthPath;

		hasNorthPath = hasWestPath;
		hasWestPath = hasSouthPath;
		hasSouthPath = hasEastPath;
		hasEastPath = temp;

		if (animate) {
			rotation += 90;
			if (rotation >= 360)
				rotation -= 360;
			to = Quaternion.Euler (0.0f, 0.0f, rotation);
		}
		updatePaths ();
        //audioSource.Play();
    }

	void RotateAround(byte b, bool animate) {
		bool temp = hasNorthPath;

		hasNorthPath = hasSouthPath;
		hasSouthPath = temp;

		temp = hasEastPath;

		hasEastPath = hasWestPath;
		hasWestPath = temp;

		if (animate) {
			rotation += 180;
			if (rotation >= 360)
				rotation -= 360;
			to = Quaternion.Euler (0.0f, 0.0f, rotation);
		}
	}

	// Get X coordinate of Tile (position)
    public int getX() {
        return x;
    }

	// Get Y coordinate of Tile (position)
	public int getY() {
        return y;
    }

	public void setX(int x) {
		this.x = x;
        debugText.text = "X: " + this.x + " Y: " + this.y;
    }

	public void setY(int y) {
		this.y = y;
        debugText.text = "X: " + this.x + " Y: " + this.y;
    }

    public void setPosition(Vector3 target)
    {
        targetPos = target;
    }
		
    TYPE getType() {
        return tileType;
    }

    byte getOrientation() {
        return orientation;
    }




	public void UpdateTileNeighbours(Tile[] tileNeighbours) {
		this.tileNeighbours = tileNeighbours;
		UpdateAllNeighbours ();
	}

	public void UpdateConnectedNeighbours() {
		connectedNeighbours[3] = hasNorthPath && tileNeighbours[3] != null && tileNeighbours[3].hasSouthPath;
		connectedNeighbours[2] = hasEastPath && tileNeighbours[2] != null && tileNeighbours[2].hasWestPath;
		connectedNeighbours[1] = hasSouthPath && tileNeighbours[1] != null && tileNeighbours[1].hasNorthPath;
		connectedNeighbours[0] = hasWestPath && tileNeighbours[0] != null && tileNeighbours[0].hasEastPath;

        debugText.text = "X: " + x + " Y: " + y + "\n" + hasNorthPath + ", " + hasEastPath + ", " + hasSouthPath + ", " + hasWestPath;
    }

	public void UpdateAllNeighbours() {
		for (int i = 0; i < 4; i++) {
			if (tileNeighbours [i] != null) {
				tileNeighbours [i].UpdateConnectedNeighbours ();
			}
		}
	}

	public bool[] getConnectedNeighbours() {
		return connectedNeighbours;
	}

	public bool canMoveN() {
		return hasNorthPath && tileNeighbours [3] != null && tileNeighbours [3].hasSouthPath;
	}

	public bool canMoveE() {
		return hasEastPath && tileNeighbours[2] != null && tileNeighbours[2].hasWestPath;
	}

	public bool canMoveS() {
		return hasSouthPath && tileNeighbours[1] != null && tileNeighbours[1].hasNorthPath;
	}

	public bool canMoveW() {
		return hasWestPath && tileNeighbours[0] != null && tileNeighbours[0].hasEastPath;
	}

	private void OnMouseDown() {
    }

	public void ChooseTile() {
		RotateRight(orientation, true);
		UpdateConnectedNeighbours ();
		UpdateAllNeighbours ();
		printTile ();
	}

    public void RotateRightAndUpdate()
    {
        RotateRight(orientation, true);
        UpdateConnectedNeighbours();
        UpdateAllNeighbours();
    }

    public void RotateLeftAndUpdate()
    {
        RotateLeft(orientation, true);
        UpdateConnectedNeighbours();
        UpdateAllNeighbours();
    }

    public void Highlight()
	{
		isSelected = true;
		sr.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	}

	public void Unhighlight()
	{
		isSelected = false;
		sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}

	public void printTile() {
		Debug.Log ("X: " + x + " Y: " + y + " N:" + hasNorthPath + "," + connectedNeighbours[0] + 
			" E:" + hasEastPath + "," + connectedNeighbours[1] + " S:" + hasSouthPath + "," + connectedNeighbours[2] + " W:" + hasWestPath + "," + connectedNeighbours[3]);
	}
}
