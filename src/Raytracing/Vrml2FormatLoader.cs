using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Lucid.Raytracing
{
    public class Vrml2FormatLoader : ISceneLoader
    {
        public Vrml2FormatLoader()
        {
        }

        public Matrix TransformMatrix;

        #region ISceneLoader Members

        public void AddToSceneFrom(Scene scene, string fileName)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                StreamTokenizer lex = new StreamTokenizer(stream);
                readWholeFile(lex, scene);
            }
        }

        #endregion

        private void readWholeFile(StreamTokenizer lex, Scene scene)
        {
            while (!lex.EndOfStream)
            {
                while (lex.Word == "Transform")
                {
                    readTransformObject(lex, scene);
                }

                lex.NextWord();
            }
        }

        private void readTransformObject(StreamTokenizer lex, Scene scene)
        {
            int startLevel = lex.BracketLevel;
            while (!lex.EndOfStream)
            {
                lex.NextWord();
                // check to return immediately after reading word
                if (lex.BracketLevel <= startLevel)
                    return;

                // transform matrix
                Matrix ShapeMatrix = TransformMatrix;
                while (lex.Word == "Transform")
                {
                    // nested transforms
                    readTransformObject(lex, scene);
                }
                if (lex.Word == "translation")
                {
                    double tx = lex.ReadDouble();
                    double ty = lex.ReadDouble();
                    double tz = lex.ReadDouble();
                    ShapeMatrix = TransformMatrix.Translate(tx, ty, tz);
                }
                while (lex.Word == "Shape")
                {
                    readShapeObject(lex, scene, ShapeMatrix);
                }

                // must have chance to return immediately after returning,
                // because the word might be needed several levels higher
                if (lex.BracketLevel <= startLevel)
                    return;
            }
        }

        private void readShapeObject(StreamTokenizer lex, Scene scene, Matrix shapeMatrix)
        {
            int startLevel = lex.BracketLevel;
            List<Vector> shapeVertices = null;
            Material shapeMaterial = new Material(Color.Black, 0, 0);
            while (!lex.EndOfStream)
            {
                lex.NextWord();
                if (lex.BracketLevel <= startLevel)
                    return;

                if (lex.Word == "diffuseColor")
                {
                    double r = lex.ReadDouble();
                    double g = lex.ReadDouble();
                    double b = lex.ReadDouble();
                    shapeMaterial.Color = new Color(r, g, b, 1.0);
                }

                if (lex.Word == "transparency")
                {
                    Color shapeColor = shapeMaterial.Color;
                    shapeColor.A = 1 - lex.ReadDouble();
                    shapeMaterial.Color = shapeColor;
                }

                if (lex.Word == "shininess")
                {
                    shapeMaterial.Reflectance = lex.ReadDouble();
                    shapeMaterial.Specular = shapeMaterial.Reflectance;
                }

                if (lex.Word == "point")
                {
                    shapeVertices = readShapeVertices(lex);
                }

                if (lex.Word == "coordIndex")
                {
                    scene.addMaterial(shapeMaterial);
                    readPolygons(lex, scene, shapeVertices, shapeMaterial, shapeMatrix);
                }

                if (lex.BracketLevel <= startLevel)
                    return;
            }
        }

        private void readPolygons(StreamTokenizer lex, Scene scene, List<Vector> vertices, Material shapeMaterial, Matrix shapeMatrix)
        {
            int startLevel = lex.BracketLevel;
            while (!lex.EndOfStream)
            {
                double i1 = 0;
                if (lex.TryReadDouble(out i1))
                {
                    // read polyline till -1
                    double iPreLast = lex.ReadDouble();
                    double iLast = lex.ReadDouble();
                    while (iLast != -1)
                    {
                        scene.addObject(new Triangle(shapeMaterial,
                        shapeMatrix * vertices[(int)i1],
                        shapeMatrix * vertices[(int)iPreLast],
                        shapeMatrix * vertices[(int)iLast]));

                        iPreLast = iLast;
                        iLast = lex.ReadDouble();
                    }
                }

                if (lex.BracketLevel <= startLevel)
                    return;
            }
        }

        private List<Vector> readShapeVertices(StreamTokenizer lex)
        {
            List<Vector> result = new List<Vector>(200);

            int startLevel = lex.BracketLevel;
            while (!lex.EndOfStream)
            {
                double x = 0;
                if (lex.TryReadDouble(out x))
                {
                    // more vertices available
                    double y = lex.ReadDouble();
                    double z = lex.ReadDouble();
                    result.Add(new Vector(x, y, z));
                }
                else
                {
                    int a = 5;
                    int b = a;
                }

                if (lex.BracketLevel <= startLevel)
                    break;
            }
            return result;
        }
    }
}
