using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 200f;
    [SerializeField] float thrustSpeed = 800f;

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
            Thrust();
            Rotate();
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
                state = State.Transcending;
                audioSource.Stop();
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {
            float movementThisFrame = thrustSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(Vector3.up * movementThisFrame);
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
    }

    private void Rotate()
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
}
