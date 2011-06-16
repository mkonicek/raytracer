using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Lucid.Raytracing
{
    /// <summary>
    /// View to the scene. In format suitable to raytracing,
    /// apart from Camera class.
    /// </summary>
    [DataContract]
    public class SceneView
    {
        /// <summary>
        /// Position of the camera (ie. viewing point for rendering).
        /// </summary>
        [DataMember]
        public Vector Origin;

        /// <summary>
        /// Top left corner of rect in space to be rendered.
        /// </summary>
        [DataMember]
        public Vector RenderRectOrigin;

        /// <summary>
        /// Top side of the render rect (in x direction). Should be perpendicular to leftSide.
        /// </summary>
        [DataMember]
        public Vector RenderRectTopSide;

        /// <summary>
        /// Left side of the render rect (in y direction). Should be perpendicular to topSide.
        /// </summary>
        [DataMember]
        public Vector RenderRectLeftSide;
    }
}
