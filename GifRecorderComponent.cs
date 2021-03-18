using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LionSpoon;

public class GifRecorderComponent : MonoBehaviour
{
    public int width;
    public int height;
    public int frames;
    public int fps;
    public float __timePerFrame;

    private float __dTime;

    private bool recording = true;

    private Queue<RenderTexture> framesBuffer = new Queue<RenderTexture>();

    void OnStart()
    {

    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest) 
    {
        if(!recording)
        {
            Graphics.Blit(src, dest);
            return;
        }

        __dTime += Time.unscaledDeltaTime;
    
        if (__dTime >= __timePerFrame)
        {
            __dTime -= __timePerFrame;

            RenderTexture rt = null;
            if (framesBuffer.Count >= frames)
					rt = framesBuffer.Dequeue();
            
            if(rt == null)
            {
                rt = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
                rt.wrapMode = TextureWrapMode.Clamp;
                rt.filterMode = FilterMode.Bilinear;
                rt.anisoLevel = 0;
            }

            Graphics.Blit(src, rt);
			framesBuffer.Enqueue(rt);
        }

        //Show onto screen
        Graphics.Blit(src, dest);
    }

    public void StopRecording(System.Action<Gif> callback)
    {
        recording = false;

        StartCoroutine(__StopRecording(callback));
    }


    private IEnumerator __StopRecording(System.Action<Gif> callback)
    {
        List<GifFrame> sprites = new List<GifFrame>();

        

        while (framesBuffer.Count > 0)
        {
            //Temporary textures
            Texture2D temp = new Texture2D(width, height, TextureFormat.RGB24, false);
            temp.hideFlags = HideFlags.HideAndDontSave;
            temp.wrapMode = TextureWrapMode.Clamp;
            temp.filterMode = FilterMode.Bilinear;
            temp.anisoLevel = 0;
        
            RenderTexture tx = framesBuffer.Dequeue();

            RenderTexture.active = tx;
			temp.ReadPixels(new Rect(0, 0, tx.width, tx.height), 0, 0);
			temp.Apply();
			RenderTexture.active = null;
            
            Flush(tx);

            GifFrame gf = new GifFrame();
            gf.width = tx.width;
            gf.height = tx.height;
            gf.data = temp.GetPixels32();
            gf.sprite = Sprite.Create(temp,new Rect(0,0,tx.width,tx.height),new Vector2(tx.width/2,tx.height/2),100);
            gf.texture = temp;

            

            sprites.Add(gf);
            
            yield return null;
        }

        callback(new Gif(fps,sprites));
        Destroy(this);
    }

    private void Flush(UnityEngine.Object obj)
    {
        #if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEngine.Object.Destroy(obj);
        else
            UnityEngine.Object.DestroyImmediate(obj);
        #else
            UnityEngine.Object.Destroy(obj);
        #endif
    }

    
}
