using Persistence.Interfaces;

namespace Persistence.Services {
	public class SystemService {
		private readonly IRepository _repository;
		private readonly ValidationService _validationService;

		public SystemService(IRepository repository, ValidationService validationService) {
			_repository = repository;
			_validationService = validationService;
		}
	}
}