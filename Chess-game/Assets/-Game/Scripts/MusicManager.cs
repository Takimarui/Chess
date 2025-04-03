using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _musicOfTheMove;
    [SerializeField] private float fadeDuration = 2f;
    private float targetVolume;
    private float startVolume;
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MenuScene")
        {
            _backgroundMusic.volume = 0f;
            targetVolume = 1f;

            StartFadeIn();
        }
    }

    private void Update()
    {
        if (isFadingIn)
        {
            FadeInMusic();
        }

        if (isFadingOut)
        {
            FadeOutMusic();
        }
    }


    private void FadeInMusic()
    {
        _backgroundMusic.volume += Time.deltaTime / fadeDuration;

        if (_backgroundMusic.volume >= targetVolume)
        {
            _backgroundMusic.volume = targetVolume;
            isFadingIn = false;
        }
    }

    private void FadeOutMusic()
    {
        _backgroundMusic.volume -= Time.deltaTime / fadeDuration;

        if (_backgroundMusic.volume <= 0f)
        {
            _backgroundMusic.volume = 0f;
            isFadingOut = false;
            _backgroundMusic.Stop();
        }
    }

    public void StartFadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
        _backgroundMusic.Play();
    }

    public void StartFadeOut()
    {
        isFadingOut = true;
        isFadingIn = false;
    }
    public void musicOfTheMove()
    {
        _musicOfTheMove.volume = Random.Range(0.4f, 1f);
        _musicOfTheMove.Play();
    }
}
