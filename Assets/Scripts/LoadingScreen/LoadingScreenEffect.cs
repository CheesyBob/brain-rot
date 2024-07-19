using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenEffect : MonoBehaviour
{
    public float transparentAmount;
    public float rotationSpeed;
    public float radius;
    public float speed;
    public float circleInterval;
    public float fadeInDuration;

    private static bool cloneExists = false;
    private bool isClone = false;
    private bool isMoving = false;
    private float angle = 0f;
    private float timer = 0f;

    void Start()
    {
        if (!cloneExists && !isClone)
        {
            StartCoroutine("WaitToCreateClone");
        }
    }

    IEnumerator WaitToCreateClone()
    {
        yield return new WaitForSeconds(circleInterval);
        GameObject clonedObject = CloneAndMakeTransparent();
        StartCoroutine(FadeIn(clonedObject.GetComponent<Image>()));

        cloneExists = true;
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

        Image clonedImage = clonedObject.GetComponent<Image>();

        if (clonedImage != null)
        {
            Color color = clonedImage.color;
            color.a = 0f;
            clonedImage.color = color;
        }

        return clonedObject;
    }

    IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            Color color = image.color;
            color.a = Mathf.Lerp(0f, transparentAmount, elapsedTime / fadeInDuration);
            image.color = color;
            yield return null;
        }

        Color finalColor = image.color;
        finalColor.a = transparentAmount;
        image.color = finalColor;
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
