using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Script execution of this is done earlier than most because of the singleton thingy.
/// </summary>
public class GlobalVolumeController : MonoBehaviour
{
    // singleton greatness
    public static GlobalVolumeController PlayVisualFX { get; private set; }

    VolumeProfile volumeProfile = null;
    IEnumerator hyperspaceVisionCO = null;
    IEnumerator hurtVisionCO = null;


    private void Awake()
    {
        volumeProfile = GetComponent<Volume>().profile;

        if (PlayVisualFX == null)
        {
            PlayVisualFX = this;
        }
    }

    public void WarpSpeedEffects(float intensity, float duration, float inTransitionSpeed, float outTransitionSpeed)
    {
        ExplodeEffect();
        HyperspaceVision(intensity, duration, inTransitionSpeed, outTransitionSpeed);
    }

    public void HurtVisionEffect()
    {
        VignetteEffect(.8f, 1f, 1f);
    }

    public void ExplodeEffect()
    {
        ChromaticAberrationEffect();
        FilmGrainEffect();
        VignetteEffect(.4f, 1f, 1f);
    }

    void ChromaticAberrationEffect()
    {
        StartCoroutine(ChromaticAberrationEffectCoroutine());
    }

    IEnumerator ChromaticAberrationEffectCoroutine()
    {
        ChromaticAberration chromaticAberration;
        volumeProfile.TryGet(out chromaticAberration);
        chromaticAberration.intensity.value = 1f;

        yield return new WaitForSeconds(1f);

        while(chromaticAberration.intensity.value > 0f)
        {
            chromaticAberration.intensity.value -= Time.deltaTime;
            yield return null;
        }

        chromaticAberration.intensity.value = 0f;
    }

    void FilmGrainEffect()
    {
        StartCoroutine(FilmGrainEffectCoroutine());
    }

    IEnumerator FilmGrainEffectCoroutine()
    {
        FilmGrain filmGrain;
        volumeProfile.TryGet(out filmGrain);
        filmGrain.intensity.value = 1f;

        yield return new WaitForSeconds(1f);

        while (filmGrain.intensity.value > 0f)
        {
            filmGrain.intensity.value -= Time.deltaTime;
            yield return null;
        }

        filmGrain.intensity.value = 0f;
    }

    void VignetteEffect(float start, float duration, float rate)
    {
        StartCoroutine(VignetteEffectCoroutine(start, duration, rate));
    }

    IEnumerator VignetteEffectCoroutine(float start, float duration, float rate)
    {
        Vignette vignette;
        volumeProfile.TryGet(out vignette);
        vignette.intensity.value = start;

        yield return new WaitForSeconds(duration);

        float timeStart = Time.realtimeSinceStartup;
        while (vignette.intensity.value > 0f)
        {
            vignette.intensity.value -= Time.deltaTime * rate;
            yield return null;
        }

        //print($"{Time.realtimeSinceStartup - timeStart}");
        vignette.intensity.value = 0f;
    }

    /// <summary>
    /// By default it takes about 1 second. The higher the number, the faster. The lower, the slower.
    /// You can't pass in a number less than 0.001 or else it gets reset to 1f.
    /// </summary>
    /// <param name="rate"></param>
    public void FadeIn(float rate = 1)
    {
        rate = rate < 0.001f ? 1f : rate;
        StartCoroutine(FadeInCoroutine(rate));
    }

    IEnumerator FadeInCoroutine(float rate)
    {
        ColorAdjustments colorAdjustments;
        volumeProfile.TryGet(out colorAdjustments);
        colorAdjustments.postExposure.value = -15f;

        float timeStart = Time.realtimeSinceStartup;
        while (colorAdjustments.postExposure.value < 0f)
        {
            colorAdjustments.postExposure.value += Time.deltaTime * 30f * rate;
            yield return null;
        }

        float endTime = Time.realtimeSinceStartup - timeStart;
        //print($"end : {endTime}");
        colorAdjustments.postExposure.value = 0f;
    }

