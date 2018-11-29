namespace Common
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; set; }

        public DescriptionAttribute()
        {

        }

        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }

    }
}
