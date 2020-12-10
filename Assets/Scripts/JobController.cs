using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobController : MonoBehaviour
{

    public JobScriptableObject jobInfo;
    private Queue<JobStageInfo> jobStages = new Queue<JobStageInfo>();
    private JobManager jobManager;
    public GameObject client;

    private JobItem jobItem;

    private JobNotification jobNotification;

    private AudioManager audioManager;

    private Sprite jobReturnSprite;

    public int score = 0;

    public float timeTaken = 0;

    private void Awake()
    {
        jobManager = FindObjectOfType<JobManager>();
    }

    void Start()
    {
        StartCoroutine(JobStageLoop());
    }

    public void Initialize(JobScriptableObject jobInfo, GameObject client)
    {
        this.jobInfo = jobInfo;
        for(int i = 0; i < jobInfo.jobStages.Length - 1; i++)
        {
            jobStages.Enqueue(jobInfo.jobStages[i]);
        }
        jobStages.Enqueue(jobInfo.jobStages[jobInfo.jobStages.Length - 1]);
        jobReturnSprite = jobInfo.jobStages[jobInfo.jobStages.Length - 2].stageImage;
        this.client = client;
    }

    IEnumerator JobStageLoop()
    {
        StartCoroutine(JobTimeLimit());

        jobItem = new GameObject().AddComponent<JobItem>();
        jobItem.controller = this;
        jobItem.transform.parent = client.transform;
        jobItem.name = "JobItem";

        jobNotification = Instantiate(jobManager.jobNotification, client.transform).GetComponent<JobNotification>();
        jobNotification.UpdateJobNotification(jobStages.Peek().stageImage);
        jobNotification.name = "JobNotification";
        jobNotification.controller = this;

        Sprite previous = null;

        while (jobStages.Count > 0)
        {
            JobStageInfo currentStage = jobStages.Dequeue();
            if (!previous)
                previous = currentStage.stageImage;
            Debug.Log("Current Stage: " + currentStage.jobBuilding);
            yield return StartCoroutine(JobStageInProgress(currentStage, previous));
            yield return StartCoroutine(JobStageExpiration(currentStage));
            previous = currentStage.stageImage;
        }
        while (jobItem.transform.parent != client.transform)
        {
            jobItem.buildingState = BuildingType.Client;
            jobNotification.DisableBar();
            jobNotification.ParentToObject(jobItem.transform.parent, jobNotification.playerOffset);
            yield return null;
            //Debug.Log("Going back to client");
        }
        CompleteJob();
    }

    IEnumerator JobTimeLimit()
    {        
        JobNotification fullDurationNotification = 
            Instantiate(jobManager.jobNotification, client.transform)
            .GetComponent<JobNotification>();
        fullDurationNotification.gameObject.name = "JobTimeLimit";
        fullDurationNotification.controller = this;
        float startTime = Time.time;
        while (Time.time - startTime < jobInfo.jobTimeLimit)
        {
            timeTaken = Time.time-startTime;
            fullDurationNotification.UpdateJobNotification(jobReturnSprite, 1 - ((Time.time - startTime) / jobInfo.jobTimeLimit));
            yield return null;
        }
        FailJob();
    }
        

    IEnumerator JobStageInProgress(JobStageInfo jobStageInfo, Sprite previous)
    {
        float startTime = Time.time;
        while (Time.time - startTime < jobStageInfo.progressDuration)
        {
            //Debug.Log("Job Stage in Progress " + jobStageInfo.jobBuilding + " " + (jobStageInfo.progressDuration - (Time.time - startTime)));
            if (jobItem.transform.parent.tag != jobStageInfo.jobBuilding.ToString())
            {
                startTime = Time.time;
                jobNotification.DisableBar();
                jobNotification.ParentToObject(jobItem.transform.parent, jobNotification.playerOffset);
                jobNotification.UpdateJobNotification(previous);
                jobItem.buildingState = jobStageInfo.jobBuilding;
            }
            else
            {
                jobNotification.EnableBar();
                jobNotification.UpdateJobNotification(jobStageInfo.stageImage, (Time.time - startTime) / jobStageInfo.progressDuration);
                jobNotification.ParentToObject(jobItem.transform.parent, 0);
                if (Mathf.RoundToInt((Time.time - startTime)) >= jobStageInfo.progressDuration && jobManager.failedJobs < jobManager.jobsRequiredToLose){
                    if(!FindObjectOfType<GameManager>().Over && (!PlayerPrefs.HasKey("Mute") || PlayerPrefs.GetInt("Mute") == 0)){
                            FindObjectOfType<AudioManager>().Play("Done");
                    }
                }
            }
            yield return null;
        }
    

    }

    IEnumerator JobStageExpiration(JobStageInfo jobStageInfo)
    {
        float startTime = Time.time;
        while (Time.time - startTime < jobStageInfo.completeExpirationDuration)
        {
            //Debug.Log("Job Expiring " + jobStageInfo.jobBuilding + " " + (jobStageInfo.completeExpirationDuration - (Time.time - startTime)));
            if (jobItem.transform.parent.tag != jobStageInfo.jobBuilding.ToString())
            {
                yield break;
            }
            yield return null;
        }
        FailJob();
    }

    public void FailJob()
    {
        Debug.Log("Job Failed ");
        jobManager.failedJobs++;
         if(!FindObjectOfType<GameManager>().Over && (!PlayerPrefs.HasKey("Mute") || PlayerPrefs.GetInt("Mute") == 0)) {
             if(jobManager.failedJobs<jobManager.jobsRequiredToLose && !FindObjectOfType<GameManager>().isPaused)
                FindObjectOfType<AudioManager>().Play("jobFailed");
         }
        //StopAllCoroutines();
        Destroy(gameObject);
    }

    public void CompleteJob()
    {
        Debug.Log("Job Complet ");
        jobManager.completedJobs++;
        score = Mathf.RoundToInt(((jobInfo.jobTimeLimit - timeTaken)/jobInfo.jobTimeLimit)*50);
        FindObjectOfType<GameManager>().score += score;
        if(!FindObjectOfType<GameManager>().Over && (!PlayerPrefs.HasKey("Mute") || PlayerPrefs.GetInt("Mute") == 0)) 
        FindObjectOfType<AudioManager>().Play("DeliverySound");
        //StopAllCoroutines();
        Destroy(gameObject);
    }
}
