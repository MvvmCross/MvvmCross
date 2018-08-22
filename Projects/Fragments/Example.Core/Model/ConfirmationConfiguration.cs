using System;
namespace Example.Core.Model
{
    public class ConfirmationConfiguration
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string PositiveCommandText { get; set; }
        public string NegativeCommandText { get; set; }
    }
}
