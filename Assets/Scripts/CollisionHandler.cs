using UnityEngine;
using UnityEngine.Playables; // Add this import
using System.Collections;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] float deathTimer = 2f;
    [SerializeField] int health = 5;
    [SerializeField] float gravityMultiplier = 2f;

    // New fields for flashing effect
    [SerializeField] float flashDuration = 0.15f;
    [SerializeField] private MeshRenderer meshRenderer;
    private Shader originalShader;
    [SerializeField] private Shader flashShader;

    // We'll store the current flash coroutine here
    private Coroutine flashRoutine;

    Rigidbody rb;
    PlayerMovement playerMovement;
    GameSceneManager gameSceneManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        gameSceneManager = FindObjectOfType<GameSceneManager>();

        if (rb != null)
        {
            // Ensure initially gravity is off (if that is how you design your player)
            rb.useGravity = false;
        }
            
        // Store the original shader from the meshRenderer
        if (meshRenderer != null)
        {
            originalShader = meshRenderer.material.shader;
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Player hit " + other.transform.root.gameObject.name);    
        ProcessHit();
    }
 
    void ProcessHit()
    {
        
        if (health > 0)
        {
            // Refresh the flash each time we're hit
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(FlashWhiteCoroutine());
        }
        
        health--;
        if (health <= 0)
        {
            DisablePLayableDirector();
            DisableControls();
            EnableGravity();
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            
            //Reloads the level from Public GameSceneManager
            gameSceneManager.ReloadLevel();

            //Timer to destroy player ship
            Invoke("DestroyPlayer", deathTimer);
        }
    }

    private void EnableGravity()
    {
        // Enable gravity and apply a multiplier by adding extra gravity force (could also be done in FixedUpdate)
        if (rb != null)
        {
            rb.useGravity = true;
            // Multiply gravity force by gravityMultiplier
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1f) * rb.mass, ForceMode.Acceleration);
        }
    }

    private void DisableControls()
    {
        // Disable ProcessRotation and ProcessTranslation in PlayerMovement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    private void DisablePLayableDirector()
    {
        // Disable any PlayableDirector on the parent-most game object to remove timeline control
        PlayableDirector director = GetComponentInParent<PlayableDirector>();
        if (director != null)
        {
            director.enabled = false;
        }
    }

    void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    private IEnumerator FlashWhiteCoroutine()
    {
        if (meshRenderer == null) yield break;

        // Change material to flash shader
        meshRenderer.material.shader = flashShader;
        yield return new WaitForSeconds(flashDuration);

        // Revert to original shader
        meshRenderer.material.shader = originalShader;
        flashRoutine = null; // Reset our reference
    }
}
