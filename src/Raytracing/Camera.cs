using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Lucid.Raytracing
{
    /// <summary>
    /// View of the scene. In format suitable for human.
    /// </summary>
    [Serializable]
    public class Camera
    {
        public Vector Origin;
        public Vector LookAt;

        /// <summary>
        /// Returns SceneView representation more suitable for ray tracer.
        /// </summary>
        /// <param name="fov">Field of view in degrees.</param>
        /// <param name="aspectRatio">Width / height of the rectangle in space to render.</param>
        /// <returns></returns>
        public SceneView ToSceneView(double fov, double aspectRatio)
        {
            SceneView view = new SceneView();
            view.Origin = this.Origin;

            Vector lookVector = LookAt - Origin;

            double rectWidth = lookVector.Len / (double)60 * fov;
            double rectHeight = rectWidth / aspectRatio;

            // use UP vector to determine left vector
            view.RenderRectTopSide = new Vector(0, 1, 0).Cross(lookVector);
            view.RenderRectTopSide.Normalize();
            view.RenderRectTopSide.Mul(rectWidth);

            // top vector must be perpendicular to lookVector and left vector
            view.RenderRectLeftSide = view.RenderRectTopSide.Cross(lookVector);
            view.RenderRectLeftSide.Normalize();
            view.RenderRectLeftSide.Mul(-rectHeight);

            // render rect centered at LookAt
            view.RenderRectOrigin = LookAt;
            view.RenderRectOrigin -= view.RenderRectLeftSide / 2;
            view.RenderRectOrigin -= view.RenderRectTopSide / 2;

            double test = lookVector.Dot(view.RenderRectLeftSide);
            double test2 = lookVector.Dot(view.RenderRectTopSide);
			Debug.Assert(Math.Abs(test) < Constants.Epsilon && Math.Abs(test2) < Constants.Epsilon, 
				"RenderRect not perpendicular to lookVector");

            return view;
        }
    }
}
