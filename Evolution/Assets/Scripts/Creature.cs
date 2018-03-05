using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Chromosome DNA;

    public float Energy;

	void Start ()
    {
        DNA = new Chromosome();
	}
	
	void Update ()
    {
		
	}
}
