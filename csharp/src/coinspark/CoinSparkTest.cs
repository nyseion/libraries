/*
 * CoinSpark 2.1 - Java test suite
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



namespace coinspark
{
// package org.coinspark.protocol;

/**
        * Test various components of the CoinSpark library.
        */

    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using coinspark;

    public class CoinSparkTest
    {

        public enum CoinSparkTestType
        {
            ADDRESS,
            REFERENCE,
            SCRIPT,
            HASH,
            GENESIS,
            TRANSFER,
            MESSAGE
        }

        public static class CoinSparkTestTypeMethods
        {
            public static string GetLetter(CoinSparkTestType t1)
            {
                switch (t1)
                {
                    case CoinSparkTestType.ADDRESS:
                        return "A";
                    case CoinSparkTestType.REFERENCE:
                        return "R";
                    case CoinSparkTestType.SCRIPT:
                        return "S";
                    case CoinSparkTestType.HASH:
                        return "H";
                    case CoinSparkTestType.GENESIS:
                        return "G";
                    case CoinSparkTestType.TRANSFER:
                        return "T";
                    case CoinSparkTestType.MESSAGE:
                        return "M";
                    default:
                        return "B"; // todo
                }
            }

            public static string GetText(CoinSparkTestType t1)
            {
                switch (t1)
                {
                    case CoinSparkTestType.ADDRESS:
                        return "Addresses";
                    case CoinSparkTestType.REFERENCE:
                        return "Asset References";
                    case CoinSparkTestType.SCRIPT:
                        return "Script Metadata";
                    case CoinSparkTestType.HASH:
                        return "Asset Hashes";
                    case CoinSparkTestType.GENESIS:
                        return "Genesis calculations";
                    case CoinSparkTestType.TRANSFER:
                        return "Transfer calculations";
                    case CoinSparkTestType.MESSAGE:
                        return "Message Hashes";
                    default:
                        return "B"; // todo
                }
            }

            public static string GetSuffix(CoinSparkTestType t1)
            {
                switch (t1)
                {
                    case CoinSparkTestType.ADDRESS:
                        return "Address";
                    case CoinSparkTestType.REFERENCE:
                        return "AssetRef";
                    case CoinSparkTestType.SCRIPT:
                        return "Script";
                    case CoinSparkTestType.HASH:
                        return "AssetHash";
                    case CoinSparkTestType.GENESIS:
                        return "Genesis";
                    case CoinSparkTestType.TRANSFER:
                        return "Transfer";
                    case CoinSparkTestType.MESSAGE:
                        return "MessageHash";
                    default:
                        return "B"; // todo
                }
            }

            public static CoinSparkTestType FromLetter(string t1)
            {
                switch (t1)
                {
                    case "A":
                        return CoinSparkTestType.ADDRESS;
                    case "R":
                        return CoinSparkTestType.REFERENCE;
                    case "S":
                        return CoinSparkTestType.SCRIPT;
                    case "H":
                        return CoinSparkTestType.HASH;
                    case "G":
                        return CoinSparkTestType.GENESIS;
                    case "T":
                        return CoinSparkTestType.TRANSFER;
                    case "M":
                        return CoinSparkTestType.MESSAGE;
                    default:
                        return CoinSparkTestType.HASH; // todo

                }
            }
        }




/*    static readonly Dictionary<string,string> CoinSparkTestTypes = new Dictionary<string, string>
                        {
                            ADDRESS = "A", //"Addresses", "Address"),
                            REFERENCE("R", "Asset References", "AssetRef"),
                            SCRIPT("S", "Script metadata", "Script"),
                            HASH("H", "Asset Hashes", "AssetHash"),
                            GENESIS("G", "Genesis calculations", "Genesis"),
                            TRANSFER("T", "Transfer calculations", "Transfer"),
                            MESSAGE("M", "Message Hashes", "MessageHash");
                        }
                
                        private string letter;
                        private string text;
                        private string suffix;
                        
                        CoinSparkTestType(string Letter,string Text,string Suffix)
                        {
                            letter=Letter;
                            text=Text;
                            suffix=Suffix;
                        }
                        
                        private string getLetter()
                        {
                            return letter;
                        }
                        
                        private string getText()
                        {
                            return text;
                        }
                        
                        private string getSuffix()
                        {
                            return suffix;
                        }
                        
                        public static CoinSparkTestType fromLetter(string Letter) {
                            if (Letter != null) {
                                Letter = Letter.Trim().toUpperCase();
                                for (CoinSparkTestType lt : CoinSparkTestType.values()) {
                                    if (lt.getLetter().equals(Letter)) {
                                        return lt;
                                    }
                                }
                            }
                            return null;
                        }        
                    }    
                    */
        private CoinSparkTestType testType;
        private string inputFile;
        private string outputFile;
        private string logFile;
        StreamReader inputBR;
        StreamWriter outputFW;
        StreamWriter logFW;

        public CoinSparkTest(string Directory, CoinSparkTestType TestType)
        {
            testType = TestType;
            if (testType != null)
            {
                inputFile = Directory + Path.DirectorySeparatorChar + CoinSparkTestTypeMethods.GetSuffix(TestType) +
                            "-Input.txt";
                outputFile = Directory + Path.DirectorySeparatorChar + CoinSparkTestTypeMethods.GetSuffix(TestType) +
                             "-Output-CSharp.txt";
                logFile = Directory + Path.DirectorySeparatorChar + CoinSparkTestTypeMethods.GetSuffix(TestType) +
                          "-Output-CSharp.log";
            }
        }

        private string getInputLine()
        {
            if (inputBR == null)
                return null;

            string line;
            try
            {
                line = inputBR.ReadLine();
            }
            catch (IOException ex)
            {
                Console.Write(string.Format("Cannot read input file {0}\n", inputFile));
                return null;
            }

            if (line != null)
            {
                line = ("x" + line).Trim().Substring(1);
                int pos = line.IndexOf(" # ", StringComparison.Ordinal);
                if (pos >= 0)
                {
                    line = line.Substring(0, pos);
                }
            }

            return line;
        }

        private string[] getInputLines(int count)
        {
            if (count <= 0)
            {
                return null;
            }
            string[] result = new String[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = getInputLine();
                if (result[i] == null)
                {
                    return null;
                }
            }
            return result;
        }

        private bool writeOutputLine(string line)
        {
            return writeOutputLine(line, "\n");
        }

        private bool writeOutput(string line)
        {
            return writeOutputLine(line, "");
        }

        private bool writeOutputLine(string line, string eol)
        {
            try
            {
                outputFW.Write(line + eol);
            }
            catch (IOException ex)
            {
                Console.Write(string.Format("Cannot write to output file {0}\n", inputFile));
                return false;
            }

            return true;
        }

        private bool writeLogLine(string line)
        {
            try
            {
                logFW.Write(line + "\n");
            }
            catch (IOException ex)
            {
                Console.Write(string.Format("Cannot write to log file {0}\n", inputFile));
                return false;
            }

            return true;
        }

        private bool performAddressTest(bool ExitOnFailure)
        {
            bool result = true;

            string line = getInputLine();
            if ((line == null) || !line.Contains("CoinSpark Address Tests Input"))
            {
                writeLogLine("Different header line expected, got " + line);
                return false;
            }
            line = getInputLine();
            if ((line == null) || (line.Length > 0))
            {
                writeLogLine("Empty line expected, got " + line);
                return false;
            }

            writeOutputLine("CoinSpark Address Tests Output");
            writeOutputLine("");

            while ((line = getInputLine()) != null)
            {
                bool try_again = true;
                bool this_result = true;
                while (try_again)
                {
                    if (this_result)
                    {
                        try_again = false;
                    }
                    CoinSparkAddress address = new CoinSparkAddress();

                    if (address.decode(line))
                    {
                        writeOutput(address.ToString());
                        string encoded = address.encode();
                        if (encoded != null)
                        {
                            int orglen = line.Length;
                            int englen = encoded.Length;

                            if (!line.Contains(encoded))
                            {
                                writeLogLine("Encode address mismatch: " + encoded + " should be " + line);
                                this_result = false;
                            }
                        }
                        else
                        {
                            writeLogLine("Failed to encode address " + line);
                            this_result = false;
                        }

                        if (!address.match(address))
                        {
                            writeLogLine("Failed to match address to itself! " + line);
                            this_result = false;
                        }
                    }
                    else
                    {
                        writeLogLine("Failed to decode address " + line);
                        this_result = false;
                    }

                    result &= this_result;
                    if (!try_again)
                    {
                        if (!this_result)
                        {
                            try_again = true;
                        }
                    }
                    else
                    {
                        if (ExitOnFailure)
                        {
                            return result;
                        }
                        else
                        {
                            try_again = false;
                        }
                    }
                }
            }

            return result;
        }

        private bool performAssetRefTest(bool ExitOnFailure)
        {
            bool result = true;

            string line = getInputLine();
            if ((line == null) || !line.Contains("CoinSpark AssetRef Tests Input"))
            {
                writeLogLine("Different header line expected, got " + line);
                return false;
            }
            line = getInputLine();
            if ((line == null) || (line.Length > 0))
            {
                writeLogLine("Empty line expected, got " + line);
                return false;
            }

            writeOutputLine("CoinSpark AssetRef Tests Output");
            writeOutputLine("");

            while ((line = getInputLine()) != null)
            {
                bool try_again = true;
                bool this_result = true;
                while (try_again)
                {
                    if (this_result)
                    {
                        try_again = false;
                    }

                    CoinSparkAssetRef assetRef = new CoinSparkAssetRef();

                    if (assetRef.decode(line))
                    {
                        writeOutput(assetRef.toStringInner(true));
                        string encoded = assetRef.encode();
                        if (encoded != null)
                        {
                            if (!line.Contains(encoded))
                            {
                                writeLogLine("Encode assetRef mismatch: " + encoded + " should be " + line);
                                this_result = false;
                            }
                        }
                        else
                        {
                            writeLogLine("Failed to encode assetRef " + line);
                            this_result = false;
                        }

                        if (!assetRef.match(assetRef))
                        {
                            writeLogLine("Failed to match assetRef to itself! " + line);
                            this_result = false;
                        }
                    }
                    else
                    {
                        writeLogLine("Failed to decode assetRef " + line);
                        this_result = false;
                    }

                    result &= this_result;
                    if (!try_again)
                    {
                        if (!this_result)
                        {
                            try_again = true;
                        }
                    }
                    else
                    {
                        if (ExitOnFailure)
                        {
                            return result;
                        }
                        else
                        {
                            try_again = false;
                        }
                    }
                }
            }

            return result;
        }


        private bool performGenesisTest(bool ExitOnFailure)
        {
            bool result = true;

            string line = getInputLine();
            if ((line == null) || !line.Contains("CoinSpark Genesis Tests Input"))
            {
                writeLogLine("Different header line expected, got " + line);
                return false;
            }
            line = getInputLine();
            if ((line == null) || (line.Length > 0))
            {
                writeLogLine("Empty line expected, got " + line);
                return false;
            }

            writeOutputLine("CoinSpark Genesis Tests Output");
            writeOutputLine("");

            string[] lines;

            while ((lines = getInputLines(7)) != null)
            {
                bool try_again = true;
                bool this_result = true;
                while (try_again)
                {
                    if (this_result)
                    {
                        try_again = false;
                    }

                    string firstSpentTxId = lines[0];
                    long firstSpentVout = long.Parse(lines[1]);
                    string metadataHex = lines[2];
                    string outputsSatoshisString = lines[3];
                    string outputsRegularString = lines[4];
                    long feeSatoshis = long.Parse(lines[5]);

                    CoinSparkGenesis genesis = new CoinSparkGenesis();

                    if (genesis.decode(metadataHex))
                    {
                        string[] outputsSatoshisSplit = outputsSatoshisString.Split(',');
                        string[] outputsRegularSplit = outputsRegularString.Split(',');
                        int outputCount = outputsSatoshisSplit.Length;
                        long[] outputsSatoshis = new long[outputCount];
                        bool[] outputsRegular = new bool[outputCount];

                        for (int i = 0; i < outputCount; i++)
                        {
                            outputsSatoshis[i] = long.Parse(outputsSatoshisSplit[i]);
                            outputsRegular[i] = true;
                            if (outputsRegularSplit[i].Contains("0")) outputsRegular[i] = false;
                        }

                        long validFeeSatoshis = genesis.calcMinFee(outputsSatoshis, outputsRegular);

                        long[] outputBalances;
                        if (feeSatoshis >= validFeeSatoshis)
                        {
                            outputBalances = genesis.apply(outputsRegular);
                        }
                        else
                        {
                            outputBalances = new long[outputCount];
                            for (int i = 0; i < outputCount; i++)
                            {
                                outputBalances[i] = 0;
                            }
                        }

                        writeOutput(string.Format("{0} # transaction fee satoshis to be valid\n", validFeeSatoshis));
                        for (int i = 0; i < outputCount; i++)
                        {
                            writeOutput((i > 0 ? "," : "") + string.Format("{0}", outputBalances[i]));
                        }
                        writeOutput(" # units of the asset in each output\n");
                        writeOutput(genesis.calcAssetURL(firstSpentTxId, firstSpentVout) + " # asset web page URL\n\n");

                        string encoded = genesis.encodeToHex(40);
                        if (encoded != null)
                        {
                            if (!metadataHex.Contains(encoded))
                            {
                                writeLogLine("Encode genesis mismatch: " + encoded + " should be " + metadataHex);
                                this_result = false;
                            }
                        }
                        else
                        {
                            writeLogLine("Failed to encode genesis " + metadataHex);
                            this_result = false;
                        }

                        if (!genesis.match(genesis, true))
                        {
                            writeLogLine("Failed to match genesis to itself! " + metadataHex);
                            this_result = false;
                        }
                    }
                    else
                    {
                        writeLogLine("Failed to decode genesis " + line);
                        this_result = false;
                    }

                    result &= this_result;
                    if (!try_again)
                    {
                        if (!this_result)
                        {
                            try_again = true;
                        }
                    }
                    else
                    {
                        if (ExitOnFailure)
                        {
                            return result;
                        }
                        else
                        {
                            try_again = false;
                        }
                    }
                }
            }

            return result;
        }

        private bool performScriptTest(bool ExitOnFailure)
        {
            bool result = true;

            string line = getInputLine();
            if ((line == null) || !line.Contains("CoinSpark Script Tests Input"))
            {
                writeLogLine("Different header line expected, got " + line);
                return false;
            }
            line = getInputLine();
            if ((line == null) || (line.Length > 0))
            {
                writeLogLine("Empty line expected, got " + line);
                return false;
            }

            writeOutputLine("CoinSpark Script Tests Output");
            writeOutputLine("");

            string[] lines;

            while ((lines = getInputLines(4)) != null)
            {
                bool try_again = true;
                bool this_result = true;

                Random rnd = new Random();
                int rounding = rnd.Next(3) - 1;

                while (try_again)
                {
                    if (this_result)
                    {
                        try_again = false;
                    }

                    int countInputs = int.Parse(lines[0]);
                    int countOutputs = int.Parse(lines[1]);
                    string ScriptPubKeyHex = lines[2];

                    byte[] metadata = CoinSparkBase.scriptToMetadata(ScriptPubKeyHex);

                    CoinSparkGenesis genesis = new CoinSparkGenesis();
                    bool hasGenesis = genesis.decode(metadata);

                    CoinSparkPaymentRef paymentRef = new CoinSparkPaymentRef();
                    bool hasPaymentRef = paymentRef.decode(metadata);

                    CoinSparkTransferList transfers = new CoinSparkTransferList();
                    bool hasTransferList = transfers.decode(metadata, countInputs, countOutputs);

                    CoinSparkMessage message = new CoinSparkMessage();
                    bool hasMessage = message.decode(metadata, countOutputs);

                    if (hasGenesis)
                    {
                        writeOutput(genesis.ToString());
                    }

                    if (hasPaymentRef)
                    {
                        writeOutput(paymentRef.ToString());
                    }

                    if (hasTransferList)
                    {
                        writeOutput(transfers.ToString());
                    }

                    if (hasMessage)
                    {
                        writeOutput(message.ToString());
                    }

                    byte[] encoded = null;
                    byte[] this_encoded;
                    int maxMetadataLen = 40;

                    if (hasGenesis)
                    {
                        encoded = genesis.encode(maxMetadataLen);
                        if (encoded == null)
                        {
                            writeLogLine("Failed to encode genesis " + ScriptPubKeyHex);
                            this_result = false;
                        }
                        else
                        {
                            maxMetadataLen = CoinSparkBase.metadataMaxAppendLen(encoded, 40);
                        }
                    }

                    if (hasPaymentRef)
                    {
                        this_encoded = paymentRef.encode(40);
                        if (this_encoded == null)
                        {
                            writeLogLine("Failed to encode payment reference " + ScriptPubKeyHex);
                            this_result = false;
                        }
                        else
                        {
                            encoded = CoinSparkBase.metadataAppend(encoded, 40, this_encoded);
                            if (encoded == null)
                            {
                                writeLogLine("Failed to append encoded metadata " + ScriptPubKeyHex);
                                this_result = false;
                            }
                            else
                            {
                                maxMetadataLen = CoinSparkBase.metadataMaxAppendLen(encoded, 40);
                            }
                        }
                    }

                    if (hasTransferList)
                    {
                        this_encoded = transfers.encode(countInputs, countOutputs, maxMetadataLen);
                        if (this_encoded == null)
                        {
                            writeLogLine("Failed to encode transfer list " + ScriptPubKeyHex);
                            this_result = false;
                        }
                        else
                        {
                            encoded = CoinSparkBase.metadataAppend(encoded, 40, this_encoded);
                            if (encoded == null)
                            {
                                writeLogLine("Failed to append encoded metadata " + ScriptPubKeyHex);
                                this_result = false;
                            }
                        }
                    }

                    if (hasMessage)
                    {
                        this_encoded = message.encode(countOutputs, maxMetadataLen);
                        if (this_encoded == null)
                        {
                            writeLogLine("Failed to encode message " + ScriptPubKeyHex);
                            this_result = false;
                        }
                        else
                        {
                            encoded = CoinSparkBase.metadataAppend(encoded, 40, this_encoded);
                            if (encoded == null)
                            {
                                writeLogLine("Failed to append encoded metadata " + ScriptPubKeyHex);
                                this_result = false;
                            }
                        }
                    }

                    string encodedScriptPubKeyHex = CoinSparkBase.metadataToScriptHex(encoded);
                    if (!encodedScriptPubKeyHex.Contains(ScriptPubKeyHex))
                    {
                        writeLogLine("Encode mismatch: " + encodedScriptPubKeyHex + " should be " + ScriptPubKeyHex);
                        this_result = false;
                    }

                    if (hasGenesis)
                    {
                        if (!genesis.match(genesis, true))
                        {
                            writeLogLine("Failed to match genesis to itself! " + ScriptPubKeyHex);
                            this_result = false;
                        }

                        CoinSparkGenesis testGenesis = new CoinSparkGenesis();
                        testGenesis.decode(metadata);



                        testGenesis.setQty(0, 0);
                        testGenesis.setQty(genesis.getQty(), rounding);

                        testGenesis.setChargeFlat(0, 0);
                        testGenesis.setChargeFlat(genesis.getChargeFlat(), rounding);

                        if (!genesis.match(testGenesis, false))
                        {
                            writeLogLine("Mismatch on genesis rounding! " + ScriptPubKeyHex);
                            this_result = false;
                        }
                    }

                    if (hasPaymentRef)
                    {
                        if (!paymentRef.match(paymentRef))
                        {
                            writeLogLine("Failed to match payment reference to itself! " + ScriptPubKeyHex);
                            this_result = false;
                        }
                    }

                    if (hasTransferList)
                    {
                        if (!transfers.match(transfers, true))
                        {
                            writeLogLine("Failed to strictly match transfer list to itself! " + ScriptPubKeyHex);
                            this_result = false;
                        }

                        if (!transfers.match(transfers, false))
                        {
                            writeLogLine("Failed to leniently match transfer list to itself! " + ScriptPubKeyHex);
                            this_result = false;
                        }
                    }

                    if (hasMessage)
                    {
                        if (!message.match(message, true))
                        {
                            writeLogLine("Failed to strictly match message to itself! " + ScriptPubKeyHex);
                            this_result = false;
                        }

                        if (!message.match(message, false))
                        {
                            writeLogLine("Failed to leniently match message to itself! " + ScriptPubKeyHex);
                            this_result = false;
                        }
                    }


                    result &= this_result;
                    if (!try_again)
                    {
                        if (!this_result)
                        {
                            try_again = true;
                        }
                    }
                    else
                    {
                        if (ExitOnFailure)
                        {
                            return result;
                        }
                        else
                        {
                            try_again = false;
                        }
                    }
                }
            }

            return result;
        }

        private bool performTransferTest(bool ExitOnFailure)
        {
            bool result = true;

            string line = getInputLine();
            if ((line == null) || !line.Contains("CoinSpark Transfer Tests Input"))
            {
                writeLogLine("Different header line expected, got " + line);
                return false;
            }
            line = getInputLine();
            if ((line == null) || (line.Length > 0))
            {
                writeLogLine("Empty line expected, got " + line);
                return false;
            }

            writeOutputLine("CoinSpark Transfer Tests Output");
            writeOutputLine("");

            string[] lines;

            while ((lines = getInputLines(8)) != null)
            {
                bool try_again = true;
                bool this_result = true;
                while (try_again)
                {
                    if (this_result)
                    {
                        try_again = false;
                    }

                    string genesisMetadataHex = lines[0];
                    string assetRefString = lines[1];
                    string metadataHex = lines[2];
                    string inputBalancesString = lines[3];
                    string outputsSatoshisString = lines[4];
                    string outputsRegularString = lines[5];
                    long feeSatoshis = long.Parse(lines[6]);

                    CoinSparkGenesis genesis = new CoinSparkGenesis();

                    if (genesis.decode(genesisMetadataHex))
                    {
                        CoinSparkAssetRef assetRef = new CoinSparkAssetRef();

                        if (assetRef.decode(assetRefString))
                        {
                            string[] inputBalancesSplit = inputBalancesString.Split(',');
                            string[] outputsSatoshisSplit = outputsSatoshisString.Split(',');
                            string[] outputsRegularSplit = outputsRegularString.Split(',');
                            int inputCount = inputBalancesSplit.Length;
                            int outputCount = outputsSatoshisSplit.Length;
                            long[] inputBalances = new long[inputCount];
                            long[] outputsSatoshis = new long[outputCount];
                            bool[] outputsRegular = new bool[outputCount];

                            for (int i = 0; i < inputCount; i++)
                            {
                                inputBalances[i] = long.Parse(inputBalancesSplit[i]);
                            }
                            for (int i = 0; i < outputCount; i++)
                            {
                                outputsSatoshis[i] = long.Parse(outputsSatoshisSplit[i]);
                                outputsRegular[i] = true;
                                if (outputsRegularSplit[i].SequenceEqual("0")) outputsRegular[i] = false;
                            }

                            CoinSparkTransferList transfers = new CoinSparkTransferList();
                            if (transfers.decode(metadataHex, inputCount, outputCount))
                            {
                                long validFeeSatoshis = transfers.calcMinFee(inputCount, outputsSatoshis, outputsRegular);

                                long[] outputBalances;
                                if (feeSatoshis >= validFeeSatoshis)
                                {
                                    outputBalances = transfers.apply(assetRef, genesis, inputBalances, outputsRegular);
                                }
                                else
                                {
                                    outputBalances = transfers.applyNone(assetRef, genesis, inputBalances,
                                        outputsRegular);
                                }

                                bool[] outputsDefault = transfers.defaultOutputs(inputCount, outputsRegular);

                                writeOutput(string.Format("{0} # transaction fee satoshis to be valid\n",
                                    validFeeSatoshis));
                                for (int i = 0; i < outputCount; i++)
                                {
                                    writeOutput((i > 0 ? "," : "") + string.Format("{0}", outputBalances[i]));
                                }
                                writeOutput(" # units of this asset in each output\n");

                                for (int i = 0; i < outputCount; i++)
                                {
                                    writeOutput((i > 0 ? "," : "") + (outputsDefault[i] ? "1" : "0"));
                                }
                                writeOutput(" # boolflags whether each output is in a default route\n\n");

                                for (int i = 0; i < inputCount; i++)
                                {
                                    long testGrossBalance = genesis.calcGross(inputBalances[i]);
                                    long testNetBalance = genesis.calcNet(testGrossBalance);

                                    if (inputBalances[i] != testNetBalance)
                                    {
                                        writeLogLine(string.Format("Net to gross to net mismatch: {0} -> {1} -> {2}!",
                                            inputBalances[i], testGrossBalance, testNetBalance));
                                        this_result = false;
                                    }
                                }

                                string encoded = transfers.encodeToHex(inputCount, outputCount, 40);
                                if (encoded != null)
                                {
                                    if (!metadataHex.Contains(encoded))
                                    {
                                        writeLogLine("Encode transfer list mismatch: " + encoded + " should be " +
                                                     metadataHex);
                                        this_result = false;
                                    }
                                }
                                else
                                {
                                    writeLogLine("Failed to encode transfer list " + metadataHex);
                                    this_result = false;
                                }

                                if (!transfers.match(transfers, true))
                                {
                                    writeLogLine("Failed to match transfer list to itself! " + metadataHex);
                                    this_result = false;
                                }

                            }
                            else
                            {
                                writeLogLine("Failed to decode transfers metadata " + metadataHex);
                                this_result = false;
                            }
                        }
                        else
                        {
                            writeLogLine("Failed to decode asset reference " + assetRefString);
                            this_result = false;
                        }
                    }
                    else
                    {
                        writeLogLine("Failed to decode genesis " + genesisMetadataHex);
                        this_result = false;
                    }

                    result &= this_result;
                    if (!try_again)
                    {
                        if (!this_result)
                        {
                            try_again = true;
                        }
                    }
                    else
                    {
                        if (ExitOnFailure)
                        {
                            return result;
                        }
                        else
                        {
                            try_again = false;
                        }
                    }
                }
            }

            return result;
        }

        private bool performMessageHashTest(bool ExitOnFailure)
        {
            bool result = true;

            string line = getInputLine();
            if ((line == null) || !line.Contains("CoinSpark MessageHash Tests Input"))
            {
                writeLogLine("Different header line expected, got " + line);
                return false;
            }
            line = getInputLine();
            if ((line == null) || (line.Length > 0))
            {
                writeLogLine("Empty line expected, got " + line);
                return false;
            }

            writeOutputLine("CoinSpark MessageHash Tests Output");
            writeOutputLine("");

            string[] lines;

            while ((lines = getInputLines(2)) != null)
            {
                bool try_again = true;
                bool this_result = true;
                string salt = lines[0];
                int countParts = int.Parse(lines[1]);
                CoinSparkMessagePart[] contentParts = new CoinSparkMessagePart[countParts];


                lines = getInputLines(3*countParts + 1);

                if (lines != null)
                {
                    while (try_again)
                    {
                        if (this_result)
                        {
                            try_again = false;
                        }

                        for (int index = 0; index < countParts; index++)
                        {
                            contentParts[index] = new CoinSparkMessagePart();
                            contentParts[index].mimeType = lines[index*3 + 0];
                            contentParts[index].fileName = lines[index*3 + 1];
                            contentParts[index].content = Encoding.UTF8.GetBytes(lines[index*3 + 2]);
                        }

                        string hash =
                            CoinSparkMessage.byteToHex(CoinSparkMessage.calcMessageHash(Encoding.UTF8.GetBytes(salt),
                                contentParts));

                        if (hash != null)
                        {
                            writeOutputLine(hash);
                        }
                        else
                        {
                            writeLogLine("Cannot calcualte hash for " + salt);
                            this_result = false;
                        }

                        result &= this_result;
                        if (!try_again)
                        {
                            if (!this_result)
                            {
                                try_again = true;
                            }
                        }
                        else
                        {
                            if (ExitOnFailure)
                            {
                                return result;
                            }
                            else
                            {
                                try_again = false;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool performAssetHashTest(bool ExitOnFailure)
        {
            bool result = true;

            string line = getInputLine();
            if ((line == null) || !line.Contains("CoinSpark AssetHash Tests Input"))
            {
                writeLogLine("Different header line expected, got " + line);
                return false;
            }
            line = getInputLine();
            if ((line == null) || (line.Length > 0))
            {
                writeLogLine("Empty line expected, got " + line);
                return false;
            }

            writeOutputLine("CoinSpark AssetHash Tests Output");
            writeOutputLine("");

            string[] lines;

            while ((lines = getInputLines(10)) != null)
            {
                bool try_again = true;
                bool this_result = true;
                while (try_again)
                {
                    if (this_result)
                    {
                        try_again = false;
                    }

                    string name = lines[0];
                    string issuer = lines[1];
                    string description = lines[2];
                    string units = lines[3];
                    string issueDate = lines[4];
                    string expiryDate = lines[5];
                    double interestRate = double.Parse(lines[6]);
                    double multiple = double.Parse(lines[7]);
                    byte[] contract = Encoding.UTF8.GetBytes(lines[8]);

                    string hash =
                        CoinSparkGenesis.byteToHex(CoinSparkGenesis.calcAssetHash(name, issuer, description, units,
                            issueDate, expiryDate, interestRate, multiple, contract));

                    if (hash != null)
                    {
                        writeOutputLine(hash);
                    }
                    else
                    {
                        writeLogLine("Cannot calcualte hash for " + name);
                        this_result = false;
                    }

                    result &= this_result;
                    if (!try_again)
                    {
                        if (!this_result)
                        {
                            try_again = true;
                        }
                    }
                    else
                    {
                        if (ExitOnFailure)
                        {
                            return result;
                        }
                        else
                        {
                            try_again = false;
                        }
                    }
                }
            }

            return result;
        }


        private bool performTest()
        {
            bool result = true;

            if (testType == null)
            {
                Console.Write(string.Format("Undefined test mode"));
                return false;
            }

            Console.Write(string.Format(CoinSparkTestTypeMethods.GetText(testType) + " test STARTED\n"));

            inputBR = null;
            outputFW = null;
            logFW = null;

            FileStream f = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

            if (!f.CanRead)
            {
                Console.Write(string.Format("Input file {0} not found\n", inputFile));
                result = false;
            }
            else
            {
                try
                {
                    inputBR = File.OpenText(inputFile);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(string.Format("Cannot open input file {0}", inputFile));
                    result = false;
                }
                try
                {
                    outputFW = File.CreateText(outputFile);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(string.Format("Cannot open output file {0}", outputFile));
                    result = false;
                }
                try
                {
                    logFW = File.CreateText(logFile);
                }
                catch (IOException ex)
                {
                    Console.Write(string.Format("Cannot open log file {0}\n", logFile));
                    result = false;
                }
            }

            switch (testType)
            {
                case CoinSparkTestType.ADDRESS:
                    result &= performAddressTest(true);
                    break;
                case CoinSparkTestType.GENESIS:
                    result &= performGenesisTest(true);
                    break;
                case CoinSparkTestType.HASH:
                    result &= performAssetHashTest(true);
                    break;
                case CoinSparkTestType.REFERENCE:
                    result &= performAssetRefTest(true);
                    break;
                case CoinSparkTestType.SCRIPT:
                    result &= performScriptTest(true);
                    break;
                case CoinSparkTestType.TRANSFER:
                    result &= performTransferTest(true);
                    break;
                case CoinSparkTestType.MESSAGE:
                    result &= performMessageHashTest(true);
                    break;
            }

            if (inputBR != null)
            {
                try
                {
                    inputBR.Dispose();
                }
                catch (IOException ex)
                {
                    //  Logger.getLogger(CoinSparkTest.class.getName()).log(Level.SEVERE, null, ex);
                }
            }
            if (outputFW != null)
            {
                try
                {
                    outputFW.Flush();
                    outputFW.Dispose();
                }
                catch (IOException ex)
                {
                    // Logger.getLogger(CoinSparkTest.class.getName()).log(Level.SEVERE, null, ex);
                }
            }
            if (inputBR != null)
            {
                try
                {
                    logFW.Flush();
                    logFW.Dispose();
                }
                catch (IOException ex)
                {
                    //  Logger.getLogger(CoinSparkTest.class.getName()).log(Level.SEVERE, null, ex);
                }
            }

            if (result)
            {
                Console.WriteLine(CoinSparkTestTypeMethods.GetText(testType) + " test COMPLETED SUCCESSFULLY");
            }
            else
            {
                Console.WriteLine(CoinSparkTestTypeMethods.GetText(testType) + " test COMPLETED WITH ERRORS!!!");
            }

            return result;
        }


        public static void main(string[] args)
        {
            TextReader br = Console.In;
            bool result = true;

            try
            {
                Console.WriteLine(string.Format("CoinSpark tests from existing input files"));
                Console.WriteLine(
                    string.Format("-----------------------------------------------------------------------"));
                Console.WriteLine();

                foreach (CoinSparkTestType tt in Enum.GetValues(typeof(CoinSparkTestType)).Cast<CoinSparkTestType>())
                {
                    Console.WriteLine(CoinSparkTestTypeMethods.GetLetter(tt) + ": " +
                                      CoinSparkTestTypeMethods.GetText(tt));
                }
                Console.WriteLine();

                Console.Write("Choose a test suite(s) to run [all]: ");

                string testMode = br.ReadLine();
                if (testMode.Length == 0)
                {
                    testMode = "ARSHGTM";
                }

                Console.Write(string.Format("Directory name for tests: "));
                string testFilePath = br.ReadLine();

                if (testFilePath.Length == 0)
                {
                    testFilePath = ".";
                }
                if (testFilePath.Length == 0)
                {
                    Console.WriteLine("Directory name not specified");
                    result = false;
                }
                else
                {
                    foreach (byte letter in Encoding.UTF8.GetBytes(testMode))
                    {
                        Console.WriteLine();
                        CoinSparkTestType testType =
                            CoinSparkTestTypeMethods.FromLetter(Encoding.UTF8.GetString(new byte[] {letter}));
                        if (testType != null)
                        {
                            CoinSparkTest test = new CoinSparkTest(testFilePath, testType);
                            result &= test.performTest();
                        }
                        else
                        {
                            Console.Write("Unsupported test mode " + letter + "\n");
                            result = false;
                        }
                    }
                }

            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
            }

            Console.Write(string.Format("\n"));
            Console.Write(string.Format("-----------------------------------------------------------------------\n"));
            Console.Write(string.Format("\n"));
            if (result)
            {
                Console.Write(string.Format("TESTS COMPLETED SUCCESSFULLY\n"));
            }
            else
            {
                Console.Write(string.Format("TESTS COMPLETED WITH ERRORS!!!\n"));
            }
            Console.Write(string.Format("\n"));

        }
    }
}