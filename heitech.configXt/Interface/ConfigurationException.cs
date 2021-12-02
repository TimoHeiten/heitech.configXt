using System;

namespace heitech.configXt
{
    public enum Crud
    {
        Create,
        Retrieve,
        Update,
        Delete
    }

    ///<summary>
    /// Special Exception that signals a failure in CRUD operation of configuration service
    ///</summary>
    public abstract class ConfigurationException : Exception
    {
        public ConfigurationException(ConfigModel model) : base($"{model?.Key} - {model?.Value}")
        {
        }

        public static ConfigurationException Create(Crud operation, ConfigModel model)
        {
            switch (operation)
            {
                case Crud.Create:
                    return new CreateException(model);
                case Crud.Update:
                    return new UpdateException(model);
                case Crud.Delete:
                    return new DeleteException(model);
                case Crud.Retrieve:
                    return new RetrieveException(model);
                default:
                    throw new ArgumentException($"no configException for {operation}");
            }
        }

        internal class CreateException : ConfigurationException
        {
            public CreateException(ConfigModel model) : base(model)
            {
            }
        }

        internal class UpdateException : ConfigurationException
        {
            public UpdateException(ConfigModel model) : base(model)
            {
            }
        }

        internal class DeleteException : ConfigurationException
        {
            public DeleteException(ConfigModel model) : base(model)
            {
            }
        }

        internal class RetrieveException : ConfigurationException
        {
            public RetrieveException(ConfigModel model) : base(model)
            {
            }
        }
    }
}