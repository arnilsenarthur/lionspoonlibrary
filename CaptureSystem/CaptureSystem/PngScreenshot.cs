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
    /// Main screenshot, used to take screenshots from game
    /// </summary>
    public class PngScreenshot
    {   
        /// <summary>
        /// Take game screenshot changing camera
        /// </summary>
        /// <param name="viewportW"></param>
        /// <param name="viewportH"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Texture2D TakeScreenShot(Camera camera,int viewportW,int viewportH,Rect rect)
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

        /// <summary>
        /// Take game screenshot
        /// </summary>
        /// <param name="viewportW"></param>
        /// <param name="viewportH"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Texture2D TakeScreenShot(int viewportW,int viewportH,Rect rect)
        {
            return TakeScreenShot(Camera.main,viewportW,viewportH,rect);
        }

        /// <summary>
        /// Save texture to file
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="filepath"></param>
        public static void SaveToFile(Texture2D texture,string filepath)
        {
            
            byte[] _bytes = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(filepath, _bytes);          
        }
    }
}