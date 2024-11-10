using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenEffect : MonoBehaviour
{
    public float transparentAmount = 1f;
    public float rotationSpeed = 1f;
    public float radius = 50f;
    public float speed = 1f;
    public float circleInterval = 1f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float displayDuration = 2f;

    private static bool cloneExists = false;
    private bool isClone = false;
    private bool isMoving = false;
    private float angle = 0f;
    private float timer = 0f;

    private MaskableGraphic graphicComponent;

    void Start()
    {
        graphicComponent = GetComponent<Image>() as MaskableGraphic ?? GetComponent<TextMeshProUGUI>() as MaskableGraphic;

        if (graphicComponent == null)
        {
            Debug.LogError("No Image or TextMeshProUGUI component found!");
            return;
        }

        if (!cloneExists && !isClone)
        {
            StartCoroutine("WaitToCreateClone");
        }
    }

    IEnumerator WaitToCreateClone()
    {
        yield return new WaitForSeconds(circleInterval);
        GameObject clonedObject = CloneAndMakeTransparent();
        MaskableGraphic clonedGraphic = clonedObject.GetComponent<Image>() as MaskableGraphic ?? clonedObject.GetComponent<TextMeshProUGUI>() as MaskableGraphic;

        if (clonedGraphic != null)
        {
            StartCoroutine(SmoothFadeIn(clonedGraphic));
        }

        cloneExists = true;

        yield return new WaitForSeconds(displayDuration);
        if (clonedGraphic != null)
        {
            StartCoroutine(SmoothFadeOutAndRestart(clonedGraphic));
        }
    }

    void Update()
    {
        if (isClone)
        {
            if (!isMoving)
            {
                timer += Time.deltaTime;

                if (timer >= 0f)
                {
                    timer = 0f;
                    StartCircularMotion();
                }
            }
            else
            {
                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
                Vector3 newPosition = new Vector3(x, y, 0f);

                transform.localPosition = newPosition;
                transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);

                angle += speed * rotationSpeed * Time.deltaTime;

                if (Mathf.Repeat(angle, 360f) == 0f && angle != 0f)
                {
                    isMoving = false;
                }
            }
        }
    }

    void StartCircularMotion()
    {
        isMoving = true;
        angle = 0f;
    }

    GameObject CloneAndMakeTransparent()
    {
        Transform parentTransform = transform;

        GameObject clonedObject = Instantiate(gameObject, parentTransform);

        LoadingScreenEffect cloneScript = clonedObject.GetComponent<LoadingScreenEffect>();
        cloneScript.isClone = true;

        MaskableGraphic clonedGraphic = clonedObject.GetComponent<Image>() as MaskableGraphic ?? clonedObject.GetComponent<TextMeshProUGUI>() as MaskableGraphic;

        if (clonedGraphic != null)
        {
            Color color = clonedGraphic.color;
            color.a = 0f;
            clonedGraphic.color = color;
        }

        return clonedObject;
    }

    IEnumerator SmoothFadeIn(MaskableGraphic graphic)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / fadeInDuration);
            Color color = graphic.color;
            color.a = Mathf.Lerp(0f, transparentAmount, t);
            graphic.color = color;
            yield return null;
        }

        Color finalColor = graphic.color;
        finalColor.a = transparentAmount;
        graphic.color = finalColor;
    }

    IEnumerator SmoothFadeOutAndRestart(MaskableGraphic graphic)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / fadeOutDuration);
            Color color = graphic.color;
            color.a = Mathf.Lerp(transparentAmount, 0f, t);
            graphic.color = color;
            yield return null;
        }

        Color finalColor = graphic.color;
        finalColor.a = 0f;
        graphic.color = finalColor;

        cloneExists = false;
        isClone = false;
        Destroy(graphic.gameObject);
        StartCoroutine(WaitToCreateClone());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameObject.scene.name)
        {
            cloneExists = false;
            isClone = false;
        }
    }
}