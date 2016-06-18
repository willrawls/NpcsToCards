using MetX.Library;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace NpcsToCards
{
    public class NpcPrinter : PrintDocument
    {
        public int CardHeight;
        public int[] CardTops;
        public string FontName;
        public int FontSize = 8;
        public int NextCardIndex;
        public List<Npc> Npcs;

        public int PageCount;
        public RectangleF[] PrintAreas;

        /// <summary>
        ///     Property to hold the font the users wishes to use
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Font PrinterFont;

        public Font PrinterFontBold;
        public Font PrinterFontBoldItalics;
        public Font PrinterFontItalics;
        public int PrintHeight;
        public int PrintWidth;

        /// <summary>
        ///     Empty constructor
        /// </summary>
        /// <remarks></remarks>
        public NpcPrinter(List<Npc> npcs) : base()
        {
            Npcs = npcs;
            NextCardIndex = 0;
        }

        /// <summary>
        ///     Empty constructor
        /// </summary>
        /// <remarks></remarks>
        public NpcPrinter() : base()
        {
            Npcs = new List<Npc>();
            NextCardIndex = 0;
            FontName = "Calibri";
        }

        /// <summary>
        ///     Override the default OnBeginPrint method of the PrintDocument Object
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected override void OnBeginPrint(PrintEventArgs e)
        {
            // Run base code
            base.OnBeginPrint(e);

            if (FontName.IsEmpty())
            {
                FontName = "Calibri";
            }

            if (PrinterFont == null)
            {
                //Create the font we need
                PrinterFont = new Font(FontName, FontSize);
                PrinterFontBold = new Font(FontName, FontSize, FontStyle.Bold);
                PrinterFontItalics = new Font(FontName, FontSize, FontStyle.Italic);
                PrinterFontBoldItalics = new Font(FontName, FontSize, FontStyle.Bold | FontStyle.Italic);
            }

            var leftRightGutterWidth = (850 - 500)/2;
            var topBottomGutterWidth = (1100 - (300*3))/2;
            DefaultPageSettings.Margins = new Margins(leftRightGutterWidth, leftRightGutterWidth, topBottomGutterWidth,
                topBottomGutterWidth);

            //Set print area size and margins
            PrintHeight = DefaultPageSettings.PaperSize.Height
                          - DefaultPageSettings.Margins.Top
                          - DefaultPageSettings.Margins.Bottom;

            PrintWidth = DefaultPageSettings.PaperSize.Width
                         - DefaultPageSettings.Margins.Left
                         - DefaultPageSettings.Margins.Right;

            CardHeight = PrintHeight/3;

            CardTops = new[]
            {
                DefaultPageSettings.Margins.Top,
                DefaultPageSettings.Margins.Top + CardHeight,
                DefaultPageSettings.Margins.Top + CardHeight*2
            };

            //Create a rectangle printing are for our document
            PrintAreas = new[]
            {
                new RectangleF(DefaultPageSettings.Margins.Left, CardTops[0], PrintWidth, CardHeight),
                new RectangleF(DefaultPageSettings.Margins.Left, CardTops[1], PrintWidth, CardHeight),
                new RectangleF(DefaultPageSettings.Margins.Left, CardTops[2], PrintWidth, CardHeight),
            };
            PageCount = 0;
        }

        /// <summary>
        ///     Override the default OnPrintPage method of the PrintDocument
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>This provides the print logic for our document</remarks>
        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            // Run base code
            base.OnPrintPage(e);

            for (var area = 0; area < 3; area++)
            {
                if (NextCardIndex >= Npcs.Count)
                {
                    e.HasMorePages = false;
                    break;
                }
                Npcs[NextCardIndex].Render(this, e.Graphics, PrintAreas[area]);
                NextCardIndex++;
            }
            e.HasMorePages = ++PageCount < 3;
        }
    }
}