using CosmosMongoCrud.Models;
using CosmosMongoCrud.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace CosmosMongoCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemRepository _itemRepository;

        public ItemsController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAll()
        {
            var items = await _itemRepository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(string id)
        {
            Item item = await _itemRepository.GetAsync(id);
            if (item is null)
                return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            using var httpClient = new HttpClient();
            var functionUrl = "http://localhost:7024/api/Function1"; // Use local or deployed URL

            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(functionUrl, content);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Azure Function call failed.");

            var modifiedItemJson = await response.Content.ReadAsStringAsync();
            var modifiedItem = JsonSerializer.Deserialize<Item>(modifiedItemJson);

            await _itemRepository.CreateAsync(modifiedItem);

            return Ok(modifiedItem);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id,Item item)
        {
            var check = await _itemRepository.GetAsync(id);
            if (check is null)
                return NotFound();
            await _itemRepository.UpdateAsync(id,item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> Delete(string id)
        {
            var item = await _itemRepository.GetAsync(id);
            if (item is null)
                return NotFound();
            await _itemRepository.DeleteAsync(id);
            return Ok(item);
        }
       



    }
}
