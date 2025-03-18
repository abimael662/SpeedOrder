using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedOrder.Models
{
    public interface IPrintService
    {
        void PrintPDF(byte[] pdfData);
    }
}