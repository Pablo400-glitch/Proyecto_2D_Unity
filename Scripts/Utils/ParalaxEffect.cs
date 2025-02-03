using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{
    public Vector2 baseSpeed = new Vector2(0.5f, 0.0f);
    private Material[] Layers;
    public float speedOffset = 0.1f;

    void Start()
    {
        Layers = GetComponent<Renderer>().materials;
    }

    void Update()
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            Material m = Layers[i];
            Vector2 newOffset = m.GetTextureOffset("_MainTex") + baseSpeed * (speedOffset / (i + 1.0f)) * Time.deltaTime;
            m.SetTextureOffset("_MainTex", newOffset);
        }
    }
}