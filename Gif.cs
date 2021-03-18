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
        private List<GifFrame> frames; 
        private int fps = 0;

        private  UnityEngine.UI.Image image;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fps"></param>
        /// <param name="frames"></param>
        /// <param name="sprites"></param>
        public Gif(int fps,List<GifFrame> frames)
        {
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
                image.sprite = frames[i%frames.Count].sprite;
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
            // Setup a worker thread and let it do its magic
            GifEncoder encoder = new GifEncoder(0, 50);
            encoder.SetDelay(Mathf.RoundToInt((1f/fps) * 1000f));

            Thread t = new Thread(() => {
                encoder.Start(filepath);

                for (int i = 0; i < frames.Count; i++)
                {
                    GifFrame frame = frames[i];
                    encoder.AddFrame(frame);

                    if(updateProgress != null)             
                        updateProgress((float)i / (float)frames.Count,false);
                        
                    
                }

                encoder.Finish();
                updateProgress(1,true);
            });
            t.Priority = System.Threading.ThreadPriority.BelowNormal;
            t.Start();

            yield return null;
        }

        /// <summary>
        /// Stops to show gif in Image
        /// </summary>
        public void Unbind()
        {
            this.image = null;
        }

        public void Dispose()
        {
            Unbind();

            foreach(GifFrame frame in frames)
            {
                Flush(frame.sprite);
                Flush(frame.texture);
            }

            frames.Clear();
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
}