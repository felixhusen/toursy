using System.Collections.Generic;
using localtour.Tours.Dto;
using localtour.DataExporting.Excel.EpPlus;

namespace localtour.Tours.Exporting
{
    public interface IToursExcelExporter
    {
        FileDto ExportToFile(List<GetTourForViewDto> assets);
    }
}