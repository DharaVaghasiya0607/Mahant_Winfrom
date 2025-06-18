using RestSharp;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WhatsAppContactManager
{
    private readonly string _baseUrl = "https://wp.axoneit.com/api";
    private readonly string _vendorUid = "a7227ae9-ceae-48a3-a837-7f0f51bd834f";
    private readonly string _token = "FKTFInX3sAU97zAI9VvZ7t6ZEFr0JfJRGBN6OGL5AQfQESJG1H3oY6ZqSBK4MpOv";

    public class CreateContactResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string contact_id { get; set; }
    }

    /// <summary>
    /// Creates a WhatsApp contact and returns success status, message, and contact ID (if created).
    /// </summary>
    public async Task<(bool isSuccess, string message, string contactId)> CreateContactAsync(string phoneNumber, string firstName, string lastName, string email)
    {
        var options = new RestClientOptions($"{_baseUrl}/{_vendorUid}/contact/create")
        {
            MaxTimeout = -1,
        };

        var client = new RestClient(options);
        var request = new RestRequest("", Method.Post);

        // Headers
        request.AddHeader("Authorization", $"Bearer {_token}");
        request.AddHeader("Content-Type", "application/json");

        // Request body
        var body = new
        {
            phone_number = phoneNumber,
            first_name = firstName,
            last_name = lastName,
            email = email,
            country = "india",
            language_code = "en",
            groups = "examplegroup1,examplegroup2",
            custom_fields = new
            {
                BDay = "2025-09-01"
            }
        };

        request.AddJsonBody(body);

        try
        {
            RestResponse response = await client.ExecuteAsync(request);
            var result = JsonConvert.DeserializeObject<CreateContactResponse>(response.Content);

            if (result != null)
            {
                return (result.status, result.message, result.contact_id);
            }
            else
            {
                return (false, "Empty or invalid response from server.", null);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Exception occurred: {ex.Message}", null);
        }
    }
}
