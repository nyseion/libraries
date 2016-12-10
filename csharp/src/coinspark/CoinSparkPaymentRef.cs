/*
 * CoinSpark 2.1 - Java library
 *
 * Copyright (c) Coin Sciences Ltd
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


using System;
using System.Security.Cryptography;
using System.Text;

namespace coinspark
{
// package org.coinspark.protocol;

//import java.util.Random;

    /**
     * Class for managing CoinSpark payment references.
     */
    public class CoinSparkPaymentRef : CoinSparkBase
    {
        public static readonly long COINSPARK_PAYMENT_REF_MAX = 0xFFFFFFFFFFFFFL; // 2^52-1

        /**
         * Payment reference long (64-bit value)
         */

        public long reff;

//	Public functions

        /**
         * Class for managing CoinSpark payment references.
         */

        public CoinSparkPaymentRef()
        {
            reff = 0;
        }

        /**
         * Class for managing CoinSpark payment references.
         * @param Ref Long (64-bit) value to set
         */

        public CoinSparkPaymentRef(long Ref)
        {
            reff = Ref;
        }

        /**
         * Returns CoinSpark Payment reference value.
         * 
         * @return CoinSpark Payment reference value
         */

        public long getRef()
        {
            return reff;
        }


        // @Override
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            long temp = reff;

            sb.Append("COINSPARK PAYMENT REFERENCE\n")
                .Append(string.Format("{0} (small endian hex {1})\n", reff, unsignedToSmallEndianHex(temp, 8)))
                .Append("END COINSPARK PAYMENT REFERENCE\n\n");

            return sb.ToString();
        }

        /**
         * Returns true if paymentRef is in the permitted range, false otherwise.
         * 
         * @return true if paymentRef is in the permitted range, false otherwise
         */

        public bool isValid()
        {
            return ((reff >= 0) && (reff <= COINSPARK_PAYMENT_REF_MAX));
        }

        /**
         * Compares two payment references
         * @param Ref2 Payment reference to compare with 
         * @return true is two payment references match, false otherwise
         */

        public bool match(CoinSparkPaymentRef Ref2)
        {
            return (reff == Ref2.reff);
        }

        /**
         * Returns a random payment reference that can be used for a CoinSpark address and embedded in a transaction.
         * 
         * @return a random payment reference that can be used for a CoinSpark address and embedded in a transaction.
         */

        public CoinSparkPaymentRef randomize()
        {
            long paymentRef = 0;
            long bitsRemaining = COINSPARK_PAYMENT_REF_MAX;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                while (bitsRemaining > 0)
                {

                    paymentRef <<= 13;

                    byte[] intBytes = new byte[8];
                    rng.GetBytes(intBytes);
                    long randInt = BitConverter.ToInt64(intBytes, 0);
                    paymentRef |= randInt;
                    bitsRemaining >>= 13;
                }
            }
            reff = paymentRef%(1 + COINSPARK_PAYMENT_REF_MAX);

            return this;
        }

        /**
         * Encodes the paymentRef into metadata (maximal size is CoinSparkBase.OP_RETURN_MAXIMUM_LENGTH);
         * 
         * @return string | null Encoded payment reference as hexadecimal, null if we failed.
         */
        /*
        public string encodeToHex()
        {
            return encodeToHex(OP_RETURN_MAXIMUM_LENGTH);
        }
        */
        /**
         * Encodes the paymentRef into metadata (maximal size is metadataMaxLen);
         * 
         * @param metadataMaxLen maximal size of encoded data
         * @return string | null Encoded payment reference as hexadecimal, null if we failed.
         */

        public string encodeToHex(int metadataMaxLen)
        {
            CoinSparkBuffer buffer = new CoinSparkBuffer();
            if (!encode(buffer, metadataMaxLen))
            {
                return null;
            }

            return buffer.toHex();
        }

        /**
         * Encodes the paymentRef into metadata (maximal size is CoinSparkBase.OP_RETURN_MAXIMUM_LENGTH);
         * 
         * @return byte [] | null Encoded payment reference as raw data, null if we failed.
         */
        /*    
        public byte [] encode()
        {        
            return encode(OP_RETURN_MAXIMUM_LENGTH);
        }
        */
        /**
         * Encodes the paymentRef into metadata (maximal size is metadataMaxLen);
         * 
         * @param metadataMaxLen maximal size of encoded data
         * @return byte [] | null Encoded payment reference as hexadecimal, null if we failed.
         */

        public byte[] encode(int metadataMaxLen)
        {
            CoinSparkBuffer buffer = new CoinSparkBuffer();

            if (!encode(buffer, metadataMaxLen))
            {
                return null;
            }

            return buffer.toBytes();
        }

        /**
         * Decodes the payment reference in metadata  into paymentRef.
         * 
         * @param metadata Metadata to decode as hexadecimal
         * @return true on success, false on failure
         */

        public bool decode(string metadata)
        {
            CoinSparkBuffer buffer = new CoinSparkBuffer(metadata, true);
            return decode(buffer);
        }

        /**
         * Decodes the payment reference in metadata  into paymentRef.
         * 
         * @param metadata Metadata to decode as raw data
         * @return true on success, false on failure
         */

        public bool decode(byte[] metadata)
        {
            CoinSparkBuffer buffer = new CoinSparkBuffer(metadata);
            return decode(buffer);
        }



//	Private functions


        private bool encode(CoinSparkBuffer buffer, int metadataMaxLen)
        {
            try
            {
                if (!isValid())
                    throw new Exception("invalid payment reference");

                buffer.writeString(COINSPARK_METADATA_IDENTIFIER); // CoinSpark metadata identifier
                buffer.writeByte(COINSPARK_PAYMENTREF_PREFIX); // CoinSpark metadate prefix

                int bytes = 0;
                long left = reff;

                while (left > 0) // do I need all these? i can use toString
                {
                    left >>= 8;
                    bytes++;
                }

                buffer.writeLong(reff, bytes); // payment reference

                if (buffer.toBytes().Length > metadataMaxLen) // check the total length is within the specified limit
                    throw new Exception("total length above limit");
            }
            catch (Exception ex)
            {
                //    System.out.print(ex.getMessage());
                return false;
            }

            return true;
        }


        private bool decode(CoinSparkBuffer buffer)
        {
            if (!buffer.locateRange(COINSPARK_PAYMENTREF_PREFIX))
                return false;

            try
            {
                reff = buffer.readLong(buffer.availableForRead()); // Payment reference

                if (!isValid())
                    throw new Exception("Payment  is invalid");
            }

            catch (Exception ex)
            {
                //     System.out.print(ex.getMessage());
                return false;
            }

            return true;
        }
    }
}