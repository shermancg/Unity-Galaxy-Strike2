using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] GameObject[] laserVFXs;
    [SerializeField] RectTransform crosshair;
    [SerializeField] Transform aimGuide;
    [SerializeField] float aimGuideZOffset = 10f;
    bool isFiring = false;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        LaserEmission();
        CrossHairPosition();
        MoveAimGuide();
        AimLasers();
    }


    public void OnFire(InputValue value)
    {
        //This recognizes if the button is pressed or not, and swaps the bool state. 
        //Since the PlayerInput control map listens to Press and Release, this will swap to true when the button is pressed (because default is false), and will swap back to false when released.
        isFiring = value.isPressed;
    }

    void LaserEmission()
    {
        //This method enables or disables the emission of the laser VFX based on the bool state in OnFire.
        foreach (GameObject laserVFX in laserVFXs)
        {
            var laserEmission = laserVFX.GetComponent<ParticleSystem>().emission;
            laserEmission.enabled = isFiring;
        }
        
    }

    void CrossHairPosition()
    {
        //This method sets the crosshair position to the mouse position.
        crosshair.position = Input.mousePosition;
    }

    void MoveAimGuide()
    {
        //This method moves the aim guide sphere to the mouse position.
        //The aim guide sphere is a child of the player, so it moves with the player.
        //The Z position is set to a fixed value, so it appears in front of the player.
        //The aim guide sphere is used to calculate the direction of the laser.
        //Be careful about Copilot auto complete making this Static, as updating postion with Vector 3 requires a non-static Method. (static void vs void)
        Vector3 aimGuidePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, aimGuideZOffset);
        aimGuide.position = Camera.main.ScreenToWorldPoint(aimGuidePosition);
        
    }

    void AimLasers()
    {
        //This method aims the lasers at the aim guide sphere.
        //In the first line, I'm having it determine the direction based on the aim guide position and the player's ship position, 
        //not the laser's position because that would cause them to cross paths out in the distance.
        //The second line is setting the rotation of the laser VFX to look at the aim guide sphere.
        foreach (GameObject laserVFX in laserVFXs)
        {
            Vector3 direction = aimGuide.position - this.transform.position;
            laserVFX.transform.rotation = Quaternion.LookRotation(direction);
        }
    }


}
