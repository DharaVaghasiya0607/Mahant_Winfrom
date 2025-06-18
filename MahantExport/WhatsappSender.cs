using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class WhatsAppSender
{
    public static async Task SendTemplateMessageAsync()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://wp.axoneit.com/api/a7227ae9-ceae-48a3-a837-7f0f51bd834f/contact/send-template-message");

        // Required headers
        request.Headers.Add("Authorization", "Bearer FKTFInX3sAU97zAI9VvZ7t6ZEFr0JfJRGBN6OGL5AQfQESJG1H3oY6ZqSBK4MpOv");

        // Optional: Only needed if your API requires it
        // request.Headers.Add("Cookie", "PHPSESSID=...");

        // JSON payload
        var jsonBody = @"
{
    ""from_phone_number_id"": """",
    ""phone_number"": ""8530067187"",
    ""template_name"" : ""ceramic_marketing_004"",
    ""template_language"" : ""en_US"",
    ""header_image"" : ""https://wp.axoneit.com/media-storage/vendors/a7227ae9-ceae-48a3-a837-7f0f51bd834f/whatsapp_media/images/68529b61886be-beige-68529b6403fec.jpg"",
    ""header_video"" : ""https://wp.axoneit.com/media-storage/vendors/a7227ae9-ceae-48a3-a837-7f0f51bd834f/whatsapp_media/images/68529b61886be-beige-68529b6403fec.jpg"",
    ""header_document"" : ""https://wp.axoneit.com/media-storage/vendors/a7227ae9-ceae-48a3-a837-7f0f51bd834f/whatsapp_media/images/68529b61886be-beige-68529b6403fec.jpg"",
    ""header_document_name"" : ""Tiles_Erp.pdf"",
    ""location_address"" : ""India"",
    ""contact"": {
        ""first_name"" : ""Ankit"",
        ""last_name"" : ""Gohil"",
        ""email"" : ""ankit@axoneit.com"",
        ""country"" : ""india"",
        ""language_code"" : ""en_US"",
        ""groups"" : """"
    }
}";

        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Message sent successfully.");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("❌ Failed to send message.");
                Console.WriteLine($"Status Code: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine("Response: " + result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Exception occurred: " + ex.Message);
        }
    }
}
