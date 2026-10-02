using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public int gemCount = 0;

    public void AddGem()
    {
        gemCount++;
        Debug.Log("Gemme ramassée ! Total : " + gemCount);
    }
}
