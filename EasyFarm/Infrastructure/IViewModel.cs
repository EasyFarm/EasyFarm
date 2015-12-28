namespace EasyFarm.Infrastructure
{
    public interface IViewModel
    {
        /// <summary>
        ///     The name of the view model.
        /// </summary>
        string ViewName { get; set; }
    }
}