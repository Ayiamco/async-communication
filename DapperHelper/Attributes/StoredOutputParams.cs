using System.Data;

namespace Dapper.BaseRepository.Attributes
{
    /// <summary>
    /// Attribute for DbType <see cref="DbType.String"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputStringAttribute : Attribute
    {
        public int Length { get; set; }

        public SpOutputStringAttribute(int Length)
        {
            this.Length = Length;
        }
    }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.StringFixedLength"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputStringFixedAttribute : Attribute
    {
        public int Length { get; set; }

        public SpOutputStringFixedAttribute(int Length)
        {
            this.Length = Length;
        }
    }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.AnsiString"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputAnsiStringAttribute : Attribute
    {
        public int Length { get; set; }

        public SpOutputAnsiStringAttribute(int Length)
        {
            this.Length = Length;
        }
    }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.AnsiStringFixedLength"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputAnsiStringFixedAttribute : Attribute
    {
        public int Length { get; set; }

        public SpOutputAnsiStringFixedAttribute(int Length)
        {
            this.Length = Length;
        }
    }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.Int32"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputIntAttribute : Attribute { }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.Int64"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputBigIntAttribute : Attribute { }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.DateTime"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputDateTime : Attribute { }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.Date"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputDate : Attribute { }

    /// <summary>
    /// Attribute for DbType <see cref="DbType.Guid"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SpOutputGuid : Attribute { }
}
