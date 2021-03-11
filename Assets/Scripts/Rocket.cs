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
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("Friend");
                    break;
                }

            case "Finish":
                {
                    print("Game Finish");
                    SceneManager.LoadScene(1);
                    break;
                }

            default:
                {
                    print("You Died");
                    SceneManager.LoadScene(0);
                    break;
                }

        }
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        float rotationSpeed = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    private void Thrust()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);

        }
        else
        {
            audioSource.Stop();
        }
        rigidBody.freezeRotation = false; //resume physics control of rotatio
    }
}
