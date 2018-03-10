using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Statistics
{
    public static Dictionary<string, List<DNAInfo>> Population;

    public static Dictionary<string, int> PopulationLife;

    public static List<float> AverageDNALength;

    private static int Time;

    static Statistics()
    {
        Population = new Dictionary<string, List<DNAInfo>>();
        PopulationLife = new Dictionary<string, int>();
        AverageDNALength = new List<float>();
    }

    public static void Analize(List<Creature> pop)
    {
        GetAverageDNA(pop);
        Time++;
        for (int i = 0; i < pop.Count; i++)
        {
            string name = pop[i].DNA.ToString();
            DNAInfo info = new DNAInfo();
            info.Time = Time;
            info.Population = 1;
            for (int j = i + 1; j < pop.Count; j++)
            {
                if (pop[j].DNA.ToString() == name)
                {
                    info.Population++;
                    pop.RemoveAt(j);
                    j--;
                }
            }
            if (!Population.ContainsKey(name))
            {
                List<DNAInfo> infoList = new List<DNAInfo>();
                infoList.Add(info);
                Population.Add(name, infoList);
            }
            else
            {
                List<DNAInfo> infoList = Population[name];
                infoList.Add(info);
            }
        }
    }

    public static void GetAverageDNA(List<Creature> pop)
    {
        float sum = 0;
        foreach (var item in pop)
        {
            sum += item.DNA.ToString().Length;
        }
        sum /= pop.Count;
        AverageDNALength.Add(sum);
    }

    public static void AddOldestDNA(Creature creature)
    {
        string name = creature.DNA.ToString();
        if (Population.ContainsKey(name))
        {
            if (PopulationLife.ContainsKey(name))
            {
                PopulationLife[name]++;
            }
            else
            {
                PopulationLife.Add(name, 1);
            }
        }
    }

    public static void Save()
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Киря\Desktop\BioStatistics.csv"))
        {
            string header = string.Empty;
            foreach (var item in Population)
            {
                header += "," + item.Key;
            }
            file.WriteLine(header);

            for (int i = 1; i <= Time; i++)
            {
                string line = i.ToString();
                foreach (var item in Population)
                {
                    List<DNAInfo> info = item.Value;
                    bool found = false;
                    foreach (var infoTime in info)
                    {
                        if (infoTime.Time == i)
                        {
                            line += "," + infoTime.Population;
                            found = true;
                            break;
                        }
                    }
                    if (!found) line += ",";
                }
                file.WriteLine(line);
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Киря\Desktop\BioLifeTime.csv"))
        {
            string header = string.Empty;
            string data = string.Empty;
            foreach (var item in PopulationLife)
            {
                header += "," + item.Key;
                data += "," + item.Value;
            }
            file.WriteLine(header);
            file.WriteLine(data);
            file.WriteLine();
            data = string.Empty;
            foreach (var item in AverageDNALength)
            {
                data += "," + item;
            }
            file.WriteLine(data);
        }
    }

}

public class DNAInfo
{
    public int Time;
    public int Population;
}
