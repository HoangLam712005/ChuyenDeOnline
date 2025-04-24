using UnityEngine;

public class Runner : MonoBehaviour
{
    public bool isRunning = true;

    void Start()
    {
        // Giả lập hoàn thành sau 5 giây
        StartCoroutine(FinishRun());
    }

    System.Collections.IEnumerator FinishRun()
    {
        yield return new WaitForSeconds(5f);
        isRunning = false; // Runner hoàn thành
        Debug.Log("Runner has finished running.");
    }
}