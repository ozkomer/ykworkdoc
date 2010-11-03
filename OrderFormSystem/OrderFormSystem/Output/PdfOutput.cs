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


        public static String[] uni = new String[27];
        public static String[] code = new String[27];
        public static String[] name = new String[27];


        /// <summary>
        /// -----------------------------测试
        /// </summary>
        private void printTest()
        {
            // step 1: creation of a document-object
		Document document = new Document();
        
		try 
		{
            
			// step 2:
			// we create a writer that listens to the document
			// and directs a PDF-stream to a file
			PdfWriter writer = PdfWriter.getInstance(document, new FileStream("Chap1007.pdf", FileMode.Create));
            
			// step 3: we open the document
			document.Open();
            
			// step 4:
			//BaseFont bf = BaseFont.createFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);
            //BaseFont bfHei = BaseFont.createFont(@"c:\windows\fonts\STSONG.TTF", iTextSharp.text.pdf.BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //iTextSharp.text.Font font = new iTextSharp.text.Font(bfHei, 12);
            iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.createFont(@"c:\windows\fonts\STSONG.TTF", iTextSharp.text.pdf.BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 12);
            Phrase ps_bold = new Phrase("中文单粗体测试", font);

            // we grab the ContentByte and do some stuff with it
			PdfContentByte cb = writer.DirectContent;
			ColumnText ct = new ColumnText(cb);
            

            //cb.setFontAndSize(bf,12);
            //float[] right = { 70, 320 };
            //float[] left = { 300, 550 };
            float[] right = { 40, 220,400 };
            float[] left = { 200, 380,560 };

            ct.addText(ps_bold);
            ct.addText(ps_bold);
            //ct.addText(new Phrase("哈哈I use to rule the world seas rise when I gvie a word now in the morning I sweep alone seeep the street I use to own",font));
			//ct.addText(new Phrase("throug early moring frog I see, version of the things to be , that ther paint are withhold for me, I realize I can see, suicide is painless, it bring many changes, you can do the samething if you believe\n", FontFactory.getFont(FontFactory.HELVETICA, 12)));
			//ct.addText(new Phrase("Apud Helvetios longe nobilissimus fuit et ditissimus Orgetorix.  Is M. Messala, [et P.] M.  Pisone consulibus regni cupiditate inductus coniurationem nobilitatis fecit et civitati persuasit ut de finibus suis cum omnibus copiis exirent:  perfacile esse, cum virtute omnibus praestarent, totius Galliae imperio potiri.  Id hoc facilius iis persuasit, quod undique loci natura Helvetii continentur:  una ex parte flumine Rheno latissimo atque altissimo, qui agrum Helvetium a Germanis dividit; altera ex parte monte Iura altissimo, qui est inter Sequanos et Helvetios; tertia lacu Lemanno et flumine Rhodano, qui provinciam nostram ab Helvetiis dividit.  His rebus fiebat ut et minus late vagarentur et minus facile finitimis bellum inferre possent; qua ex parte homines bellandi cupidi magno dolore adficiebantur.  Pro multitudine autem hominum et pro gloria belli atque fortitudinis angustos se fines habere arbitrabantur, qui in longitudinem milia passuum CCXL, in latitudinem CLXXX patebant.\n", FontFactory.getFont(FontFactory.HELVETICA, 12)));
			//ct.addText(new Phrase("His rebus adducti et auctoritate Orgetorigis permoti constituerunt ea quae ad proficiscendum pertinerent comparare, iumentorum et carrorum quam maximum numerum coemere, sementes quam maximas facere, ut in itinere copia frumenti suppeteret, cum proximis civitatibus pacem et amicitiam confirmare.  Ad eas res conficiendas biennium sibi satis esse duxerunt; in tertium annum profectionem lege confirmant.  Ad eas res conficiendas Orgetorix deligitur.  Is sibi legationem ad civitates suscipit.  In eo itinere persuadet Castico, Catamantaloedis filio, Sequano, cuius pater regnum in Sequanis multos annos obtinuerat et a senatu populi Romani amicus appellatus erat, ut regnum in civitate sua occuparet, quod pater ante habuerit; itemque Dumnorigi Haeduo, fratri Diviciaci, qui eo tempore principatum in civitate obtinebat ac maxime plebi acceptus erat, ut idem conaretur persuadet eique filiam suam in matrimonium dat.  Perfacile factu esse illis probat conata perficere, propterea quod ipse suae civitatis imperium obtenturus esset:  non esse dubium quin totius Galliae plurimum Helvetii possent; se suis copiis suoque exercitu illis regna conciliaturum confirmat.  Hac oratione adducti inter se fidem et ius iurandum dant et regno occupato per tres potentissimos ac firmissimos populos totius Galliae sese potiri posse sperant.\n", FontFactory.getFont(FontFactory.HELVETICA, 12)));
			//ct.addText(new Phrase("Ea res est Helvetiis per indicium enuntiata.  Moribus suis Orgetoricem ex vinculis causam dicere coegerunt; damnatum poenam sequi oportebat, ut igni cremaretur.  Die constituta causae dictionis Orgetorix ad iudicium omnem suam familiam, ad hominum milia decem, undique coegit, et omnes clientes obaeratosque suos, quorum magnum numerum habebat, eodem conduxit; per eos ne causam diceret se eripuit.  Cum civitas ob eam rem incitata armis ius suum exequi conaretur multitudinemque hominum ex agris magistratus cogerent, Orgetorix mortuus est; neque abest suspicio, ut Helvetii arbitrantur, quin ipse sibi mortem consciverit.", FontFactory.getFont(FontFactory.HELVETICA, 12)));
			ct.Indent = 20;

            //String text = "这是黑体字测试！";
            //document.Add(new Paragraph(text, font));


            ct.setSimpleColumn(40, 60, 200, 790, 16, Element.ALIGN_JUSTIFIED);
            ct.go();

            ct = new ColumnText(cb);
            ct.addText(ps_bold);
            //ct.addText(new Phrase("哈I use to rule the world seas rise when I gvie a word now in the morning I sweep alone seeep the street I use to own.\n", font));// FontFactory.getFont(FontFactory.HELVETICA, 12)));
            ct.setSimpleColumn(220, 60, 380, 790, 16, Element.ALIGN_JUSTIFIED);
            ct.go();

            //while((status & ColumnText.NO_MORE_TEXT) == 0) 
            //{
            //    Console.WriteLine("page " + writer.PageNumber + " column " + column);
            //    ct.setSimpleColumn(right[column], 60, left[column], 790, 16, Element.ALIGN_JUSTIFIED);
            //    status = ct.go();
            //    if ((status & ColumnText.NO_MORE_COLUMN) != 0) 
            //    {
            //        column++;
            //        if (column > 2) 
            //        {
            //            document.newPage();
            //            column = 0;
            //        }
            //    }
            //}
		}
		catch(DocumentException de) 
		{
			Console.Error.WriteLine(de.Message);
		}
		catch(IOException ioe) 
		{
			Console.Error.WriteLine(ioe.Message);
		}
        
		// step 5: we close the document
		document.Close();

        }




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
            //PrintOutTable();
            PrintOutSheet("ooo.pdf");
            //printTest();
        }

        private void PrintOutSheet(string fileName)
        {
            Document document = new Document();
            try
            {
                // step 2:
                // we create a writer that listens to the document
                // and directs a PDF-stream to a file
                PdfWriter writer = PdfWriter.getInstance(document, new FileStream(fileName, FileMode.Create));
                
                // step 3: we open the document
                document.Open();

                
                #region 创建中文字体
                BaseFont bfSong= BaseFont.createFont(@"c:\windows\fonts\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontT = new iTextSharp.text.Font(bfSong, 9);
                BaseFont bfConpany = BaseFont.createFont(@"c:\windows\fonts\msyhbd.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontM = new iTextSharp.text.Font(bfConpany, 13);
                BaseFont bfName = BaseFont.createFont(@"c:\windows\fonts\msyhbd.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontN = new iTextSharp.text.Font(bfName, 12);
                BaseFont bfTime = BaseFont.createFont(@"c:\windows\fonts\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontTM = new iTextSharp.text.Font(bfTime, 8);
                BaseFont bfYH = BaseFont.createFont(@"c:\windows\fonts\msyhbd.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontC = new iTextSharp.text.Font(bfYH, 10);
                #endregion

                //布局
                // we grab the ContentByte and do some stuff with it
                PdfContentByte cb = writer.DirectContent;
                ColumnText ct = new ColumnText(cb);
                float[] left = { 40, 220, 400 };
                float[] right = { 200, 380, 560 };

                #region 字符串资源
                Phrase companyTitle = new Phrase("浙江省威达机械有限公司", fontM);
                Phrase title = new Phrase("染色用料处方", fontN);

                Phrase orderIDTitle = new Phrase("订单号:", fontT);
                Phrase ratio1Title = new Phrase("浴比:", fontT);
                Phrase ratio2Title = new Phrase("浴比:", fontT);
                Phrase yarnCountTitle = new Phrase("纱支:", fontT);
                Phrase machineCodeTitle = new Phrase("染机代码:", fontT);
                Phrase colorNumberTitle = new Phrase("色号:", fontT);
                Phrase creelCountTitle = new Phrase("筒子个数:", fontT);
                Phrase yarnWeightTitle = new Phrase("纱重(公斤):", fontT);
                Phrase waterWeightTitle = new Phrase("水位(公斤):", fontT);
                Phrase prescriptionTitle = new Phrase("配方代码:", fontT);
                Phrase patternTitle = new Phrase("花型:", fontT);
                Phrase storeTimeTitle = new Phrase("存档时间:", fontT);

                Phrase orderIDContent= new Phrase(m_order.m_orderID, fontC);
                Phrase ratio1Content= new Phrase(m_order.m_liquorRatio1.ToString(), fontC);
                Phrase ratio2Content = new Phrase(m_order.m_liquorRatio2.ToString(), fontC);
                Phrase yarnCountContent = new Phrase(m_order.m_yarnCount.ToString(), fontC);
                Phrase machineCodeContent = new Phrase(m_order.m_machineCode, fontC);
                Phrase colorNumberContent = new Phrase(m_order.m_colorNumber, fontC);
                Phrase creelCountContent = new Phrase(m_order.m_creelCount.ToString(), fontC);
                Phrase yarnWeightContent = new Phrase(m_order.m_yarnWeight.ToString(), fontC);
                Phrase waterWeightContent = new Phrase(m_order.m_waterWight.ToString(), fontC);
                Phrase prescriptionContent = new Phrase(m_order.m_prescriptionCode, fontC);
                Phrase patternContent = new Phrase(m_order.m_pattern, fontC);
                Phrase storeTimeContent = new Phrase(m_order.m_storeTime, fontTM);

                //Phrase stepCount = new Phrase(m_order.m_stepCodes[0], fontC);
                #endregion

                #region 第一栏
                ct.addText(companyTitle);
                ct.addText(new Phrase("\n"));
                ct.addText(new Phrase("\n"));
                ct.addText(orderIDTitle);
                ct.addText(orderIDContent);
                ct.addText(new Phrase("\n"));
                ct.addText(prescriptionTitle);
                ct.addText(prescriptionContent);
                ct.addText(new Phrase("\n"));
                ct.addText(patternTitle);
                ct.addText(patternContent);
                //ct.Indent = 10;
                ct.setSimpleColumn(left[0], 60, right[0], 790, 16, Element.ALIGN_JUSTIFIED);
                ct.go();
                #endregion

                #region 第二栏
                ct.addText(title);
                ct.addText(new Phrase("\n"));
                ct.addText(new Phrase("\n"));
                ct.addText(machineCodeTitle);
                ct.addText(machineCodeContent);
                ct.addText(new Phrase("\n"));
                ct.addText(creelCountTitle);
                ct.addText(creelCountContent);
                ct.addText(new Phrase("\n"));
                ct.addText(storeTimeTitle);
                ct.addText(storeTimeContent);
                //ct.Indent = 10;
                ct.setSimpleColumn(left[1], 60, right[1], 790, 16, Element.ALIGN_JUSTIFIED);
                ct.go();
                #endregion

                #region 第三栏
                ct.addText(yarnCountTitle);
                ct.addText(yarnCountContent);
                ct.addText(new Phrase("\n"));
                ct.addText(yarnWeightTitle);
                ct.addText(yarnWeightContent);
                ct.addText(new Phrase("\n"));
                ct.addText(waterWeightTitle);
                ct.addText(waterWeightContent);
                ct.addText(new Phrase("\n"));
                ct.addText(colorNumberTitle);
                ct.addText(colorNumberContent);
                ct.addText(new Phrase("\n"));
                ct.addText(ratio1Title);
                ct.addText(ratio1Content);
                //ct.Indent = 10;
                ct.setSimpleColumn(left[2], 60, right[2], 790, 16, Element.ALIGN_JUSTIFIED);
                ct.go();
                #endregion

                #region 步骤
                Phrase phsStepTitle = new Phrase("工艺流程", fontT);
                ct.addText(phsStepTitle);
                ct.setSimpleColumn(40, 40, 560, 690, 8, Element.ALIGN_LEFT);
                ct.go();

                Phrase phsStepNo = new Phrase("步骤:", fontT);
                Phrase phsStepCode = new Phrase("工艺号:", fontT);
                ct.addText(phsStepNo);
                ct.addText(new Phrase("\n"));
                ct.addText(new Phrase("\n"));
                ct.addText(new Phrase("\n"));
                ct.addText(phsStepCode);
                ct.setSimpleColumn(40, 40, 70, 660, 8, Element.ALIGN_CENTER);
                ct.go();
                int N = m_order.m_stepCount;
                for (int i = 0; i < N; i++)
                {
                    Phrase phsNum = new Phrase((i+1).ToString(),fontT);
                    ct.addText(phsNum);
                    ct.addText(new Phrase("\n"));
                    ct.addText(new Phrase("\n"));
                    ct.addText(new Phrase("\n"));
                    ct.addText(new Phrase(m_order.m_stepCodes[i]));
                    float scLeft = 70f + (490f / N) * i + 2.5f;
                    float scRight = scLeft + (490f / N) - 2.5f;
                    ct.setSimpleColumn(scLeft, 40, scRight, 660, 8, Element.ALIGN_CENTER);
                    ct.go();
                }
                #endregion


                #region 线条
                //步骤部分
                DrawDoubleLine(document, 40, 560, 670, 5);
                DrawDoubleLine(document, 40, 560, 610, 5);

                //配方部分
                DrawDoubleLine(document, 40, 560, 540, 5);
                DrawDoubleLine(document, 40, 560, 540 - 30 - m_order.m_prescriptionCount * 25, 5);

                #endregion

                #region 配方详细
                Phrase phsPrescriptionTitle = new Phrase("详细配方:", fontT);
                ct.addText(phsPrescriptionTitle);
                ct.setSimpleColumn(40, 40, 560, 560, 8, Element.ALIGN_LEFT);
                ct.go();

                Phrase dyeNameTitle = new Phrase("染助剂名称",fontT);
                Phrase dyeCodeTitle = new Phrase("染助剂代码", fontT);
                Phrase densityTitle  = new Phrase("浓度(g/L)", fontT);
                Phrase weightTitle = new Phrase("重量(g)", fontT);
                Phrase valveTitle = new Phrase("阀号", fontT);
                Phrase[] titles = { dyeNameTitle, densityTitle, weightTitle, dyeCodeTitle, valveTitle };
                for (int i = 0; i < 5; i++)
                {
                    ct.addText(titles[i]);
                    ct.addText(new Phrase("\n"));
                    ct.addText(new Phrase("\n"));
                    ct.addText(new Phrase("\n"));
                    for (int j = 0; j < m_order.m_prescriptionCount; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                ct.addText(new Phrase(m_order.m_dyeNames[j], fontC));
                                break;
                            case 1:
                                ct.addText(new Phrase(m_order.m_density[j].ToString(), fontC));
                                break;
                            case 2:
                                ct.addText(new Phrase(m_order.m_weight[j].ToString(), fontC));
                                break;
                            case 3:
                                ct.addText(new Phrase(m_order.m_dyeCodes[j], fontC));
                                break;
                            case 4:
                                ct.addText(new Phrase(m_order.m_valve[j].ToString(), fontC));
                                break;
                            default:
                                break;
                        }
                        ct.addText(new Phrase("\n"));
                        ct.addText(new Phrase("\n"));
                        ct.addText(new Phrase("\n"));
                    }
                    float scLeft = 40f + (520f / 5) * i + 2.5f;
                    float scRight = scLeft + (520f / 5) - 2.5f;
                    ct.setSimpleColumn(scLeft, 40, scRight, 530, 8, Element.ALIGN_CENTER);
                    ct.go();
                }
                #endregion


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

        private void DrawDoubleLine(Document doc, float l, float r, float h, float w)
        {
            Graphic grx = new Graphic();
            grx.moveTo(l, h);
            grx.lineTo(r, h);
            grx.moveTo(l, h + w);
            grx.lineTo(r, h + w);
            grx.stroke();
            doc.Add(grx);
        }

        private void PrintOutTable()
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
    }
}
