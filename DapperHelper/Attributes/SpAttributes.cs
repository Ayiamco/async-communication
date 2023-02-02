namespace Dapper.BaseRepository.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputStringAttribute : Attribute
    {
        //  TODO: add size description, Add date time attribute, add date attribute, function  to map dynamic parameters to model
        public int Size { get; set; }

        public SpOutputStringAttribute(int size)
        {
            Size = size;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SpReturnStringAttribute : Attribute
    {
        public int Size { get; set; }

        public SpReturnStringAttribute(int size)
        {
            Size = size;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SpReturnIntAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputIntAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SpReturnBigIntAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputBigIntAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputDateTime : Attribute
    {

    }
}
