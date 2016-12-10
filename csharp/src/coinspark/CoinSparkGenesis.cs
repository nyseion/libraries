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
using System.Linq;
using System.Text;

namespace coinspark { 

/**
 * CoinSparkGenesis class for managing asset genesis metadata
 */

public class CoinSparkGenesis : CoinSparkBase{

    
    public static readonly int COINSPARK_GENESIS_DOMAIN_NAME_MAX_LEN    = 32;
    public static readonly int COINSPARK_GENESIS_PAGE_PATH_MAX_LEN      = 24;
    
//	Public functions

    /**
     * CoinSparkGenesis class for managing asset genesis metadata
     */
    
    public CoinSparkGenesis()
    {
        clear();
    }
    
    /**
     * Set all fields in genesis to their default/zero values, which are not necessarily valid.
     */
    
    public void clear()
    {
        qtyExponent = 0;
        qtyMantissa = 0;
        chargeFlatMantissa = 0;
        chargeFlatExponent = 0;
        chargeBasisPoints = 0;                                                  // one hundredths of a percent
        assetHash = new byte[COINSPARK_GENESIS_HASH_MAX_LEN];
        assetHashLen = 0;                                                       // number of bytes in assetHash that are valid for comparison
        domainName="";
        pagePath="";
        useHttps=false;
        usePrefix=true;
    }
    
    /**
     * Returns Charge basis points 
     * 
     * @return Charge basis points 
     */
    
    public short getChargeBasisPoints() {
        return chargeBasisPoints;
    }

    /**
     * sets Charge basis points 
     * 
     * @param ChargeBasisPoints to set
     */
    
    public void setChargeBasisPoints(short ChargeBasisPoints) {
        chargeBasisPoints = ChargeBasisPoints;
    }

    /**
     * Returns Charge flat mantissa
     * 
     * @return Charge flat mantissa
     */
    
    public short getChargeFlatMantissa() {

        return chargeFlatMantissa;
    }

    /**
     * Sets Charge flat mantissa.
     * 
     * @param ChargeFlatMantissa to set
     */
    
    public void setChargeFlatMantissa(short ChargeFlatMantissa) {
        chargeFlatMantissa = ChargeFlatMantissa;
    }

    /**
     * Returns Charge flat exponent
     * 
     * @return Charge flat exponent
     */
    
    public short getChargeFlatExponent() {
        return chargeFlatExponent;
    }

    /**
     * Sets Charge flat exponent
     * 
     * @param ChargeFlatExponent to set
     */
    
    public void setChargeFlatExponent(short ChargeFlatExponent) {
        chargeFlatExponent = ChargeFlatExponent;
    }

    /**
     * Returns Quantity exponent
     * 
     * @return Quantity exponent
     */
    
    public short getQtyExponent() {

        return qtyExponent;
    }

    /**
     * Sets Quantity exponent
     * 
     * @param QtyExponent to set
     */
    
    public void setQtyExponent(short QtyExponent) {
        qtyExponent = QtyExponent;
    }
    
    /**
     * Returns Quantity mantissa
     * 
     * @return Quantity mantissa
     */

    public short getQtyMantissa() {
        return qtyMantissa;
    }

    /**
     * Sets Quantity mantissa
     * 
     * @param QtyMantissa to set
     */
    
    public void setQtyMantissa(short QtyMantissa) {
        qtyMantissa = QtyMantissa;
    }

    /**
     * Returns Domain Name
     * 
     * @return Domain Name
     */

    public string getDomainName() {
        return domainName;
    }

    /**
     * Sets DomainName
     * 
     * @param DomainName to set
     */
    
    public void setDomainName(string DomainName) {
        domainName = DomainName;
    }

    /**
     * Returns Page Path
     * 
     * @return Page Path
     */

    public string getPagePath() {
        return pagePath;
    }

    /**
     * Sets Page Path
     * 
     * @param PagePath to set
     */
    
    public void setPagePath(string PagePath) {
        pagePath = PagePath;
    }

    /**
     * Returns Use https flag
     * 
     * @return Use https flag
     */

    public bool getUseHttps() {
        return useHttps;
    }

    /**
     * Sets Use https flag
     * 
     * @param UseHttps to set
     */
    
    public void setUseHttps(bool UseHttps) {
        useHttps = UseHttps;
    }

    /**
     * Returns Use path prefix flag
     * 
     * @return Use path prefix flag
     */

    public bool getUsePrefix() {
        return usePrefix;
    }

    /**
     * Sets Use path prefix flag
     * 
     * @param UsePrefix flag to set
     */
    
    public void setUsePrefix(bool UsePrefix) {
        usePrefix = UsePrefix;
    }

