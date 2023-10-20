using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MovementEffectType
{
    SHAKE,
    WAVE,
    DISTORT
}

// Possible Effects
// ------------------------------------------------
// Bold             ->      <b>             </b>
// Italicize        ->      <i>             </i>
// Underline        ->      <u>             </u>    [Does Not Work With Type Writer Effect]
// Strikethrough    ->      <s>             </s>    [Does Not Work With Type Writer Effect]
// Wavy             ->      <w(s)>          </w>    (s = Strength)
// Shakey           ->      <sh(s)>         </sh>   (s = Strength)
// Distort          ->      <d(s)>          <\d>    (s = Strength)
// Color            ->      <c(r,g,b,a)>    </c>    (r = Red, g = Green, b = Blue, a = Alpha)
// Size             ->      <sz(s)>         </sz>   [Does Not Work] (s = Size)
// Screen Shake     ->      <ss(s)>         </ss>   [Is Currently Not Implemented] (s = Strength)
// Typewriter Speed ->      <sp(s)>         </sp>   (s = Speed)

// Audio Event      ->      <ae(e)>                 (e = Event [String])

public class DialogueEffectManager
{
    public List<MovementEffect> movementEffectIndexes = new List<MovementEffect>();
    public List<BasicEffect> screenShakeEffectIndexes = new List<BasicEffect>();
    public List<BasicEffect> typeWriterSpeedEffectIndexes = new List<BasicEffect>();
    public List<BasicEffect> sizeEffectIndexes = new List<BasicEffect>();
    public List<ColorEffect> colorEffectIndexes = new List<ColorEffect>();
    public List<AudioEffect> audioEffectsIndexes = new List<AudioEffect>();

    private bool ranOneTimeEffects = false;

    private List<Vector3> originalTextPositions = new List<Vector3>();

    public void ApplyEffects(TextMeshProUGUI textUI)
    {
        ShakeWaveDistort(textUI, movementEffectIndexes);

        if (!ranOneTimeEffects)
        {
            ranOneTimeEffects = true;
            //ApplySizeEffect(textUI);
        }
    }

    public float ApplyTypingSpeedEffect(int index)
    {
        foreach (BasicEffect spEffect in typeWriterSpeedEffectIndexes)
        {
            if (spEffect.effectIndex == index)
            {
                return spEffect.effectValue;
            }
        }

        return 1f;
    }

