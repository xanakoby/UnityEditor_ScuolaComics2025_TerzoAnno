using System.Collections;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Renderer rend;
    Material rendMat;

    [Header("Shader Vars")]
    [SerializeField] Color targetColor = Color.white;
    [SerializeField] float intensity = 1f;
    [SerializeField] float scrollSpeed = 1f;

    [Header("Intensity Vars")]
    [SerializeField] float pulseSpeed = 2f;
    [SerializeField] float minIntensity = 0.5f;
    [SerializeField] float maxIntensity = 3f;
    bool pulse;

    [Header("ScrollSpeed Vars")]
        [SerializeField] float scrollSpeedMin = 0f;
        [SerializeField] float scrollSpeedMax = 5f;

    [Header("Color Vars")]
    private Color _currentColor;

    private Color _targetColor;

    private float _transitionTime = 1.5f;

    private void Start()
    {
        rendMat = rend.material;
        //intensity = rendMat.GetFloat("_Intensity");
        rendMat.SetFloat("_Intensity", intensity);
        //scrollSpeed = rendMat.GetFloat("_ScrollSpeed");
        rendMat.SetFloat("_ScrollSpeed", scrollSpeed);


        //scrollSpeedMin = rendMat.GetFloat("_ScrollSpeed");
    }

    private void Update()
    {
        //rendMat.SetColor("_BaseColor", targetColor);
        //rendMat.SetFloat("_Intensity", intensity);
        //rendMat.SetFloat("_ScrollSpeed", scrollSpeed);

        if (Input.GetMouseButtonDown(0))
        {
            //rendMat.SetColor("_BaseColor", Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f));
            StartCoroutine(ChangeColorInTime());
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            SetIntensity();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            scrollSpeed += 0.5f;
            rendMat.SetFloat("_ScrollSpeed", scrollSpeed);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            scrollSpeed -= 0.5f;
            rendMat.SetFloat("_ScrollSpeed", scrollSpeed);
        }
    }

    void SetIntensity()
    {
        pulse = !pulse;
        if(pulse)
        StartCoroutine(PulseIntensity());
    }
    IEnumerator PulseIntensity()
    {
        while (pulse)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

            float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            rendMat.SetFloat("_Intensity", intensity);

            yield return null;
        }
    }

    IEnumerator ChangeColorInTime()
    {
        Debug.Log("inizio a cambiare colore");
        _currentColor = rendMat.GetColor("_BaseColor");
        _targetColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);

        float elapsed = 0f;

        while (elapsed < _transitionTime)
        {

            elapsed += Time.deltaTime;

            Color c = Color.Lerp(_currentColor, _targetColor, elapsed / _transitionTime);

            rendMat.SetColor("_BaseColor", c);

            yield return null;
        }

        Debug.Log("ho finito di cambiare colore");
    }
}
