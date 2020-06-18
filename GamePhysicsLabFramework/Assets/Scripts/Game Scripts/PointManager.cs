using UnityEngine;
using System.Collections;

public static class PointManager
{
    private static int points = 0;

    public static int AddPoints (int pointsToAdd)
    {
        points += pointsToAdd;

        return points;
    }

    public static int GetPoints()
    {
        return points;
    }

}
