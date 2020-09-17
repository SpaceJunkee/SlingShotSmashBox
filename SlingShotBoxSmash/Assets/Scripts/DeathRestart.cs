﻿
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathRestart : MonoBehaviour
{

    public GameObject deathEffect;
    public TimeManager timeManager;
    public GameObject playerBig;
    GameObject[] musicObject;
    public AudioSource audio;

    private void Update()
    {
        if (this.gameObject.tag.Equals("Projectile") && GreenOrbShield.isGreenShieldActive == true)
        {
            GameObject.FindGameObjectWithTag("Projectile").GetComponent<CircleCollider2D>().isTrigger = false;
        }
        else if(this.gameObject.tag.Equals("Projectile") && GreenOrbShield.isGreenShieldActive == false)
        {
            GameObject.FindGameObjectWithTag("Projectile").GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag.Equals("Lava") && SlingShot.isHeldDown)
        {

        }
        else
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                GameObject.Find("PlayerDeathSound").GetComponent<AudioSource>().Play();
                Destroy(collision.gameObject);
                DestroyPlayer();
                SlingShot.isHeldDown = false;
            }
        }                     
        
    }

    public void DestroyPlayer()
    {
        SlingShot.isDead = true;
        playerBig.GetComponent<TrailRenderer>().enabled = false;
        Instantiate(deathEffect, playerBig.gameObject.transform.position, Quaternion.identity);
        CameraShake.Instance.ShakeCamera(25f, 0.75f);
        timeManager.StartSlowMotion(0.3f);
        Invoke("RestartGame", 0.5f);
    }


    void Start()
    {
        playerBig = GameObject.FindGameObjectWithTag("PlayerBig");
        musicObject = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObject.Length == 1)
        {
            audio.Play();
        }
        else
        {
            for (int i = 1; i < musicObject.Length; i++)
            {
                Destroy(musicObject[i]);
            }

        }
    }

    void Awake()
    {
        DontDestroyOnLoad(audio);
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("NormalCurrency", PlayerPrefs.GetInt("NormalCurrency") + Mathf.RoundToInt(ScoreDisplay.score) / 10);
        PlayerPrefs.SetInt("MoneyEarned", Mathf.RoundToInt(ScoreDisplay.score) / 10 + ScoreDisplay.moneyBagsDrops);
        GameObject music = GameObject.Find("Music");
        SlingShot.isDead = false;
        playerBig.GetComponent<TrailRenderer>().enabled = true;

        if (music.GetComponent<AudioSource>().isPlaying == false && music.GetComponent<AudioSource>()!= null)
        {
            music.GetComponent<AudioSource>().Play();
        }

        ResetScores();
        timeManager.StopSlowMotion();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ResetScores()
    {
        SpawnObjects.hasFirstBombGoneOff = false;
        ScoreDisplay.score = 0;

        if(GreenOrbShield.hasGreenShieldBeenBought == true)
        {
            GreenOrbShield.isGreenShieldActive = true;
        }
        


        if (PlayerPrefs.GetInt("AbilityTile3") == 1)
        {
            ScoreDisplay.scoreMultiplier = 4;
            ScoreDisplay.scoreMultiplierIncreaser = 1;
        }
        else
        {
            ScoreDisplay.scoreMultiplier = 1;
            ScoreDisplay.scoreMultiplierIncreaser = 1;
        }

        ScoreDisplay.multiplierGoal = 50;
        Destroy(GameObject.FindGameObjectWithTag("Projectile"));

        ComboHandler.ResetValues();
    }
}
