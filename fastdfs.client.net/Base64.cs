using System;
using System.Text;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class Base64
    {
        private String lineSeparator = "\n";

        private int lineLength = 72;

        private char[] valueToChar = new char[64];

        private int[] charToValue = new int[256];

        private int[] charToPad = new int[4];

        const int IGNORE = -1;

        const int PAD = -2;

        public Base64()
        {
            this.init('+', '/', '=');
        }

        /* constructor */
        public Base64(char chPlus, char chSplash, char chPad, int lineLength)
        {
            this.init(chPlus, chSplash, chPad);
            this.lineLength = lineLength;
        }

        public Base64(int lineLength)
        {
            this.lineLength = lineLength;
        }

        private void init(char chPlus, char chSplash, char chPad)
        {
            int index = 0;
            // build translate this.valueToChar table only once.
            // 0..25 -> 'A'..'Z'
            for (int i = 'A'; i <= 'Z'; i++)
            {
                this.valueToChar[index++] = (char)i;
            }

            // 26..51 -> 'a'..'z'
            for (int i = 'a'; i <= 'z'; i++)
            {
                this.valueToChar[index++] = (char)i;
            }

            // 52..61 -> '0'..'9'
            for (int i = '0'; i <= '9'; i++)
            {
                this.valueToChar[index++] = (char)i;
            }

            this.valueToChar[index++] = chPlus;
            this.valueToChar[index++] = chSplash;

            // build translate defaultCharToValue table only once.
            for (int i = 0; i < 256; i++)
            {
                this.charToValue[i] = IGNORE;  // default is to ignore
            }

            for (int i = 0; i < 64; i++)
            {
                this.charToValue[this.valueToChar[i]] = i;
            }

            this.charToValue[chPad] = PAD;
            for (int i = 0; i < this.charToPad.Length; i++)
                this.charToPad[i] = chPad;
        }
        /// <summary>
        /// Encode an arbitrary array of bytes as Base64 printable ASCII.
        /// It will be broken into lines of 72 chars each.  The last line is not
        /// terminated with a line separator.
        /// The output will always have an even multiple of data characters,
        ///  exclusive of \n.  It is padded out with =.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public String encode(byte[] b)
        {
            // Each group or partial group of 3 bytes becomes four chars
            // covered quotient
            int outputLength = ((b.Length + 2) / 3) * 4;

            // account for trailing newlines, on all but the very last line
            if (lineLength != 0)
            {
                int lines = (outputLength + lineLength - 1) / lineLength - 1;
                if (lines > 0)
                {
                    outputLength += lines * lineSeparator.Length;
                }
            }

            // must be local for recursion to work.
            StringBuilder sb = new StringBuilder(outputLength);

            // must be local for recursion to work.
            int linePos = 0;

            // first deal with even multiples of 3 bytes.
            int len = (b.Length / 3) * 3;
            int leftover = b.Length - len;
            for (int i = 0; i < len; i += 3)
            {
                // Start a new line if next 4 chars won't fit on the current line
                // We can't encapsulete the following code since the variable need to
                // be local to this incarnation of encode.
                linePos += 4;
                if (linePos > lineLength)
                {
                    if (lineLength != 0)
                    {
                        sb.Append(lineSeparator);
                    }
                    linePos = 4;
                }

                // get next three bytes in unsigned form lined up,
                // in big-endian order
                int combined = b[i + 0] & 0xff;
                combined <<= 8;
                combined |= b[i + 1] & 0xff;
                combined <<= 8;
                combined |= b[i + 2] & 0xff;

                // break those 24 bits into a 4 groups of 6 bits,
                // working LSB to MSB.
                int c3 = combined & 0x3f;
                //combined >>= 6;
                combined = rightMove(combined, 6);
                int c2 = combined & 0x3f;
                //combined >>= 6;
                combined = rightMove(combined, 6);
                int c1 = combined & 0x3f;
                //combined >>= 6;
                combined = rightMove(combined, 6);
                int c0 = combined & 0x3f;

                // Translate into the equivalent alpha character
                // emitting them in big-endian order.
                sb.Append(valueToChar[c0]);
                sb.Append(valueToChar[c1]);
                sb.Append(valueToChar[c2]);
                sb.Append(valueToChar[c3]);
            }

            // deal with leftover bytes
            switch (leftover)
            {
                case 0:
                default:
                    // nothing to do
                    break;

                case 1:
                    // One leftover byte generates xx==
                    // Start a new line if next 4 chars won't fit on the current line
                    linePos += 4;
                    if (linePos > lineLength)
                    {

                        if (lineLength != 0)
                        {
                            sb.Append(lineSeparator);
                        }
                        linePos = 4;
                    }

                    // Handle this recursively with a faked complete triple.
                    // Throw away last two chars and replace with ==
                    sb.Append(encode(new byte[] { b[len], 0, 0 }
                                    ).Substring(0, 2));
                    sb.Append("==");
                    break;

                case 2:
                    // Two leftover bytes generates xxx=
                    // Start a new line if next 4 chars won't fit on the current line
                    linePos += 4;
                    if (linePos > lineLength)
                    {
                        if (lineLength != 0)
                        {
                            sb.Append(lineSeparator);
                        }
                        linePos = 4;
                    }
                    // Handle this recursively with a faked complete triple.
                    // Throw away last char and replace with =
                    sb.Append(encode(new byte[] { b[len], b[len + 1], 0 }
                                    ).Substring(0, 3));
                    sb.Append("=");
                    break;

            } // end switch;

            if (outputLength != sb.Length)
            {
                Console.WriteLine("oops: minor program flaw: output length mis-estimated");
                Console.WriteLine("estimate:" + outputLength);
                Console.WriteLine("actual:" + sb.Length);
            }
            return sb.ToString();
        }
        /// <summary>
        /// decode a well-formed complete Base64 string back into an array of bytes.
        /// It must have an even multiple of 4 data characters (not counting \n),
        /// padded out with = as needed.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public byte[] decodeAuto(string s)
        {
            int nRemain = s.Length % 4;
            if (nRemain == 0)
            {
                return this.decode(s);
            }
            else
            {
                char[] value = new char[this.charToPad.Length];
                for (int i = 0; i < this.charToPad.Length; i++)
                    value[i] = (char)this.charToPad[i];
                return this.decode(s + new String(value, 0, 4 - nRemain));
            }
        }
        /// <summary>
        /// decode a well-formed complete Base64 string back into an array of bytes.
        /// It must have an even multiple of 4 data characters (not counting \n),
        /// padded out with = as needed.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public byte[] decode(string s)
        {

            // estimate worst case size of output array, no embedded newlines.
            byte[] b = new byte[(s.Length / 4) * 3];

            // tracks where we are in a cycle of 4 input chars.
            int cycle = 0;

            // where we combine 4 groups of 6 bits and take apart as 3 groups of 8.
            int combined = 0;

            // how many bytes we have prepared.
            int j = 0;
            // will be an even multiple of 4 chars, plus some embedded \n
            int len = s.Length;
            int dummies = 0;
            for (int i = 0; i < len; i++)
            {

                int c = s[i];
                int value = (c <= 255) ? charToValue[c] : IGNORE;
                // there are two magic values PAD (=) and IGNORE.
                switch (value)
                {
                    case IGNORE:
                        // e.g. \n, just ignore it.
                        break;

                    case PAD:
                    // fallthrough
                    default:
                        if (value == PAD)
                        {
                            value = 0;
                            dummies++;
                        }
                        /* regular value character */
                        switch (cycle)
                        {
                            case 0:
                                combined = value;
                                cycle = 1;
                                break;

                            case 1:
                                combined <<= 6;
                                combined |= value;
                                cycle = 2;
                                break;

                            case 2:
                                combined <<= 6;
                                combined |= value;
                                cycle = 3;
                                break;

                            case 3:
                                combined <<= 6;
                                combined |= value;
                                // we have just completed a cycle of 4 chars.
                                // the four 6-bit values are in combined in big-endian order
                                // peel them off 8 bits at a time working lsb to msb
                                // to get our original 3 8-bit bytes back

                                b[j + 2] = (byte)combined;
                                //combined >>= 8;
                                combined = rightMove(combined, 8);
                                b[j + 1] = (byte)combined;
                                //combined >>= 8;
                                combined = rightMove(combined, 8);
                                b[j] = (byte)combined;
                                j += 3;
                                cycle = 0;
                                break;
                        }
                        break;
                }
            } // end for
            if (cycle != 0)
            {
                throw new IndexOutOfRangeException("Input to decode not an even multiple of 4 characters; pad with =.");
            }
            j -= dummies;
            if (b.Length != j)
            {
                byte[] b2 = new byte[j];
                Array.Copy(b, 0, b2, 0, j);
                b = b2;
            }
            return b;

        }// end decode

        private int rightMove(int value, int pos)
        {
            if (pos != 0)  //移动 0 位时直接返回原值
            {
                int mask = 0x7fffffff;     // int.MaxValue = 0x7FFFFFFF 整数最大值
                value >>= 1;               //无符号整数最高位不表示正负但操作数还是有符号的，有符号数右移1位，正数时高位补0，负数时高位补1
                value &= mask;     //和整数最大值进行逻辑与运算，运算后的结果为忽略表示正负值的最高位
                value >>= pos - 1;     //逻辑运算后的值无符号，对无符号的值直接做右移运算，计算剩下的位
            }
            return value;
        }

        public void setLineLength(int length)
        {
            this.lineLength = (length / 4) * 4;
        }

        public void setLineSeparator(string lineSeparator)
        {
            this.lineSeparator = lineSeparator;
        }

        private static bool debug = true;
    }
}
