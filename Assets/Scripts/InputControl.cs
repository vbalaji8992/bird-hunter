﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public static InputControl Instance { get; private set; }

    public bool IsMouseDown { get; private set; }
    public Vector2 MouseDownPosition { get; private set; }
    public Vector2 MouseCurrentPosition { get; private set; }

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
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));

        graphicalElement = GraphicalElement.Instance;
        gameControl = GameControl.Instance;
    }

    // Update is called once per frame
    void Update()
    {        
        if (IsMouseDown)
        {
            MouseCurrentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);            
        }        
    }

    void OnMouseDown()
    {
        MouseDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (gameControl.arrowsLeft > 0 && gameControl.acceptPlayerInput)
        {
            SetMouseDown(true);
        }
    }

    void OnMouseUp()
    {
        if (gameControl.arrowsLeft > 0 && gameControl.acceptPlayerInput)
        {
            ArcherControl.Instance.CreateArrow();
        }

        SetMouseDown(false);
    }

    private void SetMouseDown(bool state)
    {
        IsMouseDown = state;
        graphicalElement.trajectoryGroup.SetActive(state);
        graphicalElement.TogglePowerAndAngleMeter(state);
    }
}