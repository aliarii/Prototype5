using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerAdditional : MonoBehaviour
{
    public GameObject[] targets;
    private float spawnRate = 1.0f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public bool isGameActive;
    public bool isGamePaused;
    private int score;
    private int lives;
    public GameObject titleScreen;

    public GameObject pauseMenu;
    private AudioSource backgroundAudio;
    public Slider volumeSlider;


    // Start is called before the first frame update
    void Start()
    {
        backgroundAudio = GetComponent<AudioSource>();

        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        backgroundAudio.volume = volumeSlider.value;
        backgroundAudio.Play();

    }
    public void StartGame(int difficulty)
    {

        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        //backgroundAudio.volume = 0.5f;
        spawnRate /= difficulty;
        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        isGamePaused = false;
        score = 0;
        lives = 3;
        UpdateScore(0);
        StartCoroutine(SpawnTarget());
    }
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Length);
            Instantiate(targets[index]);
        }
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
    public void ControlLives()
    {
        lives--;
        if (lives < 1)
        {
            GameOver();
            lives = 0;
        }
        livesText.text = "Lives: " + lives;
    }

    public void GameOver()
    {
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void PauseGame()
    {

        if (!isGamePaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            isGamePaused = true;

        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            isGamePaused = false;
        }

    }
    private void Update()
    {
        backgroundAudio.volume = volumeSlider.value;
        if (isGameActive)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }
        }
    }
}
