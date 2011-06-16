using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ParallelExec;

namespace Lucid.Raytracing
{
	/// <summary>
	/// The main raytracing logic.
	/// </summary>
    public class Scene
    {
        private IPrimitiveStructure objects;
        private List<Light> lights = new List<Light>();
        private List<Material> materials = new List<Material>();

        private static readonly int maxRecursionDepth = 3;

        private SceneSettings settings = null;
        public SceneSettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                if (value == null)
                    throw new Exception("Scene settings cannot be set to null");
                settings = value;
            }
        }

        //private bool compiled = false;

        /// <summary>
        /// Creates new scene.
        /// </summary>
        /// <param name="primitiveStructure">Structure used for storing 
        /// primitives and querying them for intersection.
        /// For exaple "new Octree()".</param>
        public Scene(IPrimitiveStructure primitiveStructure)
        {
            this.Clear();
            this.objects = primitiveStructure;
        }

        /// <summary>
        /// Creates scene with empty octree.
        /// </summary>
        public Scene() : this(new Octree())
        {
        }

        /// <summary>
        /// Internal so that user cannot call it, but for exaple loader can call it.
        /// </summary>
        public void addObject(Primitive primitive)
        {
            this.objects.Add(primitive);
        }

        public void addLight(Light light)
        {
            this.lights.Add(light);
        }

        public void addMaterial(Material material)
        {
            this.materials.Add(material);
        }

        /// <summary>
        /// Loads scene from file using given loader, that can read the file.
        /// </summary>
        /// <param name="fileName">File with the scene.</param>
        /// <param name="loader">Proper loader that can read the file.</param>
        public void LoadFrom(string fileName, ISceneLoader loader)
        {
            this.Clear();
            loader.AddToSceneFrom(this, fileName);
        }

        private void Clear()
        {
            if (objects != null)
                objects.Clear();
            lights = new List<Light>();
            materials = new List<Material>();
        }

        /// <summary>
        /// Compiles underlying object structure.
        /// </summary>
        public void Compile()
        {
            objects.Compile();
        }

        public delegate void RenderLinesDelegate();

        public void Render(System.Drawing.Rectangle rect, Color[] buffer)
        {
            if (this.settings == null)
            {
                throw new InvalidOperationException("Scene settings must be set before rendering.");
            }
            if (buffer.Length != rect.Width * rect.Height)
                throw new InvalidOperationException("Buffer must be same size as the rectangle to render");

            Vector eye = settings.View.Origin;

            Ray ray;
            ray.Origin = eye;

            Parallel.For(0, rect.Height, delegate(int y)
            {
                int x = 0;
                for (x = 0; x < rect.Width; x++)
                {
                    Vector rayEnd =
                        // origin + linear combination of topSide (for x) and leftSide (for y)
                        this.settings.View.RenderRectOrigin +
                        ((rect.Left + x) / (double)this.settings.ImageWidth) * this.settings.View.RenderRectTopSide +
                        ((rect.Top + y) / (double)this.settings.ImageHeight) * this.settings.View.RenderRectLeftSide;

                    Ray r = new Ray();
                    r.Origin = ray.Origin;
                    r.Direction = rayEnd - r.Origin;

                    if (x == 92 && y == 76)
                    {
                        int a = 5;
                        int b = a;
                        if (b == 300)
                            throw new Exception();
                    }

                    Color c = this.tracePixel(r, 0);
                    int bufIndex = x + y * rect.Width;
                    // concurrent writing
                    lock (this)
                    {
                        buffer[bufIndex] = c;
                    }
                }
            });
        }

        /// <summary>
        /// Gets color by shooting a ray into scene. Recursive.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="depth">Current depth of recursion (0 is primary ray).</param>
        /// <returns></returns>
        public Color tracePixel(Ray ray, int depth)
        {
            double isectDistance = Constants.Infinity;
            Primitive closest = objects.GetClosestIntersection(ref ray, out isectDistance);

            if (closest == null)
            {
                return this.settings.FillColor;
            }
            else
            {
                Vector isectPoint = ray.Origin + isectDistance * ray.Direction;

                Vector normal = closest.GetNormalAt(isectPoint);
                Material material = closest.GetMaterialAt(isectPoint);

                // reflection = v - 2*(v dot n)*n
                Vector vReflection = ray.Direction - (2 * normal.Dot(ray.Direction)) * normal;

                double specularLightness = 0.0;
                Color lightColor = lightColorAt(isectPoint, out specularLightness, vReflection, normal, material.Specular);

                Color primaryColor = material.Color * lightColor;
                Color reflectedColor = new Color();
                Color alphaColor = new Color();
                double reflectance = material.Reflectance;
                double opacity = material.Color.A;

                if (depth < Scene.maxRecursionDepth && reflectance > Constants.Epsilon)
                {
                    Ray rayReflection;
                    rayReflection.Direction = vReflection;
                    // move by a small epsilon along reflection vector
                    rayReflection.Origin = isectPoint + (Constants.Epsilon * vReflection);

                    reflectedColor = tracePixel(rayReflection, depth + 1);
                }
                if (depth < Scene.maxRecursionDepth && opacity < 1.0 - Constants.Epsilon)
                {
                    Ray rayAlpha;
                    // direction remains the same
                    rayAlpha.Direction = ray.Direction;
                    // progress by small epsilon forward
                    rayAlpha.Origin = isectPoint + (2 * Constants.Epsilon * rayAlpha.Direction);

                    alphaColor = tracePixel(rayAlpha, depth + 1);
                }

                // first reflection, then alpha (transparent mirror is still transparent)
                Color withoutAlpha = Color.Combine(primaryColor, 1 - reflectance, reflectedColor, reflectance);
                return Color.Combine(withoutAlpha, opacity, alphaColor, 1 - opacity) * (1 + specularLightness);

                // first alpha, then reflection (transparent mirror is still a mirror)
                /*Color withoutReflection = ColorCombine(primaryColor, opacity, alphaColor, 1 - opacity);
                return ColorCombine(withoutReflection, 1 - reflectance, reflectedColor, reflectance);*/
            }

        }

        /// <summary>
        /// Gets lightness and specular ligthness at given point in the scene.
        /// </summary>
        /// <param name="specularLightness">Returned specular lightness.</param>
        /// <param name="pos">Point in space where to calc ligthness.</param>
        /// <param name="reflectionDir">Direction of reflection.</param>
        /// <param name="normalDir">Direction of normal.</param>
        /// <param name="surfaceSpecular">Specular of the material.</param>
        /// <returns></returns>
        private Color lightColorAt(Vector pos, out double specularLightness, Vector vReflection, Vector normal, double surfaceSpecular)
        {
            Ray shadowRay = new Ray();
            shadowRay.Origin = pos;
            Color result = Color.Black;
            specularLightness = 0.0;

            foreach (Light light in this.lights)
	        {
                int visibleRayCount = 0;

                // test all rays to this light
                for (int i = 0; i < light.GetRayCount(); i++)
			    {
			        shadowRay.Direction = light.GetNextRaySource() - shadowRay.Origin;
                    // only front sides get lit
                    if (shadowRay.Direction.Dot(normal) >= Constants.Epsilon)
                    {
                        double isectDistance = 0;
                        objects.GetClosestIntersection(ref shadowRay, out isectDistance);
                        //double isectDistance2 = isectDistance * isectDistance;
                        //light visible from pos?
                        //if (!(isectDistance2 > Constants.Epsilon && isectDistance2 < shadowRay.Direction.LenSquared - Constants.Epsilon))
                        double lightDist2 = shadowRay.Direction.LenSquared;
                        shadowRay.Direction.Normalize();
                        if (!(isectDistance > Constants.Epsilon && isectDistance*isectDistance < lightDist2 - Constants.Epsilon))
                        {
                            visibleRayCount++;

                            Vector reflectionNorm = vReflection;
                            reflectionNorm.Normalize();
                            
                            // is reflected ray pointing towards light?
                            double specularDot = reflectionNorm.Dot(shadowRay.Direction);
                            if (specularDot > -Constants.Epsilon)
                            {
                                specularDot *= specularDot;
                                specularDot *= specularDot;
                                specularDot *= specularDot;
                                specularDot *= specularDot;
                                specularDot *= specularDot; // ^32
            
                                specularLightness += specularDot;
                            }

                        }
                    }
			    }
                result = Color.Combine(result, 1.0, light.Color,
                     visibleRayCount / (double)light.GetRayCount() * light.Value * shadowRay.Direction.Dot(normal));
        	}

            specularLightness *= surfaceSpecular;
            return result;
        }
    }
}
