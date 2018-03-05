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
        if (mutatedGene == chromSize)
        {
            int newGene = UnityEngine.Random.Range(0, 4);
            if(newGene== 0)
            {
                int foodType = VirtualMachine.GetFoodType(this);
                Gene gene = new Gene(newGene);
                gene.SubGene = new Chromosome();
                gene.SubGene.Genes.Add(new Gene(foodType));
                Genes.Add(gene);
            }
            else Genes.Add(new Gene(UnityEngine.Random.Range(0, 4)));
        }
        else
        {
            int newGene = UnityEngine.Random.Range(0, 4);
            if (!Genes[mutatedGene].IsMutable)
            {
                if (chromSize == 1)
                {
                    int newFood = 0;
                    do
                    {
                        newFood = UnityEngine.Random.Range(0, 4);
                    } while (newFood == Genes[mutatedGene].SubGene.Genes[0].Value);
                    Genes[mutatedGene].SubGene.Genes[0].Value = newFood;
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
                        try
                        {
                            int newFood = 0;
                            do
                            {
                                newFood = UnityEngine.Random.Range(0, 4);
                            } while (newFood == Genes[mutatedGene].SubGene.Genes[0].Value);
                            Genes[mutatedGene].SubGene.Genes[0].Value = newFood;
                        }
                        catch
                        {

                        }
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
                if(genePos == 0)
                {
                    Swap(0, Genes.Count - 1);
                }
                else
                {
                    Swap(genePos, genePos -1 );
                }
            }
            else
            {
                if(genePos == Genes.Count - 1)
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
}
