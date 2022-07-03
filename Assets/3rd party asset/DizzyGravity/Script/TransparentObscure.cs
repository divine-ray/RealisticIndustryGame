using UnityEngine;

/// <summary>
/// This script is dynamically called as soon as an object passes between player and camera.
/// It changes the object's material to be transparent and resets once the view is clear.
/// </summary>
public class TransparentObscure : MonoBehaviour
{
    new private Renderer renderer;
    private Color savedColor;
    private Shader savedShader;
    private float currentOpacity = 1f, hideTime;
    private const float lowestOpacity = 0.5f, transitionSpeed = 5f; // Modify these values if you want a different look

    // Called every frame when an object is obscuring the view
    public void Hide()
    {
        if (!renderer)
            renderer = GetComponent<Renderer>();
        if (!savedShader)
            ChangeToTransparent();
        hideTime = Time.time;
    }

    private void Update()
    {
        if (Time.time < hideTime + 2 * Time.deltaTime) // Still invisible (last Hide was called recently)
        {
            if (currentOpacity > lowestOpacity) // Fading out
                currentOpacity -= ((1 - lowestOpacity) * Time.deltaTime * transitionSpeed);
        }
        else
        {
            currentOpacity += ((1 - lowestOpacity) * Time.deltaTime * transitionSpeed); // Fading back in

            if (currentOpacity >= 1) // Fully opaque; terminating script
                ChangeToOpaque();
        }
        UpdateAlpha();
    }

    // Replaces the material's alpha value with the calculated one
    private void UpdateAlpha()
    {
        Color color = renderer.material.color;
        color.a = currentOpacity;
        renderer.material.color = color;
    }

    private void ChangeToTransparent()
    {
        savedColor = renderer.material.color;
        savedShader = renderer.material.shader;
        renderer.material.shader = Shader.Find("Transparent/Diffuse");
    }

    private void ChangeToOpaque()
    {
        renderer.material.color = savedColor;
        renderer.material.shader = savedShader;
        Destroy(this);
    }

    // Makes sure to revert the material to the original state before unloading
    private void OnDisable()
    {
        ChangeToOpaque();
    }
}
