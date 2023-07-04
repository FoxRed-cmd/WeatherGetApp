using System.Text;

using (HttpClient client = new HttpClient()) 
{
    byte[] buffer = await client.GetByteArrayAsync("https://www.yandex.ru/pogoda/voronezh");
    string response = Encoding.UTF8.GetString(buffer);
    Console.WriteLine(response);
}