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
                jump = Move(creature.gameObject);
                if (jump == 0)
                {
                    creature.Energy--;
                    chromosome.GenePos++;
                    return;
                }
                else
                {
                    creature.Energy--;
                    //i++;
                    //chromosome.GenePos++;
                }
            }
            else if (chromosome.Genes[i].Value == 2)
            {
                jump = LookUp(creature.gameObject);
                if(jump != 0)
                {
                    i++;
                    chromosome.GenePos++;
                }
            }
            else if (chromosome.Genes[i].Value == 3)
            {
                jump = Rotate(creature.gameObject);
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
                if (raycastHit.transform.parent.gameObject.tag == "Microorganism")
                {
                    EvolutionEngine.GiveBackObject(raycastHit.transform.parent.gameObject);
                    Move(creature.gameObject, true);
                    creature.Energy += 8;
                }
            }
        }
        return 0;
    }

    public static int Move(GameObject gameObject, bool hunter = false)
    {
        int barrier = LookUp(gameObject);
        if (barrier == 0 || hunter)
        {
            Vector3 forward = gameObject.transform.forward;
            if (Mathf.Abs(forward.x) < 0.01f) forward.x = 0;
            if (Mathf.Abs(forward.z) < 0.01f) forward.z = 0;

            Vector3 nextPos = gameObject.transform.position + (forward * Env.CellSize);
            if (nextPos.x < 0 || nextPos.x > (Env.Width - 1) * Env.CellSize || nextPos.z < 0 || nextPos.z > (Env.Height - 1) * Env.CellSize)
            {
                return -1;
            }

            //if (nextPos.z < 0 || nextPos.z > (Env.Height - 1) * Env.CellSize)
            //{
            //    return -1;
            //}   
            //if (nextPos.x < 0 || nextPos.x > (Env.Width - 1) * Env.CellSize)
            //{
            //    if (nextPos.x < 0)
            //    {
            //        Vector3 otherSide = new Vector3(Env.Width * Env.CellSize, gameObject.transform.position.y, gameObject.transform.position.z);
            //        barrier = LookUp(otherSide, gameObject.transform.forward);
            //        if (barrier == 0)
            //        {
            //            nextPos = otherSide + (forward * Env.CellSize);
            //            gameObject.transform.position = otherSide;
            //        }
            //        else return -1;
            //    }
            //    else if (nextPos.x > (Env.Width - 1) * Env.CellSize)
            //    {
            //        Vector3 otherSide = new Vector3(-1, gameObject.transform.position.y, gameObject.transform.position.z);
            //        barrier = LookUp(otherSide, gameObject.transform.forward);
            //        if (barrier == 0)
            //        {
            //            nextPos = otherSide + (forward * Env.CellSize);
            //            gameObject.transform.position = otherSide;
            //        }
            //        else return -1;
            //    }
            //}

            if (Math.Abs(forward.x) == 0f || Math.Abs(forward.x) == 1f
                || Math.Abs(forward.z) == 1f || Math.Abs(forward.z) == 0f) gameObject.transform.position = nextPos;
            else
            {
                nextPos = Vector3.zero;
                if (gameObject.transform.forward.x > 0) nextPos.x = 1;
                else nextPos.x = -1;
                if (gameObject.transform.forward.z > 0) nextPos.z = 1;
                else nextPos.z = -1;
                gameObject.transform.position += nextPos;
            }
            return 0;
        }
        return barrier;
    }

    public static int LookUp(GameObject gameObject)
    {
        int barrier = 0;
        RaycastHit raycastHit = RayCast(gameObject);
        if (raycastHit.transform != null) barrier = 1;
        return barrier;
    }

    public static int LookUp(Vector3 pos, Vector3 direction)
    {
        int barrier = 0;
        RaycastHit raycastHit = RayCast(pos, direction);
        if (raycastHit.transform != null) barrier = 1;
        return barrier;
    }

    public static int Rotate(GameObject gameObject)
    {
        int rotation = UnityEngine.Random.Range(1, 8);
        int angle = rotation * 45;
        Vector3 euler = gameObject.transform.rotation.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(Mathf.RoundToInt(euler.x), Mathf.RoundToInt(euler.y + angle), Mathf.RoundToInt(euler.z));
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
        return RayCast(gameObject, gameObject.transform.forward);
    }

    public static RaycastHit RayCast(GameObject gameObject, Vector3 direction)
    {
        return RayCast(gameObject.transform.position, direction);
    }

    public static RaycastHit RayCast(Vector3 pos, Vector3 direction)
    {
        RaycastHit raycastHit = new RaycastHit();
        float range = Env.CellSize;
        if ((Math.Abs(direction.x) != 0f && Math.Abs(direction.x) != 1f)
                    || (Math.Abs(direction.z) != 1f && Math.Abs(direction.z) != 0f)) range *= Mathf.Sqrt(2);
        Physics.Raycast(pos, direction, out raycastHit, range);
        return raycastHit;
    }
}
