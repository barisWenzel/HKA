using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace HKA
{
    public class HKAInfo : GH_AssemblyInfo
    {
        public override string Name => "HKA";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("A531ACA9-E51A-4EA5-B1E4-718E65F98110");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}