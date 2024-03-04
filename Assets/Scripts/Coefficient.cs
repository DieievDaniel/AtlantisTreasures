using UnityEngine;

public class Coefficient : MonoBehaviour
{
    public static float[][] elementMultiplier = new float[][]
    {
        new float[] {1, 20, 50, 100},    // Элемент 1
        new float[] {2, 5, 20, 50},       // Элемент 2
        new float[] {3, 3, 4, 24},        // Элемент 3
        new float[] {4, 4, 10, 30},       // Элемент 4
        new float[] {5, 2, 3, 20},        // Элемент 5
        new float[] {6, 1.6f, 2.40f, 50}, // Элемент 6
        new float[] {7, 1, 2, 10},        // Элемент 7
        new float[] {8, 0.5f, 1.5f, 4}    // Элемент 8
    };

    public static float GetMultiplier(int elementId, int count, int rows, int columns)
    {
        int indexOfElement = FindElementIndexById(elementId);

        if (indexOfElement != -1)
        {
            if (rows == 3 && columns == 4 && count == 5)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (rows == 3 && columns == 4 && count >= 6 && count < 7)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else if (rows == 3 && columns == 4 && count >= 8)
            {
                return elementMultiplier[indexOfElement][3];
            }
            else if (rows == 4 && columns == 5 && count == 7)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (rows == 4 && columns == 5 && count >= 8 && count < 9)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else if (rows == 4 && columns == 5 && count >= 9)
            {
                return elementMultiplier[indexOfElement][3];
            }
            else if (rows == 5 && columns == 6 && count == 8)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (rows == 5 && columns == 6 && count >= 9 && count < 10)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else if (rows == 5 && columns == 6 && count >= 10)
            {
                return elementMultiplier[indexOfElement][3];
            }
            else if (rows == 6 && columns == 7 && count >= 9 && count < 11)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (rows == 6 && columns == 7 && count >= 11 && count < 12)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else if (rows == 6 && columns == 7 && count >= 13)
            {
                return elementMultiplier[indexOfElement][3];
            }
        }

        return 0; // Или любое другое значение по умолчанию, если элемент не найден
    }

    private static int FindElementIndexById(int elementId)
    {
        for (int i = 0; i < elementMultiplier.Length; i++)
        {
            if (elementMultiplier[i][0] == elementId)
            {
                return i;
            }
        }

        return -1;
    }
}
