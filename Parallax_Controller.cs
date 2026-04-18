using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Header("Scrolling Settings")]
    public float scrollSpeed = 5f;          
    public float parallaxMultiplier = 1f;   

    private Transform cam;
    private Vector3 camStartPos;
    private GameObject[] backgrounds;
    private Material[] mat;
    private float[] backSpeeds;
    private float farthestBack;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        backgrounds = new GameObject[backCount];
        mat = new Material[backCount];
        backSpeeds = new float[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            Renderer rend = backgrounds[i].GetComponent<Renderer>();
            if (rend != null)
                mat[i] = rend.material;
            else
                Debug.LogWarning("Background " + i + " has no Renderer!");
        }

        CalculateBackSpeeds();
    }

    void CalculateBackSpeeds()
    {
        farthestBack = float.MinValue;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float zDist = backgrounds[i].transform.position.z - cam.position.z;
            if (zDist > farthestBack)
                farthestBack = zDist;
        }

        if (farthestBack <= 0) farthestBack = 1f;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float zDist = backgrounds[i].transform.position.z - cam.position.z;
            backSpeeds[i] = 1f - (zDist / farthestBack);
        }
    }

    void LateUpdate()
    {
        float distance = Time.time * scrollSpeed;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (mat[i] == null) continue;

            float layerSpeed = backSpeeds[i] * parallaxMultiplier;

            Vector2 offset = new Vector2(distance * layerSpeed, 0);
            mat[i].SetTextureOffset("_MainTex", offset);
        }
    }
}