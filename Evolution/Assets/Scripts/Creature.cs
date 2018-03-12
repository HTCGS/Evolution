using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Chromosome DNA;

    public int Energy;
    public int MaxEnergy;

    [Space(5)]
    public int MaxLifeTime;
    private int LifeTime;

    private float time;

    void Start()
    {
        if (DNA == null)
        {
            DNA = new Chromosome();
            DNA.Genes.Add(Gene.Photosynthesis);
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        //if (time >= 0.1)
        {
            VirtualMachine.Step(this);
            if (DNA.GenePos >= DNA.Genes.Count) DNA.GenePos = 0;
            Energy--;
            if (Energy <= 0)
            {
                EvolutionEngine.GiveBackObject(gameObject);
            }
            LifeTime++;
            if (LifeTime >= MaxLifeTime)
            {
                DNA.GenePos = 0;
                Statistics.AddOldestDNA(this);
                EvolutionEngine.GiveBackObject(gameObject);
            }
            if (Energy >= MaxEnergy) Division();
            time = 0;
        }

        //Test();
    }

    private void OnEnable()
    {
        Energy = 1;
        LifeTime = 0;
        time = 0;
    }

    private void Test()
    {
        time += Time.deltaTime;

        if (time >= 0.5)
        {
            VirtualMachine.Step(this);
            if (DNA.GenePos >= DNA.Genes.Count) DNA.GenePos = 0;
            if (Energy >= MaxEnergy) Division();
        }
    }

    public void InitializeDNA(List<Gene> genes)
    {
        DNA = new Chromosome();
        DNA.Genes = genes;
    }

    private void Division()
    {
        List<Vector3> freePos = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            Vector3 direction = GetRandomVector(i);
            RaycastHit raycastHit = VirtualMachine.RayCast(gameObject, direction);
            if (raycastHit.transform == null)
            {
                Vector3 inRange = this.transform.position + direction;
                if (inRange.x >= 0 && inRange.x < VirtualMachine.Env.Width * VirtualMachine.Env.CellSize
                    && inRange.z < VirtualMachine.Env.Height * VirtualMachine.Env.CellSize && inRange.z >= 0)
                {
                    freePos.Add(direction);
                }
            }
        }

        if (freePos.Count == 0)
        {
            //EvolutionEngine.GiveBackObject(gameObject);
        }
        else
        {
            Vector3 pos = freePos[Random.Range(0, freePos.Count)];
            GameObject child = EvolutionEngine.TakeObject();
            child.transform.position = this.transform.position + pos;
            Creature childCreature = child.GetComponent<Creature>();
            VirtualMachine.Rotate(child);
            List<Gene> genes = this.DNA.Copy();
            childCreature.InitializeDNA(genes);
            child.SetActive(true);
            child.transform.GetChild(0).gameObject.SetActive(true);
            GameObject childVisual = null;
            childVisual = child.transform.GetChild(0).gameObject;
            Color parentColor = this.GetComponentInChildren<Renderer>().material.color;
            childVisual.GetComponent<Renderer>().material.color = new Color(parentColor.r, parentColor.g, parentColor.b, parentColor.a);
            if (Random.Range(0, 100) < 5)
            {
                childCreature.DNA.Mutate();
                child.GetComponentInChildren<Renderer>().material.color = EvolutionEngine.GetMutationColor(VirtualMachine.GetFoodType(childCreature.DNA));
                //childCreature.DNA.DisplayGenes();
            }
            Energy = 1;
        }
    }

    private Vector3 GetRandomVector(int num)
    {
        Vector3 vector = Vector3.zero;
        switch (num)
        {
            case 0:
                vector = Vector3.forward;
                break;
            case 1:
                vector = Vector3.right;
                break;
            case 2:
                vector = -Vector3.forward;
                break;
            case 3:
                vector = -Vector3.right;
                break;
            default:
                break;
        }
        return vector;
    }

    public void FoodColor()
    {
        int foodType = VirtualMachine.GetFoodType(DNA);
        Material material = gameObject.GetComponentInChildren<Renderer>().material;
        if (foodType == 0)
        {
            material.color = Color.green;
        }
        else if (foodType == 1)
        {
            material.color = Color.blue;
        }
        else if (foodType == 2)
        {
            material.color = Color.gray;
        }
        else if (foodType == 3)
        {
            material.color = Color.red; ;
        }
    }
}
