using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace jGrid.Helpers
{
    [DataContract]
    public class jgridFilter
    {
        [DataMember]
        public string groupOp { get; set; }
        [DataMember]
        public Rule[] rules { get; set; }

        public static jgridFilter Create(string jsonData)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(jgridFilter));
                System.IO.StringReader reader = new System.IO.StringReader(jsonData);
                System.IO.MemoryStream ms =
                    new System.IO.MemoryStream(Encoding.Default.GetBytes(jsonData));
                return serializer.ReadObject(ms) as jgridFilter;
            }
            catch
            {
                return null;
            }
        }
    }
}