    public void ApplyColorEffect(TextMeshProUGUI textUI, int index, Color defaultColor)
    {
        Color charColor = defaultColor;

        foreach (ColorEffect cEffect in colorEffectIndexes)
        {
            if (cEffect.effectIndex == index)
            {
                charColor = cEffect.effectColor;
                break;
            } 
        }

        var textInfo = textUI.textInfo;
        int materialIndex = textInfo.characterInfo[index].materialReferenceIndex;
        var meshInfo = textInfo.meshInfo[materialIndex];
        int vertexIndex = textInfo.characterInfo[index].vertexIndex;

        meshInfo.colors32[vertexIndex] = charColor;
        meshInfo.colors32[vertexIndex + 1] = charColor;
        meshInfo.colors32[vertexIndex + 2] = charColor;
        meshInfo.colors32[vertexIndex + 3] = charColor;
        textUI.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    public void ApplySizeEffect(TextMeshProUGUI textUI)
    {
        foreach (BasicEffect szEffect in sizeEffectIndexes)
        {
            //textUI.fontSize = szEffect.effectValue;
            return;
        }

        //textUI.fontSize = 38;
    }

    public void ApplyScreenShakeEffect(int index)
    {
        foreach (BasicEffect ssEffect in screenShakeEffectIndexes)
        {
            if (ssEffect.effectIndex == index)
            {
                //Trigger Screen Shake
            }
        }
    }

    public void ApplyAudioEffect(int index)
    {
        foreach (AudioEffect aEffect in audioEffectsIndexes)
        {
            if (aEffect.effectIndex == index)
            {
                AudioManager.Instance.PlayOneShot(FMODEventManager.Instance.GetEventReferenceFromID(aEffect.effectValue), Vector3.zero);
            }
        }
    }

    public void ShakeWaveDistort(TextMeshProUGUI textUI, List<MovementEffect> movementEffects)
    {
        var textInfo = textUI.textInfo;

        foreach (MovementEffect effect in movementEffects)
        {
            var charInfo = textInfo.characterInfo[effect.effectIndex];

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            float randomNum1 = Random.Range(-2, 2);
            float randomNum2 = Random.Range(-2, 2);

            for (int j = 0; j < 4; j++)
            {
                var orig = originalTextPositions[effect.effectIndex * 4 + j];

                Vector3 offset = Vector3.zero;



                if (effect.effectType.Contains(MovementEffectType.SHAKE))
                {
                    offset = offset + new Vector3(randomNum1, randomNum2, 0) * effect.effectValue[effect.effectType.IndexOf(MovementEffectType.SHAKE)];
                }

                if (effect.effectType.Contains(MovementEffectType.WAVE))
                {
                    offset = offset + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0) * effect.effectValue[effect.effectType.IndexOf(MovementEffectType.WAVE)];
                }

                if (effect.effectType.Contains(MovementEffectType.DISTORT))
                {
                    offset = offset + new Vector3(Random.Range(-1.9f, 1.9f), Random.Range(-1.9f, 1.9f), 0) * effect.effectValue[effect.effectType.IndexOf(MovementEffectType.DISTORT)];
                }

                verts[charInfo.vertexIndex + j] = orig + offset;
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textUI.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    public void SaveOriginalTextPositions(TextMeshProUGUI textUI)
    {
        textUI.ForceMeshUpdate();

        var textInfo = textUI.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                originalTextPositions.Add(orig);
            }
        }
    }

    public void ResetEffects()
    {
        movementEffectIndexes.Clear();
        screenShakeEffectIndexes.Clear();
        typeWriterSpeedEffectIndexes.Clear();
        sizeEffectIndexes.Clear();
        colorEffectIndexes.Clear();
        audioEffectsIndexes.Clear();

        ranOneTimeEffects = false;
        originalTextPositions.Clear();
    }

    public string FormatDialogueText(string text)
    {
        ResetEffects();

        bool waveEffect = false;
        bool shakeEffect = false;
        bool distortEffect = false;
        bool screenShakeEffect = false;
        bool typeWriterSpeedEffect = false;
        bool sizeEffect = false;
        bool colorEffect = false;
        bool audioEffect = false;

        float waveStrength = 1;
        float shakeStrength = 1;
        float distortStrength = 1;
        float screenShakeStrength = 1;
        float typeWriterSpeed = 1;
        float size = 1;
        string audioID = "";
        Color color = Color.black;

        string formattedText = "";

        int skipIndexes = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            string effect = "";
            string data = "";

            if (c == '<')
            {
                while (c != '>' && i < 10000)
                {
                    if (c == '(')
                    {
                        while (c != ')' && i < 10000)
                        {
                            data = data + c;
                            i++;
                            c = text[i];
                        }
                        data = data + c;
                    } 
                    else
                    {
                        effect = effect + c;
                    }
                    i++;
                    c = text[i];
                }
                effect = effect + c;

                skipIndexes = skipIndexes + effect.Length + data.Length;

                //Debug.Log($"Skipping {skipIndexes} indexes, the skipped text is [{effect}] and [{data}] with a combined length of {effect.Length + data.Length}");

                switch (effect)
                {
                    case "<w>":
                        waveEffect = true;
                        if (data != "")
                        {
                            waveStrength = float.Parse(data.Substring(1, data.Length - 2));
                        } 
                        break;
                    case "<sh>":
                        shakeEffect = true;
                        if (data != "")
                        {
                            shakeStrength = float.Parse(data.Substring(1, data.Length - 2));
                        }
                        break;
                    case "<d>":
                        distortEffect = true;
                        if (data != "")
                        {
                            distortStrength = float.Parse(data.Substring(1, data.Length - 2));
                        }
                        break;
                    case "<c>":
                        colorEffect = true;
                        string[] values = data.Substring(1, data.Length - 2).Split(",");
                        color = new Color(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), float.Parse(values[3]));
                        break;
                    case "<sz>":
                        sizeEffect = true;
                        size = float.Parse(data.Substring(1, data.Length - 2));
                        break;
                    case "<ss>":
                        screenShakeEffect = true;
                        if (data != "")
                        {
                            screenShakeStrength = float.Parse(data.Substring(1, data.Length - 2));
                        }
                        break;
                    case "<sp>":
                        typeWriterSpeedEffect = true;
                        typeWriterSpeed = float.Parse(data.Substring(1, data.Length - 2));
                        break;
                    case "<ae>":
                        audioEffect = true;
                        audioID = data.Substring(1, data.Length - 2);
                        break;
                    case "</w>":
                        waveEffect = false;
                        waveStrength = 1;
                        break;
                    case "</sh>":
                        shakeEffect = false;
                        shakeStrength = 1;
                        break;
                    case "</d>":
                        distortEffect = false;
                        distortStrength = 1;
                        break;
                    case "</c>":
                        colorEffect = false;
                        break;
                    case "</sz>":
                        sizeEffect = false;
                        break;
                    case "</ss>":
                        screenShakeEffect = false;
                        screenShakeStrength = 1;
                        break;
                    case "</sp>":
                        typeWriterSpeedEffect = false;
                        break;
                    default:
                        Debug.LogError($"An invalid ENTER/EXIT effect was found [{effect}] - [{data}]");
                        break;
                }
            }
            else
            {
                formattedText = formattedText + c;

                if (c == ' ')
                {
                    continue;
                }

                if (waveEffect || shakeEffect || distortEffect)
                {
                    MovementEffect mEffect = new MovementEffect();
                    mEffect.effectIndex = i - skipIndexes;

                    if (waveEffect)
                    {
                        mEffect.effectValue.Add(waveStrength);
                        mEffect.effectType.Add(MovementEffectType.WAVE);
                    }

                    if (shakeEffect)
                    {
                        mEffect.effectValue.Add(shakeStrength);
                        mEffect.effectType.Add(MovementEffectType.SHAKE);
                    }

                    if (distortEffect)
                    {
                        mEffect.effectValue.Add(distortStrength);
                        mEffect.effectType.Add(MovementEffectType.DISTORT);
                    }

                    movementEffectIndexes.Add(mEffect);
                }

                if (colorEffect)
                {
                    ColorEffect cEffect = new ColorEffect();
                    cEffect.effectIndex = i - skipIndexes;
                    cEffect.effectColor = color;
                    colorEffectIndexes.Add(cEffect);
                }

                if (sizeEffect)
                {
                    BasicEffect sEffect = new BasicEffect();
                    sEffect.effectIndex = i - skipIndexes;
                    sEffect.effectValue = size;
                    sizeEffectIndexes.Add(sEffect);
                }

                if (screenShakeEffect)
                {
                    BasicEffect ssEffect = new BasicEffect();
                    ssEffect.effectIndex = i - skipIndexes;
                    ssEffect.effectValue = screenShakeStrength;
                    screenShakeEffectIndexes.Add(ssEffect);
                }

                if (typeWriterSpeedEffect)
                {
                    BasicEffect spEffect = new BasicEffect();
                    spEffect.effectIndex = i - skipIndexes;
                    spEffect.effectValue = typeWriterSpeed;
                    typeWriterSpeedEffectIndexes.Add(spEffect);
                }

                if (audioEffect)
                {
                    AudioEffect aEffect = new AudioEffect();
                    aEffect.effectIndex = i - skipIndexes;
                    aEffect.effectValue = audioID;
                    audioEffectsIndexes.Add(aEffect);
                    audioEffect = false;
                }
            }
        }

        return formattedText;
    }
}

public class BasicEffect
{
    public int effectIndex;
    public float effectValue;
}

public class MovementEffect
{
    public int effectIndex;

    public List<float> effectValue = new List<float>();

    public List<MovementEffectType> effectType = new List<MovementEffectType>();
}

public class ColorEffect
{
    public int effectIndex;
    public Color effectColor = Color.white;
}

public class AudioEffect
{
    public int effectIndex;
    public string effectValue;
}