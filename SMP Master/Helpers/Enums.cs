using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Helpers
{
    public class Enums
    {
    }

    public enum StatusTypes
    {
        [Description("Tree")]
        Tree = 1,
        [Description("Incident")]
        Incident = 2,
        [Description("TaskTree")]
        TaskTree = 3
    }

    public enum Raportet
    {
        [Description("Raporti për pagat - tabelare")]
        PagatTabelare = 1,
    }
}
