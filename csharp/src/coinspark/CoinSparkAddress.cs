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

/**
 * CoinSparkAddress class for managing CoinSpark addresses
 */

using System;
using System.Linq;
using System.Text;

namespace coinspark
{
    public class CoinSparkAddress : CoinSparkBase
    {

// Public functions    

        /**
         * Address supports assets if a (flags & COINSPARK_ADDRESS_FLAG_ASSETS != 0)
         */

        public static readonly int COINSPARK_ADDRESS_FLAG_ASSETS = 1;

        /**
         * Address supports payment references if a (flags & COINSPARK_ADDRESS_FLAG_PAYMENT_REFS != 0)
         */

        public static readonly int COINSPARK_ADDRESS_FLAG_PAYMENT_REFS = 2;

        /**
         * Address supports text messages if a (flags & COINSPARK_ADDRESS_FLAG_TEXT_MESSAGES != 0)
         */

        public static readonly int COINSPARK_ADDRESS_FLAG_TEXT_MESSAGES = 4;

        /**
         * Address supports file messages if a (flags & COINSPARK_ADDRESS_FLAG_FILE_MESSAGES != 0)
         */

        public static readonly int COINSPARK_ADDRESS_FLAG_FILE_MESSAGES = 8;


        /**
         * (flags & COINSPARK_ADDRESS_FLAG_MASK) is used 
         */

        public static readonly int COINSPARK_ADDRESS_FLAG_MASK = 0x7FFFFF; // 23 bits are currently usable

        /**
         * Returns bitcoin address
         * @return 
         */

        public string getBitcoinAddress()
        {
            return bitcoinAddress.ToString();
        }

        /**
         * Sets bitcoin address
         * 
         * @param BitcoinAddress bitcoin address to set
         */

        public void setBitcoinAddress(string BitcoinAddress)
        {
            bitcoinAddress = new StringBuilder(BitcoinAddress);
        }

        /**
         * Returns asset flags
         * 
         * @return Asset flags
         */

        public int getAddressFlags()
        {
            return addressFlags;
        }

        /**
         * Set address flags
         * @param AddressFlags address flags to set
         */

        public void setAddressFlags(int AddressFlags)
        {
            addressFlags = AddressFlags;
        }

        /**
         * Returns payment reference.
         * 
         * @return Payment reference
         */

        public CoinSparkPaymentRef getPaymentRef()
        {
            return paymentRef;
        }

        /**
         * Set s payment reference
         * @param PaymentRef Payment reference to set
         */

        public void setPaymentRef(CoinSparkPaymentRef PaymentRef)
        {
            paymentRef = PaymentRef;
        }


        /**
         * CoinSparkAddress class for managing CoinSpark addresses
         */

        public CoinSparkAddress()
        {
            clear();
        }

        /**
         * Set all fields in address to their default/zero values, which are not necessarily valid.
         */

        public void clear()
        {
            bitcoinAddress = new StringBuilder();
            addressFlags = 0;
            paymentRef = new CoinSparkPaymentRef(0);
        }

        /**
         * CoinSparkAddress class for managing CoinSpark addresses
         * 
         * @param BitcoinAddress bitcoin address to set
         * @param AddressFlags  address flags to set
         * @param PaymentRef Payment reference to set
         */

        public CoinSparkAddress(string BitcoinAddress, int AddressFlags, CoinSparkPaymentRef PaymentRef)
        {
            clear();
            setBitcoinAddress(BitcoinAddress);
            setAddressFlags(AddressFlags);
            setPaymentRef(PaymentRef);
        }


        // @Override
        public override string ToString()
        {
            FlagToString[] flagsToStrings = new FlagToString[4];

            flagsToStrings[0] = new FlagToString(COINSPARK_ADDRESS_FLAG_ASSETS, "assets");
            flagsToStrings[1] = new FlagToString(COINSPARK_ADDRESS_FLAG_PAYMENT_REFS, "payment references");
            flagsToStrings[2] = new FlagToString(COINSPARK_ADDRESS_FLAG_TEXT_MESSAGES, "text messages");
            flagsToStrings[3] = new FlagToString(COINSPARK_ADDRESS_FLAG_FILE_MESSAGES, "file messages");

            StringBuilder sb = new StringBuilder();
            sb.Append("COINSPARK ADDRESS\n")
                .Append(string.Format("  Bitcoin address: {0}\n", bitcoinAddress))
                .Append(string.Format("    Address flags: {0}", addressFlags));

            bool flagOutput = false;

            foreach (FlagToString flagsTostring in flagsToStrings)
            {
                if ((addressFlags & flagsTostring.flag) != 0)
                {
                    sb.Append(string.Format("{0}{1}", flagOutput ? ", " : " [", flagsTostring.str));
                    flagOutput = true;
                }
            }

            sb.Append(string.Format("{0}\n", flagOutput ? "]" : ""));

            sb.Append(string.Format("Payment reference: {0}\n", paymentRef.reff))
                .Append("END COINSPARK ADDRESS\n\n");

            return sb.ToString();
        }