    /**
     * Returns Asset hash
     * 
     * @return Asset hash
     */

    public byte [] getAssetHash() {
        return assetHash;
    }
    
    /**
     * Returns Asset hash length (to be) encoded
     * 
     * @return Asset hash length
     */

    public int getAssetHashLen() {
        return assetHashLen;
    }
    
    /**
     * Sets asset hash.
     * 
     * @param AssetHash to set
     */
    
    public void setAssetHash(byte [] AssetHash) {
        assetHash=Arrays.copyOfRange(AssetHash, 0, AssetHash.Length);
    }
    
    /**
     * Sets asset hash.
     * 
     * @param AssetHashLen to set
     */
    
    public void setAssetHashLen(int AssetHashLen) {
        assetHashLen=AssetHashLen;
    }
    
    //@Override
    public override string ToString()
    {
        long quantity = getQty();
        int quantityEncoded = (qtyExponent * COINSPARK_GENESIS_QTY_EXPONENT_MULTIPLE + qtyMantissa) &
                                COINSPARK_GENESIS_QTY_MASK;

        long chargeFlat = getChargeFlat();
        int chargeFlatEncoded = chargeFlatExponent*COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MULTIPLE + chargeFlatMantissa;

        StringBuilder sb = new StringBuilder();
        
        CoinSparkBuffer assetWebPageBuffer=new CoinSparkBuffer();
        string encodedWebPage="";
        
        CoinSparkDomainPath assetWebPage=new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
        if(assetWebPage.encode(assetWebPageBuffer,false))
        {
            encodedWebPage=assetWebPageBuffer.toHex();
        }
        
        sb.Append("COINSPARK GENESIS\n")
                .Append(string.Format("   Quantity mantissa: {0}\n", qtyMantissa))
                .Append(string.Format("   Quantity exponent: {0}\n", qtyExponent))
                .Append(string.Format("    Quantity encoded: {0} (small endian hex {1})\n", quantityEncoded,
                        unsignedToSmallEndianHex(quantityEncoded, 2)))
                .Append(string.Format("      Quantity value: {0}\n", quantity))
                .Append(string.Format("Flat charge mantissa: {0}\n", chargeFlatMantissa))
                .Append(string.Format("Flat charge exponent: {0}\n", chargeFlatExponent))
                .Append(string.Format(" Flat charge encoded: {0} (small endian hex {1})\n", chargeFlatEncoded,
                        unsignedToSmallEndianHex(chargeFlatEncoded, COINSPARK_GENESIS_CHARGE_FLAT_LENGTH)))
                .Append(string.Format("   Flat charge value: {0}\n", chargeFlat))
                .Append(string.Format(" Basis points charge: {0} (hex {1})\n", chargeBasisPoints,
                        unsignedToSmallEndianHex(chargeBasisPoints, COINSPARK_GENESIS_CHARGE_BPS_LENGTH)))
                .Append(string.Format("           Asset URL: {0}://{1}/{2}{3}/ (length {4}+{5} encoded {6} length {7})\n",
                        assetWebPage.useHttps ? "https" : "http", assetWebPage.domainName, 
                        assetWebPage.usePrefix ? "coinspark/" : "",
                        (assetWebPage.path.Length>0) ? assetWebPage.path : "[spent-txid]",
                        assetWebPage.domainName.Length,assetWebPage.path.Length,
                        encodedWebPage,assetWebPage.encodedLen(false)))
                .Append("          Asset hash: ")
                .Append(byteToHex(Arrays.copyOfRange(assetHash, 0, assetHashLen)))
                .Append(string.Format(" (length {0})\n", assetHashLen))
                .Append("END COINSPARK GENESIS\n\n");

        return  sb.ToString();
    }

    /**
     * Returns true if the two CoinSparkGenesis structures are the same. If strict is true then
     * the qtyMantissa, qtyExponent, chargeFlatMantissa and chargeFlatExponent fields must be identical.
     * If strict is false then it is enough if each pair just represents the same readonly quantity.
     *
     * @param genesis2 CoinSparkGenesis to compare with
     * @param strict Strict comparison flag
     * @return true if two CoinSparkGenesis match, false otherwise
     */
    
    
    public bool match(CoinSparkGenesis genesis2, bool strict)
    {
        bool floatQuantitiesMatch;
        int hashCompareLen = Math.Min(assetHashLen, genesis2.assetHashLen);
        hashCompareLen = Math.Min(hashCompareLen, COINSPARK_GENESIS_HASH_MAX_LEN);

        CoinSparkDomainPath assetWebPage=new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
        CoinSparkDomainPath assetWebPage2=new CoinSparkDomainPath(genesis2.getDomainName(), 
                genesis2.getPagePath(), genesis2.getUseHttps(), genesis2.getUsePrefix());
        
        if (strict)
            floatQuantitiesMatch=(qtyMantissa == genesis2.qtyMantissa) &&
                    (qtyExponent == genesis2.qtyExponent) &&
                    (chargeFlatMantissa == genesis2.chargeFlatMantissa) &&
                    (chargeFlatExponent == genesis2.chargeFlatExponent);
        else
            floatQuantitiesMatch=(getQty() == genesis2.getQty()) &&
                    (getChargeFlat()==genesis2.getChargeFlat());

        return
                floatQuantitiesMatch && (chargeBasisPoints == genesis2.chargeBasisPoints) &&
                        assetWebPage.match(assetWebPage2) && 
                        (memcmp(assetHash, genesis2.assetHash, hashCompareLen) == 0)
                ;
    }
    
