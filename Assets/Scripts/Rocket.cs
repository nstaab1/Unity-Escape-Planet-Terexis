using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource rocketNoise;
    public AudioClip firstAudioClip;
    public AudioClip secondAudioClip;
    [SerializeField] float rcsThrust = 220f;
    [SerializeField] float mainThrust = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketNoise = GetComponent<AudioSource>();
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
                print("Friendly");
                break;
            default:
                print("Not Friendly");
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!rocketNoise.isPlaying)
            {
                System.Random random = new System.Random();
                int rand = random.Next(0, 2);
                if (rand == 1)
                {
                    rocketNoise.clip = firstAudioClip;
                } else
                {
                    rocketNoise.clip = secondAudioClip;
                }
                rocketNoise.Play();
            }

            float mainThrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * mainThrustThisFrame);
        }
        else
        {
            rocketNoise.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }
}
