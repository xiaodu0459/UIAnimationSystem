// ========================================================================
// UGUI Animation
// Created: 2016/10/14

// Brief: Implement this interface in TimelineItem that can be optimized in someway before played!
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    interface IOptimizable
    {
        // User can choose to disable the optimization in some situations
        bool CanOptimize { get; set; }

        // Perform optimization
        void Optimize();
    }

}//namespace UIDirector