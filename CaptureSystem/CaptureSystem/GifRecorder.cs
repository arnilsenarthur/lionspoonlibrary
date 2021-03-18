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
    /// Main gif recorder class
    /// </summary>
    public class GifRecorder
    {
        /// <summary>
        /// Starts to record a gif
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="frames"></param>
        /// <param name="fps"></param>
        /// <returns></returns>
        public static GifRecorderComponent StartRecording(Camera camera,int width,int height,int frames,int fps)
        {
            GifRecorderComponent rec = camera.gameObject.AddComponent<GifRecorderComponent>();
            rec.width = width;
            rec.height = height;
            rec.frames = frames;
            rec.fps = fps;
            rec.__timePerFrame = 1f/fps;

            return rec;
        }

        /// <summary>
        /// Check if some recorder is recoding camera
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsRecording(Camera camera)
        {
            return camera.GetComponent<GifRecorderComponent>() != null;
        }

        /// <summary>
        /// Stop camera recorder
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="callback"></param>
        public static void StopRecording(Camera camera,System.Action<Gif> callback)
        {
            GifRecorderComponent rec = camera.GetComponent<GifRecorderComponent>();
            if(rec != null)   
                rec.StopRecording(callback);

            return;
        }
    }
}