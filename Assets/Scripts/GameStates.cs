using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-3)]
public class GameStates : MonoBehaviour
{
    public static GameStates Instance { get; private set; }

    [SerializeField] private Scene gameScene;
    [SerializeField] private Scene menuScene;

    public enum StatesOfGame
    {
        Menu,
        Playing,
        Pause,
        Failed,
    }

    public enum StatesOfDifficulty
    {
        Easy,
        Medium,
        Hard,
    }

    public static StatesOfGame CurrentGameState;
    private StatesOfGame tempGameState;

    public delegate void ChangeGameState(StatesOfGame fromState, StatesOfGame toState);
    public event ChangeGameState OnChangeGameState;

    public static bool isPaused;

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

        CurrentGameState = StatesOfGame.Menu;
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void Start()
    {
        tempGameState = CurrentGameState;
    }

    private void Update()
    {
        if (tempGameState != CurrentGameState)
        {
            OnChangeGameState(tempGameState, CurrentGameState);

            switch (CurrentGameState)
            {
                case StatesOfGame.Menu:
                    SceneManager.SetActiveScene(menuScene);
                    Time.timeScale = 1;
                    break;

                case StatesOfGame.Playing:
                    SceneManager.SetActiveScene(gameScene);
                    Gameplay.currentScore = 0;
                    Time.timeScale = 1;
                    break;

                case StatesOfGame.Pause:
                    Time.timeScale = 0;
                    break;

                case StatesOfGame.Failed:
                    Time.timeScale = 0;
                    break;
            }
        }

        tempGameState = CurrentGameState;
    }

    private void OnSceneChange(Scene current, Scene next)
    {

    }
}
