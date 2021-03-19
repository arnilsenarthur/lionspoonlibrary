/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    

    Lion Spoon Dream Game Technology© - 2021

    General Event Library
*/
using System.Collections.Generic;

namespace LionSpoon
{
    /// <summary>
    /// Main event system class
    /// </summary>
    public class EventSystem
    {
        private static Dictionary<string,EventHandler> events = new Dictionary<string, EventHandler>();
        
        /// <summary>
        /// Add a handler of a certain event type
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="handler"></param>
        public static void AddHandler(string ev,EventHandler handler)
        {
            if(!events.ContainsKey(ev))
                events[ev] = handler;
            else
                events[ev] += handler;
        }

        /// <summary>
        /// Call all handlers of a certain event
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="parameter"></param>
        public static void CallEvent(string ev,object parameter)
        {
            if(!events.ContainsKey(ev))
                return;

            events[ev].Invoke(parameter);
        }

        /// <summary>
        /// Clear all handlers of a event
        /// </summary>
        /// <param name="ev"></param>
        public static void ClearHandlers(string ev)
        {
            events.Remove(ev);
        }
        
    }

    public delegate void EventHandler(object argument);
}