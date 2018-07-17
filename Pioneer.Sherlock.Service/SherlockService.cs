using Pioneer.Sherlock.Service.Model;

namespace Pioneer.Sherlock.Service
{
    public class SherlockService
    {
        protected IRepository _repository = null;
        public SherlockService()
        {
            _repository = new Repository();
        }
        public int NewAdminLeadCapture(object lead, string url)
        {
            return _repository.SubmitAdwordsPerformance(lead, url);
        }

    }
}
