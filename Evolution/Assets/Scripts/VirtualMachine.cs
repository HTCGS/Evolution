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
            int jump = 0;
            if (chromosome.Genes[i].Value == 0)
            {
                jump = FeedOn(creature);
                chromosome.GenePos++;
                return;
            }
            else if (chromosome.Genes[i].Value == 1)
            {
                jump = Move(creature);
                chromosome.GenePos++;
                if (jump == 0)
                {
                    creature.Energy -= 1;
                    chromosome.GenePos = i + 1;
                    return;
                }
                else
                {
                    i++;
                }
            }
            else if (chromosome.Genes[i].Value == 2)
            {
                jump = LookUp(creature);
            }
            else if (chromosome.Genes[i].Value == 3)
            {
                jump = Rotate(creature);
            }
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
            Vector3 forward = creature.transform.forward;
            if (Mathf.Abs(forward.x) < 0.01f) forward.x = 0;
            if (Mathf.Abs(forward.z) < 0.01f) forward.z = 0;

            Vector3 nextPos = creature.transform.position + (forward * Env.CellSize);
            if (nextPos.x < 0 || nextPos.x > (Env.Width - 1) * Env.CellSize || nextPos.z < 0 || nextPos.z > (Env.Height - 1) * Env.CellSize)
            {
                return -1;
            }
            if (Math.Abs(forward.x) == 0f || Math.Abs(forward.x) == 1f
                || Math.Abs(forward.z) == 1f || Math.Abs(forward.z) == 0f) creature.transform.position = nextPos;
            else
            {
                nextPos = Vector3.zero;
                if (creature.transform.forward.x > 0) nextPos.x = 1;
                else nextPos.x = -1;
                if (creature.transform.forward.z > 0) nextPos.z = 1;
                else nextPos.z = -1;
                creature.transform.position += nextPos;
            }
            return 0;
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
        creature.transform.rotation = Quaternion.Euler(Mathf.RoundToInt(euler.x), Mathf.RoundToInt(euler.y + angle), Mathf.RoundToInt(euler.z));
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
        float range = Env.CellSize;
        if ((Math.Abs(gameObject.transform.forward.x) != 0f && Math.Abs(gameObject.transform.forward.x) != 1f)
                || (Math.Abs(gameObject.transform.forward.z) != 1f && Math.Abs(gameObject.transform.forward.z) != 0f)) range *= Mathf.Sqrt(2);
            Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out raycastHit, range);
        return raycastHit;
    }

    public static RaycastHit RayCast(GameObject gameObject, Vector3 direction)
    {
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(gameObject.transform.position, direction, out raycastHit, Env.CellSize);
        return raycastHit;
    }
}
