using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float xClampRange = 5.0f;
    [SerializeField] float yClampRange = 5.0f;
    [SerializeField] float clampedYPosOffset = 1f;
    [SerializeField] float rollAmount = 0f;
    [SerializeField] float pitchAmount = 0f;
    [SerializeField] float yawAmount = 0f;
    [SerializeField] float rollSpeed = 5f;

    Vector2 moveInput;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * -2f;
        float pitchDueToControlThrow = moveInput.y * pitchAmount;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = moveInput.x * -yawAmount;

        float roll = moveInput.x * rollAmount;
        
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rollSpeed * Time.deltaTime);
    }

    public void ProcessTranslation()
    {
        float xOffset = moveInput.x * moveSpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xClampRange, xClampRange);
        
        float yOffset = moveInput.y * moveSpeed * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yClampRange + clampedYPosOffset, yClampRange + clampedYPosOffset);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, 0);
    }
}