    /**
     * Returns true if all values in the genesis are in their permitted ranges, false otherwise.
     * 
     * @return true if genesis structure is valid
     */
    
    public bool isValid()
    {
        if ( (qtyMantissa<COINSPARK_GENESIS_QTY_MANTISSA_MIN) || (qtyMantissa>COINSPARK_GENESIS_QTY_MANTISSA_MAX) )
            return false;

        if ( (qtyExponent<COINSPARK_GENESIS_QTY_EXPONENT_MIN) || (qtyExponent>COINSPARK_GENESIS_QTY_EXPONENT_MAX) )
            return false;

        if ( (chargeFlatExponent<COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MIN) ||
             (chargeFlatExponent>COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MAX) )
            return false;

        if (chargeFlatMantissa<COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MIN)
            return false;

        if (chargeFlatMantissa > ((chargeFlatExponent==COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MAX) ?
                COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX_IF_EXP_MAX : COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX))
            return false;

        if ( (chargeBasisPoints<COINSPARK_GENESIS_CHARGE_BASIS_POINTS_MIN) ||
             (chargeBasisPoints>COINSPARK_GENESIS_CHARGE_BASIS_POINTS_MAX) )
            return false;
        
        CoinSparkDomainPath assetWebPage=new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
        if (!assetWebPage.isValid())
            return false;
        
        if ( (assetHashLen<COINSPARK_GENESIS_HASH_MIN_LEN) || (assetHashLen>COINSPARK_GENESIS_HASH_MAX_LEN) )
            return false;

        return true;
    }

    /**
     * Returns the number of units denoted by the genesis qtyMantissa and qtyExponent fields.
     *
     * @return the number of units denoted by the genesis qtyMantissa and qtyExponent fields.
     */
    
    public long getQty()
    {
        return new CoinSparkAssetQty(qtyMantissa, qtyExponent).value;
    }

    /**
     * Sets the qtyMantissa and qtyExponent fields in genesis to be as close to desiredQty as possible.
     * Set rounding to [-1, 0, 1] for rounding [down, closest, up] respectively.
     * Returns the quantity that was actually encoded, via CoinSparkGenesisGetQty().
     *
     * @param desiredQty desired quantity
     * @param rounding [-1, 0, 1] for rounding [down, closest, up] respectively.
     * @return  the quantity that was actually encoded
     */
    
    public long setQty(long desiredQty, int rounding)
    {
        CoinSparkAssetQty qty=new CoinSparkAssetQty(desiredQty, rounding,
                COINSPARK_GENESIS_QTY_MANTISSA_MAX, COINSPARK_GENESIS_QTY_EXPONENT_MAX);
        
        qtyMantissa = (short)qty.mantissa;
        qtyExponent = (short)qty.exponent;        
        
        return getQty();
    }

    /**
     * Returns the number of units denoted by the genesis chargeFlatMantissa and chargeFlatExponent fields.
     * 
     * @return the number of units denoted by the genesis chargeFlatMantissa and chargeFlatExponent fields.
     */
    
    public long getChargeFlat()
    {
        return new CoinSparkAssetQty(chargeFlatMantissa, chargeFlatExponent).value;
    }

    /**
     * Sets the chargeFlatMantissa and chargeFlatExponent fields in genesis to be as close to desiredChargeFlat as possible.
     * Set rounding to [-1, 0, 1] for rounding [down, closest, up] respectively.
     * Returns the quantity that was actually encoded, via CoinSparkGenesisGetChargeFlat().
     *
     * @param desiredChargeFlat desired quantity
     * @param rounding [-1, 0, 1] for rounding [down, closest, up] respectively.
     * @return the quantity that was actually encoded
     */
    
