/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    
                                                             
    Lion Spoon Dream Game Technology© - 2021

    Gif library
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LionSpoon
{
    /// <summary>
    /// Used to record gif from camera
    /// </summary>
    public class GifRecorder
    {
        private int framesDuration;
        private int framesPerSecond;

        private int viewW;
        private int viewH;
        private Rect bounds;
        private bool recording = false;
        private List<Texture2D> textures = new List<Texture2D>();

        private Camera camera;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="framesPerSecond"></param>
        /// <param name="viewW"></param>
        /// <param name="viewH"></param>
        /// <param name="imageBounds"></param>
        public GifRecorder(int duration,int framesPerSecond,int viewW,int viewH,Rect imageBounds)
        {
            this.framesDuration = duration;
            this.framesPerSecond = framesPerSecond;
            this.viewH = viewH;
            this.viewW = viewW;
            this.bounds = imageBounds;

            SetCamera(Camera.main);
        }

        /// <summary>
        /// Change view camera
        /// </summary>
        /// <param name="camera"></param>
        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        /// <summary>
        /// Record camera info
        /// </summary>
        /// <param name="behaviour"></param>
        public void Record(MonoBehaviour behaviour)
        {
            recording = true;
            behaviour.StartCoroutine(__Record());
        }

        /// <summary>
        /// __internal__
        /// </summary>
        /// <returns></returns>
        private IEnumerator __Record()
        {
            while(recording)
            {
                textures.Add(__CreateFrame(viewW,viewH,bounds));
                if(textures.Count > framesDuration)
                    textures.RemoveAt(0);

                yield return new WaitForSeconds(1f/framesPerSecond);
            }
        }

        /// <summary>
        /// Create a gif object from recording
        /// </summary>
        /// <returns></returns>
        public Gif CreateGif()
        {
            recording = false;

            List<Sprite> sprites = new List<Sprite>();
            foreach(Texture2D tx in textures)
            {
                sprites.Add(Sprite.Create(tx,new Rect(0,0,tx.width,tx.height),new Vector2(tx.width/2,tx.height/2),100));
            }
            
            Gif gif = new Gif(framesPerSecond,textures,sprites);;
            textures = new List<Texture2D>();
            return gif;
        }

        /// <summary>
        /// __internal__
        /// </summary>
        /// <param name="viewportW"></param>
        /// <param name="viewportH"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private Texture2D __CreateFrame(int viewportW,int viewportH,Rect rect)
        {
            RenderTexture renderTexture = new RenderTexture(viewportW, viewportH, 24);
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);

            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            camera.targetTexture = null;
            RenderTexture.active = null;

            MonoBehaviour.Destroy(renderTexture);
            renderTexture = null;
            screenShot.Apply();
            
            return screenShot;
        }
    }
}