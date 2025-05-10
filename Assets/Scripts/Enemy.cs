using UnityEngine;
using System.Collections;
// using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Playables; // Add this import
// using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 10;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float deletionDelay = 1f;
    [SerializeField] float gravityMultiplier = 2f;
    [SerializeField] float flashDuration = 0.15f;
    
    private Rigidbody rb;
    private bool inDeathSequence = false;
    
    [SerializeField] private MeshRenderer meshRenderer;
    private Shader originalShader;
    [SerializeField] private Shader flashShader;
    
    // We'll store the current flash coroutine here
    private Coroutine flashRoutine;
    
    Scoreboard scoreboard;
    [SerializeField] int scoreValue = 10;

    private void Awake()
    {
        scoreboard = FindFirstObjectByType<Scoreboard>();

        rb = GetComponent<Rigidbody>();        
        rb.useGravity = false;
        
        if (meshRenderer != null)
        {
            originalShader = meshRenderer.material.shader;
        }
    }
    
    private void FixedUpdate()
    {
        if (inDeathSequence)
        {
            rb.AddForce(Physics.gravity * rb.mass * gravityMultiplier);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        TakeDamage();
    }

    private void TakeDamage()
    {
        if (!inDeathSequence && health > 0)
        {
            // Refresh the flash each time we're hit
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(FlashWhiteCoroutine());
        }

        health--;

        if (health <= 0 && !inDeathSequence)
        {
            inDeathSequence = true;
            rb.useGravity = true;
            scoreboard.IncreaseScore(scoreValue);

            // Disable Playable Director component at the top-most parent game object
            PlayableDirector director = GetComponentInParent<PlayableDirector>();
            if (director != null)
            {
                director.enabled = false;
            }

            GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
            vfx.transform.parent = transform;
            StartCoroutine(DetachAndDestroy(vfx));
        }
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

    private IEnumerator DetachAndDestroy(GameObject vfx)
    {
        yield return new WaitForSeconds(deletionDelay);
        
        if (vfx != null)
        {
            vfx.transform.parent = null;
        }
        
        Destroy(gameObject);
    }
}
