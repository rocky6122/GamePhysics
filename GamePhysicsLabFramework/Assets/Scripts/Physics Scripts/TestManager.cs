using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] GameObject particleObj;
    [SerializeField] GameObject slope;
    [SerializeField] GameObject platform;
    private Particle2D particle;

    private void Start()
    {
        particle = particleObj.GetComponent<Particle2D>();
        Reset();
    }

    private void Reset()
    {
        particleObj.SetActive(false);
        slope.SetActive(false);
        platform.SetActive(false);

        particle.force = Vector2.zero;
    }

    private void SetUpGravity()
    {
        //particle.SetTest(Test.gravity);
        particle.SetRotation(0f);
        particle.position = Vector2.zero;
        particle.acceleration = Vector2.zero;
        particle.velocity = Vector2.zero;

        particleObj.SetActive(true);
    }

    private void SetUpSliding()
    {
        //particle.SetTest(Test.sliding);
        particle.SetRotation(0f);
        particle.position = Vector2.zero;
        particle.acceleration = Vector2.zero;
        particle.velocity = Vector2.zero;

        particleObj.SetActive(true);
        platform.SetActive(true);
    }

    private void SetUpFriction()
    { 
       //particle.SetTest(Test.friction);
        particle.SetRotation(45f);
        particle.position = new Vector2(-2.75f, 2.25f);
        particle.acceleration = Vector2.zero;
        particle.velocity = Vector2.zero;

        particleObj.SetActive(true);
        slope.SetActive(true);
    }

    private void SetUpDrag()
    {
        //particle.SetTest(Test.drag);
        particle.SetRotation(45f);
        particle.position = new Vector2(-2.75f, 2.25f);
        particle.acceleration = Vector2.zero;
        particle.velocity = Vector2.zero;

        particleObj.SetActive(true);
        slope.SetActive(true);
    }

    private void SetUpSpring()
    {
       //particle.SetTest(Test.spring);
        particle.SetRotation(0f);
        particle.position = Vector2.up;
        particle.acceleration = Vector2.zero;
        particle.velocity = Vector2.zero;

        particleObj.SetActive(true);
    }

    public void ChangeTest(int test)
    {
        Reset();

        switch (test)
        {
            case 1:
                SetUpSliding();
                break;
            case 2:
                SetUpFriction();
                break;
            case 3:
                SetUpDrag();
                break;
            case 4:
                SetUpSpring();
                break;
            case 5:
                SetUpGravity();
                break;
            default:
                break;
        }
    }
}
