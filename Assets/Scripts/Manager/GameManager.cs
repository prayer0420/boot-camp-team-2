using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    public enum GameState
    {
        Lobby,
        GameScene,
        Pause,
        Win,
        Lose
    }

    public GameState CurrentState { get; private set; }

    public delegate void StateChanged(GameState newState);
    public event StateChanged OnStateChanged; // 상태 변경 이벤트

    private BrickManager brickManager;
    public LevelManager levelManager;


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

        levelManager = GetComponent<LevelManager>();
    }

    private void Start()
    {
        SetState(GameState.Lobby);

        brickManager = FindObjectOfType<BrickManager>();
        if (brickManager != null)
        {
            brickManager.OnAllBrickBroken += HandleAllBricksBroken;
        }
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState); 
    }


    private void HandleAllBricksBroken()
    {
        SetState(GameState.Win);
    }

    public void StartGameScene()
    {
        SetState(GameState.GameScene);
        SceneManager.LoadScene(1);
    }

    public void StartLobby()
    {   
        SetState(GameState.Lobby);
        SceneManager.LoadScene(0);
    }

}
