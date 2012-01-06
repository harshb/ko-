using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jGrid.Helpers
{
    /// <summary>
    /// The supported operations in where-extension
    /// </summary>
    public enum WhereOperation
    {
        [StringValue("eq")]
        Equal,
       
        [StringValue("ne")]
        NotEqual,
       
        [StringValue("cn")]
        Contains,
       
        [StringValue("gt")]
        greater,
       
        [StringValue("lt")]
        less
    }
}