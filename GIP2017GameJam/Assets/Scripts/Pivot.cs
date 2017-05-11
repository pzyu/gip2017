using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour {
    
    private ArrayList selectionList = new ArrayList();
    public TileManager tileManager;
    private AudioSource audioSource;

    public GameObject test1, test2, test3, test4;
    public int i, j;

    // Use this for initialization
    void Start () {
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Takes in tileArray, and coords to start with
    public void Initialize(int i, int j)
    {
        if (tileManager == null)
        {
            tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        }
        // Just maintain pivot coords, every rotate, pick out 4 coords from tileArray 
        this.i = i;
        this.j = j;
    }

    public void RotateLeft()
    {
        tileManager.RotateLeft(j, i);
        audioSource.Play();
    }

    public void RotateRight()
    {
        tileManager.RotateRight(j, i);
        audioSource.Play();
    }
}
