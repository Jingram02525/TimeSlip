using UnityEngine;

public class StarfieldLooper : MonoBehaviour
{
    public GameObject player;
    public Transform[] starfields; // Assign Starfields in Inspector

    private float backgroundHeight;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject not assigned to StarfieldLooper!");
            enabled = false;
            return;
        }

        if (starfields == null || starfields.Length < 2)
        {
            Debug.LogError("You must assign at least 2 starfield objects!");
            enabled = false;
            return;
        }

        backgroundHeight = starfields[0].GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        foreach (Transform starfield in starfields)
        {
            // If the starfield is below the player by more than one height, move it above the topmost one
            if (player.transform.position.y - starfield.position.y > backgroundHeight)
            {
                float highestY = GetHighestStarfieldY();
                starfield.position = new Vector3(
                    starfield.position.x,
                    highestY + backgroundHeight,
                    starfield.position.z
                );
            }
        }
    }

    float GetHighestStarfieldY()
    {
        float maxY = starfields[0].position.y;
        foreach (Transform s in starfields)
        {
            if (s.position.y > maxY)
                maxY = s.position.y;
        }
        return maxY;
    }
}
