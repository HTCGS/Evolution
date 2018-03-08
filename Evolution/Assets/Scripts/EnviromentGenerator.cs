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

    [Space(10)]
    public float SunIntensity;
    public float SunPower;

    [Space(5)]
    public float MineralIntensity;
    public float MineralPower;

    private float time;

    void Start()
    {
        CellPrefab.transform.localScale = new Vector3(CellSize, CellSize, CellSize);
        BioPrefab.transform.localScale = new Vector3(BioSize, BioSize, BioSize);
        Vector3 pos = Vector3.zero;
        pos.x = (Width / 2) * CellSize - (CellSize /2);
        pos.z = (Height / 2) * CellSize - (CellSize / 2);
        GameObject cell = Instantiate(CellPrefab, pos, Quaternion.identity);
        cell.transform.SetParent(gameObject.transform);
        cell.transform.localScale = new Vector3(Width * CellSize, 1, Height * CellSize);
        EvolutionEngine.InitializeObjectPool(Width * Height, BioPrefab);
        GeneratePopulation();
        //GeneratePopulation();
    }

    void Update()
    {
    }

    public int GetSunEnergy(Vector3 position)
    {
        int energy = 0;
        float height = Height * CellSize;
        if (position.z >= height - SunPower)
        {
            energy = Mathf.FloorToInt(Mathf.Lerp(SunIntensity, 0, (height - position.z - 1) / (SunPower * CellSize)));
        }
        return energy;
    }

    public int GetMineralEnergy(Vector3 position)
    {
        int energy = 0;
        if (position.z < MineralPower)
        {
            energy = Mathf.FloorToInt(Mathf.Lerp(MineralIntensity, 0, position.z / (MineralPower * CellSize)));
        }
        return energy;
    }

    public void GeneratePopulation()
    {
        Vector3 pos = new Vector3(Random.Range(0, Width) * CellSize, 0, (Height - 1) * CellSize);
        GameObject bio = EvolutionEngine.TakeObject();
        bio.transform.position = pos;
        bio.SetActive(true);
        bio.transform.GetChild(0).gameObject.SetActive(true);
    }
}
