using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int x, y;
    private byte orientation;
    
    public float rotation = 0.0f;
    public Quaternion to = Quaternion.identity;
    private float speed = 1000.0f;

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
                orientation = 0x3C; //00111100
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

		rotation += 90;
		to = Quaternion.Euler (0.0f, 0.0f, rotation);
    }

	// Updates the byte representation of the tile type
	// according to its orientation
	// Clockwise
	void RotateRight(byte b) {
        byte mask = 0xff;
        orientation = (byte)(((b >> 2) | (b << 6)) & mask);
        
		rotation -= 90;
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
    int getX() {
        return x;
    }

	// Get Y coordinate of Tile (position)
    int getY() {
        return y;
    }

	// Returns true if there is a path in this direction
    bool getN() {
        byte mask = 0xC0;
        return (orientation & mask) != 0;
    }

	// Returns true if there is a path in this direction
    bool getE() {
        byte mask = 0x30;
        return (orientation & mask) != 0;
    }

	// Returns true if there is a path in this direction
    bool getS() {
        byte mask = 0x0C;
        return (orientation & mask) != 0;
    }

	// Returns true if there is a path in this direction
    bool getW() {
        byte mask = 0x03;
        return (orientation & mask) != 0;
    }

    TYPE getType() {
        return tileType;
    }

    byte getOrientation() {
        return orientation;
    }

    private void OnMouseDown() {
        RotateRight(orientation);
    }
}
