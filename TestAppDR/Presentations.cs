using System.Text;
using Spire.Presentation;
using System;
using Spire.Presentation.Diagrams;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TestAppDR
{
    public class Presentations
    {
        public void ModFiles(string filePath, string fontName, int fontSize)
        {
            Presentation ppt = new Presentation();
            ppt.LoadFromFile(filePath);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ppt.Slides.Count; i++)
            {
                for (int j = 0; j < ppt.Slides[i].Shapes.Count; j++)
                {
                    IAutoShape shape = ppt.Slides[i].Shapes[j] as IAutoShape;
                    if (ppt.Slides[i].Shapes[j] is ISmartArt)
                    {
                        ISmartArt smartArt = ppt.Slides[i].Shapes[j] as ISmartArt;
                        for (int k = 0; k < smartArt.Nodes.Count; k++)
                        {
                            //get text 3, text 4, text 5
                            foreach (TextParagraph tp in smartArt.Nodes[k].TextFrame.Paragraphs)
                            {
                                //set font and size
                                foreach (TextRange tr in tp.TextRanges)
                                {
                                    tr.LatinFont = new TextFont(fontName);
                                    tr.FontHeight = fontSize;

                                }

                            }
                            var nodeText = smartArt.Nodes[k].TextFrame.Text;
                            sb.Append(nodeText);
                        }
                    }
                    else
                    {
                        if (ppt.Slides[i].Shapes[j] is IAutoShape && (ppt.Slides[i].Shapes[j] as IAutoShape).TextFrame != null)
                        {
                            //get text 1, text 2, text 3
                            foreach (TextParagraph tp in shape.TextFrame.Paragraphs)
                            {
                                //set font and size
                                foreach (TextRange tr in tp.TextRanges)
                                {
                                    tr.LatinFont = new TextFont(fontName);
                                    tr.FontHeight = fontSize;

                                }
                                sb.Append(tp.Text + Environment.NewLine);
                            }
                        }
                    }
                }
            }
            ppt.SaveToFile(filePath, FileFormat.Pptx2010);
        }
    }
}
