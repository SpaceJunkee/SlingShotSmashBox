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

    private void Start()
    {
        spawnConfiner = GameObject.FindGameObjectWithTag("Confiner");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            doublePointBoxBreakAudio.Play();

            //Trigger floating text if prefab is not null

            ScoreDisplay.score += (5 * 2) * ScoreDisplay.scoreMultiplier;

            if (floatingTextPrefab)
            {
                ShowFloatingText(10 * ScoreDisplay.scoreMultiplier, new Color32(89, 74, 0, 255));
            }

            scoreTextPop.scoreText.fontSize = 100;

            if (ScoreDisplay.score % 50 == 0)
            {
                ScoreDisplay.scoreMultiplier++;
                scoreTextPop.scoreMultiplierText.fontSize = 70;
            }

            CameraShake.Instance.ShakeCamera(13f, 0.2f);
            Vector3 vel = collision.gameObject.GetComponentInParent<Rigidbody2D>().velocity;
            var force = transform.position - collision.transform.position;
            force.Normalize();

            collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce((-force * vel.magnitude * 125));
            Die();
        }
    }

    private void Die()
    {
        DisableObject();
        Destroy(gameObject, doublePointBoxClip.length);
        Instantiate(doublePointObjectDeathEffect, transform.position, Quaternion.identity);
    }

    private void ShowFloatingText(int hitScore, Color32 color)
    {
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

}
