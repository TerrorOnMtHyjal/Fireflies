using UnityEngine;
using Random = UnityEngine.Random;

public class Firefly : MonoBehaviour
{
    [Range(1, 7)]
    [SerializeField] private float litIntensity;
    [SerializeField] private GameObject assLight;
    
    private Material lightMaterial;
    private Color assColor = new Color(0.9f, 1.7f, 0f, 1f);
    private bool isLit;
    private static readonly int emissionColor = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        lightMaterial = assLight.GetComponent<MeshRenderer>().material;
        
        assColor = RandomAssColor();
        SetAssLightIntensity();
    }

    private void FixedUpdate()
    {
        if (Random.Range(0, 5000) > 100) return;
        
        ToggleAssLight();
        SetAssLightIntensity();
    }

    private void ToggleAssLight() => isLit = !isLit;
    private Color RandomAssColor() =>  Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);

    private void SetAssLightIntensity()
    {
        var intensity = isLit ? litIntensity : 0;
        
        lightMaterial.SetVector(emissionColor, assColor * intensity);
    }
}
