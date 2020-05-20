using System.Collections.Generic;
using localtour.Disputes.Dto;
using localtour.DataExporting.Excel.EpPlus;

namespace localtour.Disputes.Exporting
{
    public interface IDisputesExcelExporter
    {
        FileDto ExportToFile(List<GetDisputeForViewDto> assets);
    }
}