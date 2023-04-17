using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Assets.Scripts;

public class ArcherControl : MonoBehaviour
{
    public static ArcherControl Instance { get; private set; }

    public GameObject archerTop;
    public GameObject arrow;    

    private Vector2 archerTopPos;
    private float mouseCurrentDist;

    public float forceConstant;
    public float maxForce;
    private float launchForce;
    private float percentagePower;
    private Vector2 forceVector;
    private Rigidbody2D rb2dArrow;

    private GraphicalElement graphicalElement;
    private GameControl gameControl;
    private InputControl inputControl;

    void Awake()
    {      

        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {       

        transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.05f, Screen.height * 0.5f, 10));

        archerTopPos = archerTop.transform.position;

        rb2dArrow = arrow.GetComponent<Rigidbody2D>();

        graphicalElement = GraphicalElement.Instance;
        gameControl = GameControl.Instance;
        inputControl = InputControl.Instance;

        graphicalElement.GenerateTrajectory(archerTopPos);     
        graphicalElement.TogglePowerAndAngleMeter(false);
    }


    // Update is called once per frame
    void Update()
    {        

        if (InputControl.Instance.IsMouseDown)
        {
            float inputAngle = CalculateInputAngle();

            RotateArcherTop(inputAngle);

            CalculateLaunchForce();

            graphicalElement.UpdatePowerBar(percentagePower, GetLaunchAngle());

            forceVector = GetDirection(GetLaunchAngle(), launchForce);

            graphicalElement.UpdateTrajectory(archerTopPos);
        }
    }

    private float GetLaunchAngle()
    {
        return archerTop.transform.eulerAngles.z;
    }


    private void CalculateLaunchForce()
    {
        if (inputControl.MouseCurrentPosition.x < inputControl.MouseDownPosition.x)
        {
            mouseCurrentDist = Vector2.Distance(inputControl.MouseDownPosition, inputControl.MouseCurrentPosition);
        }        

        launchForce = forceConstant * mouseCurrentDist;

        if (launchForce > maxForce)
        {
            launchForce = maxForce;
        }

        percentagePower = launchForce / maxForce;
    }

    private void RotateArcherTop(float inputAngle)
    {
        if (inputAngle < 180f)
        {
            archerTop.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (inputAngle > 270f)
        {
            archerTop.transform.eulerAngles = new Vector3(0f, 0f, 90f);
        }
        else if (inputAngle < 270f && inputAngle > 180f)
        {
            archerTop.transform.eulerAngles = new Vector3(0f, 0f, inputAngle + 180);
        }
    }

    private float CalculateInputAngle()
    {
        Vector2 inputVector = (inputControl.MouseCurrentPosition - inputControl.MouseDownPosition).normalized;
        float inputAngle = Mathf.Atan2(inputVector.y, inputVector.x);
        inputAngle = (inputAngle < 0f) ? (inputAngle + 2f * Mathf.PI) * Mathf.Rad2Deg : inputAngle * Mathf.Rad2Deg;
        return inputAngle;
    }    

    public void CreateArrow()
    {
        GameObject arrowObj = Instantiate(arrow, archerTopPos, Quaternion.identity);

        arrowObj.GetComponent<Arrow>().Fire(forceVector);

        gameControl.UpdateArrowCountOnFire();

        graphicalElement.UpdateLastShotStats(percentagePower, GetLaunchAngle());
    }

    Vector2 GetDirection(float degree, float multiplier)
    {
        float radian = degree * Mathf.Deg2Rad;

        Vector2 directionVector = Vector2.zero;

        directionVector.x = Mathf.Cos(radian) * multiplier;
        directionVector.y = Mathf.Sin(radian) * multiplier;

        return directionVector;
    }

    public Vector2 GetInitialAccelerationVector()
    {
        return (forceVector / rb2dArrow.mass);
    }


}
