using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Chromosome
{
    public List<Gene> Genes;

    public int GenePos;

    public Chromosome()
    {
        Genes = new List<Gene>();
    }

    public Chromosome(Chromosome chromosome)
    {
        Genes = chromosome.Copy();
    }

    public List<Gene> Copy()
    {
        List<Gene> geneCopy = new List<Gene>();
        foreach (var gene in Genes)
        {
            geneCopy.Add(gene.Copy());
        }
        return geneCopy;
    }

    public void Mutate()
    {
        int chromSize = Genes.Count;
        int mutatedGene = UnityEngine.Random.Range(0, chromSize + 1);
        if (mutatedGene == chromSize) Genes.Add(new Gene(UnityEngine.Random.Range(0, 4)));
        else
        {
            int newGene = UnityEngine.Random.Range(0, 4);
            if (Genes[mutatedGene].IsImmutable)
            {
                if (chromSize == 1)
                {
                    Gene food = GetFoodGene();
                    int newFood = 0;
                    do
                    {
                        newFood = UnityEngine.Random.Range(0, 3);
                    } while (newFood == food.SubGene.Genes[0].Value);
                    food.SubGene.Genes[0].Value = newFood;
                }
                else
                {
                    int changeFoodType = UnityEngine.Random.Range(0, 100);
                    if (changeFoodType < 50)
                    {
                        SwapGene(mutatedGene);
                    }
                    else
                    {
                        Gene food = GetFoodGene();
                        int newFood = 0;
                        do
                        {
                            newFood = UnityEngine.Random.Range(0, 3);
                        } while (newFood == food.SubGene.Genes[0].Value);
                        food.SubGene.Genes[0].Value = newFood;
                    }
                }
            }
            else
            {
                if (Genes[mutatedGene].Value == newGene)
                {
                    SwapGene(mutatedGene);
                }
                else
                {
                    Genes[mutatedGene].Value = newGene;
                }
            }
        }
    }

    private void SwapGene(int genePos)
    {
        if (Genes.Count != 1)
        {
            int direction = UnityEngine.Random.Range(0, 100);
            if (direction < 50)
            {
                if (genePos == 0)
                {
                    Swap(0, Genes.Count - 1);
                }
                else
                {
                    Swap(genePos, genePos - 1);
                }
            }
            else
            {
                if (genePos == Genes.Count - 1)
                {
                    Swap(0, Genes.Count - 1);
                }
                else
                {
                    Swap(genePos, genePos + 1);
                }
            }
        }
    }

    private void Swap(int pos1, int pos2)
    {
        Gene tmp = Genes[pos1];
        Genes[pos1] = Genes[pos2];
        Genes[pos2] = tmp;
    }

    private Gene GetFoodGene()
    {
        foreach (var gene in Genes)
        {
            if (gene.IsImmutable) return gene;
        }
        return null;
    }

    public void DisplayGenes()
    {
        Debug.Log(ToString());
    }

    public override string ToString()
    {
        string genes = string.Empty;
        foreach (var gene in Genes)
        {
            if(gene.IsImmutable)
            {
                if(gene.SubGene.Genes[0].Value == 0) genes += "F";
                if (gene.SubGene.Genes[0].Value == 1) genes += "M";
                if (gene.SubGene.Genes[0].Value == 2) genes += "P";
            }
            else genes += gene.Value;
        }
        return genes;
    }
}