    public long setChargeFlat(long desiredChargeFlat, int rounding)
    {
        CoinSparkAssetQty qty=new CoinSparkAssetQty(desiredChargeFlat, rounding,
                COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX, COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MAX);
        
        chargeFlatMantissa = (short)qty.mantissa;
        chargeFlatExponent = (short)qty.exponent;
        
        if (chargeFlatExponent == COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MAX)
            chargeFlatMantissa  = (short)Math.Min((int)chargeFlatMantissa, COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX_IF_EXP_MAX);
        
        return getChargeFlat();
    }

    /**
     * Calculates the payment charge specified by genesis for sending the raw quantity qtyGross.
     *
     * @param qtyGross quantity to send
     * @return the payment charge 
     */
    
    public long calcCharge(long qtyGross)
    {
        long charge = getChargeFlat() +(qtyGross*chargeBasisPoints+5000)/10000; // rounds to nearest
        if (charge > qtyGross)// can't charge more than the readonly amount
            charge = qtyGross;
        
        return charge;
    }

    /**
     * Calculates the quantity that will be received after the payment charge specified by genesis is applied to qtyGross.
     *
     * @param qtyGross quantity to send
     * @return the quantity that will be received
     */
    
    public long calcNet(long qtyGross)
    {
        return qtyGross - calcCharge(qtyGross);
    }

    /**
     * Calculates the quantity that should be sent so that, after the payment charge specified by genesis
     * is applied, the recipient will receive qtyNet units.
     *
     * @param qtyNet quantity to receive
     * @return the quantity that should be sent
     */
    
    public long calcGross(long qtyNet)
    {
        if (qtyNet <=0)
            return 0;                                    // no point getting past charges if we end up with zero anyway


        long lowerGross = ((qtyNet + getChargeFlat()) * 10000)/
                (10000-chargeBasisPoints);                                      // divides rounding down

        if (calcNet(lowerGross) < qtyNet)
            lowerGross+=1;
        return lowerGross;
    }

    /**
     * Calculates the appropriate asset hash length of genesis so that when encoded as metadata the genesis will
     * fit in metadataMaxLen bytes. For now, set metadataMaxLen to 40 (see Bitcoin's MAX_OP_RETURN_RELAY parameter).
     *
     * @param metadataMaxLen - metadata maximal length
     * @return asset hash length of genesis
     */
    
    public int calcHashLen(int metadataMaxLen)
    {
        int HashLen = metadataMaxLen-COINSPARK_METADATA_IDENTIFIER.Length-1-COINSPARK_GENESIS_QTY_FLAGS_LENGTH;

        if (chargeFlatMantissa>0)
            HashLen-=COINSPARK_GENESIS_CHARGE_FLAT_LENGTH;

        if (chargeBasisPoints>0)
            HashLen-=COINSPARK_GENESIS_CHARGE_BPS_LENGTH;

        
        CoinSparkDomainPath assetWebPage=new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
        HashLen-=assetWebPage.encodedLen(false);

        if (HashLen > COINSPARK_GENESIS_HASH_MAX_LEN)
            HashLen = COINSPARK_GENESIS_HASH_MAX_LEN;

        return HashLen;
    }
    
    /**
     * Encodes the genesis into metadata (maximal size is CoinSparkBase.OP_RETURN_MAXIMUM_LENGTH);
     * 
     * @return string | null Encoded genesis as hexadecimal, null if we failed.
     */
    /*
    public string encodeToHex()
    {
        return encodeToHex(OP_RETURN_MAXIMUM_LENGTH);
    }
    */
    
    /**
     * Encodes the genesis into metadata (maximal size is metadataMaxLen);
     * 
     * @param metadataMaxLen maximal size of encoded data
     * @return string | null Encoded genesis as hexadecimal, null if we failed.
     */
    
    public string encodeToHex(int metadataMaxLen)
    {
        CoinSparkBuffer buffer=new CoinSparkBuffer();
        if(!encode(buffer,metadataMaxLen))
        {
            return null;
        }
        
        return buffer.toHex();
    }
    
    /**
     * Encodes the genesis into metadata (maximal size is CoinSparkBase.OP_RETURN_MAXIMUM_LENGTH);
     * 
     * @return byte [] | null Encoded genesis as raw data, null if we failed.
     */
    /*    
    public byte [] encode()
    {        
        return encode(OP_RETURN_MAXIMUM_LENGTH);
    }
    */
    /**
     * Encodes the genesis into metadata (maximal size is metadataMaxLen);
     * 
     * @param metadataMaxLen maximal size of encoded data
     * @return byte [] | null Encoded genesis as hexadecimal, null if we failed.
     */
    
    public byte [] encode(int metadataMaxLen)
    {
        CoinSparkBuffer buffer=new CoinSparkBuffer();
        
        if(!encode(buffer,metadataMaxLen))
        {
            return null;
        }
        
        return buffer.toBytes();
    }
    
