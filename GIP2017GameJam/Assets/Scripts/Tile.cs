using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int x, y;
    private byte orientation;
    
    public float rotation = 0.0f;
    public Quaternion to = Quaternion.identity;
    private float speed = 1000.0f;

	public bool isSelected;

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

    // Constructor
	public void Initialize (int x, int y, int type, int rotation) {
        //Debug.Log("Initializing new tile: " + x + " " + y + " " + (TYPE)type);
        this.x = x;
        this.y = y;
		this.to = Quaternion.Euler (0.0f, 0.0f, rotation * 90);
		SetType((TYPE)type);

		switch (rotation) {
			case 0:
				break; // 12o-clock
			case 1:
				RotateLeft(orientation); // 9o-clock
				break;
			case 2:
				RotateLeft(orientation); // 6o-clock
				RotateLeft(orientation);
				break;
			case 3:
				RotateRight(orientation); // 3-oclock
				break;
			default:
				Debug.Log("Invalid orientation. Must be 0 to 3");
				break;
		}

        sr = GetComponent<SpriteRenderer>();
        //Debug.Log(sr);
    }

    // Use this for initialization
    void Start () {
        //Debug.Log(getN() + " " + getE() + " " + getS() + " " + getW());
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, to, speed * Time.deltaTime);
    }

    // Sets type in a less cryptic manner
    void SetType(TYPE type) {
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
                orientation = 0x0F; //11110000
                break;
            case TYPE.DEAD:
				orientation = 0xC0; //11000000
                break;
            default:
                Debug.Log("Invalid type");
                break;
        }
    }

    int GetBit(byte b, int bitNumber) {
        return (b & (1 << bitNumber)) != 0 ? 1 : 0;
    }

	// Updates the byte representation of the tile type
	// according to its orientation
	// Anti-clockwise
	void RotateLeft(byte b) {
        byte mask = 0xff;
        orientation = (byte)(((b << 2) | (b >> 6)) & mask);

		rotation -= 90;
		to = Quaternion.Euler (0.0f, 0.0f, rotation);
    }

	// Updates the byte representation of the tile type
	// according to its orientation
	// Clockwise
	void RotateRight(byte b) {
        byte mask = 0xff;
        orientation = (byte)(((b >> 2) | (b << 6)) & mask);
        
		rotation += 90;
		to = Quaternion.Euler (0.0f, 0.0f, rotation);
    }

    void PrintByte(byte b) {
        string byteToPrint = "";
        for (int i = 7; i >= 0; i--) {
            byteToPrint += GetBit(b, i).ToString();
        }
        Debug.Log(byteToPrint);
    }

	// Get X coordinate of Tile (position)
    public int getX() {
        return x;
    }

	// Get Y coordinate of Tile (position)
	public int getY() {
        return y;
    }

	public void setX( int x) {
		this.x = x;
	}

	public void setY(int y) {
		this.y = y;
	}

	// Returns true if there is a path in this direction
    private bool getN() {
        byte mask = 0xC0;
        return (orientation & mask) != 0;
    }

	// Returns true if there is a path in this direction
	private bool getE() {
        byte mask = 0x30;
        return (orientation & mask) != 0;
    }

	// Returns true if there is a path in this direction
	private bool getS() {
        byte mask = 0x0C;
        return (orientation & mask) != 0;
    }

	// Returns true if there is a path in this direction
	private bool getW() {
        byte mask = 0x03;
        return (orientation & mask) != 0;
    }

    TYPE getType() {
        return tileType;
    }

    byte getOrientation() {
        return orientation;
    }




	public void UpdateTileNeighbours(Tile[] tileNeighbours) {
		this.tileNeighbours = tileNeighbours;
	}

	public void UpdateConnectedNeighbours() {
		connectedNeighbours[0] = getN() && tileNeighbours[0] != null && tileNeighbours[0].getS();
		connectedNeighbours[1] = getE() && tileNeighbours[1] != null && tileNeighbours[1].getW();
		connectedNeighbours[2] = getS() && tileNeighbours[2] != null && tileNeighbours[2].getN();
		connectedNeighbours[3] = getW() && tileNeighbours[3] != null && tileNeighbours[3].getE();
		Debug.Log ("N:" + connectedNeighbours[0] + " E:" + connectedNeighbours[1] + " S:" + connectedNeighbours[2] + " W:" + connectedNeighbours[3]);
	}

	public void UpdateAllNeighbours() {
		for (int i = 0; i < 4; i++) {
			if (tileNeighbours [i] != null) {
				tileNeighbours [i].UpdateConnectedNeighbours ();
			}
		}
	}

	public bool canMoveN() {
		return connectedNeighbours[0];
	}

	public bool canMoveE() {
		return connectedNeighbours[1];
	}

	public bool canMoveS() {
		return connectedNeighbours[2];
	}

	public bool canMoveW() {
		return connectedNeighbours[3];
	}

	private void OnMouseDown() {
    }

	public void ChooseTile() {
		RotateRight(orientation);
		UpdateConnectedNeighbours ();
		UpdateAllNeighbours ();
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
}
