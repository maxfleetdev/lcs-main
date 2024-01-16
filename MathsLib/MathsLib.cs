using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsLib : MonoBehaviour
{
    #region Quick Sort Algorithm

    public static void Sort(List<GameObject> objects, Vector3 origin)
    {
        if (objects == null || objects.Count <= 1)
            return;

        QuickSortAlgorithm(objects, origin, 0, objects.Count - 1);
    }

    protected private static void QuickSortAlgorithm(List<GameObject> objects, Vector3 origin, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(objects, origin, low, high);

            QuickSortAlgorithm(objects, origin, low, partitionIndex - 1);
            QuickSortAlgorithm(objects, origin, partitionIndex + 1, high);
        }
    }

    protected private static int Partition(List<GameObject> objects, Vector3 origin, int low, int high)
    {
        float pivotDistance = Vector3.Distance(objects[high].transform.position, origin);
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            float currentDistance = Vector3.Distance(objects[j].transform.position, origin);

            if (currentDistance < pivotDistance)
            {
                i++;
                Swap(objects, i, j);
            }
        }

        Swap(objects, i + 1, high);
        return i + 1;
    }

    protected private static void Swap(List<GameObject> objects, int i, int j)
    {
        GameObject temp = objects[i];
        objects[i] = objects[j];
        objects[j] = temp;
    }

    #endregion
}