using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class VirtualMachine
{

    //public static void Step(Chromosome chromosome)
    public static void Step(Creature creature)
    {
        Chromosome chromosome = creature.DNA;
        for (int i = chromosome.GenePos; i < chromosome.Genes.Count; i++)
        {
            if (chromosome.Genes[i].Value == 0)
            {
                
            }
            else if (chromosome.Genes[i].Value == 1)
            {

            }
            else if (chromosome.Genes[i].Value == 2)
            {

            }
            else if (chromosome.Genes[i].Value == 3)
            {

            }

        }
    }

    public static int FeedOn(Creature creature)
    {
        int foodType = GetFoodType(creature.DNA);
        if(foodType  == 0)
        {

        }
        else if (foodType == 1)
        {

        }
        else if (foodType == 2)
        {

        }
        else if (foodType == 3)
        {

        }
        return 0;
    }

    public static int Move(Creature creature)
    {


        return 0;
    }

    public static int LookUp(Creature creature)
    {


        return 0;
    }

    public static int Rotate(Creature creature)
    {


        return 0;
    }

    private static int GetFoodType(Chromosome chromosome)
    {
        int foodType = -1;
        foreach (var gene in chromosome.Genes)
        {
            if (!gene.IsMutable) foodType = gene.SubGene.Genes[0].Value;
        }
        return foodType;
    }

}
