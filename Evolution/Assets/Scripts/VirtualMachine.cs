using UnityEngine;
using System;

static class VirtualMachine
{
    public static EnviromentGenerator Env;

    static VirtualMachine()
    {
        GameObject gameObject = GameObject.Find("BioSpace");
        Env = gameObject.GetComponent<EnviromentGenerator>();
    }

    public static void Step(Creature creature)
    {
        Chromosome chromosome = creature.DNA;
        for (int i = chromosome.GenePos; i < chromosome.Genes.Count; i++)
        {
            if (chromosome.Genes[i].Value == 0)
            {
                FeedOn(creature);
                chromosome.GenePos++;
                return;
            }
            else if (chromosome.Genes[i].Value == 1)
            {
                Move(creature);
                chromosome.GenePos++;
                return;
            }
            else if (chromosome.Genes[i].Value == 2)
            {
                LookUp(creature);
            }
            else if (chromosome.Genes[i].Value == 3)
            {
                Rotate(creature);
            }
            chromosome.GenePos++;
        }
    }

    public static int FeedOn(Creature creature)
    {
        int foodType = GetFoodType(creature.DNA);
        if (foodType == 0)
        {
            creature.Energy += Env.GetSunEnergy(creature.transform.position);
        }
        else if (foodType == 1)
        {
            creature.Energy += Env.GetMineralEnergy(creature.transform.position);
        }
        else if (foodType == 2)
        {
            RaycastHit raycastHit = RayCast(creature.gameObject);
            if (raycastHit.transform != null)
            {
                if (raycastHit.transform.gameObject.tag == "Dead")
                {
                    UnityEngine.Object.Destroy(raycastHit.transform.parent.gameObject);
                    UnityEngine.Object.Destroy(raycastHit.transform.gameObject);
                    Move(creature, true);
                    creature.Energy += 5;
                }
            }
        }
        else if (foodType == 3)
        {
            RaycastHit raycastHit = RayCast(creature.gameObject);
            if (raycastHit.transform != null)
            {
                if (raycastHit.transform.gameObject.tag == "Microorganism")
                {
                    UnityEngine.Object.Destroy(raycastHit.transform.parent.gameObject);
                    UnityEngine.Object.Destroy(raycastHit.transform.gameObject);
                    Move(creature, true);
                    creature.Energy += 8;
                }
            }
        }
        return 0;
    }

    public static int Move(Creature creature, bool hunter = false)
    {
        int barrier = LookUp(creature);
        if (barrier == 0 || hunter)
        {
            float xF = creature.transform.position.x - (float)Math.Truncate(creature.transform.position.x);
            float zF = creature.transform.position.z - (float)Math.Truncate(creature.transform.position.z);
            if (xF > 0.001f && zF > 0.001f)
            {
                if (xF != 0f || zF != 0)
                {
                    Debug.LogError(creature.transform.forward);
                }
            }

            Vector3 nextPos = creature.transform.position + (creature.transform.forward * Env.CellSize);
            if (nextPos.x < 0 || nextPos.x > (Env.Width - 1) * Env.CellSize || nextPos.z < 0 || nextPos.z > (Env.Height - 1) * Env.CellSize)
            {
                return -1;
            }
            if ( nextPos.x % Env.CellSize == 0 && nextPos.z % Env.CellSize == 0) creature.transform.position = nextPos;
            else
            {
                creature.transform.position += creature.transform.forward * Mathf.Sqrt(2) * Env.CellSize;
            }
            creature.Energy -= 1;

            xF = creature.transform.position.x - (float)Math.Truncate(creature.transform.position.x);
            zF = creature.transform.position.z - (float)Math.Truncate(creature.transform.position.z);
            if (xF > 0.001f && zF > 0.001f)
            {
                if (xF != 0f || zF != 0)
                {
                    Debug.LogError(creature.transform.forward);
                }
            }
        }
        return barrier;
    }

    public static int LookUp(Creature creature)
    {
        int barrier = 0;
        RaycastHit raycastHit = RayCast(creature.gameObject);
        if (raycastHit.transform != null) barrier = 1;
        return barrier;
    }

    public static int Rotate(Creature creature)
    {
        int rotation = UnityEngine.Random.Range(0, 7);
        int angle = rotation * 45;
        Vector3 euler = creature.transform.rotation.eulerAngles;
        creature.transform.rotation = Quaternion.Euler((int)euler.x, (int)euler.y + angle, (int)euler.z);
        return 0;
    }

    public static int GetFoodType(Chromosome chromosome)
    {
        int foodType = -1;
        foreach (var gene in chromosome.Genes)
        {
            if (gene.IsImmutable) foodType = gene.SubGene.Genes[0].Value;
        }
        return foodType;
    }

    public static RaycastHit RayCast(GameObject gameObject)
    {
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out raycastHit, Env.CellSize * 1.2f);
        return raycastHit;
    }

    public static RaycastHit RayCast(GameObject gameObject, Vector3 direction)
    {
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(gameObject.transform.position, direction, out raycastHit, Env.CellSize * 1.2f);
        return raycastHit;
    }
}
