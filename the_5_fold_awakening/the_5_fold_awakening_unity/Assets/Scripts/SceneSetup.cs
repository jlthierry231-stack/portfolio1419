using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [Header("Player Setup")]
    public GameObject playerPrefab;
    public Vector3 playerStartPosition = Vector3.zero;

    [Header("Environment Setup")]
    public GameObject[] environmentPrefabs;
    public Vector3[] environmentPositions;

    [Header("Character Setup")]
    public GameObject[] characterPrefabs;
    public Vector3[] characterPositions;

    [Header("Lighting Setup")]
    public Light directionalLight;
    public Color ambientLightColor = new Color(0.5f, 0.5f, 0.5f);

    void Awake()
    {
        SetupScene();
    }

    void SetupScene()
    {
        // Setup lighting
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }
        RenderSettings.ambientLight = ambientLightColor;

        // Spawn player
        if (playerPrefab != null)
        {
            GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
            player.name = "Player";

            // Add necessary components if not already present
            if (player.GetComponent<CharacterController>() == null)
            {
                player.AddComponent<CharacterController>();
            }
            if (player.GetComponent<PlayerController>() == null)
            {
                player.AddComponent<PlayerController>();
            }
            if (player.GetComponent<CharacterManager>() == null)
            {
                player.AddComponent<CharacterManager>();
            }
        }

        // Spawn environment objects
        for (int i = 0; i < Mathf.Min(environmentPrefabs.Length, environmentPositions.Length); i++)
        {
            if (environmentPrefabs[i] != null)
            {
                GameObject envObj = Instantiate(environmentPrefabs[i], environmentPositions[i], Quaternion.identity);
                envObj.name = $"Environment_{i}";
            }
        }

        // Spawn characters
        for (int i = 0; i < Mathf.Min(characterPrefabs.Length, characterPositions.Length); i++)
        {
            if (characterPrefabs[i] != null)
            {
                GameObject charObj = Instantiate(characterPrefabs[i], characterPositions[i], Quaternion.identity);
                charObj.name = $"Character_{i}";
            }
        }

        // Setup camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0, 5, -10);
            mainCamera.transform.LookAt(Vector3.zero);
        }
    }
}