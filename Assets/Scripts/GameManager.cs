using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image fadeImage;
    public Text scoreText, highScoreText;
    private int score;
    private Blade blade;
    private Spawner spawner;
    public GameObject gameOverScreen;
    private AudioSource gameManagerAudioSource;
    public AudioClip bombSound, gameMusic;

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        gameManagerAudioSource = GetComponent<AudioSource>();
        highScoreText.text = "BestScore: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        gameManagerAudioSource.clip = gameMusic;
        gameManagerAudioSource.loop = true;
        gameManagerAudioSource.Play();
        gameOverScreen.SetActive(false);
        blade.enabled = true;
        spawner.enabled = true;

        Fruit[] existedFruits = FindObjectsOfType<Fruit>();
        Bomb[] existedBombs = FindObjectsOfType<Bomb>();

        foreach(Fruit fruit in existedFruits)
        {
            Destroy(fruit.gameObject);
        }

        foreach(Bomb bomb in existedBombs)
        {
            Destroy(bomb.gameObject);
        }

        score = 0;
        scoreText.text = score.ToString();
        Time.timeScale = 1.0f;        
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOverScreen.SetActive(true);        
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = "Best Score: " + PlayerPrefs.GetInt("HighScore", score).ToString();
        }
    }

    public void Explosion()
    {
        gameManagerAudioSource.Stop();
        gameManagerAudioSource.loop = false;
        gameManagerAudioSource.clip = bombSound;
        gameManagerAudioSource.Play();
        blade.enabled = false;
        spawner.enabled = false;
        StartCoroutine(ExplosionSequence());
    }

    public IEnumerator ExplosionSequence()
    {
        float elapsed = 0.0f;
        float duration = 1.5f;
        gameManagerAudioSource.Play();

        while (elapsed < duration)
        {    
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        GameOver();
    }
}
