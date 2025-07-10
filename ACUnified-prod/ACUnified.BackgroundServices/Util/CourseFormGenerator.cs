using ACUnified.Shared.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACUnified.BackgroundServices.Util;

using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using ACUnified.Data.Models;
using PdfSharp.Pdf.IO;
using Gma.QrCodeNet.Encoding;
using System.Drawing.Imaging;
using System.Drawing;



namespace ACUnified.BackgroundServices.NewFolder
{
    public class MyFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            using (var ms = new MemoryStream())
            {
                // Replace "Arial.ttf" with the path to your Arial font file
                string fontPath = "C:\\Windows\\Fonts\\Arial.ttf";
                using (var fs = File.OpenRead(fontPath))
                {
                    fs.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                }
                return ms.ToArray();
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // You can customize this method based on your font requirements
            return new FontResolverInfo("Arial");
        }
    }
    public class CourseFormGenerator
    {
        
        public  static MemoryStream GenerateCourseFormPdf(CourseRegistrationGeneratorDto courseRegGenDto,MyFontResolver mfr)
        {
            string logob64 = "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";
            //string templatePdfFilePath = "C:\\Users\\1010\\Downloads\\bells_bells.pdf";
            // string templatePdfFilePath = "C:\\Users\\1010\\Downloads\\bells-with-sig.pdf";
            //string templatePdfFilePath = "C:\\Users\\1010\\Downloads\\Bells_update_update_.pdf";
            string templatePdfFilePath = "C:\\Users\\1010\\Downloads\\Bell_bells_update.pdf";

            GlobalFontSettings.FontResolver = mfr;
            //PdfDocument document = new PdfDocument();
            MemoryStream ms = new MemoryStream();
            using (PdfDocument document =new PdfDocument())
            {
                foreach (var stud in courseRegGenDto.Students)
                {
                   using (PdfDocument templatepdf = PdfReader.Open(templatePdfFilePath, PdfDocumentOpenMode.Import))
                    {
                        // Add a page to the document
                        PdfPage page = document.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);

                        // Define font and styles
                        XFontFamily fontFamily = new XFontFamily("Arial");
                        XFont font = new XFont("Arial", 11, XFontStyleEx.Regular);
                        XFont font2 = new XFont("Arial Rounded MT Bold", 11, XFontStyleEx.Bold);
                        XFont sfont = new XFont("Arial", 10, XFontStyleEx.Regular);
                        XFont ifont = new XFont("Arial", 10, XFontStyleEx.Italic);
                        XFont Bfont = new XFont("Arial", 14, XFontStyleEx.Bold);
                        XPen pen = new XPen(XColors.Blue, 1);
                        pen.DashStyle = XDashStyle.DashDotDot;
                        //DrawLogo(gfx, logob64, 50, 100);

                        //DrawCenteredText(gfx,$"Department of {stud.ProgramSetup.Department.Name}", font, XBrushes.Black,20); ;
                        //DrawCenteredText(gfx, $"{courseRegGenDto.SchoolName}", font, XBrushes.Black,40);
                        //DrawCenteredText(gfx,$"{courseRegGenDto.Address}", font, XBrushes.Black,60);

                       

                        foreach(PdfPage templatePage in templatepdf.Pages)
                        {
                            XImage image = XImage.FromFile(templatePdfFilePath);
                            gfx.DrawImage(image, 0, 0);
                            DrawCenteredText(gfx, "FIRST SEMESTER 2023/2024 100L COURSE REGISTRATION FORM", Bfont, XBrushes.Gray, 80);
                            DrawBorderlessTable(gfx, font, "Student Information", 100, 100, stud);
                            DrawBorderedTable(gfx, font2, "Courses Information", 100, 300, courseRegGenDto.Courses);
                        }

                        // Draw line for signatures
                        // gfx.DrawLine(pen, 100, 700, page.Width - 70,700);
                        //DrawCenteredText(gfx, $"Advisor's Signature & Date HOD's Signature & Date Faculty Officer's Signature & Date", font, XBrushes.Black, 620);



                    }



                }
                if (document.PageCount > 0)
                {
                    document.Save(ms, false);
                    document.Close();
                }

            }
           
            
            return ms;
        }

        private static void DrawLogo(XGraphics gfx, string base64Logo, int x, int y)
        {
            Byte[] imageBytes = Convert.FromBase64String(base64Logo);
            XImage image;
            

            using (MemoryStream ms22 = new MemoryStream(imageBytes))
            {

                image = XImage.FromFile("C:\\Users\\1010\\Downloads\\bellsuni.jpg");
            }

            gfx.DrawImage(image, x, y, 50, 50);
           
        }

        private static void DrawBorderedTable(XGraphics gfx, XFont font, string title, int startX, int startY,IEnumerable<CourseDto> courses)
        {
            int cellPadding = 10;
            int rowHeight = 20;
            int serialNumberColumnWidth = 50;
            int courseTitleColumnWidth = 250; // Adjust as needed for a wider "Course Title" column
            int courseCodeColumnWidth = 100;
            int unitsColumnWidth = 40;

            // Draw title
            gfx.DrawString(title, font, XBrushes.Black, new XPoint(startX, startY));
            startY += rowHeight;

            // Draw table headers
            //gfx.DrawLine(XPens.AliceBlue, startX, startY, startX + serialNumberColumnWidth + courseTitleColumnWidth + courseCodeColumnWidth + unitsColumnWidth, startY); // Top line
            XBrush titleBrush = XBrushes.White; // White text color for better visibility on blue background
            XBrush backgroundColor = XBrushes.SaddleBrown;
            gfx.DrawRectangle(backgroundColor, startX, startY, serialNumberColumnWidth, rowHeight);
            gfx.DrawString("S/N", font, titleBrush, new XPoint(startX + cellPadding, startY + cellPadding));
            gfx.DrawRectangle(backgroundColor, startX + serialNumberColumnWidth, startY, courseTitleColumnWidth, rowHeight);
            gfx.DrawString("Course Title", font, titleBrush, new XPoint(startX + serialNumberColumnWidth + cellPadding, startY + cellPadding));
            gfx.DrawRectangle(backgroundColor, startX + serialNumberColumnWidth + courseTitleColumnWidth, startY, courseCodeColumnWidth, rowHeight);
            gfx.DrawString("Course Code", font, titleBrush, new XPoint(startX + serialNumberColumnWidth + courseTitleColumnWidth + cellPadding, startY + cellPadding));
            gfx.DrawRectangle(backgroundColor, startX + serialNumberColumnWidth + courseTitleColumnWidth + courseCodeColumnWidth, startY, unitsColumnWidth, rowHeight);
            gfx.DrawString("Units", font, titleBrush, new XPoint(startX + serialNumberColumnWidth + courseTitleColumnWidth + courseCodeColumnWidth + cellPadding, startY + cellPadding));
            startY += rowHeight;

            // Draw course data
            int i = 1;
            foreach (var course in courses)
            {
                //gfx.DrawRectangle(XPens.Black, startX, startY, serialNumberColumnWidth, rowHeight);
                //gfx.DrawLine(XPens.Gray, startX, startY, startX+serialNumberColumnWidth + courseTitleColumnWidth + courseCodeColumnWidth + unitsColumnWidth, startY);

                gfx.DrawString(i.ToString(), font, XBrushes.Black, new XPoint(startX + cellPadding, startY + cellPadding));
                //gfx.DrawRectangle(XPens.Black, startX + serialNumberColumnWidth, startY, courseTitleColumnWidth, rowHeight);
                gfx.DrawString(course.Name, font, XBrushes.Black, new XPoint(startX + serialNumberColumnWidth + cellPadding, startY + cellPadding));
                //gfx.DrawRectangle(XPens.Black, startX + serialNumberColumnWidth + courseTitleColumnWidth , startY, courseCodeColumnWidth, rowHeight);
                gfx.DrawString(course.CourseCode, font, XBrushes.Black, new XPoint(startX +  serialNumberColumnWidth+courseTitleColumnWidth + cellPadding, startY + cellPadding));
                //gfx.DrawRectangle(XPens.Black, startX + serialNumberColumnWidth + courseTitleColumnWidth + courseCodeColumnWidth, startY, unitsColumnWidth, rowHeight);
                gfx.DrawString(course.CourseUnit.ToString(), font, XBrushes.Black, new XPoint(startX + serialNumberColumnWidth + courseTitleColumnWidth+courseCodeColumnWidth + cellPadding, startY + cellPadding));
                //gfx.DrawRectangle(XPens.Black, startX + serialNumberColumnWidth + courseTitleColumnWidth + courseCodeColumnWidth, startY, unitsColumnWidth, rowHeight);

                startY += rowHeight;
                gfx.DrawLine(XPens.Gray, startX , startY, startX + serialNumberColumnWidth + courseTitleColumnWidth + courseCodeColumnWidth + unitsColumnWidth, startY);

                i++;
            }
            XFont ifont = new XFont("Arial", 8, XFontStyleEx.Italic);
            XFont bfont = new XFont("Arial", 8, XFontStyleEx.Bold);

            gfx.DrawString($"             Total Outstanding Courses:N/A       Total Courses:{courses.Count()}   Total Units:{courses.Sum(x => x.CourseUnit)}  ", bfont, XBrushes.Black, new XPoint(startX + cellPadding*2, startY + cellPadding*2));
            //startY += rowHeight*2;
            DrawQrCode(gfx, "https://www.bellsuniversity.edu.ng", 100, startY,1);
           // startY += rowHeight+150;
            //gfx.DrawString("Powered By: eTechnosoft (c) 2024",font,XBrushes.DarkSlateGray,210,startY);

        }

        private static void DrawBorderlessTable(XGraphics gfx, XFont font, string title, int startX, int startY, StudentDto student)
        {
            int cellPadding = 10;
            int rowHeight = 20;

            // Draw title
            //gfx.DrawString(title, font, XBrushes.Black, new XPoint(startX, startY+95));
            startY += rowHeight;
            XFont ifont = new XFont("Arial", 10, XFontStyleEx.Italic);
            XFont Bfont = new XFont("Arial", 10, XFontStyleEx.Bold);


            // Draw student information
            gfx.DrawString($"Surname: {student.LastName}   ", font, XBrushes.Black, new XPoint(startX, startY + 100));
            gfx.DrawString($"Firstname: {student.FirstName}    ", font, XBrushes.Black, new XPoint(startX+cellPadding*14+10, startY + 100));
            gfx.DrawString($" Othernames: {student.OtherName}", font, XBrushes.Black, new XPoint(startX+cellPadding*30+27, startY + 100));

            gfx.DrawString($"College: {student.ProgramSetup.Faculty.Name} ", font, XBrushes.Black, new XPoint(startX, startY + 120));
            gfx.DrawString($"Dept.: {student.ProgramSetup.Department.Name}", font, XBrushes.Black, new XPoint(startX+cellPadding*30+27 , startY + 120));
            gfx.DrawString($"Matric No: {student.Matric}", font, XBrushes.Black, new XPoint(startX, startY + 140));
            gfx.DrawString($"Level: {student.Level.Name}", font, XBrushes.Black, new XPoint(startX+cellPadding*14, startY + 140));
            gfx.DrawString("Session: 2023/2024", font, XBrushes.Black, new XPoint(startX+cellPadding*30+27, startY + 140));

            startY += rowHeight;


        }

        static void DrawCenteredText(XGraphics gfx, string text, XFont font, XBrush brush,double ycoord)
        {
            // Measure the width and height of the text
            XSize textSize = gfx.MeasureString(text, font);

            // Calculate the X-coordinate to center the text on the page
            double x = (gfx.PageSize.Width - textSize.Width) / 2;

            // Calculate the Y-coordinate to center the text on the page
            double y = 100;

            // Draw the text at the calculated position
            gfx.DrawString(text, font, brush, new XPoint(x, y+ycoord));
        }

        static void DrawQrCode(XGraphics gfx, string qrCodeData, double x, double y, double size)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode;
            if (qrEncoder.TryEncode(qrCodeData, out qrCode))
            {
                // Create a brush for drawing the QR code (you can customize colors as needed)
                XSolidBrush brush = new XSolidBrush(XColor.FromArgb(0, 0, 0));

                // Loop through the QR code matrix and draw rectangles for each module
                for (int xModule = 0; xModule < qrCode.Matrix.Width; xModule++)
                {
                    for (int yModule = 0; yModule < qrCode.Matrix.Height; yModule++)
                    {
                       
                        if (qrCode.Matrix[xModule, yModule])
                        {
                            // Draw a black rectangle for the QR code module
                            gfx.DrawRectangle(brush, x + xModule * size, y + yModule * size, size, size);
                        }
                    }
                }
            }
        }



    }
}

