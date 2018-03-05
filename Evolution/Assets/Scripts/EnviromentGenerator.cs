using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentGenerator : MonoBehaviour
{
    public int Width;
    public int Height;

    public float CellSize;
    public float BioSize;

    public GameObject CellPrefab;
    public GameObject BioPrefab;

    void Start ()
    {
        CellPrefab.transform.localScale = new Vector3(CellSize, CellSize, CellSize);
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                GameObject cell = Instantiate(CellPrefab, pos, Quaternion.identity);
                cell.transform.SetParent(gameObject.transform);
                pos.x += CellSize;
            }
            pos.x = 0;
            pos.z -= CellSize;
        }
	}
	
	void Update ()
    {
		
	}
}
