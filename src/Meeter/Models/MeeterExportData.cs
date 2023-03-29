using System;
using System.Collections.Generic;

namespace Meeter.Models;

public class MeeterExportData
{
    public DateTime ExportDateTime { get; set; }

    public DateTime MeetingsByDateTime { get; set; }

    public List<Meeting> Items { get; set; }
}
