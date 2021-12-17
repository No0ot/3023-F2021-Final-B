using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WorldController : MonoBehaviour
{
    private static WorldController instance;
    public static WorldController Instance { get { return instance; } }

    [Header("References")]
    public GameObject player;
    public AudioSource audioSource;
    public List<AudioClip> worldAudioEffects;
    float pitchVariance = 0.1f;

    [Header("World Effects")]
    [SerializeField]
    GameObject globalLight;
    [SerializeField]
    List<GameObject> weatherEffects;
    public Color eventLighting;

    [Header("Regular Day")]
    [SerializeField]Color regularDayColor;
    [SerializeField] Color timeColor;
    [SerializeField]float regularDayIntensity;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        worldAudioEffects = new List<AudioClip>();
    }

    private void Update()
    {
        if (worldAudioEffects.Count > 0)
        {
            if (!audioSource.isPlaying)
            {
                int temp = Random.Range(1, 101);
                if (temp < 2)
                {
                    PlayRandomWorldAudioEffect();
                }
            }
        }
        else
            audioSource.Stop();
    }

    public void UpdateEventLighting(Color newcolor, float intensity)
    {
        eventLighting = newcolor;
        globalLight.GetComponent<Light2D>().intensity = intensity;
        UpdateLighting();
    }

    public void UpdateDayLighting(Color newcolor)
    {
        timeColor = newcolor;
        UpdateLighting();
    }

    public void UpdateLighting()
    {
        globalLight.GetComponent<Light2D>().color = (timeColor + eventLighting) / 2;
    }

    public void PlayWeatherEffect(int i)
    {
        foreach(GameObject effect in weatherEffects)
        {
            effect.SetActive(false);
        }
        weatherEffects[i].SetActive(true);
    }


    public void ActivateEvent(CalendarEvent cEvent)
    {
        if (cEvent.alterSun)
            UpdateEventLighting(cEvent.sunColor, cEvent.sunIntensity);
        else if (cEvent.alterNaturalLight)
            UpdateDayLighting(cEvent.sunColor);
            
        if (cEvent.hasWeather)
            PlayWeatherEffect(cEvent.weatherEvent);

        if (cEvent.worldAudioEffect)
        {
            if (!worldAudioEffects.Contains(cEvent.worldAudioEffect))
            {
                worldAudioEffects.Add(cEvent.worldAudioEffect);
            }
        }
        if (cEvent.playerAudioEffect)
        {
            if (!player.GetComponent<FootstepSounds>().footstepSoundClips.Contains(cEvent.playerAudioEffect))
            {
                player.GetComponent<FootstepSounds>().AddFootstepAudioClip(cEvent.playerAudioEffect);
            }
        }
    }

    public void EndEvent(CalendarEvent cEvent)
    {
        if (cEvent.playerAudioEffect)
        {
            if (cEvent.playerAudioEffect)
            {
                player.GetComponent<FootstepSounds>().RemoveFootstepAudioClip(cEvent.playerAudioEffect);
            }
        }
        if (cEvent.worldAudioEffect)
        {
            if (cEvent.worldAudioEffect)
            {
                worldAudioEffects.Remove(cEvent.worldAudioEffect);
            }
        }
    }

    public void RegularDay()
    {
        UpdateEventLighting(regularDayColor, regularDayIntensity);
        foreach(GameObject weather in weatherEffects)
        {
            weather.SetActive(false);
        }
    }

    public void PlayRandomWorldAudioEffect()
    {
        audioSource.clip = worldAudioEffects[Random.Range(0, worldAudioEffects.Count)];
        audioSource.pitch = 1.0f + Random.Range(-pitchVariance, pitchVariance);
        audioSource.Play();
    }
}
