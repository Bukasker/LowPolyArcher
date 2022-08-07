using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Light Preset", menuName = "Scriptables/Light Preset")]
public class LightingPreset : MonoBehaviour
{
    public Gradient AmbientColor;
    public Gradient Brightness;
    public Gradient DirectiobalColor;
    public Gradient FogColor;
}
