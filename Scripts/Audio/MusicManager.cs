using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager = null;

    [Header("Music")]
    [SerializeField] AudioClip tutorial;
    [SerializeField] AudioClip level001;
    [SerializeField] AudioClip level002;
    [SerializeField] AudioClip level003;
    [SerializeField] AudioClip level004;
    [SerializeField] AudioClip ending;
    [SerializeField] AudioClip musicTest;

    AudioFade audioFade = null;
    AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioFade = GetComponent<AudioFade>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager.OnWarpSceneBegin += NewSceneLoadStart;
        gameManager.OnWarpSceneFinished += NewSceneLoadFinished;
    }

    private void OnDestroy()
    {
        gameManager.OnWarpSceneBegin -= NewSceneLoadStart;
        gameManager.OnWarpSceneFinished -= NewSceneLoadFinished;
    }

    private void NewSceneLoadStart(Level level)
    {
        audioFade.FadeOut();
    }

    private void NewSceneLoadFinished(Level level)
    {
        audioSource.Stop();


        audioFade.FadeIn();
        audioSource.clip = GetAudioClipByLevel(level);
        audioSource.Play();
    }

    AudioClip GetAudioClipByLevel(Level level)
    {
        switch (level)
        {
            case Level.Level001:    return level001;
            case Level.Level002:    return level002;
            case Level.Level003:    return level003;
            case Level.Level004:    return level004;
            case Level.Ending:      return ending;
            case Level.Tutorial:    return tutorial;
            default:                return musicTest;
        }
    }
}
