using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(-1)]
public class Gameplay : MonoBehaviour
{
    public static Gameplay Instance { get; private set; }
    
    private InputManager inputManager;

    [SerializeField] private TextMeshProUGUI textMesh;

    [HideInInspector] public static int currentScore = 0;

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

        inputManager = InputManager.Instance;
    }

    // Subscribing to the event which fires upon user input.
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        inputManager.OnTouchInput += Shoot;
    }

    // Unsubscribing to the event which fires upon user input.
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
        inputManager.OnTouchInput -= Shoot;
    }

    // Updating the score only while the user is playing.
    private void Update()
    {
        if (GameStates.CurrentGameState == GameStates.StatesOfGame.Playing)
        {
            textMesh.text = currentScore.ToString();
        }
    }

    private void OnSceneChange(Scene current, Scene next)
    {
        inputManager = InputManager.Instance;
        currentScore = 0;
    }

    // Seeing if we hit any balloons with the ray transferred from the InputManager class.
    // If we hit, we call the Boom method in the Balloon class of the balloon that we hit.
    private void Shoot(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Balloon")))
        {
            GameObject obj = hit.transform.gameObject;
            obj.GetComponent<Balloon>().Boom();
        }
    }
}
