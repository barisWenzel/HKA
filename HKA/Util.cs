using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;


namespace HKA
{ 
    static class Util
    {

  
        public static void CleanMesh(Mesh mesh)
        {
            mesh.Normals.ComputeNormals();
            mesh.FaceNormals.ComputeFaceNormals();
            mesh.Vertices.CombineIdentical(true, true);
            mesh.UnifyNormals();
            mesh.Normals.ComputeNormals();
            mesh.FaceNormals.ComputeFaceNormals();
            mesh.Compact();
            mesh.RebuildNormals();
        }

        public static Polyline MakeSlotOnLine(Line line, Vector3d direction, double heigth, double width)
        {
            var pts = new List<Point3d>();
            pts.Add(line.From);

            Point3d pt1 = line.PointAtLength((line.Length / 2) - width);
            pts.Add(pt1);

            var pt2 = pt1 + direction * heigth;
            pts.Add(pt2);


            Point3d pt3 = line.PointAtLength((line.Length / 2) + width);
            var pt4 = pt3 + direction * heigth;
            pts.Add(pt4);
            pts.Add(pt3);

            pts.Add(line.To);

            return new Polyline(pts);
        }
   



    }
}
