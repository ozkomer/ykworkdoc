using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;

namespace OrderFormSystem.Output
{
    class PdfOutput : Output
    {
        public PdfOutput(Order od)
        {
            m_order = od;
        }

        private StreamReader streamToPrint;
        private System.Drawing.Font printFont; 

        static public String[] headings = {
										  "Book/Product Model:",
										  "Sales Handle:",
										  "Why We Published this Book/Product Model:",
										  "Key benefits:",
										  "About the Author(s):",
										  "Technology/Topic Overview: ",
										  "Book/Product Content Summary:", 
										  "Audience:",
										  "What's on the CD/DVD/Web:"
									  };

        static public String[] texts = {
									   "Ideally, choose one title (2-3 if absolutely necessary) that this book should perform like. Include full title, ISBN, author, and any sell through numbers if possible.",
									   "One line description about the sales.",
									   "Brief description (one-two lines) on the importance of this book to the audience.",
									   "What benefit does this book provide to the consumer? (expert advice, speed, fun, productivity). Why should the Retailer/Wholesaler select this book over its competition? What are the unique features about this book should be highlighted? What makes this book different, better? From other books and the previous edition?",
									   "What makes this person so special?  Is she/he an expert, creator of the technology, educational leader, etc.? What is their background, and what relevant experiences do they have to make them the BEST choice? Have he/she/they won awards or been recognized in any way. Other books poublished by the author.\n1. Book one.\n2. Book two.",
									   "In brief two to five line description of the technology, topic or relevant information. Please keep descriptions succinct.",
									   "Ideal describe the contents of this book. What will this book do for the reader? Will this book help them optimize their system? Increase productivity? offer tips and stragegies?",
									   "Who is your intended customer? Experts? Power users? Business professionals? Programmers? What are the demographics?",
									   "What is included on the Cd or Web site? Why is it necessary and what will it do for the purchaser (source code, examples, case studies)?\nIs there a value that can be associated with what is on the CD/DVD or Web?"
								   };



        private void PdfPrint(string fileName)
        {
            // step 1: creation of a document-object
            Document document = new Document();
            try
            {
                // step 2:
                // we create a writer that listens to the document
                // and directs a PDF-stream to a file
                PdfWriter.getInstance(document, new FileStream(fileName, FileMode.Create));
                // step 3: we open the document
                document.Open();
                BaseFont bfHei = BaseFont.createFont(@"c:\windows\fonts\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bfHei, 12);
                // step 4: we create a table and add it to the document
                Table table = new Table(7);
                table.BorderWidth = 1;
                table.BorderColor = new iTextSharp.text.Color(0, 0, 255);
                table.Padding = 5;
                table.Spacing = 5;
                Cell cell = new Cell(new Paragraph("订单系统",font));
                cell.Header = true;
                cell.Colspan = 7;
                cell.HorizontalAlignment = 1;//居中对齐
                table.addCell(cell);

                cell = new Cell(new Paragraph("基本信息",font));
                cell.BackgroundColor = new iTextSharp.text.Color(0xC0, 0xC0, 0xC0);
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 2;
                cell.Rowspan = 4;
                cell.BorderColor = new iTextSharp.text.Color(255, 0, 0);
                table.addCell(cell);

                table.addCell(new Paragraph("生产单号", font));
                table.addCell(m_order.m_orderID);
                table.addCell(new Paragraph("浴比", font));
                table.addCell(m_order.m_liquorRatio1.ToString());
                table.addCell(new Paragraph("纱支", font));
                table.addCell(m_order.m_yarnCount.ToString());
                table.addCell(new Paragraph("染机代码", font));
                table.addCell(m_order.m_machineCode);
                table.addCell(new Paragraph("色号", font));
                table.addCell(m_order.m_colorNumber);
                table.addCell(new Paragraph("筒子个数", font));
                table.addCell(m_order.m_creelCount.ToString());
                table.addCell(new Paragraph("纱重", font));
                table.addCell(m_order.m_yarnWeight.ToString());
                table.addCell(new Paragraph("浴比", font));
                table.addCell(m_order.m_liquorRatio2.ToString());
                table.addCell(new Paragraph("水位", font));
                table.addCell(m_order.m_waterWight.ToString());
                table.addCell(new Paragraph("配方代码", font));
                table.addCell(m_order.m_prescriptionCode);
                table.addCell(new Paragraph("花型", font));
                table.addCell(m_order.m_pattern);
                table.addCell(new Paragraph("存档时间", font));
                table.addCell(m_order.m_storeTime);
                //table.addCell(" ");
                //table.addCell(" ");

                cell = new Cell(new Paragraph("工艺步骤", font));
                cell.Colspan = 7;
                cell.HorizontalAlignment = 1;//居中对齐
                table.addCell(cell);

                for (int i = 0; i < 7; i++ )
                {
                    int index = i+1;
                    cell = new Cell(new Paragraph("步骤"+index.ToString(), font));
                    cell.HorizontalAlignment = 1;//居中对齐
                    table.addCell(cell);
                }
                for (int i = 0; i < 7; i++)
                {
                    int index = i + 1;
                    if (index >= m_order.m_stepCount)
                    {
                        cell = new Cell(" ");
                    }
                    else
                    {
                        cell = new Cell(m_order.m_stepCodes[i]);
                    }
                    
                    cell.HorizontalAlignment = 1;//居中对齐
                    table.addCell(cell);
                }
                for (int i = 0; i < 7; i++)
                {
                    int index = i + 8;
                    cell = new Cell(new Paragraph("步骤" + index.ToString(), font));
                    cell.HorizontalAlignment = 1;//居中对齐
                    table.addCell(cell);
                }
                for (int i = 0; i < 7; i++)
                {
                    int index = i + 8;
                    if (index >= m_order.m_stepCount)
                    {
                        cell = new Cell(" ");
                    }
                    else
                    {
                        cell = new Cell(m_order.m_stepCodes[i]);
                    }
                    cell.HorizontalAlignment = 1;//居中对齐
                    table.addCell(cell);
                }


                cell = new Cell(new Paragraph("详细配方", font));
                cell.HorizontalAlignment = 1;//居中对齐
                cell.Rowspan = m_order.m_prescriptionCount+1;
                cell.Colspan = 2;
                cell.BackgroundColor = new iTextSharp.text.Color(0xC0, 0xC0, 0xC0);
                table.addCell(cell);

                cell = new Cell(new Paragraph("染助剂代号", font));
                table.addCell(cell);
                cell = new Cell(new Paragraph("让助剂名称", font));
                table.addCell(cell);
                cell = new Cell(new Paragraph("阀号", font));
                table.addCell(cell);
                cell = new Cell(new Paragraph("浓度", font));
                table.addCell(cell);
                cell = new Cell(new Paragraph("重量", font));
                table.addCell(cell);

                for (int i = 0; i < m_order.m_prescriptionCount; i++ )
                {
                    cell = new Cell(new Paragraph(m_order.m_dyeCodes[i], font));
                    table.addCell(cell);
                    cell = new Cell(new Paragraph(m_order.m_dyeNames[i], font));
                    table.addCell(cell);
                    cell = new Cell(new Paragraph(m_order.m_valve[i].ToString(), font));
                    table.addCell(cell);
                    cell = new Cell(new Paragraph(m_order.m_density[i].ToString(), font));
                    table.addCell(cell);
                    cell = new Cell(new Paragraph(m_order.m_weight[i].ToString(), font));
                    table.addCell(cell);
                }

                cell = new Cell(new Paragraph("订单日期", font));
                cell.Colspan = 3;
                table.addCell(cell);
                cell = new Cell(new Paragraph(m_order.m_storeTime, font));
                cell.Colspan = 4;
                table.addCell(cell);

                document.Add(table);
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }
            // step 5: we close the document
            document.Close();
	}


        // The PrintPage event is raised for each page to be printed.   
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = null;

            // Calculate the number of lines per page.   
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Iterate over the file, printing each line.   
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, System.Drawing.Brushes.Black,
                   leftMargin, yPos, new System.Drawing.StringFormat());
                count++;
            }

