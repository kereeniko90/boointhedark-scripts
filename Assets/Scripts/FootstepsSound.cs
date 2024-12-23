using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Footstep Sounds")]
    public AudioClip grassFootstepSound;
    public AudioClip stoneFootstepSound;
    

    [SerializeField] private float footstepInterval = 0.5f; // Time between footsteps
    private AudioSource audioSource;
    private CharacterController controller;

    private Transform player;
    private float footstepTimer = 0f;

    private void Start()
    {
        // Get the necessary components
        audioSource = GetComponent<AudioSource>();
        //player = transform.Find("pfPlayer").GetComponent<Transform>();
        controller = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        PlayFootstepSounds();
    }

    private void PlayFootstepSounds()
    {
        
        if (controller.velocity.magnitude <= 0.1f || !controller.isGrounded) {
            audioSource.Stop();
            return;
        }

        footstepTimer -= Time.deltaTime;
        if (footstepTimer > 0) return;

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            string groundTag = hit.collider.tag;

            switch (groundTag)
            {
                case "Grass":
                    audioSource.clip = grassFootstepSound;
                    break;
                case "Dungeon":
                    audioSource.clip = stoneFootstepSound;
                    break;
                
                default:
                    audioSource.clip = grassFootstepSound; 
                    break;
            }

            audioSource.Play();
             
        }

        footstepTimer = footstepInterval; 
    }
}
