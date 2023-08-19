using API.Controllers.Api;
using ApplicationCore.Dtos;
using ApplicationCore.Interface;
using Domain.Entities.MemoAggregate;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Mem : BaseApiController
    {
        
        private readonly ILogger<Mem> _logger;
        private readonly IService<Memo> _memoService;

        public Mem(ILogger<Mem> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Mem")]
        public async Task<List<MemoDto>> Get()
        {
            var memos = await _memoService.ListAsync();
            return new List<MemoDto> { new MemoDto { Id = 1, Name = "Job 1" } };
        }
    }
}