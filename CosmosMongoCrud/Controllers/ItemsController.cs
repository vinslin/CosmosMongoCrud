using CosmosMongoCrud.Models;
using CosmosMongoCrud.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            await _itemRepository.CreateAsync(item);
            return Ok();
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
