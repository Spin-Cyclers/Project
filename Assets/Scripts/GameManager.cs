using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Manages the state of the game
public class GameManager : MonoBehaviour
{
    public JobManager jobManager;
    public Text HighScoreText;
    public Text finalScore;
    public GameObject GameOverUI;
    private AudioManager audioManager;
    private float startSoundTimer = 0;
    private float songTimer = 0;
    private float loseTimer = 0;
    private float endSongTimer = 0;
    private bool hasPlayed = false;
    private bool hasPlayedEnd = false;
    public bool Over = false;
    public int score = 0;
    public bool isPaused = false; 
    


    private void Awake()
    {
        jobManager = FindObjectOfType<JobManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    void Start(){
        if(!PlayerPrefs.HasKey("Mute") || PlayerPrefs.GetInt("Mute") == 0)
            audioManager.Play("GameStartReset");
            if(PlayerPrefs.HasKey("Mute") && PlayerPrefs.GetInt("Mute") == 1){
         for(int i = 0; i<audioManager.sounds.Length; i++)
            audioManager.Mute(audioManager.sounds[i].name);
        }
    }
    //Display Game Over Screen
    public void GameOver(){
        if (score > PlayerPrefs.GetInt("HighScore",0))
            PlayerPrefs.SetInt("HighScore", score);
        HighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore",0).ToString();
        finalScore.text = "Score: " + score;
        GameOverUI.SetActive(true); 
            audioManager.Stop("Music");
        if(!isPaused && !Over)
            audioManager.Play("Lose");
        Over = true;
    }   
     public void Restart(){
         SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reload the current scene
         
    }
     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetPlayerPrefs()
    {
        if(PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.DeleteKey("HighScore"); //clear high score
        if(PlayerPrefs.HasKey("Mute"))
            PlayerPrefs.DeleteKey("Mute"); //clear high score
    }
        
    void OnApplicationFocus(bool hasFocus){
        isPaused = !hasFocus;
       if (isPaused){
           //Debug.Log("2 "+songTimer);
            if(!Over){
                if (startSoundTimer<2){
                    //audioManager.Pause("GameStartReset");
                    audioManager.Stop("GameStartReset");
                }
                else{
                    //audioManager.Pause("Music");
                    songTimer -= Time.deltaTime;
                    audioManager.Stop("Music");
                }
            }
            else{
                if (loseTimer<5.55){
                    //audioManager.Pause("Lose");
                    audioManager.Stop("Lose");
                }
                else{
                    //audioManager.Pause("GameOver");
                    //endSongTimer -= 4*Time.deltaTime;
                    audioManager.Stop("GameOver");
                }
                    
            }
        }
        else{
            if(!Over){
                if (startSoundTimer<2){
                    audioManager.Play("GameStartReset");
                    startSoundTimer = -3*Time.deltaTime;//0;
                  
                }
                else{
                    audioManager.Play("Music");
                     hasPlayed = true;
                     songTimer = -3*Time.deltaTime;//0;;
                }
                //Debug.Log("3 "+songTimer);
            }
            else{
                if (loseTimer<5.55){
                    audioManager.Play("Lose");
                    loseTimer = -3*Time.deltaTime;//0;;
                }
                else{
                    audioManager.Play("GameOver");
                    hasPlayedEnd = true;
                    endSongTimer = -3*Time.deltaTime;//0;;
                }
            }
        }
        }
void OnApplicationPause(bool pauseStatus)
    {
        OnApplicationFocus(!pauseStatus);
    }
    // Update is called once per frame
    void Update()
    {
        
    if(!isPaused){
       if (startSoundTimer<2)
            startSoundTimer += Time.deltaTime;
        else if (!hasPlayed && !Over) {
           audioManager.Play("Music");
            hasPlayed = true;
        }
        
        else{
            songTimer += Time.deltaTime;
           // Debug.Log(songTimer);
            if(songTimer>39){
                songTimer = 0;
                hasPlayed = false;
            }
        }
        if (Over){
            if(loseTimer < 5.55){
                loseTimer += Time.deltaTime;
            }
            else if(!hasPlayedEnd){
                audioManager.Play("GameOver");
                hasPlayedEnd = true;
            }
            
            else{
                endSongTimer += Time.deltaTime;
                if(endSongTimer> 24.55){
                    endSongTimer = 0;
                    hasPlayedEnd = false;

                }
            }

        }
    }

        if (Input.GetKeyDown("r") && Over)
            Restart();
        if (Input.GetKeyDown("e"))
            GameOver();
        if (Input.GetKeyDown("m")){
            if(!PlayerPrefs.HasKey("Mute"))
                PlayerPrefs.SetInt("Mute", 1);
            else  PlayerPrefs.SetInt("Mute", -1*(PlayerPrefs.GetInt("Mute")-1)); // toggles between 1 and 0
            for(int i = 0; i<audioManager.sounds.Length; i++)
            audioManager.Mute(audioManager.sounds[i].name);
            if(Over){
                audioManager.Stop("Music");
            }
            /*if(PlayerPrefs.GetInt("Mute") == 0 && !Over){
            audioManager.Play("Music");
             hasPlayed = true;
             songTimer = 0;
            }
            else if (PlayerPrefs.GetInt("Mute") == 0){
            audioManager.Play("GameOver");
            hasPlayedEnd = true;
            loseTimer = 6;
            endSongTimer = 0;
            }
            */
        }
         if (Over && Input.GetKeyDown("escape"))
            Application.OpenURL("https://www.laundr.io/");
        
    }
}