﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Gene
{
    public static Gene Photosynthesis
    {
        get
        {
            Gene gene = new Gene(0, false);
            gene.SubGene.Genes.Add(new Gene(0));
            return gene;
        }
    }

    public int Value;

    public Chromosome SubGene;

    public bool IsMutable;

    public Gene()
    {
        SubGene = new Chromosome();
    }

    public Gene(bool isMutable) : this()
    {
        this.IsMutable = isMutable;
    }

    public Gene(int value, bool isMutable = true) : this(isMutable)
    {
        this.Value = value;
    }

    public Gene Copy()
    {
        Gene geneCopy = new Gene();
        geneCopy.Value = Value;
        return geneCopy;
    }
}