    /**
     * Decodes the genesis in metadata  into paymentRef.
     * 
     * @param metadata Metadata to decode as hexadecimal
     * @return true on success, false on failure
     */
    
    public bool decode(string metadata)
    {
        CoinSparkBuffer buffer=new CoinSparkBuffer(metadata, true);
        return decode(buffer);
    }
    
    /**
     * Decodes the genesis in metadata  into paymentRef.
     * 
     * @param metadata Metadata to decode as raw data
     * @return true on success, false on failure
     */
    
    public bool decode(byte [] metadata)
    {
        CoinSparkBuffer buffer=new CoinSparkBuffer(metadata);
        return decode(buffer);
    }
    
    
    /**
     * Returns the minimum transaction fee (in bitcoin satoshis) required to make the genesis transaction valid.
     * Pass the number of bitcoin satoshis in each output in outputsSatoshis (array size countOutputs).
     * Use CoinSparkScriptIsRegular() to pass an array of bools in outputsRegular for whether each output script is regular.
     *
     * @param outputsSatoshis array of output values
     * @param outputsRegular pass array of bools for whether each output script is regular
     * @return minimum transaction fee
     */
    
    public long calcMinFee(long[] outputsSatoshis, bool[] outputsRegular)
    {
        return getMinFeeBasis(outputsSatoshis, outputsRegular) *
                countNonLastRegularOutputs(outputsRegular);
     }

    /**
     * For the asset specified by genesis, calculate the number of newly created asset units in each
     * output of the genesis transaction into the outputBalances array (size countOutputs).
     * Use CoinSparkScriptIsRegular() to pass an array of bools in outputsRegular for whether each output script is regular.
     * ** This is only relevant if the transaction DOES HAVE a sufficient fee to make the genesis valid **
     * 
     * @param outputsRegular array of bools in outputsRegular for whether each output script is regular
     * @return output balances 
    */
    
    public long [] apply(bool[] outputsRegular)
    {
        int countOutputs=outputsRegular.Length;
        
        long [] outputBalances=new long [countOutputs];
        long qtyPerOutput;

        int lastRegularOutput = getLastRegularOutput(outputsRegular);
        int divideOutputs = countNonLastRegularOutputs(outputsRegular);
        long genesisQty = getQty();

        if (divideOutputs==0)
            qtyPerOutput = 0;
        else
            qtyPerOutput = genesisQty / (divideOutputs); // rounds down

        long extraFirstOutput = genesisQty - (qtyPerOutput * divideOutputs);

        for (int outputIndex=0; outputIndex<countOutputs; outputIndex++)
        {
            outputBalances[outputIndex] = 0;
            if (outputsRegular[outputIndex] && (outputIndex!=lastRegularOutput)) {
                outputBalances[outputIndex] = qtyPerOutput +  extraFirstOutput;
                extraFirstOutput = 0; // so it will only contribute to the first
            } else
                outputBalances[outputIndex] = 0;
        }
        
        return outputBalances;
    }

    /**
     * Calculates the URL for the asset web page of genesis.
     * 
     * @param FirstSpentTxID if path=null or path.Length=0 pass the previous txid whose output was spent by the first input of the genesis
     * @param FirstSpentVout if path=null or path.Length=0 pass the output index of firstSpentTxID spent by the first input of the genesis
     * @return string | null URL of the Asset web page, null on failure
     */
    
    public string calcAssetURL(string FirstSpentTxID,long FirstSpentVout)
    {
        CoinSparkDomainPath assetWebPage=new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
        return assetWebPage.getAssetURL(FirstSpentTxID, FirstSpentVout);
    }    
    
    /**
     * Calculates the URL for the home page, based on domain and useHttps flag
     * 
     * @return string | null URL of the domain home page, null on failure
     */
    
    
    public string getDomainURL()
    {
        CoinSparkDomainPath assetWebPage=new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
        return assetWebPage.getDomainURL();
    }    
    
    /**
     * Calculates the assetHash for the key information from a CoinSpark asset web page JSON specification.
     * All char* string parameters except contractContent must be passed using UTF-8 encoding.
     * You may pass NULL (and if appropriate, a length of zero) for any parameter which was not in the JSON.
     * Note that you need to pass in the contract *content* and length, not its URL.
     * To call this you must provide a CoinSparkCalcSHA256Hash() function (prototype below) in your code.
     *
     * @param name asset name
     * @param issuer issuer name
     * @param description asset description
     * @param units asset units
     * @param issueDate issue data
     * @param expiryDate expiry date, if not specified pass null or zero-length String
     * @param interestRate interest rate
     * @param multiple asset multiple
     * @param contractContent contract
     * 
     * @return asset hash or null on failure
    */
    
