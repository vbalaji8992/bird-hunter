using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class GraphicalElement : MonoBehaviour
    {
        public static GraphicalElement Instance { get; private set; }

        public GameObject trajectoryPoint;
        public int numberOfPoints;
        public float spaceBetweenPoints;
        public GameObject trajectoryGroup;
        private GameObject[] trajectoryPoints;

        public GameObject arrowIndicator;
        public GameObject arrowImage;

        public GameObject pauseButton;
        public GameObject pauseMenu;
        public GameObject scoreBoard;
        
        public TMP_Text powerMeter;
        public Image powerBar;
        public TMP_Text angleText;
        public TMP_Text levelTimer;

        public TMP_Text lastShotPower;
        public TMP_Text lastShotAngle;

        public GameObject levelScore;
        public Sprite[] levelScoreImage;
        public TMP_Text levelStatus;
        public TMP_Text levelNumber;

        public Image hand;
        public float handAnimationTime;
        private Animator handAnimator;

        private GameControl gameControl;
        private SaveGame saveGame;

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

        void Start()
        {
            gameControl = GameControl.Instance;
            saveGame = SaveGame.Instance;

            handAnimator = hand.GetComponent<Animator>();

            if (saveGame.CurrentData.showHand == true)
            {
                handAnimator.SetTrigger("Start");
                saveGame.CurrentData.showHand = false;
                saveGame.SaveData();
            }
        }

        void Update()
        {
            levelTimer.text = TimeSpan.FromSeconds(GameControl.Instance.timeLeft).ToString(@"mm\:ss");

            if (handAnimationTime <= 0)
            {
                handAnimator.SetTrigger("Stop");
            }
            else
            {
                handAnimationTime -= Time.deltaTime;
            }
        }

        public void GenerateArrowCount(int count)
        {
            int offset = -40;

            for (int i = 0; i < count; i++)
            {
                var arrow = Instantiate(arrowImage);
                arrow.transform.SetParent(arrowIndicator.transform, false);
                arrow.transform.localPosition = new Vector3((float)offset, 0, 0);
                offset += 30;
            }
        }

        public void GenerateTrajectory(Vector2 startingPosition)
        {
            trajectoryPoints = new GameObject[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                trajectoryPoints[i] = Instantiate(trajectoryPoint, startingPosition, Quaternion.identity);
                trajectoryPoints[i].transform.parent = trajectoryGroup.transform;
            }
        }

        public void UpdateTrajectory(Vector2 startingPosition)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                float pointPosition = i * spaceBetweenPoints;
                trajectoryPoints[i].transform.position = TrajectoryPointPosition(startingPosition, pointPosition);
            }
        }

        public void UpdatePowerBar(float percentagePower, float launchAngle)
        {
            powerMeter.text = (percentagePower * 100).ToString("00");
            powerBar.fillAmount = percentagePower;
            powerBar.transform.position = trajectoryPoints[0].transform.position + new Vector3(-1.15f, 1.25f, 0f);
            powerMeter.transform.position = powerBar.transform.position + new Vector3(powerBar.GetComponent<RectTransform>().rect.width * powerBar.transform.localScale.x * powerBar.fillAmount, 0f, 0f);

            angleText.text = launchAngle.ToString("00") + "°";
            angleText.transform.position = trajectoryPoints[3].transform.position + new Vector3(0.75f, 0.75f, 0f);
        }

        public void TogglePowerAndAngleMeter(bool state)
        {
            powerMeter.enabled = state;
            powerBar.enabled = state;
            angleText.enabled = state;
        }

        Vector2 TrajectoryPointPosition(Vector2 startingPosition, float t)
        {
            Vector2 position = startingPosition + (ArcherControl.Instance.GetInitialAccelerationVector() * Time.fixedDeltaTime * t) + (0.5f * Physics2D.gravity * t * t);
            return position;
        }

        public void UpdateArrowCountImage(int count)
        {
            const int transparent = 0;
            const int opaque = 255;

            foreach (Transform child in arrowIndicator.transform)
            {                
                child.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, transparent);
            }

            for (int i = 0; i < count; i++)
            {
                arrowIndicator.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, opaque);
            }
            
        }

        public void SetLevelScoreAndStatus(uint score)
        {
            levelScore.GetComponent<Image>().sprite = levelScoreImage[score];

            if(score == 0)
            {
                levelStatus.text = "YOU LOSE";
            }
            else
            {
                levelStatus.text = "YOU WIN";
            }
        }

        public void UpdateLastShotStats(float percentagePower, float launchAngle)
        {
            lastShotPower.text = (percentagePower * 100).ToString("00");
            lastShotAngle.text = launchAngle.ToString("00");
        }

        public void TogglePauseMenu(bool state)
        {
            pauseMenu.SetActive(state);
            pauseButton.SetActive(!state);
        }

        public void DisplayScoreBoard()
        {
            scoreBoard.SetActive(true);
            pauseButton.SetActive(false);
        }

        public void PauseGame()
        {
            TogglePauseMenu(true);
            gameControl.PauseGame();
        }

        public void ResumeGame()
        {
            TogglePauseMenu(false);
            gameControl.ResumeGame();
        }

        public void GoToMainMenu()
        {
            gameControl.GoToMainMenu();
        }

        public void ReloadLevel()
        {
            gameControl.ReloadLevel();
        }

        public void LoadNextLevel()
        {
            gameControl.LoadNextLevel();
        }

    }
}
