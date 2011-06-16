using System;
using System.Collections.Generic;
using System.Text;
using Lucid.Raytracing;

namespace Lucid.Editor
{
    /// <summary>
    /// Helper class for creating scenes.
    /// </summary>
    public class Scenes
    {
        private static Random r = new Random();

        private static double rand(double from, double to)
        {
            return from + r.NextDouble() * (to - from);
        }

        private static Vector randVector(double range)
        {
            return new Vector(rand(-range, range), rand(-range, range), rand(-range, range));
        }

        private static Vector randVector(double rangeX, double rangeY, double rangeZ)
        {
            return new Vector(rand(-rangeX, rangeX), rand(-rangeY, rangeY), rand(-rangeZ, rangeZ));
        }

        private static Material getRandReflMaterial()
        {
            return new Material(new Color(rand(0.2, 1.2), rand(0.2, 0.8), rand(0.2, 0.8), 1.0),
                rand(0.2, 0.6), 0);
        }

        private static Material getRandMaterial()
        {
            return new Material(new Color(rand(0.2, 1.0), rand(0.2, 1.0), rand(0.2, 1.0), 1.0),
                0, 0);
        }

        private static void addSphereGrid(ScnFile file, int xs, int ys, int zs)
        {
            Material mat = getRandMaterial();
            file.Materials.Add(mat);
            // real dimensions
            double xw = 8; double yw = 8; double zw = 8;
            for (int x = 0; x < xs; x++)
            {
                for (int y = 0; y < ys; y++)
                {
                    for (int z = 0; z < zs; z++)
                    {
                        Vector center = new Vector(-4 + x / (double)xs * xw,
                                       -4 + y / (double)ys * yw,
                                       2 + z / (double)zs * zw);
                        file.Primitives.Add(new Sphere(center, 0.05/*rand(0.06, 0.16)*/,
                            mat));
                    }
                }
            }
        }
        private static void addBoxes(List<Primitive> objects)
        {
            /*for (int i = 0; i < 3; i++)
            {
                Vector left = new Vector(rand(-1.3, 1.3), rand(-1.3, 1.3), rand(-1.3, 1.3));
                objects.Add(new BoxPrimitive(
                    left, left + new Vector(rand(0.3, 1), rand(1.3, 5), rand(4, 50)),
                    getRandMaterial()));
            }*/
            for (int i = 0; i < 3; i++)
            {
                objects.Add(new Sphere(
                    new Vector(rand(-1, 1), rand(-1, 1), rand(-1, 1)), 2 * rand(0.1, 0.3),
                    getRandReflMaterial()));
            }
        }
        
        private static void addTriangles(List<Primitive> objects, int n, double size)
        {
            for (int i = 0; i < n; i++)
            {
                Vector b = randVector(2, 2, 2);
                objects.Add(new Triangle(getRandReflMaterial(),
                    b, b + randVector(size), b + randVector(size)));
            }
        }

        public static ScnFile getSceneBenchTriangles(int triangleCount)
        {
            ScnFile s = new ScnFile();

            s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Color(1, 1, 1, 1), 1));

            s.DefaultCamera.Origin = new Vector(0, 0, -4);
            s.DefaultCamera.LookAt = new Vector(0, 0, 0);

            //addSphereGrid(s);
            //addBoxes(s.Primitives);
            addTriangles(s.Primitives, triangleCount, 0.1);

            /*s.Primitives.Add(new Triangle(getRandMaterial(),
                new Vector(2, 0.7, 0.3), new Vector(-2, -2.7, 0.3),
                new Vector(-2, 0.7, 4.3)));*/

