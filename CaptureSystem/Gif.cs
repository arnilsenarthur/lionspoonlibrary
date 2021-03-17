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
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


namespace LionSpoon
{   
    /// <summary>
    /// Main gif class, holds gif data
    /// </summary>
    public class Gif
    {  
        private Sprite[] sprites; 
        private List<Texture2D> frames;
        private int fps = 0;

        private  UnityEngine.UI.Image image;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fps"></param>
        /// <param name="frames"></param>
        /// <param name="sprites"></param>
        public Gif(int fps,List<Texture2D> frames,List<Sprite> sprites)
        {
            this.sprites = sprites.ToArray();
            this.fps = fps;
            this.frames = frames;
        }

        /// <summary>
        /// Starts to show a gif in a UnityEngine.UI.Image
        /// </summary>
        /// <param name="image"></param>
        public void BindTo(Image image)
        {
            this.image = image;
            image.StartCoroutine(__BindTo());
        }

        /// <summary>
        /// __internal__
        /// </summary>
        /// <returns></returns>
        private IEnumerator __BindTo()
        {
            int i = 0;
            while(image != null)
            {
                image.sprite = sprites[i%sprites.Length];
                yield return new WaitForSeconds(1f/fps);
                i ++;
            }
        }

        /// <summary>
        /// Starts to save gif to file
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="filepath"></param>
        /// <param name="updateProgress"></param>
        public void Save(MonoBehaviour behaviour,string filepath,Action<float,bool> updateProgress)
        {
            behaviour.StartCoroutine(__Save(filepath,updateProgress));
        }

        /// <summary>
        /// __internal__
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="updateProgress"></param>
        /// <returns></returns>
        public IEnumerator __Save(string filepath,Action<float,bool> updateProgress)
        {
            List<GifFrame> gframes = new List<GifFrame>();

            foreach(Texture2D tx in frames)
            {
                GifFrame gf = new GifFrame();
                gf.width = tx.width;
                gf.height = tx.height;
                gf.data = tx.GetPixels32();

                gframes.Add(gf);
                yield return null;
            }

            // Setup a worker thread and let it do its magic
            GifEncoder encoder = new GifEncoder(0, 50);
            encoder.SetDelay(Mathf.RoundToInt((1f/fps) * 1000f));

            Thread t = new Thread(() => {
                encoder.Start(filepath);

                for (int i = 0; i < gframes.Count; i++)
                {
                    GifFrame frame = gframes[i];
                    encoder.AddFrame(frame);

                    if(updateProgress != null)             
                        updateProgress((float)i / (float)gframes.Count,false);
                    
                }

                encoder.Finish();
                updateProgress(1,true);
            });
            t.Priority = System.Threading.ThreadPriority.BelowNormal;
            t.Start();
        }

        /// <summary>
        /// Stops to show gif in Image
        /// </summary>
        public void Unbind()
        {
            this.image = null;
        }
    }
}