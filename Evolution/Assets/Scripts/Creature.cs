using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Chromosome DNA;

    public int Energy;
    public int MaxEnergy;

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

        if (time >= 0.5)
        {
            VirtualMachine.Step(this);
            time = 0;
        }

        if (Energy >= MaxEnergy) Division();
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
            if (raycastHit.transform != null) freePos.Add(direction);
        }
        
        if(freePos.Count == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 pos = freePos[Random.Range(0, freePos.Count)];
            GameObject child = Instantiate(VirtualMachine.Env.BioPrefab, this.transform.position + pos, Quaternion.identity);
            Creature childCreature = child.GetComponent<Creature>();
            List<Gene> genes = this.DNA.Copy();
            childCreature.InitializeDNA(genes);
            //childCreature.DNA.Genes = genes;
            if (Random.Range(0, 100) < 5)
            {
                childCreature.DNA.Mutate();
            }
            child.SetActive(true);
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
}
