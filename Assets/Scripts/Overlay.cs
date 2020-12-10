using UnityEngine;
using UnityEngine.UI;
public class Overlay : MonoBehaviour
{
    public Text scoreText;
    public Text failedJobsText;
    private JobManager jobManager;
    private GameManager GameManager;

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
        jobManager = FindObjectOfType<JobManager>();
    }

    private void Update()
    {
        if (jobManager.failedJobs >= jobManager.jobsRequiredToLose)
            GameManager.GameOver();
        failedJobsText.text = "Failed Jobs: " +
            jobManager.failedJobs +
            "/" +
            jobManager.jobsRequiredToLose;
        scoreText.text = "Score: " + GameManager.score;
    }
}
