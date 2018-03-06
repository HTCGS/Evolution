using UnityEngine;

public static class EvolutionEngine
{
    public static int GreenMutation;
    public static int BlueMutation;
    public static int GrayMutation;
    public static int RedMutation;

    public static Color GetMutationColor(int foodType)
    {
        Color color = Color.white;
        if (foodType == 0)
        {
            //color = new Color(Random.Range(0, 127), Random.Range(100, 255), Random.Range(0, 150), 1);
            GreenMutation++;
            color = Color.Lerp(new Color(0, 255, 0, 1) ,new Color(0, 100, 0, 1), GreenMutation / 1000);
        }
        else if (foodType == 1)
        {
            //color = new Color(Random.Range(0, 100), Random.Range(0, 100), Random.Range(255, 150), 1);
            BlueMutation++;
            color = Color.Lerp(new Color(0, 0, 255, 1), new Color(0, 0, 100, 1), BlueMutation / 1000);
        }
        else if (foodType == 2)
        {
            GrayMutation++;
            int col = (int)Mathf.Lerp(64, 224, GrayMutation / 1000);
            color = new Color(col, col, col, 1);
        }
        else if (foodType == 3)
        {
            //color = new Color(Random.Range(100, 255), Random.Range(0, 100), Random.Range(0, 100), 1);
            RedMutation++;
            color = Color.Lerp(new Color(255, 0, 0, 1), new Color(100, 0, 0, 1), RedMutation / 1000);
        }
        return color;
    }

}