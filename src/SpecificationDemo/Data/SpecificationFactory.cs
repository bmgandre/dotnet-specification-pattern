namespace SpecificationDemo.Data
{
    public class SpecificationFactory : ISpecificationFactory
    {
        public ISpecification<T> Create<T>()
        {
            return new NullSpecification<T>();
        }
    }
}
