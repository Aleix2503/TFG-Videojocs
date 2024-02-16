using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private RenderTexture m_RenderTexture;
    //private Texture2D m_Texture;

    public GameObject mainCameraPrefab;

    private Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        //m_Texture = (Texture2D)GetComponent<Material>().GetTexture("Diffuse");

        GameObject mainCameraPrefabInstance = Instantiate(mainCameraPrefab, Camera.main.transform.position, Camera.main.transform.rotation);

        m_Camera = mainCameraPrefabInstance.GetComponent<Camera>();

        // Create a render texture
        //m_RenderTexture = new RenderTexture(512, 512, 0);
        //m_RenderTexture.Create();
        m_RenderTexture = (RenderTexture)GetComponent<Renderer>().material.mainTexture;

        // Set the render texture as the target for rendering
        //RenderTexture.active = m_RenderTexture;

        //// (Optional) Clear the render texture to a specific color
        //GL.Clear(true, true, Color.clear);

        m_Camera.targetTexture = m_RenderTexture; // Assign the Render Texture to the stamping camera
        m_Camera.Render(); // Render the stamping camera

        RenderTexture.active = m_RenderTexture; // Set the Render Texture as the active render texture
        Texture2D stampTexture = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, false);
        stampTexture.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0); // Read pixels from the Render Texture
        stampTexture.Apply(); // Apply changes

        GetComponent<Renderer>().material.mainTexture = stampTexture;

        //RenderTexture.active = m_RenderTexture;
        //m_Texture.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0);
        //m_Texture.Apply();

        // Release the render texture to free up memory
        //m_RenderTexture.Release();
    }
}
