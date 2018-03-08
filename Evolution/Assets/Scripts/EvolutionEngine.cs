using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EvolutionEngine
{
    public static int GreenMutation;
    public static int BlueMutation;
    public static int GrayMutation;
    public static int RedMutation;

    public static List<GameObject> ObjectPool;

    public static void InitializeObjectPool(int objectNum, GameObject prefab)
    {
        ObjectPool = new List<GameObject>();
        Vector3 pos = new Vector3(-3f, 0, -3f);
        for (int i = 0; i < objectNum; i++)
        {
            GameObject gameObject = Object.Instantiate(prefab, pos, Quaternion.identity);
            gameObject.name = i.ToString();
            gameObject.GetComponentInChildren<Renderer>().material.color = new Color(0, 255, 0, 1);
            gameObject.SetActive(false);
            ObjectPool.Add(gameObject);
        }
    }

    public static GameObject TakeObject()
    {
        GameObject gameObject = ObjectPool[0];
        ObjectPool.RemoveAt(0);
        return gameObject;
    }

    public static void GiveBackObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(-3f, 0, -3f);
        ObjectPool.Add(gameObject);
    }

    public static Color GetMutationColor(int foodType)
    {
        if (GreenMutation == 1000000) GreenMutation = 0;
        if (BlueMutation == 1000000) BlueMutation = 0;
        if (RedMutation == 1000000) RedMutation = 0;
        if (GrayMutation == 1000000) GrayMutation = 0;

        Color color = Color.white;
        if (foodType == 0)
        {
            //color = new Color(Random.Range(0, 127), Random.Range(100, 255), Random.Range(0, 150), 1);
            GreenMutation++;
            color = Color.Lerp(new Color(0, 255, 0, 1) ,new Color(0, 100, 0, 1), GreenMutation / 1000000);
        }
        else if (foodType == 1)
        {
            //color = new Color(Random.Range(0, 100), Random.Range(0, 100), Random.Range(255, 150), 1);
            BlueMutation++;
            color = Color.Lerp(new Color(0, 0, 255, 1), new Color(0, 0, 100, 1), BlueMutation / 1000000);
        }
        else if (foodType == 2)
        {
            GrayMutation++;
            int col = (int)Mathf.Lerp(64, 224, GrayMutation / 1000000);
            color = new Color(col, col, col, 1);
        }
        else if (foodType == 3)
        {
            //color = new Color(Random.Range(100, 255), Random.Range(0, 100), Random.Range(0, 100), 1);
            RedMutation++;
            color = Color.Lerp(new Color(255, 0, 0, 1), new Color(100, 0, 0, 1), RedMutation / 1000000);
        }
        return color;
    }

}