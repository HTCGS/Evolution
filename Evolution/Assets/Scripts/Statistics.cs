using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Statistics
{
    public static Dictionary<string, List<DNAInfo>> Population;

    public static Dictionary<string, List<DNAInfo>> OldLifePopulation;

    public static Dictionary<string, int> OldLife;

    public static List<float> AverageDNALength;

    private static int Time;

    static Statistics()
    {
        Population = new Dictionary<string, List<DNAInfo>>();
        OldLifePopulation = new Dictionary<string, List<DNAInfo>>();
        OldLife = new Dictionary<string, int>();
        AverageDNALength = new List<float>();
    }

    public static void Analize(List<Creature> pop)
    {
        Time++;
        GetAverageDNA(pop);
        LifeAnalize();
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

    public static void LifeAnalize()
    {
        foreach (var item in OldLife)
        {
            string name = item.Key;
            DNAInfo info = new DNAInfo();
            info.Time = Time;
            info.Population = item.Value;
            if (OldLifePopulation.ContainsKey(name))
            {
                List<DNAInfo> infoList = OldLifePopulation[name];
                infoList.Add(info);
            }
            else
            {
                List<DNAInfo> infoList = new List<DNAInfo>();
                infoList.Add(info);
                OldLifePopulation.Add(name, infoList);
            }
        }
        OldLife.Clear();
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
            if (OldLife.ContainsKey(name))
            {
                OldLife[name]++;
            }
            else
            {
                OldLife.Add(name, 1);
            }
        }
    }

    public static void Save()
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Киря\Desktop\Population.csv"))
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
                        if (infoTime.Time > i) break;
                    }
                    if (!found) line += ",";
                }
                file.WriteLine(line);
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Киря\Desktop\LifeTime.csv"))
        {
            string header = string.Empty;
            string data = string.Empty;

            foreach (var item in OldLifePopulation)
            {
                header += "," + item.Key;
            }
            file.WriteLine(header);

            for (int i = 1; i <= Time; i++)
            {
                data += i.ToString();
                foreach (var item in OldLifePopulation)
                {
                    List<DNAInfo> infoList = item.Value;
                    bool found = false;
                    foreach (var info in infoList)
                    {
                        if (info.Time == i)
                        {
                            data += "," + info.Population;
                            found = true;
                            break;
                        }
                        if (info.Time > i) break;
                    }
                    if (!found) data += ",";
                }
                file.WriteLine(data);
                data = "";
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Киря\Desktop\DNA.csv"))
        {
            string data = string.Empty;
            foreach (var item in AverageDNALength)
            {
                data += "," + item;
            }
            file.WriteLine(data);
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Киря\Desktop\LastPopulation.csv"))
        {
            string header = string.Empty;
            string data = string.Empty;

            foreach (var item in Population)
            {
                List<DNAInfo> infoList = item.Value;
                foreach (var info in infoList)
                {
                    if (info.Time >= Time - 5 && info.Time <= Time) 
                    {
                        header += "," + item.Key;
                        break;
                    }
                }
            }
            file.WriteLine(header);
            for (int i = Time - 5; i <= Time; i++)
            {
                data += i.ToString();
                foreach (var item in Population)
                {
                    List<DNAInfo> infoList = item.Value;
                    bool foundInRange = false;
                    bool found = false;
                    foreach (var info in infoList)
                    {
                        if (info.Time >= Time - 5 && info.Time <= Time)
                        {
                            foundInRange = true;
                            if (info.Time == i)
                            {
                                data += "," + info.Population;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (foundInRange && !found) data += ",";
                }
                file.WriteLine(data);
                data = "";
            }
        }
    }

}

public class DNAInfo
{
    public int Time;
    public int Population;
}