            // If more lines exist, print another page.   
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }   

        // Print the file.   
        public void Printing(string filePath)
        {
            try
            {
                streamToPrint = new StreamReader(filePath);
                try
                {
                    printFont = new System.Drawing.Font("Arial", 10);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintController = new StandardPrintController();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    // Print the document.   
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }   

        public override void PrintOut()
        {
            PdfPrint("iii.pdf");
            //return;

            string totalText = "订单号: " + m_order.m_orderID + "\n"
                + "浴比: " + m_order.m_liquorRatio1 + "\n"
                + "纱支: " + m_order.m_yarnCount + "\n"
                + "染机代码: " + m_order.m_machineCode + "\n"
                + "色号: " + m_order.m_colorNumber + "\n"
                + "筒子个数: " + m_order.m_creelCount + "\n"
                + "纱重: " + m_order.m_yarnWeight + "\n"
                + "浴比: " + m_order.m_liquorRatio2 + "\n"
                + "水位: " + m_order.m_waterWight + "\n"
                + "步骤数: " + m_order.m_stepCount + "\n"
                ;

            string stepText = "";
            for (int i = 0; i < m_order.m_stepCount; i++)
            {
                int index = i + 1;
                stepText += "步骤" + index.ToString() + ": " + m_order.m_stepCodes[i] + "\n";
            }

            string textPrescription = "配方代码: " + m_order.m_prescriptionCode + "\n"
                + "花型: " + m_order.m_pattern + "\n"
                + "存档时间: " + m_order.m_storeTime + "\n"
                ;

            string textDetails = "";
            for (int i = 0; i < m_order.m_prescriptionCount; i++)
            {
                int weight = (int)(m_order.m_density[i] * m_order.m_yarnWeight * m_order.m_liquorRatio2);
                textDetails += "染助剂代码: " + m_order.m_dyeCodes[i] + "   "
                    + "浓度: " + m_order.m_density[i] + "   "
                    + "重量: " + weight.ToString() + "   "
                    + "阀号: " + m_order.m_valve[i] + "   "
                    + "染助剂名称: " + m_order.m_dyeNames[i]
                    + "\n"
                    ;
            }

            string text = totalText + stepText + textPrescription + textDetails;


            //Document document = new Document();
            //PdfWriter.getInstance(document, new FileStream("xxx.pdf", FileMode.Create));
            //document.Open();
            //BaseFont bfHei = BaseFont.createFont(@"c:\windows\fonts\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //Font font = new Font(bfHei, 12);
            //document.Add(new Paragraph(text,font));
            //document.Close();


            File.WriteAllText("xxxx.txt", text);


            //PrintDialog pdlg = new PrintDialog();
            //PrintDocument doc = new PrintDocument();
            //doc.PrintController = new StandardPrintController();
            //doc.PrintPage += new PrintPageEventHandler(docToPrint_PrintPage);
            //pdlg.AllowSomePages = true;
            //pdlg.ShowHelp = true;
            //pdlg.Document = doc;
            //doc.Print();

            Printing("xxxx.txt");
        }

        void docToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            
        }

        
    }
}
