using System.Collections.Generic;

namespace heitech.configXt.Models
{
    ///<summary>
    /// Use Result for the Actual OperationResult
    ///</summary>
    public class UiOperationResult
    {
        ///<summary>
        /// For Serialization
        ///</summary>
        public UiOperationResult()
        {
        }

        ///<summary>
        /// For use inside the app
        ///</summary>
        public UiOperationResult(bool success, string resultType, string errorMessage, IEnumerable<ConfigurationModel> model)
        {
            Initialize(success, resultType, errorMessage, model);
        }

        private void Initialize(bool success, string resultType, string errorMessage, IEnumerable<ConfigurationModel> entity)
        {
            IsSuccess = success;
            ResultName = resultType;
            ConfigurationModels = new List<ConfigurationModel>();

            if (IsSuccess == false)
            {
                ErrorMessage = errorMessage;
                return;
            }
            ConfigurationModels = new List<ConfigurationModel>(entity);
        }

        ///<summary>
        /// All the Returned Models. Zero or one Item, except when ReadAll was used
        ///</summary>
        public List<ConfigurationModel> ConfigurationModels { get; set; }
        ///<summary>
        /// Indicates if the operation succedeed
        ///</summary>
        public bool IsSuccess { get; set; }
        ///<summary>
        /// If no Success, this is the error that occured
        ///</summary>
        public string ErrorMessage { get; set; }
        ///<summary>
        /// The ResultType of the Operation (think http status code)
        ///</summary>
        public string ResultName { get; set; }
    }

}