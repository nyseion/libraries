/*
 * CoinSpark 2.1 - .NET C# library (derived from the CoinSpark 2.1 Java library)
 *
 * Copyright (c) Coin Sciences Ltd
 * Modified work Copyright (c) Vincent M. Mele, nyseion.com
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
using coinspark;

namespace coinspark_test
{
    public class Program
    {
        public static void CreateCoinSparkAddress()
        {
            Console.WriteLine("\nCreating a CoinSpark address...\n");
            CoinSparkAddress address = new CoinSparkAddress();
            
            address.setBitcoinAddress("149wHUMa41Xm2jnZtqgRx94uGbZD9kPXnS");
            address.setAddressFlags(CoinSparkAddress.COINSPARK_ADDRESS_FLAG_ASSETS | CoinSparkAddress.COINSPARK_ADDRESS_FLAG_PAYMENT_REFS);
            address.setPaymentRef(new CoinSparkPaymentRef(0)); // or any unsigned 52-bit integer up to CoinSparkPaymentRef.COINSPARK_PAYMENT_REF_MAX
            String addressString = address.encode();
            if (addressString != null)
                Console.WriteLine("CoinSpark address: " + addressString);
        else
            Console.WriteLine("CoinSpark address encode failed!");
        }
        public static void DecodeCoinSparkAddress()
    {
        Console.WriteLine("\nDecoding a CoinSpark address...\n");

        CoinSparkAddress address=new CoinSparkAddress();

        if (address.decode("s6GUHy69HWkwFqzFhJCY49seL8EFv")) {
            Console.WriteLine("Bitcoin address: "+address.getBitcoinAddress());
            Console.WriteLine("Address flags: "+address.getAddressFlags());
            Console.WriteLine("Payment reference: "+address.getPaymentRef().getRef());

            Console.Write(address.ToString());

        } else
            Console.WriteLine("CoinSpark address decode failed!");
    }

        public static void ProcessTransaction(byte[][] scriptPubKeys, int countInputs)
        {
            Console.WriteLine("\nExtracting CoinSpark metadata from a transaction...\n");

            // scriptPubKeys is an array containing each output script of a transaction as raw binary data.       
            // The transaction has scriptPubKeys.length outputs and countInputs inputs.

            byte[] metadata = CoinSparkBase.scriptsToMetadata(scriptPubKeys);

            if (metadata != null)
            {
                CoinSparkGenesis genesis = new CoinSparkGenesis();
                if (genesis.decode(metadata))
                    Console.Write(genesis.ToString());

                CoinSparkTransferList transferList = new CoinSparkTransferList();
                if (transferList.decode(metadata, countInputs, scriptPubKeys.Length))
                    Console.Write(transferList.ToString());

                CoinSparkPaymentRef paymentRef = new CoinSparkPaymentRef();
                if (paymentRef.decode(metadata))
                    Console.Write(paymentRef.ToString());
            }
        }

        public static void ProcessTransaction(String[] scriptPubKeys, int countInputs)
        {
            Console.WriteLine("\nExtracting CoinSpark metadata from a transaction...\n");

            // scriptPubKeys is an array containing each output script of a transaction as a hex string
            // or raw binary (commented above). The transaction has scriptPubKeys.length outputs and
            // countInputs inputs.

            byte[] metadata = CoinSparkBase.scriptsToMetadata(scriptPubKeys);

            if (metadata != null)
            {
                CoinSparkGenesis genesis = new CoinSparkGenesis();
                if (genesis.decode(metadata))
                    Console.Write(genesis.ToString());

                CoinSparkTransferList transferList = new CoinSparkTransferList();
                if (transferList.decode(metadata, countInputs, scriptPubKeys.Length))
                    Console.Write(transferList.ToString());

                CoinSparkPaymentRef paymentRef = new CoinSparkPaymentRef();
                if (paymentRef.decode(metadata))
                    Console.Write(paymentRef.ToString());

                CoinSparkMessage message = new CoinSparkMessage();
                if (message.decode(metadata, scriptPubKeys.Length))
                    Console.Write(message.ToString());
            }
        }

        public static byte[] EncodeMetaData(byte[] metadata)
        {
            Console.WriteLine("\nEncoding CoinSpark metadata in a script...\n");

            // first get metadata from the encode() method of a CoinSparkGenesis, CoinSparkTransferList
            // or CoinSparkPaymentRef object, or the CoinSparkBase.metadataAppend() method.

            byte[] scriptPubKey = null;

            if (metadata != null)
            {
                scriptPubKey = CoinSparkBase.metadataToScript(metadata);

                if (scriptPubKey != null)
                    ; // now embed the raw bytes in $scriptPubKey directly in a transaction output
                else
                    ; // handle the error

            }
            else
                ; // handle the error

            return scriptPubKey;
        }

        public static String EncodeMetaDataToHex(byte[] metadata)
        {
            Console.WriteLine("\nEncoding CoinSpark metadata in a script...\n");

            // first get metadata from the encode() method of a CoinSparkGenesis, CoinSparkTransferList
            // or CoinSparkPaymentRef object, or the CoinSparkBase.metadataAppend() method.

            String scriptPubKey = null;

            if (metadata != null)
            {
                scriptPubKey = CoinSparkBase.metadataToScriptHex(metadata);

                if (scriptPubKey != null)
                    Console.WriteLine("Script: " + scriptPubKey);
            else
                Console.WriteLine("Metadata encode failed!");

            }
            else
                ; // handle the error

            return scriptPubKey;
        }



        public static void Main(string[] args)
        {
            CoinSparkTest.main(args);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
