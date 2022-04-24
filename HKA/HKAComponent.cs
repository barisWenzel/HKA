using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace HKA
{
    public class HKAComponent : GH_Component
    {
  
        public HKAComponent() : base("HKA", "Nickname","Description","Category", "Subcategory") {}

 
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh", GH_ParamAccess.item);
            pManager.AddNumberParameter("flangeLength", "flangeLength", "flangeLength", GH_ParamAccess.item);
            pManager.AddNumberParameter("materialThickness", "materialThickness", "materialThickness", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Mesh", "Mesh", "Mesh", GH_ParamAccess.tree);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {


            #region Retrieve Inputs
            Mesh mesh = new Mesh();
            if (!DA.GetData(0, ref mesh)) return;
            Util.CleanMesh(mesh);

            double flangeLength = 0.0;
            if (!DA.GetData(1, ref flangeLength)) return;

            double materialThickness = 0.0;
            if (!DA.GetData(2, ref materialThickness)) return;


            #endregion


            #region Collections
            var MTE = mesh.TopologyEdges;
            var facePlanes = new DataTree<Plane>();
            var outlines = new DataTree<Curve>();

            #endregion




            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                // make plane per face
                Vector3d normal = mesh.FaceNormals[i];
                Point3d center = mesh.Faces.GetFaceCenter(i);
                facePlanes.Add(new Plane(center, normal), new GH_Path(i));

                //get edgeIndices for actual face
                bool[] sameOrientation;
                var edgeIndices = MTE.GetEdgesForFace(i, out sameOrientation);


                for (int j = 0; j < edgeIndices.Length; j++)
                {
                    //if it is a naked edge,continue
                    if (MTE.IsNgonInterior(edgeIndices[j])) continue;

                    Line line = MTE.EdgeLine(edgeIndices[j]);
                    var connF = MTE.GetConnectedFaces(edgeIndices[j]);

                    if (connF.Length == 1) // if it is an exterior edge
                    {
                        outlines.Add(line.ToNurbsCurve(), new GH_Path(i));
                        continue;
                    }

                    if (sameOrientation[j] == false)
                        line.Flip();

                    var cross = Vector3d.CrossProduct(normal, line.Direction);
                    cross.Unitize();

                    outlines.Add(Util.MakeSlotOnLine(line, cross, flangeLength, materialThickness / 2).ToNurbsCurve(), new GH_Path(i));
                }
            }
            DA.SetDataTree(0, outlines);
        }

        protected override System.Drawing.Bitmap Icon => null;


        public override Guid ComponentGuid => new Guid("B533D734-4362-4B67-AC2B-BDA6A87261C1");
    }
}