        /**
         * Returns true if all values in the address are in their permitted ranges, false otherwise.
         * 
         * @return Returns true if CoinSpark Address is valid
         */

        public bool isValid()
        {
            if ((bitcoinAddress == null) || (bitcoinAddress.Length == 0))
                return false;

            if ((addressFlags & COINSPARK_ADDRESS_FLAG_MASK) != addressFlags)
                return false;

            if (paymentRef == null)
                return false;

            return paymentRef.isValid();
        }

        /**
         * Compares two CoinSpark addresses
         *  
         * @param address2 Address to compare with
         * @return Returns true if the two CoinSparkAddress structures are identical.
         */

        public bool match(CoinSparkAddress address2)
        {
            return bitcoinAddress.ToString().SequenceEqual(address2.bitcoinAddress.ToString()) &&
                   addressFlags == address2.addressFlags &&
                   paymentRef.match(address2.paymentRef);
        }

        /**
         * Encodes the fields in address CoinSpark address string.
         * 
         * @return string | null, Encoded CoinSpark address, null if we failed.
         */

        public string encode()
        {
            try
            {
                if (!isValid())
                    throw new Exception("Invalid CoinSpark address");

                CoinSparkBuffer buffer = new CoinSparkBuffer();

                int[] stringBase58 = new int[1024];

                //  Build up extra data for address flags

                int addressFlagChars = 0;
                int testAddressFlags = addressFlags;

                while (testAddressFlags > 0)
                {
                    stringBase58[2 + addressFlagChars] = testAddressFlags%58;
                    testAddressFlags /= 58; // keep as integer
                    addressFlagChars++;
                }

                //  Build up extra data for payment reference

                int paymentRefChars = 0;
                long testPaymentRef = paymentRef.reff;

                while (testPaymentRef > 0)
                {
                    stringBase58[2 + addressFlagChars + paymentRefChars] = (int) (testPaymentRef%58);
                    testPaymentRef /= 58; // keep as integer
                    paymentRefChars++;
                }

                //  Calculate total length required

                int extraDataChars = addressFlagChars + paymentRefChars;
                int bitcoinAddressLen = bitcoinAddress.Length;
                int stringLen = bitcoinAddressLen + 2 + extraDataChars;

                stringBase58[1] = addressFlagChars*COINSPARK_ADDRESS_FLAG_CHARS_MULTIPLE + paymentRefChars;

                //  Convert the address itself

                for (int charIndex = 0; charIndex < bitcoinAddress.Length; charIndex++)
                {
                    int charValue = base58ToInteger((byte) bitcoinAddress[(charIndex)]);
                    if (charValue < 0)
                        throw new Exception("Wrong address character");

                    charValue += COINSPARK_ADDRESS_CHAR_INCREMENT;

                    if (extraDataChars > 0)
                        charValue += stringBase58[2 + charIndex%extraDataChars];

                    stringBase58[2 + extraDataChars + charIndex] = charValue%58;
                }

                //  Obfuscate first half of address using second half to prevent common prefixes

                int halfLength = (stringLen + 1)/2;
                for (int charIndex = 1; charIndex < halfLength; charIndex++) // exclude first character
                {
                    stringBase58[charIndex] = (stringBase58[charIndex] + stringBase58[stringLen - charIndex])%58;
                }

                //  Convert to base 58 and add prefix and terminator

                buffer.writeByte(COINSPARK_ADDRESS_PREFIX);
                for (int charIndex = 1; charIndex < stringLen; charIndex++)
                {
                    buffer.writeByte((byte) (integerToBase58[(stringBase58[charIndex])]));
                }

                return buffer.toAscii();
            }

            catch (Exception ex)
            {
                //  System.out.print(ex.getMessage());
                return null;
            }
        }

        /**
         * Decodes the CoinSpark address string into the fields in address.
         * 
         * @param Address CoinSpark address to decode
         * @return true on success, false on failure
         */

