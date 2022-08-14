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

    private bool isMouseDown;

    private Vector2 archerTopPos;
    private Vector2 mouseCurrentPos;
    private float mouseCurrentDist;

    public float forceConstant;
    public float maxForce;
    private float launchForce;
    private float percentagePower;
    private Vector2 forceVector;
    private Rigidbody2D rb2dArrow;

    private GraphicalElement graphicalElement;
    private GameControl gameControl;

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

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(75, Screen.height / 2f, 10));

        archerTopPos = archerTop.transform.position;

        rb2dArrow = arrow.GetComponent<Rigidbody2D>();

        graphicalElement = GraphicalElement.Instance;
        gameControl = GameControl.Instance;

        graphicalElement.GenerateTrajectory(archerTopPos);     
        graphicalElement.TogglePowerAndAngleMeter(false);        

    }


    // Update is called once per frame
    void Update()
    {        

        if (isMouseDown)
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
        mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseCurrentDist = Vector2.Distance(transform.position, mouseCurrentPos);

        //Debug.Log(mouseCurrentDist);
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
        Vector2 inputVector = (mouseCurrentPos - archerTopPos).normalized;
        float inputAngle = Mathf.Atan2(inputVector.y, inputVector.x);
        inputAngle = (inputAngle < 0f) ? (inputAngle + 2f * Mathf.PI) * Mathf.Rad2Deg : inputAngle * Mathf.Rad2Deg;
        return inputAngle;
    }

    void OnMouseDown()
    {
        if (gameControl.arrowsLeft > 0 && gameControl.acceptPlayerInput)
        {
            SetMouseDown(true);
        }
    }    

    void OnMouseUp()
    {  
        if (gameControl.arrowsLeft > 0 && gameControl.acceptPlayerInput)
        {
            CreateArrow();
        }

        SetMouseDown(false);
    }

    private void SetMouseDown(bool state)
    {
        isMouseDown = state;
        graphicalElement.trajectoryGroup.SetActive(state);
        graphicalElement.TogglePowerAndAngleMeter(state);
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
