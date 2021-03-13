using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelSuccess;
    [SerializeField] AudioClip deathExplosion;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem levelSuccessParticles;
    [SerializeField] ParticleSystem deathExplosionParticles;
    enum State {  Alive, Dying, Transcending };
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state==State.Alive) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
 
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    //print("Friend");
                    break;
                }

            case "Finish":
                {
                    //print("Game Finish");
                    StartSuccessSequence();
                    break;
                }

            default:
                {
                    //print("You Died");
                    StartDeathSequence();
                    break;
                }

        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathExplosion);
        deathExplosionParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelSuccess);
        levelSuccessParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
        state = State.Alive;
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        float rotationSpeed = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) && (state == State.Alive))
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.A) && (state == State.Alive))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    private void RespondToThrustInput()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            ApplyThrust();

        }
        else
        {
            audioSource.Stop();
        }
        rigidBody.freezeRotation = false; //resume physics control of rotatio
    }

    private void ApplyThrust()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
        //rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
    }
}
