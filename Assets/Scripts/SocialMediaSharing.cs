using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SocialMediaSharing : MonoBehaviour
{
    GameManager gameMan;
    public Button Facebook;
    public Button Twitter;
    public Button Instagram;
    public Button Linkedin;

    void Awake()
    {
        gameMan = FindObjectOfType<GameManager>();
    }

	void Start () {
		Facebook.onClick.AddListener(TaskOnClick0);
        Twitter.onClick.AddListener(TaskOnClick1);
        Instagram.onClick.AddListener(TaskOnClick2);
        Linkedin.onClick.AddListener(TaskOnClick3);
	}

	void TaskOnClick0(){
         if(gameMan.Over)
		Application.OpenURL("https://www.facebook.com/laundrofficial/");
	}
    void TaskOnClick1(){
         if(gameMan.Over)
		Application.OpenURL("https://twitter.com/laundrofficial");
	}
    void TaskOnClick2(){
         if(gameMan.Over)
		Application.OpenURL("https://www.instagram.com/laundrofficial/");
	}
    void TaskOnClick3(){
        if(gameMan.Over)
		Application.OpenURL("https://www.linkedin.com/in/laundrofficial");
	}
}