            return s;
        }

        public static ScnFile getSceneBenchSpheres(int gridSize)
        {
            ScnFile s = new ScnFile();

            s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Color(1, 1, 1, 1), 1));

            s.DefaultCamera.Origin = new Vector(0, 0, -4);
            s.DefaultCamera.LookAt = new Vector(0, 0, 0);

            addSphereGrid(s, gridSize, gridSize, gridSize);
            //addBoxes(s.Primitives);
            //addTriangles(s.Primitives, triangleCount, 0.1);

            /*s.Primitives.Add(new Triangle(getRandMaterial(),
                new Vector(2, 0.7, 0.3), new Vector(-2, -2.7, 0.3),
                new Vector(-2, 0.7, 4.3)));*/

            return s;
        }

        public static ScnFile getSceneMc1()
        {
            ScnFile s = new ScnFile();

            s.DefaultCamera.Origin = new Vector(2, -2, -3);
            s.DefaultCamera.LookAt = new Vector(0, 0, 0);

            //s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Lucid.Base.Color(1, 1, 1, 1), 2));
            s.Lights.Add(new PointLight(new Vector(2, -2.5, 1), new Color(1, 1, 1, 1), 1.6));
            s.Lights.Add(new PointLight(new Vector(-2, -1, -3), new Color(1, 1, 1, 1), 2.5));

            //s.Primitives.Add(new Sphere(new Vector(-1, 8, -1), 6, getRandMaterial()));
            //s.Primitives.Add(new BoxPrimitive(new Vector(-10, 4, -10), new Vector(10, 4, 10), getRandMaterial()));

            //addSphereGrid(s);

            s.ModelFiles.Add(new ModelFileInfo("Mclaren.wrl", Matrix.RotateZ90 * Matrix.RotateZ90 * Matrix.RotateY90));

            return s;
        }

        public static ScnFile getSceneMc2()
        {
            ScnFile s = new ScnFile();

            //s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2);
            s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2)*0.9;
            s.DefaultCamera.LookAt = new Vector(1, -0.2, 0);

            //s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Lucid.Base.Color(1, 1, 1, 1), 2));
            s.Lights.Add(new PointLight(new Vector(4, -3, -3), new Color(1, 1, 1, 1), 2.6));
            //s.Lights.Add(new PointLight(new Vector(0, -1, -2), new Lucid.Base.Color(1, 1, 1, 1), 1.5));

            double d = 0.1;
            double l = 2;
            Material m1 = getRandReflMaterial();
            Material m2 = getRandReflMaterial();
            s.Primitives.Add(new Triangle(m1,
                new Vector(-l, d, l), new Vector(-l, d, -l), new Vector(l, d, l)));
            s.Primitives.Add(new Triangle(m1,
                new Vector(l, d, -l), new Vector(l, d, l), new Vector(-l, d, -l)));
            s.Primitives.Add(new Triangle(m2,
                new Vector(-l, d, -l), new Vector(-l, d, l), new Vector(-l, -2*l, -l)));
            s.Primitives.Add(new Triangle(m2,
                new Vector(-l, -2*l, -l), new Vector(-l, d, l), new Vector(-l, -2*l, l)));
            //s.Primitives.Add(new Sphere(new Vector(-1, 5, -1), 5.5, getRandMaterial()));
            //s.Primitives.Add(new BoxPrimitive(new Vector(-10, 4, -10), new Vector(10, 4, 10), getRandMaterial()));

            //addSphereGrid(s);

            s.ModelFiles.Add(new ModelFileInfo("Mclaren.wrl",  (Matrix.RotateY90 * Matrix.RotateY90 * Matrix.RotateZ90 * Matrix.RotateZ90 * Matrix.RotateY90).Translate(0.3, 0, 0)));

            return s;
        }

        public static ScnFile getSceneBug()
        {
            ScnFile s = new ScnFile();

            //s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2);
            s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2);
            s.DefaultCamera.LookAt = new Vector(1, -0.2, 0);

            //s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Lucid.Base.Color(1, 1, 1, 1), 2));
            //s.Lights.Add(new PointLight(new Vector(0, -3, 1), new Lucid.Base.Color(1, 1, 1, 1), 2.6));
            s.Lights.Add(new PointLight(new Vector(-1, -0.8, -4), new Color(1, 1, 1, 1), 1.5));

            double d = 0.1;
            double l = 4;
            Material m1 = getRandReflMaterial();
            Material m2 = getRandReflMaterial();
            /*s.Primitives.Add(new Triangle(m1,
                new Vector(-l, d, l), new Vector(-l, d, -l), new Vector(l, d, l)));
            s.Primitives.Add(new Triangle(m1,
                new Vector(l, d, -l), new Vector(l, d, l), new Vector(-l, d, -l)));*/
            /*s.Primitives.Add(new Triangle(m2,
                new Vector(-l, d, -l), new Vector(-l, d, l), new Vector(-l, -2*l, -l)));
            s.Primitives.Add(new Triangle(m2,
                new Vector(-l, -2*l, -l), new Vector(-l, d, l), new Vector(-l, -2*l, l)));*/
            s.Primitives.Add(new Sphere(new Vector(1.5, 10, 1.5), 10, getRandReflMaterial()));
            //s.Primitives.Add(new BoxPrimitive(new Vector(-10, 4, -10), new Vector(10, 4, 10), getRandMaterial()));

            //addSphereGrid(s);

            s.ModelFiles.Add(new ModelFileInfo("Mclaren.wrl", (Matrix.RotateY90 * Matrix.RotateY90 * Matrix.RotateZ90 * Matrix.RotateZ90 * Matrix.RotateY90).Translate(0.3, 0, 0)));

            return s;
        }

        public static ScnFile getSceneChair()
        {
            ScnFile s = new ScnFile();

            //s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2);
            s.DefaultCamera.Origin = new Vector(3, -3, -2)*1.4;
            s.DefaultCamera.LookAt = new Vector(0, -1, 0);

            //s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Lucid.Base.Color(1, 1, 1, 1), 2));
            s.Lights.Add(new PointLight(new Vector(2, -5, -3), new Color(1, 1, 1, 1), 1.2));
            //s.Lights.Add(new PointLight(new Vector(-8, -4, 4), new Lucid.Base.Color(1, 1, 1, 1), 0.5));
            s.Lights.Add(new PointLight(new Vector(3, -5, 1.5), new Color(1, 1, 1, 1), 0.5));

            double d = 0.5;
            double l = 20;
            Material m1 = getRandReflMaterial();
            Material m2 = getRandReflMaterial();
            s.Primitives.Add(new Triangle(m1,
                new Vector(-l, d, l), new Vector(-l, d, -l), new Vector(l, d, l)));
            s.Primitives.Add(new Triangle(m1,
                new Vector(l, d, -l), new Vector(l, d, l), new Vector(-l, d, -l)));
            /*s.Primitives.Add(new Triangle(m2,
                new Vector(-l, d, -l), new Vector(-l, d, l), new Vector(-l, -2*l, -l)));
            s.Primitives.Add(new Triangle(m2,
                new Vector(-l, -2*l, -l), new Vector(-l, d, l), new Vector(-l, -2*l, l)));*/
            //s.Primitives.Add(new Sphere(new Vector(-6, -6, 0), 5, getRandMaterial()));
            //s.Primitives.Add(new BoxPrimitive(new Vector(-10, 4, -10), new Vector(10, 4, 10), getRandMaterial()));

            //addSphereGrid(s);

            s.ModelFiles.Add(new ModelFileInfo("chair.wrl", Matrix.RotateX90*Matrix.Scaling(0.1)));

            return s;
        }

        public static ScnFile getSceneChairManyLight()
        {
            ScnFile s = getSceneChair();
            s.DefaultCamera.Origin = new Vector(6, -8, 5);

            s.Lights.Clear();
            double y = -3;
            double r = 1.5;
            int count = 40;
            for (int i = 0; i < count; i++)
            {
                s.Lights.Add(new PointLight(
                    new Vector(-1 + rand(-r, r), y, -4 + rand(-r, r)), new Color(1, 1, 1, 1), 1.8/(double)count));
            }

            return s;
        }

        public static ScnFile getSceneSimple()
        {
            ScnFile s = new ScnFile();

            //s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2);
            s.DefaultCamera.Origin = new Vector(3, -3, -2) * 1.4;
            s.DefaultCamera.LookAt = new Vector(0, -1, 0);

            s.Lights.Add(new PointLight(new Vector(2, -5, -3), new Color(1, 1, 1, 1), 1.2));

            double d = 0.5;
            double l = 20;
            Material m1 = getRandReflMaterial();
            s.Primitives.Add(new Triangle(m1,
                new Vector(-l, d, l), new Vector(-l, d, -l), new Vector(l, d, l)));
            s.Primitives.Add(new Triangle(m1,
                new Vector(l, d, -l), new Vector(l, d, l), new Vector(-l, d, -l)));

            return s;
        }

        public static ScnFile getSceneChairTest()
        {
            ScnFile s = new ScnFile();

            //s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2);
            s.DefaultCamera.Origin = new Vector(3, -0.4, -2) * 1.6;
            s.DefaultCamera.LookAt = new Vector(0, -1, 0);

            //s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Lucid.Base.Color(1, 1, 1, 1), 2));
            s.Lights.Add(new PointLight(new Vector(2, -5, -3), new Color(1, 1, 1, 1), 1.2));
            //s.Lights.Add(new PointLight(new Vector(-8, -4, 4), new Lucid.Base.Color(1, 1, 1, 1), 0.5));
            s.Lights.Add(new PointLight(new Vector(3, -5, 1.5), new Color(1, 1, 1, 1), 1.5));

            double d = 0.4;
            double l = 20;
            Material m1 = getRandReflMaterial();
            Material m2 = getRandReflMaterial();
            s.Primitives.Add(new Triangle(m1,
                new Vector(-l, d, l), new Vector(-l, d, -l), new Vector(l, d, l)));
            s.Primitives.Add(new Triangle(m1,
                new Vector(l, d, -l), new Vector(l, d, l), new Vector(-l, d, -l)));
            /*s.Primitives.Add(new Triangle(m2,
                new Vector(-l, d, -l), new Vector(-l, d, l), new Vector(-l, -2*l, -l)));
            s.Primitives.Add(new Triangle(m2,
                new Vector(-l, -2*l, -l), new Vector(-l, d, l), new Vector(-l, -2*l, l)));*/
            s.Primitives.Add(new Sphere(new Vector(-6, -6, 0), 5.7, getRandReflMaterial()));
            //s.Primitives.Add(new Sphere(new Vector(0, 0, 10), 6, getRandMaterial()));
            //s.Primitives.Add(new BoxPrimitive(new Vector(-10, 4, -10), new Vector(10, 4, 10), getRandMaterial()));

            //addSphereGrid(s);

            s.ModelFiles.Add(new ModelFileInfo("chair.wrl", Matrix.RotateX90 * Matrix.Scaling(0.1)));

            return s;
        }

        public static ScnFile getSceneCarTest()
        {
            ScnFile s = new ScnFile();

            //s.DefaultCamera.Origin = new Vector(3.2, -1.2, -2);
            s.DefaultCamera.Origin = new Vector(3, -0.4, -2) * 1.6;
            s.DefaultCamera.LookAt = new Vector(0, -1, 0);

            //s.Lights.Add(new PointLight(new Vector(1.5, -2.7, -5.0), new Lucid.Base.Color(1, 1, 1, 1), 2));
            s.Lights.Add(new PointLight(new Vector(2, -5, -3), new Color(1, 1, 1, 1), 1.2));
            //s.Lights.Add(new PointLight(new Vector(-8, -4, 4), new Lucid.Base.Color(1, 1, 1, 1), 0.5));
            s.Lights.Add(new PointLight(new Vector(6, -4, 0), new Color(1, 1, 1, 1), 1.5));
            s.Lights.Add(new PointLight(new Vector(-16, -16, -4), new Color(1, 1, 1, 1), 1));

            double d = 0.05;
            double l = 20;
            Material m1 = getRandReflMaterial();
            Material m2 = getRandReflMaterial();
            s.Primitives.Add(new Triangle(m1,
                new Vector(-l, d, l), new Vector(-l, d, -l), new Vector(l, d, l)));
            s.Primitives.Add(new Triangle(m1,
                new Vector(l, d, -l), new Vector(l, d, l), new Vector(-l, d, -l)));
            s.Primitives.Add(new Sphere(new Vector(-6, -6, 0), 6, getRandReflMaterial()));
            s.Primitives.Add(new Sphere(new Vector(0.5, -5, 7), 5, getRandReflMaterial()));
            //s.Primitives.Add(new BoxPrimitive(new Vector(-10, 1, -10), new Vector(10, 1.1, 10), getRandMaterial()));

            //addSphereGrid(s);

            s.ModelFiles.Add(new ModelFileInfo("Mclaren.wrl", (Matrix.Scaling(1.5) * Matrix.RotateY90 * Matrix.RotateZ90 * Matrix.RotateZ90 * Matrix.RotateY90).Translate(0.6, 0, 0)));

            return s;
        }
    }
}
