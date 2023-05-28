using needy_dataAccess.Interfaces;
using needy_dataAccess.Repositories;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class RatingLogic : IRatingLogic
    {
        #region Properties and Fields

        private readonly IRatingRepository _ratingRepository;
        private readonly INeedRepository _needRepository;
        private readonly ITokenLogic _tokenLogic;

        #endregion

        #region Builders

        public RatingLogic(IRatingRepository ratingRepository, INeedRepository needRepository, ITokenLogic tokenLogic)
        {
            _ratingRepository = ratingRepository;
            _needRepository = needRepository;
            _tokenLogic = tokenLogic;
        }

        #endregion

        #region Implements IRatingLogic

        public async Task<IEnumerable<Rating>> GetUserRatingsAsync(string userCI)
        {
            return await _ratingRepository.GetUserRatingsAsync(userCI);
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            if (await IsAcceptedApplier(parameters.NeedId, parameters.ReceiverCI) &&
                await IsNeedExists(parameters.NeedId))
            {
                string userCI = await _tokenLogic.GetUserCIFromToken();

                if(await IsRequestor(parameters.NeedId, userCI))
                {
                    return await _ratingRepository.InsertRatingAsync(userCI, parameters);
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Private Methods

        private async Task<bool> IsAcceptedApplier(int needId, string receiverCI)
        {
            string acceptedApplierCI = await _needRepository.GetNeedAcceptedApplierAsync(needId);

            if (acceptedApplierCI is not null)
            {
                return acceptedApplierCI.Equals(receiverCI);
            }

            return false;
        }

        private async Task<bool> IsNeedExists(int needId)
        {
            NeedData need = await _needRepository.GetNeedByIdAsync(needId);

            return need is not null ? true : false;
        }

        private async Task<bool> IsRequestor(int needId, string giverCI)
        {
            string requestorCI = await _needRepository.GetNeedRequestorAsync(needId);

            if (requestorCI is not null)
            {
                return requestorCI.Equals(giverCI);
            }

            return false;
        }

        #endregion
    }
}
