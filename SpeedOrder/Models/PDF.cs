using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOrder.Models
{
    public interface IPdfPrinter
    {
        Task PrinterPDF(byte[] pdfData);
    }
}

