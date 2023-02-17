using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using pulsenet.api.models;

namespace pulsenet.api.Services
{
    public class ComparisonService : IComparisonService
    {
        private readonly ILogger<ComparisonService> _logger;
        public readonly IMapper _mapper;
        public readonly IDataService<Comparison> _comparisonDataService;

        public ComparisonService(ILogger<ComparisonService> logger, IMapper mapper, IDataService<Comparison> comparisonDataService)
        {
            _logger = logger;
            _mapper = mapper;
            _comparisonDataService = comparisonDataService;
        }

        public async Task<List<Comparison>> GetAllComparisonsAsync()
        {
            var comparisons = await _comparisonDataService.GetAllAsync();
            return comparisons;
        }

        public async Task<Comparison?> GetComparisonByIdAsync(int id)
        {
            var comparison = await _comparisonDataService.GetByIdAsync(id);
            return comparison;
        }

        public async Task<bool> InsertComparisonsAsync(ComparisonDTO comparisonDTO)
        {
            var comparison = _mapper.Map<Comparison>(comparisonDTO);
            var result = await _comparisonDataService.InsertAsync(comparison);
            return result;
        }

        public async Task<bool> UpdateComparisonsAsync(int id, ComparisonDTO comparisonDTO)
        {
            var comparison = _mapper.Map<Comparison>(comparisonDTO);
            comparison.id = id;
            var result = await _comparisonDataService.UpdateAsync(comparison);
            return result;
        }

        public async Task<bool> DeleteComparisonsAsync(int id)
        {
            var result = await _comparisonDataService.DeleteAsync(id);
            return result;
        }

        public async Task<Comparison?> GetComparisonByNameAsync(string name)
        {
            var comparison = await _comparisonDataService.GetByNameAsync(name);
            return comparison;
        }
    }
}
