using System.Collections.Generic;
using localtour.Transactions.Dto;
using localtour.DataExporting.Excel.EpPlus;

namespace localtour.Transactions.Exporting
{
    public interface ITransactionsExcelExporter
    {
        FileDto ExportToFile(List<GetTransactionForViewDto> assets);
    }
}