    public static byte [] calcAssetHash(string name, string issuer, string description, string units, string issueDate, string expiryDate,
                           Double interestRate, Double multiple, byte [] contractContent)
    {
        int bufferSize=1024;
        bufferSize += (name != null) ? Encoding.UTF8.GetBytes(name).Length : 0;
        bufferSize += (issuer != null) ? Encoding.UTF8.GetBytes(issuer).Length : 0;
        bufferSize += (description != null) ? Encoding.UTF8.GetBytes(description).Length : 0;
        bufferSize += (units != null) ? Encoding.UTF8.GetBytes(units).Length : 0;
        bufferSize += (issueDate != null) ? Encoding.UTF8.GetBytes(issueDate).Length : 0;
        bufferSize += (expiryDate != null) ? Encoding.UTF8.GetBytes(expiryDate).Length : 0;
        bufferSize += (contractContent != null) ? contractContent.Length : 0;
        
        byte[] buffer = new byte[bufferSize ];
        int offset = 0;
        
        offset+=addToHashBuffer(name, buffer, offset);
        offset+=addToHashBuffer(issuer, buffer, offset);
        offset+=addToHashBuffer(description, buffer, offset);
        offset+=addToHashBuffer(units, buffer, offset);
        offset+=addToHashBuffer(issueDate, buffer, offset);
        offset+=addToHashBuffer(expiryDate, buffer, offset);

            
        long interestRateToHash = (long)Math.Round(Math.Floor(((interestRate != null)  ? interestRate : 0)*1000000.0+0.5));
        long multipleToHash = (long)Math.Round(Math.Floor(((multiple != null) ? multiple : 1)*1000000.0+0.5));
        
        string temp = string.Format("{0}", interestRateToHash);
        offset+=addToHashBuffer(temp, buffer, offset);
        temp = string.Format("{0}", multipleToHash);
        offset+=addToHashBuffer(temp, buffer, offset);

        if(contractContent != null)
        {
                Array.Copy(contractContent, 0, buffer, offset, contractContent.Length);
              //  System.arraycopy(contractContent, 0, buffer, offset, contractContent.Length);
            offset += contractContent.Length+1;buffer[offset-1]=0x00;
        }
        else
        {
            offset+=addToHashBuffer(null, buffer, offset);
        }

        return coinSparkCalcSHA256Hash(buffer, offset);
    }
    
    /**
     * Compares given hash with value encoded in genesis.
     * 
     * @param AssetHashToCheck asset hash to validate
     * @return true if hashes match, false otherwise
     */
    
    public bool validateAssetHash(byte [] AssetHashToCheck)
    {
        if(AssetHashToCheck == null)
        {
            return false;
        }

        if(AssetHashToCheck.Length < assetHashLen)
        {
            return false;
        }
        
        byte [] arr1=Arrays.copyOf(assetHash, assetHashLen);
        byte [] arr2=Arrays.copyOf(AssetHashToCheck, assetHashLen);

        return arr1.SequenceEqual(arr2);        
    }

    
// Private variables/constants/functions       
        
    
    private const int      COINSPARK_GENESIS_QTY_FLAGS_LENGTH = 2;
    private const int      COINSPARK_GENESIS_QTY_MASK = 0x3FFF;
    private const int      COINSPARK_GENESIS_QTY_EXPONENT_MULTIPLE = 1001;
    private const int      COINSPARK_GENESIS_FLAG_CHARGE_FLAT = 0x4000;
    private const int      COINSPARK_GENESIS_FLAG_CHARGE_BPS = 0x8000;
    private const int      COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MULTIPLE = 101;
    private const int      COINSPARK_GENESIS_CHARGE_FLAT_LENGTH = 1;
    private const int      COINSPARK_GENESIS_CHARGE_BPS_LENGTH = 1;

    private const int      COINSPARK_GENESIS_QTY_MANTISSA_MIN                  = 1;
    private const short    COINSPARK_GENESIS_QTY_MANTISSA_MAX                  = 1000;
    private const int      COINSPARK_GENESIS_QTY_EXPONENT_MIN                  = 0;
    private const short    COINSPARK_GENESIS_QTY_EXPONENT_MAX                  = 11;
    private const int      COINSPARK_GENESIS_CHARGE_FLAT_MAX                   = 5000;
    private const int      COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MIN = 0;
    private const short    COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX = 100;
    private const int      COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX_IF_EXP_MAX = 50;
    private const int      COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MIN = 0;
    private const short    COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MAX = 2;
    private const int      COINSPARK_GENESIS_CHARGE_BASIS_POINTS_MIN = 0;
    private const int      COINSPARK_GENESIS_CHARGE_BASIS_POINTS_MAX = 250;
    private const int      COINSPARK_GENESIS_HASH_MIN_LEN = 12;
    private const int      COINSPARK_GENESIS_HASH_MAX_LEN = 32;


