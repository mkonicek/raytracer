using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lucid.Raytracing
{
    /// <summary>
    /// Gets words one by one from Stream, skipping whitespace.
    /// </summary>
    public class StreamTokenizer
    {
        private StreamReader reader;
        private StringBuilder sBuilder = new StringBuilder(64);

        private IFormatProvider usNumberFormat = System.Globalization.CultureInfo.GetCultureInfo("en-US").NumberFormat;

        private string curWord;
        /// <summary>
        /// Gets last word read.
        /// </summary>
        public string Word
        {
            get
            {
                return curWord;
            }
        }

        private int bracketLevel;
         /// <summary>
        /// Current nesting level of brackets.
        /// </summary>
        public int BracketLevel
        {
            get
            {
                return bracketLevel;
            }
        }

        /// <summary>
        /// Indicates whether end of underlying stream was reached.
        /// </summary>
        public bool EndOfStream
        {
            get
            {
                return reader.EndOfStream;
            }
        }

        public StreamTokenizer(Stream s)
            : this(s, Encoding.ASCII)
        {
        }

        public StreamTokenizer(Stream s, Encoding e)
        {
            // performance is good even without buffered stream
            reader = new StreamReader(new BufferedStream(s, 4 * 1024), e);
        }

        public StreamTokenizer(StreamReader r)
        {
            reader = r;
        }

        private bool isWhite(char c)
        {
            return c == ' ' || c == '\r' || c == '\n' || c == '\t' || c == ',' 
                || c == ')' || c == '(' || c == '[' || c== ']' || c == '{' || c == '}';
        }

        public void NextWord()
        {
            // clear the builder
            sBuilder.Length = 0;

            char c;
            // skip whitespace
            do
            {
                c = (char)reader.Read();
                updateBracketLevel(c);
                // EOF ends the loop by returning char '.'
            } while (isWhite(c));

            if (EndOfStream)
            {
                this.curWord = string.Empty;
                return;
            }

            while (!isWhite(c) && !EndOfStream)
            {
                sBuilder.Append(c);
                c = (char)reader.Read();
                updateBracketLevel(c);
            }

             this.curWord = sBuilder.ToString();
        }

        private void updateBracketLevel(char c)
        {
            if (c == '(' || c == '[' || c == '{')
                this.bracketLevel++;
            if (c == ')' || c == ']' || c == '}')
                this.bracketLevel--;
        }

        public double ReadDouble()
        {
            this.NextWord();
            return Double.Parse(this.Word, usNumberFormat);
        }

        public bool TryReadDouble(out double d)
        {
            this.NextWord();
            return double.TryParse(this.Word, System.Globalization.NumberStyles.Any, usNumberFormat, out d);
        }
    }
}
