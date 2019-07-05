using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] AudioClip mainEngineNoiseOne;
    [SerializeField] AudioClip mainEngineNoiseTwo;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    [SerializeField] float rcsThrust = 220f;
    [SerializeField] float mainThrust = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        //if(state != State.Alive)
        //{
        //    audioSource.Stop();
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive){return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            case "Finish":
                SuccessSequence();
                break;
            default:
                DeathSequence();
                break;
        }
    }

    private void SuccessSequence()
    {
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        state = State.Transcending;
        Invoke("LoadNextScene", 1f);
    }

    private void DeathSequence()
    {
        crashParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        state = State.Dying;
        Invoke("LoadFirstScene", 1f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); // todo allow for mote than 2 levels
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0); // todo allow for mote than 2 levels
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float mainThrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * mainThrustThisFrame);
            if (!audioSource.isPlaying)
            {
                System.Random random = new System.Random();
                int rand = random.Next(0, 2);
                if (rand == 1)
                {
                    audioSource.PlayOneShot(mainEngineNoiseOne);
                }
                else
                {
                    audioSource.PlayOneShot(mainEngineNoiseTwo);
                }
            }
                mainEngineParticles.Play();

        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
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
