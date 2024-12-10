using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChallengeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RepositoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetRepositories()
        {
            var githubApiUrl = "https://api.github.com/orgs/takenet/repos";

            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("ChallengeApp");

            try
            {
                var response = await _httpClient.GetAsync(githubApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Erro ao acessar a API do GitHub");
                }

                var repositories = JsonSerializer.Deserialize<List<Repository>>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (repositories == null || !repositories.Any())
                {
                    return NotFound("Nenhum repositório encontrado na API do GitHub.");
                }

                var filteredRepositories = repositories
                    .Where(repo => repo.Language != null && repo.Language.Equals("C#", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(repo => repo.CreatedAt)
                    .Take(5)
                    .Select(repo => new
                    {
                        header = new
                        {
                            type = "application/vnd.lime.media-link+json",
                            value = new
                            {
                                title = repo.FullName,
                                text = repo.Description,
                                type = "image/jpeg",
                                uri = repo.Owner.AvatarUrl
                            }
                        }
                    })
                    .ToList();

                return Ok(filteredRepositories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

    }
}