    private short qtyExponent;
    private short qtyMantissa;
    private short chargeFlatMantissa;
    private short chargeFlatExponent;
    private short chargeBasisPoints; // one hundredths of a percent
    private byte[] assetHash = new byte[COINSPARK_GENESIS_HASH_MAX_LEN];
    private int assetHashLen; // number of bytes in assetHash that are valid for comparison
//    private CoinSparkDomainPath assetWebPage;
    private string domainName;
    private string pagePath;
    private bool useHttps;
    private bool usePrefix;


    
    private bool encode(CoinSparkBuffer buffer,int metadataMaxLen)
    {
        try
        {
            if (!isValid())
                throw new Exception("invalid genesis");

            buffer.writeString(COINSPARK_METADATA_IDENTIFIER);
            buffer.writeByte(COINSPARK_GENESIS_PREFIX);
            
            //  Quantity mantissa and exponent
            
            int quantityEncoded = (qtyExponent * COINSPARK_GENESIS_QTY_EXPONENT_MULTIPLE + qtyMantissa) &
                    COINSPARK_GENESIS_QTY_MASK;
            if (chargeFlatMantissa>0)
                quantityEncoded|=COINSPARK_GENESIS_FLAG_CHARGE_FLAT;
            if (chargeBasisPoints>0)
                quantityEncoded|=COINSPARK_GENESIS_FLAG_CHARGE_BPS;
            
            buffer.writeInt(quantityEncoded, COINSPARK_GENESIS_QTY_FLAGS_LENGTH);

            //  Charges - flat and basis points

            if ((quantityEncoded & COINSPARK_GENESIS_FLAG_CHARGE_FLAT) != 0)
            {
                int chargeEncoded=chargeFlatExponent*COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MULTIPLE+chargeFlatMantissa;

                buffer.writeInt(chargeEncoded, COINSPARK_GENESIS_CHARGE_FLAT_LENGTH);
            }

            if ((quantityEncoded & COINSPARK_GENESIS_FLAG_CHARGE_BPS) != 0)
            {
                buffer.writeInt(chargeBasisPoints, COINSPARK_GENESIS_CHARGE_BPS_LENGTH);
            }

            //  Asset web page
            CoinSparkDomainPath assetWebPage = new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
            if (!assetWebPage.encode(buffer,false))
                throw new Exception("cannot write domain name/path");

            //  Asset hash
            
            buffer.writeBytes(assetHash, assetHashLen);
            
            if(buffer.toBytes().Length>metadataMaxLen)
                throw new Exception("total length above limit");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        
        return true;
    }

    
    private bool decode(CoinSparkBuffer buffer)
    {
        int quantityEncoded, chargeEncoded;
        if(!buffer.locateRange(COINSPARK_GENESIS_PREFIX))
            return false;

        try
        {            
            //  Quantity mantissa and exponent
            
            if(buffer.canRead(COINSPARK_GENESIS_QTY_FLAGS_LENGTH))
            {
                quantityEncoded = buffer.readInt(COINSPARK_GENESIS_QTY_FLAGS_LENGTH);

                qtyMantissa = (short)((quantityEncoded&COINSPARK_GENESIS_QTY_MASK) % COINSPARK_GENESIS_QTY_EXPONENT_MULTIPLE);
                qtyExponent = (short)((quantityEncoded&COINSPARK_GENESIS_QTY_MASK) / COINSPARK_GENESIS_QTY_EXPONENT_MULTIPLE);

                if ((qtyMantissa < COINSPARK_GENESIS_QTY_MANTISSA_MIN) ||
                        (qtyMantissa > COINSPARK_GENESIS_QTY_MANTISSA_MAX) )
                    throw new Exception("mantissa out of range");

                if ((qtyExponent < COINSPARK_GENESIS_QTY_EXPONENT_MIN) ||
                        (qtyExponent > COINSPARK_GENESIS_QTY_EXPONENT_MAX) )
                    throw new Exception("exponent out of range");
            }
            else
                throw new Exception("cannot read genesis flags");

            //  Charges - flat and basis points
            
            if ((quantityEncoded & COINSPARK_GENESIS_FLAG_CHARGE_FLAT) !=0)
            {
                if(buffer.canRead(COINSPARK_GENESIS_CHARGE_FLAT_LENGTH))
                {
                    chargeEncoded = buffer.readInt(COINSPARK_GENESIS_CHARGE_FLAT_LENGTH);

                    chargeFlatMantissa = (short)(chargeEncoded % COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MULTIPLE);
                    chargeFlatExponent = (short)(chargeEncoded / COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MULTIPLE);

                    if ( (chargeFlatExponent<COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MIN) ||
                            (chargeFlatExponent>COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MAX) )
                        throw new Exception("flat exponent out of range");

                    if (chargeFlatMantissa<COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MIN)
                        throw new Exception("flat mantissa out of range");

                    if (chargeFlatMantissa > (
                            (chargeFlatExponent==COINSPARK_GENESIS_CHARGE_FLAT_EXPONENT_MAX) ?
                                    COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX_IF_EXP_MAX : COINSPARK_GENESIS_CHARGE_FLAT_MANTISSA_MAX))
                        throw new Exception("flat mantissa out of range");

                }
                else
                    throw new Exception("cannot read charge flat");

            }
            else
            {
                chargeFlatMantissa=0;
                chargeFlatExponent=0;
            }

            if ((quantityEncoded & COINSPARK_GENESIS_FLAG_CHARGE_BPS) != 0)
            {
                if(buffer.canRead(COINSPARK_GENESIS_CHARGE_BPS_LENGTH))
                {
                    chargeBasisPoints = (short)buffer.readInt(COINSPARK_GENESIS_CHARGE_BPS_LENGTH);

                    if ( (chargeBasisPoints<COINSPARK_GENESIS_CHARGE_BASIS_POINTS_MIN) ||
                            (chargeBasisPoints>COINSPARK_GENESIS_CHARGE_BASIS_POINTS_MAX) )
                        throw new Exception("basic points out of range");

                }
                else
                    throw new Exception("cannot read basic points");

            } else
                chargeBasisPoints=0;

            //  Domain name
            
            CoinSparkDomainPath assetWebPage=new CoinSparkDomainPath(domainName, pagePath, useHttps, usePrefix);
            if (!assetWebPage.decode(buffer,false))
                throw new Exception("cannot decode domain name");

            domainName=assetWebPage.domainName;
            pagePath=assetWebPage.path;
            useHttps=assetWebPage.useHttps;
            usePrefix=assetWebPage.usePrefix;
            
            //  Hash of key information
            
            assetHashLen = buffer.availableForRead();//TBD loss
            assetHashLen = Math.Min(assetHashLen, COINSPARK_GENESIS_HASH_MAX_LEN); // apply maximum

            if (assetHashLen < COINSPARK_GENESIS_HASH_MIN_LEN)
                // not enough hash data
                throw new Exception("has data out of range");

            assetHash=buffer.readBytes(assetHashLen);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        
        return isValid();
    }

    private static int countNonLastRegularOutputs(bool[] outputsRegular)
    {        
        int countRegularOutputs, outputIndex;
        int countOutputs=outputsRegular.Length;
        countRegularOutputs=0;

        for (outputIndex=0; outputIndex<countOutputs; outputIndex++) {
            if (outputsRegular[outputIndex])
                countRegularOutputs++;
        }

        return countRegularOutputs > 1 ? countRegularOutputs-1 :  0;
    }
    
    private static string trimForHash(string Source)
    {
        if(Source == null)
        {
            return null;
        }
        
        bool keepTrimming;
        int from = 0;
        int to = Source.Length-1;
        
        keepTrimming = true;
        while(keepTrimming && (from<Source.Length))
        {
            switch ((int)Source[from])
            {
                case 0x09: case 0x0A: case 0x0D: case 0x20:
                    from++;
                    break;
                default:
                    keepTrimming = false;
                    break;
            }            
        }
        
        keepTrimming = true;
        while(keepTrimming && (to>=0))
        {
            switch ((int)Source[(to)])
            {
                case 0x09: case 0x0A: case 0x0D: case 0x20:
                    to--;
                    break;
                default:
                    keepTrimming = false;
                    break;
            }            
        }

        to++;
        
        if(from>=to)
        {
            return null;
        }
        
        return Source.Substring(from,to-from); //todo
    }
    
    private static int addToHashBuffer(string str, byte[] buffer, int offset)
    {
        string trimmed=trimForHash(str);
        if (!string.IsNullOrEmpty(trimmed))
        {
            Array.Copy(Encoding.UTF8.GetBytes(trimmed),0,buffer,offset,trimmed.Length);
            //System.arraycopy(Encoding.UTF8.GetBytes(trimmed), 0, buffer, offset, trimmed.Length);
            buffer[offset+trimmed.Length] = 0x00;
            return trimmed.Length+1;
        }
        buffer[offset]=0x00;
        return 1;
    }
    
}
}