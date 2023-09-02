using System.Xml;
using static System.Console;
using static System.Environment;
using static System.IO.Path;
using System.IO.Compression; //BrotliStream,GZipStream, CompressionMode

//Viper.WorkWithText();
//Viper.WorkWithXml();
Viper.workWithCompression();
Viper.workWithCompression(useBrotil : false);
static class Viper
{
    public static string[] Callsigns = new[]
    {
        "Husker","Starbuck","Apollo","Boomer",
        "Bulldog","Athena","Helo","Racetrack"
    };

    public static void WorkWithText()
    {
        string textFile = Combine(CurrentDirectory, "streams.txt");

        //파일 생성
        StreamWriter text = File.CreateText(textFile);

        foreach (string item in Viper.Callsigns)
        {
            text.WriteLine(item);
        }
        text.Close();
        //출력
        WriteLine("{0} conatins {1:N0} bytes.", arg0: textFile, arg1: new FileInfo(textFile).Length);
        WriteLine(File.ReadAllText(textFile));
    }

   public static void WorkWithXml()
    {

        FileStream? xmlFileStream = null;
        XmlWriter? xml = null;
        try
        {
            //파일 정의
            string xmlFile = Combine(CurrentDirectory, "streams.xml");

            //파일스트림 생성
             xmlFileStream = File.Create(xmlFile);

            //Xml writer 헬퍼 -> 파일 스르팀 래핑 후 중첩요소 들여오기
             xml = XmlWriter.Create(xmlFileStream, new XmlWriterSettings { Indent = true });

            //파일에 Xml선언
            xml.WriteStartDocument();

            //root element
            xml.WriteStartElement("callsigns");

            foreach (string item in Viper.Callsigns)
            {
                xml.WriteElementString("callsign", item);
            }

            xml.WriteEndElement();
            xml.Close();
            xmlFileStream.Close();

            WriteLine("{0} contains {1:N0} bytes.", arg0: xmlFile, arg1: new FileInfo(xmlFile).Length);
            WriteLine(File.ReadAllText(xmlFile));
        }catch(Exception ex)
        {
            //경로 미존재
            WriteLine($"{ex.GetType()} says {ex.Message}");
        }
        finally
        {
            if(xml != null)
            {
                xml.Dispose();
                WriteLine("The XML writer`s unmanaged resources have been disposed.");
            }
            if(xmlFileStream != null)
            {
                xmlFileStream.Close();
                WriteLine("The file stream`s unmanaged resources have been disposed.");
            }
        }
    }

    public static void workWithCompression(bool useBrotil = true)
    {
        string fileExt = useBrotil ? "brotil" : "gzip";
        string filePath = Combine(CurrentDirectory, $"stream.{fileExt}");
        FileStream file = File.Create(filePath);

        Stream compressor;
        if(useBrotil)
        {
            compressor = new BrotliStream(file, CompressionMode.Compress);
        }
        else
        {
            compressor = new GZipStream(file, CompressionMode.Compress);
        }

        using (compressor)
        {
            using(XmlWriter xml = XmlWriter.Create(compressor))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("callsigns");
                foreach(string item in Viper.Callsigns)
                {
                    xml.WriteStartElement("callsign",item);
                }
            }
        }

        //압축파일 내용출력
        WriteLine("{0} contains {1:N0} bytes.", filePath, new FileInfo(filePath).Length);
       // WriteLine($"The compressed contents: ");
        //WriteLine(File.ReadAllText(filePath));

        WriteLine("Reading the compressed XML file: ");
        file = File.Open(filePath, FileMode.Open);

        Stream decompressor;
        if (useBrotil)
        {
            decompressor = new BrotliStream(file,CompressionMode.Decompress);
        }
        else
        {
            decompressor = new GZipStream(file, CompressionMode.Decompress);
        }

        using (decompressor)
        {
            using(XmlReader reader = XmlReader.Create(decompressor))
            {
                while (reader.Read())
                {
                    //노드이름이 callsign 확인
                    if((reader.NodeType == XmlNodeType.Element) && (reader.Name == "callsign"))
                    {
                        reader.Read();
                        WriteLine($"{reader.Value}");
                    }
                }
            }
        }
    }
}





