using DistributedGIS.ServerDataToGDBExtensions;
using DistributedGIS.Utils;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            object[] vs2 = new object[4];
            ConcurrentQueue<int> vs = new ConcurrentQueue<int>();
            ConcurrentBag<int> VS1 = new ConcurrentBag<int>();
            VS1.Add(1);
            VS1.Add(2);
            VS1.Add(3);
            VS1.Add(4);
            //vs.Prepend(1);
            //vs.Prepend(2);
            //vs.Prepend(3);
            //vs.Prepend(4);
            int i;
            VS1.TryTake(out i);
            Console.WriteLine(i);
            VS1.TryTake(out i);
            Console.WriteLine(i);
            VS1.TryTake(out i);
            Console.WriteLine(i);
            DistributedGIS.ESRI.Extensions.LicenseUtil.CheckOutLicenseAdvanced();
            TestJsonToPolygon();
            SpatialDataObtain spatialDataObtain = new SpatialDataObtain();
            spatialDataObtain.ServerDataToGDB("http://52.82.98.186:6080/arcgis/rest/services/YZT/YZTYLSJ/MapServer/31", "1=1", null, 1000);
            // SpatialDataObtain.ServerDataToGDB("http://52.82.98.186:6080/arcgis/rest/services/YZT/YZTYLSJ/MapServer/30", "1=1",null,10);
            Console.Read();
            RSA pubKeyRsaProvider = CreateRsaProviderFromPrivateKey("MIIEpQIBAAKCAQEAuVOrbikW3sddQCyNQR+CygA3Khp+ZyEE6S5lnUJS4u13S4ZT1Y578Ik9vO4dCSeNMLZaIGl8LgntHIDPHJy/YZdkTnF5/I3C7uKTsG2HbzcNHKlh16mqTNTKjCKbJ7IlLYbA3YbPb+UThQSKLwMJR10ONqSCOgahlRnR6fREZlG3yiNXd358T5njqsCgjhBcUyYmAcc5ae+a+bT0NvwAtlOiUkWtL5TMuB91RuZrkQIs68S8jIItKaQVmm79aHMOK+wRZfiHOW3vJzXBw1aeQ44blcrgyNlMsUDO5DVx9pluQxu+P+omXksTJmaYPdDfRlFw915zJiKhqfmK1N7bJwIDAQABAoIBAQCigOVfMRRmwDXaYbZylije65Vzi8uurltaBF8Bp/h6F3N1fBHivo+a+Rxzn+9B6bZ6MkuRBC3pSLbrOV9iXWfLMS65QfOPKtb1FSQfncsWEBWlPjkXHCTCPgxlGKsoAaNr6i8PkHgwS5P0HWj1lBn8w3IFI/c03j+tN/jCxnd7p0i6dK0Fzy+9oFbK9aI4jJsfJuOqI9qK8T5lwTi9yyD+3PD3lr8r+UOgHNOqLwRSn4yzcX6UkxUZ/1Xo806nscuyTly/F/IZXdBEzwLHjkbLHOF+6SEG1XrcFDAtisPT0WBwCOZmX5KGi8kcRS+jbiapZfxy3No1NWoKiguwMICBAoGBAO0z3vZosfvlaM6vmfdkdA5NDH/SYiqnScjf6d50/KaJdDzHq9HZcMusPXT/Lh4ZLF5nmVXOkEEXDbTIyvocsxIQZ5j9hMitmMnEOU6cEdzw2imNvRMg6zpxwTPksqMj/pIJGeY9I4IucnCxGi5ODE0qNTlLHDXQdhsmjpXz7SUHAoGBAMgDZQ2SkjU/6k1WZsKd0uwU3QZFUIbgUVVxHj7BXY8bezoBtL4cF7FaSRGK4Kkxt5IK1B9kucHjwauhtd6XvPC/TXwCinViLjeTy9T17CN5X0G/KVxuyAvhL30yVgx+EKvFEkn8Y0uNsuCO105k69ya6LPnoGsaZBo+cG0RgDDhAoGBAIAOkd9rlDMOne9/g9q4g6M423eiZ9bpK39jywmLFN7/tB4gGdWX0zpRyXgBT692Har1uSVG+D/7py2jfVlb5xzoeFVzAJ2qgqLi6aFTxp0F9nGakKnkCropsYlHfV1v0D6c5TuUZDgixSuroRvAjQmXsNY2g4tV+H0d6rpvzuY/AoGAOrskdTPQ0uoNtt/kFMmbIc+Oh0TbPH+p4ljw9KR7AIalTIcrt5cwLdfBFPGevo/mw2+CkiVVlmV2fRti+BW+WUMTUZK9bXKC97biZ4o+6Lu/CONFFetBuptJAo5BTjHGW2nm0OrXjVvyZpYLCu3/hCvIoOsm48xAEKS9BonZBoECgYEAi1muApbZJYalb9rtVUNCs0CE8Zz5X4jIHJdCDPNHVxWqOR1bKDiKI84lEteDbZp5zAggImssIadYWd3klgFVo85pE58wG9a7+UlxFmzu+Di8jj4Y2LlQql5oE8xbpF5lwZrgGEhhYGtZUDuz0pES7fV5xydymyzctlR6BcqWMQ0=");
            //JcVRfx9irMiGMhGslc6Mz5mnwwrfRTJQjNgPpnPrnv9X6WJoTKCRO8rw2Ji+gAWfM9u00qyj474+ZzNTxrmbNHXHbV3xmULJreoOvMkdZY8NMErBpXPB3Mt7J0yxZVCmnrSsu5fhwNkK5N0WtofWz+lLHd3U7mLBdsyqtDsCWiSur+5tg5PpFLx51gj5l3J0YWklsmEXHKDEgqzzY3XXRYsV2O7zgV8kjVu8fwlBJSvtiMVEpuxFUTKiceqQWiHceQawMamQ4GZfSr4ubCShgZxwzw7IuER8lomRZ+i3D6uTb3PsQGqdwZ9PFt81nrhClMdmDtUmXuiL2jfyTTclTQ==
            string pas = "JcVRfx9irMiGMhGslc6Mz5mnwwrfRTJQjNgPpnPrnv9X6WJoTKCRO8rw2Ji+gAWfM9u00qyj474+ZzNTxrmbNHXHbV3xmULJreoOvMkdZY8NMErBpXPB3Mt7J0yxZVCmnrSsu5fhwNkK5N0WtofWz+lLHd3U7mLBdsyqtDsCWiSur+5tg5PpFLx51gj5l3J0YWklsmEXHKDEgqzzY3XXRYsV2O7zgV8kjVu8fwlBJSvtiMVEpuxFUTKiceqQWiHceQawMamQ4GZfSr4ubCShgZxwzw7IuER8lomRZ+i3D6uTb3PsQGqdwZ9PFt81nrhClMdmDtUmXuiL2jfyTTclTQ==";
            // Console.WriteLine(lD4CknpHHHZxk68PUHOUaKnPcac2t5mNay3Svjc0x4p2ZgBiU0kaSrUIxx7S1t0Ztc47KcbMl0A2NRdSBV74mG9QU42KKvBgBwPP / Eh4JQA4LLeVRxoDTf4T8FLyVyaKT / 7oy6XqC2NGlOOcrrJbUd9dETRiw ==);


            string tt = Encoding.UTF8.GetString(pubKeyRsaProvider.Decrypt(Convert.FromBase64String(pas), RSAEncryptionPadding.Pkcs1));
            Console.WriteLine(Convert.ToString(Convert.FromBase64String("JSCeBYsEQpy7RWttqGI/w0RbEMYqUjZpPNjARHDi2MEX9IjcKwiSeIeUbpL450qIbqnNHOgBHo5vJDvHXNACHTiUqc4TKXgrXlXCY8P3zknfxjIOuYlmkDXoXGj/1nxvp0aJoRgqLs33gtjH15JUtEyn0g2lp8ZkJQMrUhV6R/8vJrnlRg1G4uS7hlD4CknpHHHZxk68PUHOUaKnPcac2t5mNay3Svjc0x4p2ZgBiU0kaSrUIxx7S1t0Ztc47KcbMl0A2NRdSBV74mG9QU42KKvBgBwPP/Eh4JQA4LLeVRxoDTf4T8FLyVyaKT/7oy6XqC2NGlOOcrrJbUd9dETRiw==")));
            Console.WriteLine("Hello World!");
        }
        public static void TestJsonToPolygon()
        {
            string json = "{\"rings\":[[[120.32892328100002,30.800848293],[120.32892502300001,30.800707623999983],[120.32889502499995,30.800257430999977],[120.32888342199999,30.800148931000024],[120.32878373699998,30.799215616000026],[120.32880196300005,30.799185717],[120.32896307399994,30.79916738999998],[120.32923182000002,30.799116826999978],[120.32951267399994,30.799026718999983],[120.32969864999995,30.798947569000006],[120.32978478899997,30.79893698500001],[120.32978258100002,30.798936592000018],[120.329715093,30.798924562000025],[120.32889056299996,30.799135509999985],[120.32840741400003,30.79911301499999],[120.32841549299997,30.799058419999994],[120.32853243600005,30.799054857999977],[120.32830239600003,30.799056683999993],[120.32829125399996,30.799171742999988],[120.32853309699999,30.799179379999998],[120.32856622899999,30.799182807000022],[120.32857849200002,30.799217371999987],[120.328721008,30.800029229000017],[120.32872694499997,30.800148103000026],[120.32873394499995,30.80028998199998],[120.328734747,30.800675802],[120.32873499899995,30.80079732500002],[120.32872976399995,30.800820871999974],[120.32871909200003,30.800868664999996],[120.32871043600005,30.80090819999998],[120.32867752699997,30.801011895999977],[120.32862135100004,30.801188737000018],[120.32866250300003,30.801210830000002],[120.32872915899998,30.801203457999975],[120.32876243600003,30.801174563000018],[120.32879561100003,30.801095254000018],[120.32880544199998,30.800836190999973],[120.32892328100002,30.800848293]]]}";
            JObject jObject = JObject.Parse(json);
            Polygon polygon = SpatialDataObtain.GeometryJsonToPolygon(jObject);
           
        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        /// <summary>
        /// 使用公钥创建RSA实例
        /// </summary>
        /// <param name="publicKeyBase64"></param>
        /// <returns></returns>
        public static RSA CreateRsaProviderFromPublicKey(string publicKeyBase64)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            var x509Key = Convert.FromBase64String(publicKeyBase64);

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (MemoryStream mem = new MemoryStream(x509Key))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    var rsa = RSA.Create();
                    RSAParameters rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);

                    return rsa;
                }

            }
        }

        public static RSA CreateRsaProviderFromPrivateKey(string privateKeyBase64)
        {
            var privateKeyBits = Convert.FromBase64String(privateKeyBase64);

            var rsa = RSA.Create();
            var rsaParameters = new RSAParameters();

            using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                rsaParameters.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.D = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.P = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Q = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DP = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DQ = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(rsaParameters);
            return rsa;
        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
            if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
    }
}
