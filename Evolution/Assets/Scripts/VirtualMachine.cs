using UnityEngine;

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
            }
            else if (chromosome.Genes[i].Value == 1)
            {
                Move(creature);
            }
            else if (chromosome.Genes[i].Value == 2)
            {
                LookUp(creature);
            }
            else if (chromosome.Genes[i].Value == 3)
            {
                Rotate(creature);
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
                    creature.transform.position += creature.transform.forward * Env.CellSize;
                    Object.Destroy(raycastHit.transform.gameObject);
                    creature.Energy -= 1;
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
                    creature.transform.position += creature.transform.forward * Env.CellSize;
                    Object.Destroy(raycastHit.transform.gameObject);
                    creature.Energy -= 1;
                    creature.Energy += 8;
                }
            }
        }
        return 0;
    }

    public static int Move(Creature creature)
    {
        int barrier = LookUp(creature);
        if (barrier == 0)
        {
            Vector3 nextPos = creature.transform.position + (creature.transform.forward * Env.CellSize);
            if(nextPos.x < 0 || nextPos.x > (Env.Width - 1) * Env.CellSize || nextPos.z < 0 || nextPos.z > (Env.Height - 1) * Env.CellSize)
            {
                return -1;
            }
            creature.transform.position += creature.transform.forward * Env.CellSize;
            creature.Energy -= 1;
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
        int rotation = Random.Range(0, 7);
        int angle = rotation * 45;
        Vector3 euler = creature.transform.rotation.eulerAngles;
        creature.transform.rotation = Quaternion.Euler(euler.x, euler.y + angle, euler.z);
        return 0;
    }

    public static int GetFoodType(Chromosome chromosome)
    {
        int foodType = -1;
        try
        {
            foreach (var gene in chromosome.Genes)
            {
                if (!gene.IsMutable) foodType = gene.SubGene.Genes[0].Value;
            }
        }
        catch
        {

        }
        return foodType;
    }

    public static RaycastHit RayCast(GameObject gameObject)
    {
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out raycastHit, Env.CellSize);
        return raycastHit;
    }

    public static RaycastHit RayCast(GameObject gameObject, Vector3 direction)
    {
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(gameObject.transform.position, direction, out raycastHit, Env.CellSize);
        return raycastHit;
    }
}
