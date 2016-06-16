using MetX.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace NpcsToCards
{
    [Serializable]
    public class Npc
    {
        //Use the StringFormat class for the text layout of our document
        private readonly StringFormat _format = new StringFormat(StringFormatFlags.LineLimit);

        [XmlAttribute]
        public string Appearance;

        [XmlAttribute]
        public string Background;

        [XmlAttribute]
        public string Motivation;

        [XmlAttribute]
        public int NpcId;

        [XmlAttribute]
        public string NpcName;

        [XmlAttribute]
        public string Personality;

        [XmlAttribute]
        public string QuoteFromNpc;

        [XmlAttribute]
        public string Roleplaying;

        [XmlAttribute]
        public string Title;

        public List<string> Traits;

        public Npc()
        {
        }

        public Npc(int npcId, string npcName, string title, string quoteFromNpc, string appearance, string roleplaying,
            string personality, string motivation, string background, string traits)
        {
            NpcId = npcId;
            NpcName = npcName;
            Title = title;
            QuoteFromNpc = quoteFromNpc;
            Appearance = appearance;
            Roleplaying = roleplaying;
            Personality = personality;
            Motivation = motivation;
            Background = background;
            Traits = ToTraits(traits);

            if (QuoteFromNpc.IsNotEmpty())
            {
                QuoteFromNpc = QuoteFromNpc.Replace("“", string.Empty).Replace("”", string.Empty);
                if (QuoteFromNpc.StartsWith("\""))
                    QuoteFromNpc = QuoteFromNpc.Substring(1);
                if (QuoteFromNpc.EndsWith("\""))
                    QuoteFromNpc = QuoteFromNpc.Substring(0, QuoteFromNpc.Length - 1);
            }
            Appearance = Appearance.TokensAfterFirst(": ");
            Roleplaying = Roleplaying.TokensAfterFirst(": ");
            Personality = Personality.TokensAfterFirst(": ");
            Motivation = Motivation.TokensAfterFirst(": ");
            Background = Background.TokensAfterFirst(": ");
        }

        public string RenderableTraits
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var trait in Traits)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(trait);
                }
                return sb.ToString();
            }
        }

        public string ToXml()
        {
            return Xml.ToXml(this, true);
        }

        public static Npc FromXml(string xml)
        {
            return Xml.FromXml<Npc>(xml);
        }

        private List<string> ToTraits(string line)
        {
            line = line.TokensAfterFirst(": ");
            var ret = new List<string> { line.FirstToken(" ") };
            ret.AddRange(line.TokensAfterFirst(" ").Split(','));
            for (var i = 0; i < ret.Count; i++)
            {
                ret[i] = ret[i].Trim();
            }
            return ret;
        }

        /// <summary>
        ///     Function to replace any zeros in the size to a 1
        ///     Zero's will mess up the printing area
        /// </summary>
        /// <param name= value>Value to check</param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int RemoveZeros(int value)
        {
            return value == 0 ? 1 : value;
        }

        public void Render(NpcPrinter document, Graphics graphics, RectangleF area)
        {
            /*
            [NpcId] (+6 point) [Name]  +6 point Bold
            [Title]                    +4 point
            [QuoteFromNpc]             +2 point Italics
            [thin line]
            Appearance: (bold) [Appearance]
            Roleplaying: (bold) [Roleplaying]
            Personality: (bold) [Personality]
            Motivation: (bold) [Motivation]
            Background: (bold) [Background]
            Traits: (bold) [Trait list]
            */

            var newArea = Render(document.PrinterFontBold, graphics, NpcId.ToString(), area, 6);
            if (newArea.Height <= 0) return;

            newArea = Render(document.PrinterFont, graphics, Title, newArea, 4);
            if (newArea.Height <= 0) return;

            newArea = Render(document.PrinterFontItalics, graphics, QuoteFromNpc, newArea, 2);
            if (newArea.Height <= 0) return;

            newArea = Render(document, document.PrinterFont, graphics, "Appearance", Appearance, newArea);
            if (newArea.Height <= 0) return;

            newArea = Render(document, document.PrinterFont, graphics, "Roleplaying", Roleplaying, newArea);
            if (newArea.Height <= 0) return;

            newArea = Render(document, document.PrinterFont, graphics, "Personality", Personality, newArea);
            if (newArea.Height <= 0) return;

            newArea = Render(document, document.PrinterFont, graphics, "Motivation", Motivation, newArea);
            if (newArea.Height <= 0) return;

            newArea = Render(document, document.PrinterFont, graphics, "Background", Background, newArea);
            if (newArea.Height <= 0) return;

            Render(document, document.PrinterFont, graphics, "Traits", RenderableTraits, newArea);
        }

        private RectangleF Render(Font font, Graphics graphics, string text, RectangleF area, int relativeFontSize = 0)
        {
            if (text.IsEmpty())
            {
                return area;
            }

            var f = relativeFontSize == 0 ? font : new Font(font.FontFamily, font.Size + relativeFontSize);
            var textArea = graphics.MeasureString(text, f, area.Size, _format);
            graphics.DrawString(text, f, Brushes.Black, area, _format);
            if (area.Height - textArea.Height <= 0)
            {
                // No room left
                return RectangleF.Empty;
            }
            return new RectangleF(area.X, area.Y + textArea.Height, area.Width, area.Height - textArea.Height);
        }

        private RectangleF Render(NpcPrinter document, Font font, Graphics graphics, string label, string text,
            RectangleF area, int relativeFontSize = 0)
        {
            if (text.IsEmpty() || label.IsEmpty())
            {
                return area;
            }

            //SizeF labelArea = graphics.MeasureString(label, document.PrinterFontBold, area.Size, _format);
            graphics.DrawString(label, document.PrinterFontBold, Brushes.Black, area, _format);

            text = new string(' ', label.Length + 2) + text;
            var f = relativeFontSize == 0 ? font : new Font(font.FontFamily, font.Size + relativeFontSize);
            var textArea = graphics.MeasureString(text, f, area.Size, _format);
            graphics.DrawString(text, f, Brushes.Black, area, _format);

            if (area.Height - textArea.Height <= 0)
            {
                // No room left
                return RectangleF.Empty;
            }
            return new RectangleF(area.X, area.Y + textArea.Height, area.Width, area.Height - textArea.Height);
        }
    }
}