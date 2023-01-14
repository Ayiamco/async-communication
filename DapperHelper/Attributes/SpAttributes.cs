namespace Dapper.BaseRepository.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputStringAttribute : Attribute
    {
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
}
