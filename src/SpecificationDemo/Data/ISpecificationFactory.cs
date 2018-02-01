namespace SpecificationDemo.Data
{
    public interface ISpecificationFactory
    {
        ISpecification<T> Create<T>();
    }
}
