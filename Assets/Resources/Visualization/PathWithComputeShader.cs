using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;

public class PathWithComputeShader : MonoBehaviour
{
    public float decayRate = 1;

    public ComputeShader computeShader;
    RenderTexture renderTexture;
    RenderTexture diffuseRenderTexture;

    UpperHandle upperHandle;
    LowerHandle lowerHandle;

    Vector3 u_lastPos;
    Vector3 l_lastPos;
    int doItCount = 0;
    const int growIntervalFrames = 20;
    const int HEIGHT = 1024;
    const int WIDTH = HEIGHT * 2;
    Bounds bounds;

    void Start()
    {
        bounds = GetComponent<MeshFilter>().mesh.bounds;
        upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    void Update()
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(WIDTH, HEIGHT, 24, RenderTextureFormat.ARGBFloat);
            renderTexture.enableRandomWrite = true;
            renderTexture.autoGenerateMips = false;
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.filterMode = FilterMode.Point;
            renderTexture.Create();

            GetComponent<MeshRenderer>().material.mainTexture = renderTexture;
        }

        if (diffuseRenderTexture == null)
        {
            diffuseRenderTexture = new RenderTexture(WIDTH, HEIGHT, 24, RenderTextureFormat.ARGBFloat);
            diffuseRenderTexture.enableRandomWrite = true;
            diffuseRenderTexture.autoGenerateMips = false;
            diffuseRenderTexture.wrapMode = TextureWrapMode.Clamp;
            diffuseRenderTexture.filterMode = FilterMode.Point;
            diffuseRenderTexture.Create();
        }

        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetTexture(1, "Result", renderTexture);
        computeShader.SetTexture(2, "Result", renderTexture);
        computeShader.SetTexture(2, "DiffuseResult", diffuseRenderTexture);
        Vector3 upperPosition = upperHandle.HandlePosition(new Vector3(0, 0, 0)) - transform.TransformPoint(bounds.min);
        Vector3 lowerPosition = lowerHandle.HandlePosition(new Vector3(0, 0, 0)) - transform.TransformPoint(bounds.min);

        float[] positionXs = new float[10];
        float[] positionYs = new float[10];
        AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();
        int i = 0;
        foreach (AudioSource s in sounds)
        {
            if (!s.isPlaying) return;
            if (i >= 10) return; //limited number of sources can be displayed
            Vector3 pos = s.transform.position - transform.TransformPoint(bounds.min);
            positionXs[i] = 1.0f - pos.x / bounds.size.x / transform.lossyScale.x;
            positionYs[i] = 1.0f - pos.z / bounds.size.z / transform.lossyScale.z;
            i++;
        }

        if (u_lastPos != null && l_lastPos != null)
        {
            computeShader.SetInts("numberOfSoundSources", sounds.Length);
            computeShader.SetFloats("soundPositionXs", positionXs);
            computeShader.SetFloats("soundPositionYs", positionYs);
            computeShader.SetBool("moving", true);
            computeShader.SetBool("doIt", doItCount == 0);
            if (++doItCount == growIntervalFrames)
                doItCount = 0;
            computeShader.SetInt("width", WIDTH);
            computeShader.SetInt("height", HEIGHT);
            computeShader.SetFloat("decayRate", decayRate);
            computeShader.SetFloat("u_last_x", 1.0f - u_lastPos.x / bounds.size.x / transform.lossyScale.x);
            computeShader.SetFloat("u_last_y", 1.0f - u_lastPos.z / bounds.size.z / transform.lossyScale.z);
            computeShader.SetFloat("u_x", 1.0f - upperPosition.x / bounds.size.x / transform.lossyScale.x);
            computeShader.SetFloat("u_y", 1.0f - upperPosition.z / bounds.size.z / transform.lossyScale.z);
            computeShader.SetFloat("l_last_x", 1.0f - l_lastPos.x / bounds.size.x / transform.lossyScale.x);
            computeShader.SetFloat("l_last_y", 1.0f - l_lastPos.z / bounds.size.z / transform.lossyScale.z);
            computeShader.SetFloat("l_x", 1.0f - lowerPosition.x / bounds.size.x / transform.lossyScale.x);
            computeShader.SetFloat("l_y", 1.0f - lowerPosition.z / bounds.size.z / transform.lossyScale.z);
            computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
            computeShader.Dispatch(1, renderTexture.width / 8, renderTexture.height / 8, 1);
            computeShader.Dispatch(2, renderTexture.width / 8, renderTexture.height / 8, 1);
            l_lastPos = lowerPosition;
            u_lastPos = upperPosition;

            Graphics.Blit(diffuseRenderTexture, renderTexture);
        }
    }
}

#if false
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using ComputeShaderUtility;

public class Simulation : MonoBehaviour
{

    const int updateKernel = 0;
    const int diffuseMapKernel = 1;

    public ComputeShader compute;


    [Header("Display Settings")]
    public bool showAgentsOnly;
    public FilterMode filterMode = FilterMode.Point;
    public GraphicsFormat format = ComputeHelper.defaultGraphicsFormat;


    [SerializeField, HideInInspector] protected RenderTexture trailMap;
    [SerializeField, HideInInspector] protected RenderTexture diffusedTrailMap;
    [SerializeField, HideInInspector] protected RenderTexture displayTexture;

    Texture2D colourMapTexture;

    protected virtual void Start()
    {
        Init();
        transform.GetComponentInChildren<MeshRenderer>().material.mainTexture = displayTexture;
    }


    void Init()
    {

        // Create render textures
        ComputeHelper.CreateRenderTexture(ref trailMap, settings.width, settings.height, filterMode, format);
        ComputeHelper.CreateRenderTexture(ref diffusedTrailMap, settings.width, settings.height, filterMode, format);
        ComputeHelper.CreateRenderTexture(ref displayTexture, settings.width, settings.height, filterMode, format);

        compute.SetInt("width", settings.width);
        compute.SetInt("height", settings.height);
    }

    void Update()
    {

        // Assign textures
        compute.SetTexture(updateKernel, "TrailMap", trailMap);
        compute.SetTexture(diffuseMapKernel, "TrailMap", trailMap);
        compute.SetTexture(diffuseMapKernel, "DiffusedTrailMap", diffusedTrailMap);

        // Assign settings
        compute.SetFloat("deltaTime", Time.deltaTime);
        compute.SetFloat("time", Time.time);

        compute.SetFloat("trailWeight", settings.trailWeight);
        compute.SetFloat("decayRate", settings.decayRate);
        compute.SetFloat("diffuseRate", settings.diffuseRate);


        ComputeHelper.Dispatch(compute, settings.numAgents, 1, 1, kernelIndex: updateKernel);
        ComputeHelper.Dispatch(compute, settings.width, settings.height, 1, kernelIndex: diffuseMapKernel);

        ComputeHelper.CopyRenderTexture(diffusedTrailMap, trailMap);

        ComputeHelper.CopyRenderTexture(trailMap, displayTexture);

    }
}
#endif