    /// <summary>
    /// By default it takes about 1 second. The higher the number, the faster. The lower, the slower.
    /// You can't pass in a number less than 0.001 or else it gets reset to 1f.
    /// </summary>
    /// <param name="rate"></param>
    public void FadeOut(float rate = 1)
    {
        rate = rate <= 0.001f ? 1f : rate; 
        StartCoroutine(FadeOutCoroutine(rate));
    }

    IEnumerator FadeOutCoroutine(float rate)
    {
        ColorAdjustments colorAdjustments;
        volumeProfile.TryGet(out colorAdjustments);
        colorAdjustments.postExposure.value = 0;

        while (colorAdjustments.postExposure.value > -15f)
        {
            colorAdjustments.postExposure.value -= Time.deltaTime * 30f * rate;
            yield return null;
        }

        colorAdjustments.postExposure.value = -15f;
    }

    /// <summary>
    /// Distorts lens' vision to make it look like you are moving fast.
    /// Intensity [-1,1] distorts lens. -1 concaves... +1 convexes. Just don't make it 0.
    /// Duration will be the length of time it stays within the hypervision. Should be greater than zero.
    /// inTransitionSpeed is the speed it transitions into hyperspace vision. Should be greater than zero.
    /// outTransitionSpeed is the speed it transitions out of hyperspace visio. Should be greater than zero.
    /// Default values if none are assigned (-0.7f, 0.5f, 1f, 1f)
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="duration"></param>
    /// <param name="speedTransitionIntoHyperspaceVision"></param>
    public void HyperspaceVision(float intensity, float duration, float inTransitionSpeed, float outTransitionSpeed)
    {
        intensity = intensity == 0f ? -0.7f : intensity;
        duration = duration <= 0f ? 0.5f : duration;
        inTransitionSpeed = inTransitionSpeed <= 0f ? 1f : inTransitionSpeed;
        outTransitionSpeed = outTransitionSpeed <= 0f ? 1f : outTransitionSpeed;

        intensity = Mathf.Clamp(intensity, -1f, 1f);

        // if it is null, it means it is free. 
        // if it is not null, then it is currently performing the coroutine. it will just reset it and start over its duration
        if (hyperspaceVisionCO == null)
        {
            hyperspaceVisionCO = HyperspaceVisionCoroutine(intensity, duration, inTransitionSpeed, outTransitionSpeed);
            StartCoroutine(hyperspaceVisionCO);
        }
        else if(hyperspaceVisionCO != null)
        {
            StopCoroutine(hyperspaceVisionCO);
            hyperspaceVisionCO = HyperspaceVisionCoroutine(intensity, duration, inTransitionSpeed, outTransitionSpeed);
            StartCoroutine(hyperspaceVisionCO);
        }
    }

    IEnumerator HyperspaceVisionCoroutine(float intensity, float duration, float inTransitionSpeed, float outTransitionSpeed)
    {
        LensDistortion lensDistortion;
        volumeProfile.TryGet(out lensDistortion);
        lensDistortion.intensity.value = 0f;

        if (intensity < 0f)
        {
            while (lensDistortion.intensity.value > intensity)
            {
                lensDistortion.intensity.value -= Time.deltaTime * inTransitionSpeed;
                yield return null;
            }
        }
        else if(intensity > 0f)
        {
            while (lensDistortion.intensity.value < intensity)
            {
                lensDistortion.intensity.value += Time.deltaTime * inTransitionSpeed;
                yield return null;
            }
        }

        yield return new WaitForSeconds(duration);

        if (intensity < 0f)
        {
            while (lensDistortion.intensity.value < 0f)
            {
                lensDistortion.intensity.value += Time.deltaTime * outTransitionSpeed;
                yield return null;
            }
        }
        else if(intensity > 0f)
        {
            while (lensDistortion.intensity.value > 0f)
            {
                lensDistortion.intensity.value -= Time.deltaTime * outTransitionSpeed;
                yield return null;
            }
        }

        lensDistortion.intensity.value = 0f;
        hyperspaceVisionCO = null;
    }
}