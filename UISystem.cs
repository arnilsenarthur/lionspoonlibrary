using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LionSpoon
{
    public class UISystem
    {
        private static CanvasGroup[] openScreens = new CanvasGroup[8];
        private static Dictionary<string,CanvasGroup> screens = new Dictionary<string, CanvasGroup>();
        private static MonoBehaviour Behaviour;



        /// <summary>
        /// Add an ui to manager
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ui"></param>
        public static void AddUI(string id,CanvasGroup ui)
        {
            screens[id] = ui;
        }

        /// <summary>
        /// Clear ALL UI Data
        /// </summary>
        public static void ClearScreens()
        {
            screens.Clear();
            openScreens = new CanvasGroup[8];
        }

        /// <summary>
        /// Open an interface
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="function"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void ChangeUI(int slot,string screen,Func<CanvasGroup,CanvasGroup,IEnumerator> function)
        {
            Behaviour.StartCoroutine(function(openScreens[slot],screens[screen]));
            openScreens[slot] = screens[screen];
        }


        /// <summary>
        /// Default open UI
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="screen"></param>
        public static void ChangeUI(int slot,string screen)
        {
            ChangeUI(slot,screen,FadeFuncDefault);
        }

        /// <summary>
        /// Close an interface
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="function"></param>
        public static void CloseUI(int slot,Func<CanvasGroup,CanvasGroup,IEnumerator> function)
        {
            Behaviour.StartCoroutine(function(openScreens[slot],null));
            openScreens[slot] = null;
        }

        /// <summary>
        /// Close all UIs
        /// </summary>
        /// <param name="function"></param>
        public static void CloseAllUI(Func<CanvasGroup,CanvasGroup,IEnumerator> function)
        {
            foreach(CanvasGroup group in openScreens)
                if(group != null)
                {
                    Behaviour.StartCoroutine(function(group,null));
                }
                
            openScreens = new CanvasGroup[8];
        }

        /// <summary>
        /// Needed to run coroutines
        /// </summary>
        /// <param name="behaviour"></param>
        public static void SetMonobehaviour(MonoBehaviour behaviour)
        {
            Behaviour = behaviour;
        }

        

        /// <summary>
        /// Default FadeInOut function 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static IEnumerator FadeFuncDefault(CanvasGroup from,CanvasGroup to)
        {
            if(to != null)
                to.gameObject.SetActive(true);

            float f = 0f;
            while(f <= 1f)
            {
                if(from != null)
                    from.alpha = 1f-f;
                if(to != null)
                    to.alpha = f;

                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if(from != null)
                from.alpha = 0f;
            if(to != null)
                to.alpha = 1f;
            if(from != null)
                from.gameObject.SetActive(false);
        }

        /// <summary>
        /// Default FadeInOut no fade function
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static IEnumerator FadeFuncNoFade(CanvasGroup from,CanvasGroup to)
        {
            if(to != null)
                to.gameObject.SetActive(true);

            if(from != null)
                from.alpha = 0f;
            if(to != null)
                to.alpha = 1f;

            if(from != null)
                from.gameObject.SetActive(false);
            yield return 0;
        }
    }
}