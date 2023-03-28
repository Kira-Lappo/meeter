using System;
using System.Collections.Generic;

namespace Meeter.Models.Exports;

public class MeeterExportData
{
    public DateTime ExportDateTime { get; set; }

    public List<Meeting> Items { get; set; }
}
