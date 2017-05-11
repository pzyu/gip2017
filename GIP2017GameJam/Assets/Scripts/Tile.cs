using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int x, y, id;
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

    public Sprite[] spriteArray = new Sprite[6];

    // Constructor
    public void Initialize (int x, int y, int id, int type)
    {
        Debug.Log("Initializing new tile: " + id + " " + x + " " + y + " " + (TYPE)type);
        this.x = x;
        this.y = y;
        this.id = id;
        SetType((TYPE)type);
        sr = GetComponent<SpriteRenderer>();
        isSelected = false;
        Debug.Log(sr);
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
        // No checks here :(
        Debug.Log("Creating " + type.ToString());
        tileType = type;
        Debug.Log("Current sprite: " + sr);
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
            case TYPE.DEAD:         //11000000
                orientation = 0xC0;
                break;
            default:
                Debug.Log("Invalid type");
                break;
        }
    }

    int GetBit(byte b, int bitNumber) {
        return (b & (1 << bitNumber)) != 0 ? 1 : 0;
    }

    void RotateLeft(byte b) {
        //Debug.Log("Rotating Left");
        byte mask = 0xff;
        orientation = (byte)(((b << 2) | (b >> 6)) & mask);

        rotation += 90;
        to = Quaternion.Euler(0.0f, 0.0f, rotation);
    }

    void RotateRight(byte b) {
        //Debug.Log("Rotating Right");
        byte mask = 0xff;
        orientation = (byte)(((b >> 2) | (b << 6)) & mask);
        
        rotation -= 90;
        to = Quaternion.Euler(0.0f, 0.0f, rotation);
    }

    void PrintByte(byte b) {
        string byteToPrint = "";
        for (int i = 7; i >= 0; i--)
        {
            byteToPrint += GetBit(b, i).ToString();
        }
        Debug.Log(byteToPrint);
    }

    // Getters
    public int getX() {
        return x;
    }

    public int getY() {
        return y;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public void setY(int y)
    {
        this.y = y;
    }

    bool getN() {
        byte mask = 0xC0;
        return (orientation & mask) != 0;
    }

    bool getE()
    {
        byte mask = 0x30;
        return (orientation & mask) != 0;
    }

    bool getS()
    {
        byte mask = 0x0C;
        return (orientation & mask) != 0;
    }

    bool getW()
    {
        byte mask = 0x03;
        return (orientation & mask) != 0;
    }

    public TYPE getType() {
        return tileType;
    }

    byte getOrientation() {
        return orientation;
    }

    private void OnMouseDown() {
        RotateRight(orientation);
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
