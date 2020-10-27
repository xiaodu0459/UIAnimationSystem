// ========================================================================
// UGUI Animation
// Created: 2016/10/17

// Brief: Abstract class: When firetime reached, trigger a global event
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    public abstract class CTimelineGlobalEvent : CTimelineItem
    {
        /// <summary>
        /// when time running pass the firetime, event will be triggered;
        /// </summary>
        public abstract void Trigger();

        /// <summary>
        /// this event will be triggered when animation played in reverse.
        /// </summary>
        public virtual void Reverse() { }
    }

}//namespace UIDirector