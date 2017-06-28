using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PS1_HTTP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener httpListener = new HttpListener();
            string[] array = { "http://localhost:8080/", "http://127.0.0.1:8080/" };
            for (int i = 0; i < array.Length; i++) httpListener.Prefixes.Add(array[i]);

            httpListener.Start();
            Console.WriteLine("Server started on localhost port 8080");
            Console.WriteLine("Listening for clients...");

            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                string outputPage = "";
                int suma = 0;
                string test = "";

                
                if (!(request.RawUrl.Equals("/favicon.ico")))
                {
                    array[0] = request.RawUrl;
                    array[0] = array[0].TrimStart('/');
                    array = array[0].Split(new char[] { '_' });
                    Console.WriteLine(array[0]);

                    for (int i = 0; i < array.Length; i++)
                    {
                        string text = array[i];
                        try
                        {
                            string fileName = text + ".txt";
                            StreamReader streamReader = new StreamReader(fileName);
                            test += streamReader.ReadToEnd();
                            Console.WriteLine("Reading from file: " + fileName);
                            streamReader.Close();

                            int num = 0;
                            ArrayList arrayList = new ArrayList();

                            while (true)
                            {
                                int num2 = test.IndexOf("<td>", num) + 4; // +4 bo pomijamy <td>
                                num = test.IndexOf("</td>", num + 1); // +1 jakbysmy wyszli za plik
                                if (num < 0)
                                {
                                    break;
                                }
                                string value = test.Substring(num2, num - num2);
                                arrayList.Add(value);
                            }
                            suma = 0;
                            for (int j = 0; j < arrayList.Count; j++)
                            {
                                if (j % 2 == 1) // co drugi element w liscie jest liczba
                                {
                                    suma += Convert.ToInt32(arrayList[j]);
                                }
                            }
                            outputPage = "<HTML><BODY><table border='1'>" + test + "</table><br>Suma:" + suma + "</BODY></HTML>";


                        }
                        catch (FileNotFoundException e)
                        {
                            outputPage = "Wpisales zly adres, sprobuj http://localhost:8080/1/ lub  http://localhost:8080/2/ ";
                        }
                    }

                }


                HttpListenerResponse response = context.Response;
                byte[] bytes = Encoding.UTF8.GetBytes(outputPage);
                response.ContentLength64 = (long)bytes.Length;
                Stream outputStream = response.OutputStream;
                outputStream.Write(bytes, 0, bytes.Length);
                outputStream.Close();
            }
        }
    }
}
