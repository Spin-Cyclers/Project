using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JobManager : MonoBehaviour
{

    public int jobsRequiredToLose;
    public float baseTimeBetweenTask;
    public float timeBetweenTaskDecreasePerTaskComplete;

    
    public JobScriptableObject[] jobVarients;

    public GameObject jobNotification;

    [HideInInspector]
    public int failedJobs = 0;
    [HideInInspector]
    public int completedJobs = 0;
    int currentJobs = 0;

    private GameObject[] clients;
    
    void Start()
    {
        clients = GameObject.FindGameObjectsWithTag("Client");
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(JobGameLoop());
    }

    private float scaledTimeBetweenTask()
    {
        return baseTimeBetweenTask - (timeBetweenTaskDecreasePerTaskComplete * completedJobs);
    }

    IEnumerator JobGameLoop()
    {
        while(failedJobs < jobsRequiredToLose)
        {
            yield return StartCoroutine(NewJobTimer());            
            StartJob();
        }
    }

    IEnumerator NewJobTimer()
    {
        float startTime = Time.time;
        while (Time.time - startTime < scaledTimeBetweenTask())
        {
            if (currentJobs == 0)
                yield break;
            yield return null;
        }
    }

    //Dont start job at a house with a job
    public void StartJob()
    {
        Debug.Log("Starting New Job");
        currentJobs++;
        GameObject[] noTaskClients = clients.Where(client => client.transform.childCount == 0).ToArray();
        GameObject jobController = new GameObject();
        jobController.AddComponent<JobController>().Initialize(
            jobVarients[Random.Range(0, jobVarients.Length)],
            noTaskClients[Random.Range(0, noTaskClients.Length)]);
        jobController.name = "Job Controller " + (completedJobs + failedJobs + currentJobs);
    }

    
}

public enum BuildingType
{
    None,
    Client,
    Washer,
    Dryer,
    Packing
}
