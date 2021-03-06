﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePointObstacle : MonoBehaviour
{
    public GameObject doublePointObjectDeathEffect;

    public ScoreDisplay scoreTextPop;
    public GameObject floatingTextPrefab;
    public AudioSource doublePointBoxBreakAudio;
    public AudioClip doublePointBoxClip;
    public GameObject spawnConfiner;
    public TimeManager timeManager;
    public static float comboSlowMo = 1;

    public static int velocityPower = 135;

    public AudioClip combo1SoundDouble;
    public AudioClip combo2SoundDouble;
    public AudioClip combo3SoundDouble;
    public AudioClip combo4SoundDouble;
    public AudioClip combo5SoundDouble;

    private void Start()
    {
        spawnConfiner = GameObject.FindGameObjectWithTag("Confiner");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    

        if (ScoreDisplay.score >= ScoreDisplay.multiplierGoal)
        {
            ScoreDisplay.scoreMultiplier += ScoreDisplay.scoreMultiplierIncreaser;
            scoreTextPop.scoreMultiplierText.fontSize = 70;
            ScoreDisplay.multiplierGoal *= 3;
        }

        if (collision.gameObject.tag.Equals("Bomb"))
        {
            Destroy(gameObject);
            ScoreDisplay.score += 10;
        }

        if (collision.gameObject.tag.Equals("OrbBullet"))
        {
            GameObject.Find("OrbBulletHitEffect").GetComponent<AudioSource>().Play();
            ScoreDisplay.score += ComboHandler.doubleScoreValue * ScoreDisplay.scoreMultiplier;
            scoreTextPop.scoreText.fontSize = 100;
            Die();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag.Equals("Player"))
        {
            if (!SlingShot.isHeldDown)
            {
                ComboHandler.hitCount++;
            }else if (SlingShot.isHeldDown)
            {
                ComboHandler.hitCount = 1;
            }

            doublePointBoxBreakAudio.Play();

            //Trigger floating text if prefab is not null

            ScoreDisplay.score += ComboHandler.doubleScoreValue * ScoreDisplay.scoreMultiplier * ComboHandler.hitCount;

            if (floatingTextPrefab)
            {
                ShowFloatingText(ComboHandler.doubleScoreValue * ScoreDisplay.scoreMultiplier * ComboHandler.hitCount, new Color32(89, 74, 0, 255));
            }

            scoreTextPop.scoreText.fontSize = 100;

            CameraShake.Instance.ShakeCamera(13f, 0.2f);

            Vector3 vel = collision.gameObject.GetComponentInParent<Rigidbody2D>().velocity;
            var force = transform.position - collision.transform.position;
            force.Normalize();

            if (SlingShot.isInBerzerkMode)
            {
                collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce((-force * vel.magnitude * (125 * 0.15f)));
            }
            else
            {
                collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce((-force * vel.magnitude * velocityPower));
            }
                       
            Die();
        }
    }

    private void Die()
    {
        if (SlingShot.isHeldDown == false)
        {
            timeManager.Invoke("StopSlowMotion", 0.05f);
        }

        DisableObject();
        GameObject newDeathEffect = (GameObject)Instantiate(doublePointObjectDeathEffect, transform.position, Quaternion.identity);
        Destroy(newDeathEffect, 2);
        Destroy(gameObject, doublePointBoxClip.length);
    }

    private void ShowFloatingText(int hitScore, Color32 color)
    {
        IncreaseComboFloatScoreSize();
        var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = hitScore.ToString();
        go.GetComponent<TextMesh>().color = color;
    }

    private void DisableObject()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
            sr.enabled = false;

        this.gameObject.GetComponent<ParticleSystem>().Stop();
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void IncreaseComboFloatScoreSize()
    {

        if (ComboHandler.hitCount < 2)
        {
            doublePointBoxBreakAudio.clip = combo1SoundDouble;
            doublePointBoxBreakAudio.Play();
            comboSlowMo = 1f;
            floatingTextPrefab.GetComponent<TextMesh>().fontSize = 20;
        }
        else if (ComboHandler.hitCount == 2)
        {
            doublePointBoxBreakAudio.clip = combo2SoundDouble;
            doublePointBoxBreakAudio.Play();
            comboSlowMo = 1f;
            floatingTextPrefab.GetComponent<TextMesh>().fontSize = 30;
        }
        else if (ComboHandler.hitCount == 3)
        {
            doublePointBoxBreakAudio.clip = combo3SoundDouble;
            doublePointBoxBreakAudio.Play();
            comboSlowMo = 2.25f;
            timeManager.StartSlowMotion(0.2f);
            floatingTextPrefab.GetComponent<TextMesh>().fontSize = 40;
        }
        else if (ComboHandler.hitCount == 4)
        {
            doublePointBoxBreakAudio.clip = combo4SoundDouble;
            doublePointBoxBreakAudio.Play();
            comboSlowMo = 4.5f;
            timeManager.StartSlowMotion(0.1f);
            floatingTextPrefab.GetComponent<TextMesh>().fontSize = 50;
        }
        else if (ComboHandler.hitCount == 5)
        {
            doublePointBoxBreakAudio.clip = combo5SoundDouble;
            doublePointBoxBreakAudio.Play();
            comboSlowMo = 6f;
            timeManager.StartSlowMotion(0.07f);
            floatingTextPrefab.GetComponent<TextMesh>().fontSize = 60;
        }
        else if (ComboHandler.hitCount > 5)
        {
            doublePointBoxBreakAudio.clip = combo5SoundDouble;
            doublePointBoxBreakAudio.Play();
            comboSlowMo = 6.75f;
            timeManager.StartSlowMotion(0.05f);
            floatingTextPrefab.GetComponent<TextMesh>().fontSize = 70;
        }

    }

}
