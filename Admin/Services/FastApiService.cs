using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class FastApiService
{
    private readonly HttpClient _httpClient;

    public FastApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ExtractTextFromImage(IFormFile file)
    {
        using var content = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

        content.Add(fileContent, "file", file.FileName);

        var response = await _httpClient.PostAsync("http://localhost:8000/analyze", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