        public bool decode(string Address)
        {
            try
            {
                CoinSparkBuffer buffer = new CoinSparkBuffer(Address, false);
                int[] stringBase58 = new int[1024];
                int stringLen = Address.Length;

                //  Check for basic validity and get length of address flags

                if (buffer.toBytes().Length < 2)
                    throw new Exception("Too Short");

                if (buffer.readByte() != COINSPARK_ADDRESS_PREFIX)
                    throw new Exception("Wrong Prefix");

                //  Convert from base 58

                stringBase58[0] = COINSPARK_ADDRESS_PREFIX;
                for (int charIndex = 1; charIndex < stringLen; charIndex++)
                {
                    // exclude first character
                    int charValue2 = base58ToInteger(buffer.readByte());
                    if (charValue2 < 0)
                        throw new Exception("Invalid Address");
                    stringBase58[charIndex] = charValue2;
                }

                //  De-obfuscate first half of address using second half

                int halfLength = (buffer.toBytes().Length + 1)/2;
                for (int charIndex = 1; charIndex < halfLength; charIndex++) // exclude first character
                {
                    stringBase58[charIndex] = (stringBase58[charIndex] + 58 - stringBase58[stringLen - charIndex])%58;
                }

                int charValue = stringBase58[1];
                int addressFlagChars = charValue/COINSPARK_ADDRESS_FLAG_CHARS_MULTIPLE; // keep as integer
                int paymentRefChars = charValue%COINSPARK_ADDRESS_FLAG_CHARS_MULTIPLE;
                int extraDataChars = addressFlagChars + paymentRefChars;

                if (stringLen < (2 + extraDataChars))
                {
                    throw new Exception("Invalid Address");
                }

                //  Check we have sufficient length for the decoded address

                int bitcoinAddressLength = buffer.toBytes().Length - 2 - extraDataChars;
                bitcoinAddress.Length = bitcoinAddressLength;

                //  Read the extra data for address flags

                addressFlags = 0;
                long multiplier = 1;

                for (int charIndex = 0; charIndex < addressFlagChars; charIndex++)
                {
                    charValue = stringBase58[2 + charIndex];
                    if (charValue < 0)
                    {
                        throw new Exception("Invalid Value (58 based < 0)");
                    }
                    addressFlags += (int) (multiplier*charValue);
                    multiplier *= 58;
                }

                if ((addressFlags & COINSPARK_ADDRESS_FLAG_MASK) != addressFlags)
                {
                    throw new Exception("Wrong Address Flag Mask");
                }

                //  Read the extra data for payment assetRef

                paymentRef = new CoinSparkPaymentRef(0);
                multiplier = 1;

                for (int charIndex = 0; charIndex < paymentRefChars; charIndex++)
                {
                    charValue = stringBase58[2 + addressFlagChars + charIndex];
                    if (charValue < 0)
                    {
                        throw new Exception("Invalid Value (58 based < 0)");
                    }

                    paymentRef.reff += multiplier*charValue;
                    multiplier *= 58;
                }

                if (!paymentRef.isValid())
                    throw new Exception("Wrong Payment Range");

                //  Convert the address itself

                for (int charIndex = 0; charIndex < bitcoinAddressLength; charIndex++)
                {
                    charValue = stringBase58[2 + extraDataChars + charIndex];
                    if (charValue < 0)
                    {
                        throw new Exception("Invalid Value (58 based < 0)");
                    }

                    charValue += 58*2 - COINSPARK_ADDRESS_CHAR_INCREMENT;
                        // avoid worrying about the result of modulo on negative numbers in any language

                    if (extraDataChars > 0)
                    {
                        charValue -= stringBase58[2 + charIndex%extraDataChars];
                    }
                    bitcoinAddress[charIndex] = integerToBase58[charValue%58];
                }

                return true;
            }
            catch (Exception ex)
            {
                //  System.out.print(ex.getMessage());
                return false;
            }
        }

// Private variables/constants/functions   

        private static readonly byte COINSPARK_ADDRESS_PREFIX = (byte) 's';
        private static readonly int COINSPARK_ADDRESS_FLAG_CHARS_MULTIPLE = 10;
        private static readonly int COINSPARK_ADDRESS_CHAR_INCREMENT = 13;

        private StringBuilder bitcoinAddress;
        private int addressFlags;
        private CoinSparkPaymentRef paymentRef;


        private static readonly StringBuilder integerToBase58 =
            new StringBuilder("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz");


        private class FlagToString
        {
            public int flag;
            public string str;

            public FlagToString(int flag, string strr)
            {
                this.flag = flag;
                str = strr;
            }

            private FlagToString()
            {
            } // disabled
        }

    }
}