using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rcsThrust = 100f;    //rcs: reaction control system
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float levelLoadDelay = 2f;

    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip deadRocketExplosion;
    [SerializeField] private AudioClip levelChangeSound;

    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem deadExplosionParticles;
    [SerializeField] private ParticleSystem levelChangeParticles;

    bool collisionsDisabled = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
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
        if(state == State.Alive)
        {
            Thrusting();
            RocketRotation();
        }
        DebugKeys();    //remove on final build
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || collisionsDisabled) { return; }    //ignore the next logic

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        //Load level2
        state = State.Transcending;
        audioSource.Stop();
        levelChangeParticles.Play();
        audioSource.PlayOneShot(levelChangeSound);
        Invoke("LoadNextLevel", levelLoadDelay);    //make this a design lever
    }

    private void StartDeathSequence()
    {
        //load previous level
        state = State.Dying;
        audioSource.Stop();
        deadExplosionParticles.Play();
        audioSource.PlayOneShot(deadRocketExplosion);
        Invoke("LoadPreviousLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadPreviousLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;

        if (previousSceneIndex < 1)
        {
            previousSceneIndex = 0;
        }

        SceneManager.LoadScene(previousSceneIndex);
    }

    private void Thrusting()
    {
        //Thrust control
        if (Input.GetKey(KeyCode.Space))
        {
            //print("Thrusting");
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }
        
        //Sound and particle effects
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void RocketRotation()
    {
        rigidBody.angularVelocity = Vector3.zero;       //remove physics rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }

    private void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }
}
