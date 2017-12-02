using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum OverStatus
{
    None,
    Win,
    Lose
}

public class UIManager : MonoBehaviour
{
    public Text textCloudCount;
    public Slider sliderSpeed;
    public Button btnRetry;
    public GameObject overPanel;
    public Text overText;

    private static UIManager _instace;
    private OverStatus currentState = OverStatus.None;

    private readonly Color32 WIN_COLOR = new Color32(0,204,204,255);
    private readonly Color32 LOSE_COLOR = new Color32(55, 66, 77, 255);

    private Image mask;
    private Animator overAnim;

    public static UIManager Instace
    {
        get
        {
            return _instace;
        }

        private set
        {
            _instace = value;
        }
    }

    private void Awake()
    {
        _instace = this;
    }

    private void Start()
    {
        UpdateText();

        btnRetry.gameObject.SetActive(false);
        overText.gameObject.SetActive(false);
        btnRetry.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });

        mask = overPanel.GetComponent<Image>();
        overAnim = overPanel.GetComponent<Animator>();
    }

    public void UpdateText()
    {
        string cloudCount = string.Format("{0}/{1}", DataManager.Instance.CloudCount, DataManager.CloudLimit);
        textCloudCount.text = cloudCount;
    }

    public void UpdateSpeed(float speed)
    {
        sliderSpeed.value = speed;
    }

    public void Retry()
    {
        btnRetry.gameObject.SetActive(true);
    }

    public void GameOver(OverStatus status)
    {
        if (currentState != OverStatus.None)
        {
            return;
        }
        currentState = status;
        switch (status)
        {
            case OverStatus.Win:
                mask.color = WIN_COLOR;
                overText.text = "You Win!";
                break;

            case OverStatus.Lose:
                mask.color = LOSE_COLOR;
                overText.text = "Lost?";
                break;

            default:
                break;
        }
        overText.gameObject.SetActive(true);
        overAnim.enabled = true;
        Retry();
    }
}