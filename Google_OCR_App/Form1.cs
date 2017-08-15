using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using static Newtonsoft.Json.JsonConvert;


namespace Google_OCR_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void TextBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //get image into base64string
                string imageFilePath = textBox4.Text.Trim();
                FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fileStream);
             
                byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);
                string base64String = Convert.ToBase64String(byteData);               
                string image_base64string = base64String;

                //generate json content string
                string jsonstring_orig = @"{ 
                      
                     ""requests"": [
                           {
                                                 ""image"": {
                                                               ""content"": ""rep_image_base64string""
                                                            },

                                               ""features"": [
                                                            {
                                                              ""type"": ""DOCUMENT_TEXT_DETECTION""
                                                             }
                                                            ],
                                               ""imageContext"": { ""languageHints"": [""en"" ,""zh-CN"",""zh-TW"" ,""zh""]},


                            }
                    ]
                   }";
                string jsonstring = jsonstring_orig.Replace("rep_image_base64string", image_base64string);

                textBox2.Text = jsonstring;

                //new http client and call api
                HttpClient client = new HttpClient();
                var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                var url = "https://vision.googleapis.com/v1/images:annotate?key=AIzaSyBf3aybUgE0aEvKgFRnBhZVN09V3S-A2js";
                var response = await client.PostAsync(url, content);

                var status = response.IsSuccessStatusCode;
                if (status)
                {

                    string contentString = await response.Content.ReadAsStringAsync();
                    string obj_descrpiton = "";
                    var obj = DeserializeObject<RootObject>(JsonPrettyPrint(contentString));

                    //Get description from textAnnotations
                    foreach (var obj_response in obj.responses)
                    {

                        foreach (var obj_text in obj_response.textAnnotations)
                        {
                            if (obj_text.locale == "en" || obj_text.locale == "zh" || obj_text.locale == "nl")
                            {

                                obj_descrpiton += obj_text.description;
                            }

                        }



                    }

                    textBox3.Text = obj_descrpiton;

                    textBox1.Text = "Call API Successfully!" ;
                }
                else
                {
                    textBox1.Text = "Call API failed !";

                }


            }
            catch (Exception ee)
            {

                textBox1.Text = "Call API Error:" + ee.Message.ToString();
            }

        }

        static string GetFromImageFile(string imageFilePath)
        {

            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);
            string base64String = Convert.ToBase64String(byteData);
            return base64String;




        }
        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }

        public class Vertex
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingPoly
        {
            public List<Vertex> vertices { get; set; }
        }

        public class TextAnnotation
        {
            public string locale { get; set; }
            public string description { get; set; }
            public BoundingPoly boundingPoly { get; set; }
        }

        public class DetectedLanguage
        {
            public string languageCode { get; set; }
        }

        public class Property
        {
            public List<DetectedLanguage> detectedLanguages { get; set; }
        }

        public class DetectedLanguage2
        {
            public string languageCode { get; set; }
        }

        public class Property2
        {
            public List<DetectedLanguage2> detectedLanguages { get; set; }
        }

        public class Vertex2
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox
        {
            public List<Vertex2> vertices { get; set; }
        }

        public class DetectedLanguage3
        {
            public string languageCode { get; set; }
        }

        public class Property3
        {
            public List<DetectedLanguage3> detectedLanguages { get; set; }
        }

        public class Vertex3
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox2
        {
            public List<Vertex3> vertices { get; set; }
        }

        public class DetectedLanguage4
        {
            public string languageCode { get; set; }
        }

        public class Property4
        {
            public List<DetectedLanguage4> detectedLanguages { get; set; }
        }

        public class Vertex4
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox3
        {
            public List<Vertex4> vertices { get; set; }
        }

        public class DetectedLanguage5
        {
            public string languageCode { get; set; }
        }

        public class DetectedBreak
        {
            public string type { get; set; }
        }

        public class Property5
        {
            public List<DetectedLanguage5> detectedLanguages { get; set; }
            public DetectedBreak detectedBreak { get; set; }
        }

        public class Vertex5
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox4
        {
            public List<Vertex5> vertices { get; set; }
        }

        public class Symbol
        {
            public Property5 property { get; set; }
            public BoundingBox4 boundingBox { get; set; }
            public string text { get; set; }
        }

        public class Word
        {
            public Property4 property { get; set; }
            public BoundingBox3 boundingBox { get; set; }
            public List<Symbol> symbols { get; set; }
        }

        public class Paragraph
        {
            public Property3 property { get; set; }
            public BoundingBox2 boundingBox { get; set; }
            public List<Word> words { get; set; }
        }

        public class Block
        {
            public Property2 property { get; set; }
            public BoundingBox boundingBox { get; set; }
            public List<Paragraph> paragraphs { get; set; }
            public string blockType { get; set; }
        }

        public class Page
        {
            public Property property { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public List<Block> blocks { get; set; }
        }

        public class FullTextAnnotation
        {
            public List<Page> pages { get; set; }
            public string text { get; set; }
        }

        public class Respons
        {
            public List<TextAnnotation> textAnnotations { get; set; }
            public FullTextAnnotation fullTextAnnotation { get; set; }
        }

        public class RootObject
        {
            public List<Respons> responses { get; set; }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                //get image into base64string
                string imageFilePath = textBox4.Text.Trim();
                FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fileStream);

                byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);
                string base64String = Convert.ToBase64String(byteData);
                string image_base64string = base64String;

                //generate json content string
                string jsonstring_orig = @"{ 
                      
                     ""requests"": [
                           {
                                                 ""image"": {
                                                               ""content"": ""rep_image_base64string""
                                                            },

                                               ""features"": [
                                                            {
                                                              ""type"": ""TEXT_DETECTION""
                                                             }
                                                            ],
                                               ""imageContext"": { ""languageHints"": [""en"" ,""zh-CN"",""zh-TW"" ,""zh""]},


                            }
                    ]
                   }";
                string jsonstring = jsonstring_orig.Replace("rep_image_base64string", image_base64string);

                textBox2.Text = jsonstring;

                //new http client and call api
                HttpClient client = new HttpClient();
                var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                var url = "https://vision.googleapis.com/v1/images:annotate?key=AIzaSyBf3aybUgE0aEvKgFRnBhZVN09V3S-A2js";
                var response = await client.PostAsync(url, content);

                var status = response.IsSuccessStatusCode;
                if (status)
                {

                    string contentString = await response.Content.ReadAsStringAsync();
                    string obj_descrpiton = "";
                    var obj = DeserializeObject<RootObject>(JsonPrettyPrint(contentString));

                    //Get description from textAnnotations
                    foreach (var obj_response in obj.responses)
                    {

                        foreach (var obj_text in obj_response.textAnnotations)
                        {
                              if (obj_text.locale =="en" || obj_text.locale == "zh" || obj_text.locale == "nl")
                             {

                            obj_descrpiton += obj_text.description;
                              }

                        }



                    }

                    textBox3.Text = obj_descrpiton;

                    textBox1.Text = "Call API Successfully!";
                }
                else
                {
                    textBox1.Text = "Call API failed !";

                }


            }
            catch (Exception ee)
            {

                textBox1.Text = "Call API Error:" + ee.Message.ToString();
            }

        }
  

        /// <summary>
        /// call Google Speech to Text api
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //get audio file into base64string
                //string speechFilePath = textBox4.Text.Trim();
                //FileStream fileStream = new FileStream(speechFilePath, FileMode.Open, FileAccess.Read);
                //BinaryReader binaryReader = new BinaryReader(fileStream);

                //byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);
                //string base64String = Convert.ToBase64String(byteData);
                //string audio_base64string = base64String;


                string audioFilePath = textBox4.Text.Trim();
                FileStream fileStream = File.OpenRead(@audioFilePath);
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.SetLength(fileStream.Length);
                fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
                byte[] BA_AudioFile = memoryStream.GetBuffer();
                string base64String = Convert.ToBase64String(BA_AudioFile);
                string audio_base64string = base64String;

                //generate json content string
                string jsonstring_orig = @"{
                              ""config"": {
                              ""encoding"": ""LINEAR16"",   
                              ""sampleRateHertz"": 16000,                    
                              ""languageCode"": ""en-US"",
                              ""enableWordTimeOffsets"": false
                                          },
                             ""audio"": {
                              ""content"": ""rep_aution""
                             }
                             }";

                string jsonstring = jsonstring_orig.Replace("rep_aution", audio_base64string);

                textBox2.Text = jsonstring;

                File.WriteAllText("C:\\Jerry Shen\\google_text_dection.txt", jsonstring);

                //new http client and call api
                HttpClient client = new HttpClient();
                var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                var url = "https://speech.googleapis.com/v1/speech:recognize?key=AIzaSyBf3aybUgE0aEvKgFRnBhZVN09V3S-A2js";
                var response = await client.PostAsync(url, content);

                var resutl = response.StatusCode;

                var status = response.IsSuccessStatusCode;
                if (status)
                {

                    string contentString = await response.Content.ReadAsStringAsync();
                    string obj_descrpiton = "";
                    var obj = DeserializeObject<RootObject_Audio>(JsonPrettyPrint(contentString));

                    //Get description from textAnnotations
                    foreach (var obj_response in obj.results)
                    {

                        foreach (var obj_alternativew in obj_response.alternatives)
                        {
                          
                                obj_descrpiton += obj_alternativew.transcript;
                        

                        }



                    }

                    textBox3.Text = obj_descrpiton;

                    textBox1.Text = "Call API Successfully!";
                }
                else
                {
                    textBox1.Text = "Call API failed !" + resutl.ToString(); ;

                }


            }
            catch (Exception ee)
            {

                textBox1.Text = "Call API Error:" + ee.Message.ToString();
            }
        }

        //calss for speech to text 
        public class Alternative
        {
            public string transcript { get; set; }
            public double confidence { get; set; }
        }

        public class Result
        {
            public List<Alternative> alternatives { get; set; }
        }

        public class RootObject_Audio
        {
            public List<Result> results { get; set; }
        }
    }
}
