using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;
using System.Dynamic;


namespace Massive
{
    public class Placements : DynamicModel
    {

        public Placements() : base("mi", "MEDIA_CAT_DIGITAL_PLCMNT", "OID_MEDIA_CAT_DIGITAL_PLCMNT") { }
    }
}