using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 200f;
    [SerializeField] float thrustSpeed = 800f;
    [SerializeField] float leveLoadDelay = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Use this for initialization
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
            RespondToThrustInput();
            RespondToRotationInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive){ return; } // Ignore collisions when dead
        switch (collision.gameObject.tag)
        {
            case "Friendly":
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
        state = State.Transcending;
        mainEngineParticles.Stop();
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        Invoke("LoadNextLevel", leveLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        mainEngineParticles.Stop();
        deathParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", leveLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput()
    {
            float movementThisFrame = thrustSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(movementThisFrame);
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void RespondToRotationInput()
    {
        rigidBody.freezeRotation = true;
            float rotationThisFrame = rotationSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.forward * rotationThisFrame);
            }
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {

                transform.Rotate(Vector3.back * rotationThisFrame);
            }
            rigidBody.freezeRotation = false;
    }

    private void ApplyThrust(float movementThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * movementThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
