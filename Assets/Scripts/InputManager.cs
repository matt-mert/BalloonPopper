using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-2)]
public class InputManager : MonoBehaviour
{
    // Basic singleton implementation.
    public static InputManager Instance { get; private set; }

    public delegate void TouchInput(Ray ray);
    public event TouchInput OnTouchInput;

    private TouchControls touchControls;
    private Camera currentCam;

    // Making the singleton persistent and caching the variables.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        touchControls = new TouchControls();
        currentCam = Camera.main;
    }

    // Subscribing to the scene change event.
    // Enabling the class that we generated via the input actions asset and
    // subscribing to the performed event of the touch input.
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;

        if (touchControls != null)
        {
            touchControls.Enable();
            touchControls.Touch.Position.performed += ctx => TouchPrimary(ctx);
        }
    }

    // Unsubscribing to the scene change event.
    // Unsubscribing to the performed event of the touch input and
    // disabling the class that we generated via the input actions asset.
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;

        if (touchControls != null)
        {
            touchControls.Touch.Position.performed -= ctx => TouchPrimary(ctx);
            touchControls.Disable();
        }
    }

    // Called when the activeSceneChange event is fired (i.e. upon scene change).
    // Re-caching the variables.
    private void OnSceneChange(Scene current, Scene next)
    {
        touchControls = new TouchControls();
        currentCam = Camera.main;
    }

    // Called when the performed event is fired (i.e. upon user input).
    // Transferring the ray that we shoot into the scene from the camera.
    // This ray will be used to see if we hit any balloons.
    // Firing the event will call every method listening to it.
    private void TouchPrimary(InputAction.CallbackContext ctx)
    {
        if (OnTouchInput != null)
        {
            Ray touchRay = currentCam.ScreenPointToRay(ctx.ReadValue<Vector2>());
            OnTouchInput(touchRay);
        }
    }
}
