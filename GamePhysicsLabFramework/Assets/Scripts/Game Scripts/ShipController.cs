using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    public static ShipController instance;

    PlayerControls controls;
    Particle3D particle;

    public CollisionManager manager;

    public Transform phaserCannonPos;

    Vector3 moveSpeed;

    Bullet temp;

    float health;
    const float MAXHEALTH = 100;

    float horizontalInput;
    float verticalInput;

    float thrusterInput;

    float rotSpeed = .15f;

    [SerializeField] private Image healthbarImage;
    [SerializeField] private TMPro.TMP_Text pointsText;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        health = MAXHEALTH;
        //assign variables
        controls = new PlayerControls();
        particle = gameObject.GetComponent<Particle3D>();
        moveSpeed = new Vector3(10, 0, 0);

        //Assign the input controls into movement directions
        controls.Starship.VerticalMovement.performed += ctx => verticalInput = ctx.ReadValue<float>();
        controls.Starship.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<float>();

        //Phasers
        controls.Starship.Shoot.started += ignoredCTX => FirePhaser();

        //Thrusters
        controls.Starship.Thrusters.performed += ctx => thrusterInput = ctx.ReadValue<float>();

        //Enable controls
        controls.Starship.Enable();
    }

    void FixedUpdate()
    {
        //Update Position
        UpdatePosition();

        //Apply Torque as needed
        UpdateRotation();
    }

    public void HandleACollision(CollisionHull3D col)
    {
        if (col.gameObject.CompareTag("Asteroid"))
        {
            health -= 34;

            healthbarImage.fillAmount = health / MAXHEALTH;

            if (health <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
        }
    }

    public void ChangePoints(int points)
    {
        pointsText.text = points.ToString();
    }

    private void UpdatePosition()
    {
        //Move the ship forward in space
        if (thrusterInput > 0)
        {
            particle.AddForce(ForceGenerator.GenerateForce_drag(particle.velocity, Vector3.zero, 1, 4f, .95f));
        }
        else
        {
            particle.AddForce(ForceGenerator.GenerateForce_Gravity(particle.GetMass(), 2f, transform.forward));
        }

        if (horizontalInput > .3f)
        {
            particle.AddForce(moveSpeed);
        }
        else if(horizontalInput < -.3f)
        {
            particle.AddForce(-moveSpeed);
        }
        else //Reduce particle Velocity over time
        {
            if (particle.velocity.x < -.1f)
            {
                particle.velocity.x += .02f;
            }
            else if (particle.velocity.x > .1f)
            {
                particle.velocity.x -= .02f;
            }
            else
            {
                particle.velocity.x = 0;
            }
        }

        particle.velocity = ClampMagnitude(particle.velocity, 4);
    }

    private void UpdateRotation()
    {
        if (verticalInput > .3f)
        {
            particle.ApplyTorque(transform.forward, Vector3.up * rotSpeed);
        }
        else if (verticalInput < -.3f)
        {
            particle.ApplyTorque(transform.forward, -(Vector3.up * rotSpeed));
        }
        else //Reduce angular Velocity over time
        {
            if (particle.angularVelocity.x < -.01f)
            {
                particle.angularVelocity.x += .005f;
            }
            else if (particle.angularVelocity.x > .01f)
            {
                particle.angularVelocity.x -= .005f;
            }
            else
            {
                particle.angularVelocity = Vector3.zero;
            }
        }
    }

    //Shoot Phasers
    void FirePhaser()
    {
       temp = Instantiate((GameObject)Resources.Load("Phaser"), phaserCannonPos.position, Quaternion.identity).GetComponent<Bullet>();

        Debug.Log(temp);

       temp.Initialize(particle.velocity, phaserCannonPos.position, phaserCannonPos, manager);
    }

    public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
    {
        float sqrmag = vector.sqrMagnitude;
        if (sqrmag > maxLength * maxLength)
        {
            float mag = (float)Mathf.Sqrt(sqrmag);
            //these intermediate variables force the intermediate result to be
            //of float precision. without this, the intermediate result can be of higher
            //precision, which changes behavior.
            float normalized_x = vector.x / mag;
            float normalized_y = vector.y / mag;
            float normalized_z = vector.z / mag;
            return new Vector3(normalized_x * maxLength,
                normalized_y * maxLength,
                normalized_z * maxLength);
        }
        return vector;
    }
}
