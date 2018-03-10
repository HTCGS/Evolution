using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Gene
{
    public static Gene Photosynthesis
    {
        get
        {
            Gene gene = new Gene(0, true);
            gene.SubGene.Genes.Add(new Gene(0));
            return gene;
        }
    }
    public static Gene Predator
    {
        get
        {
            Gene gene = new Gene(0, true);
            gene.SubGene.Genes.Add(new Gene(3));
            return gene;
        }
    }
    public static Gene Dead
    {
        get
        {
            Gene gene = new Gene(1, false);
            return gene;

        }
    }

    public int Value;

    public Chromosome SubGene;

    public bool IsImmutable;

    public Gene()
    {
        SubGene = new Chromosome();
    }

    public Gene(bool isImmutable) : this()
    {
        this.IsImmutable = isImmutable;
    }

    public Gene(int value, bool isImmutable = false) : this(isImmutable)
    {
        this.Value = value;
    }

    public Gene Copy()
    {
        Gene geneCopy = new Gene();
        geneCopy.Value = Value;
        geneCopy.IsImmutable = IsImmutable;
        if (SubGene.Genes.Count != 0)
        {
            Chromosome subGeneCopy = new Chromosome();
            subGeneCopy.Genes = SubGene.Copy();
            geneCopy.SubGene = subGeneCopy;
        }
        return geneCopy;
    }
}
